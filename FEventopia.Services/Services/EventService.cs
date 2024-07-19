using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventStallRepository _eventStallRepository;
        private readonly ISponsorEventRepository _sponsorEventRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ISponsorManagementRepository _sponsorManagementRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper, IUserRepository userRepository, ITicketRepository ticketRepository, ISponsorManagementRepository sponsorManagementRepository, ITransactionRepository transactionRepository, IEventStallRepository eventStallRepository, ISponsorEventRepository sponsorEventRepository, IEventDetailRepository eventDetailRepository, ITaskRepository taskRepository)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _sponsorManagementRepository = sponsorManagementRepository;
            _transactionRepository = transactionRepository;
            _eventStallRepository = eventStallRepository;
            _sponsorEventRepository = sponsorEventRepository;
            _eventDetailRepository = eventDetailRepository;
            _taskRepository = taskRepository;
        }

        public async Task<EventOperatorModel> AddEventAsync(EventProcessModel eventProcessModel)
        {
            var eventModel = _mapper.Map<Event>(eventProcessModel); 
            eventModel.Status = EventStatus.INITIAL.ToString();
            var result = await _eventRepository.AddAsync(eventModel);
            return _mapper.Map<EventOperatorModel>(result);
        }

        public async Task<bool> CancelEventAsync(string id)
        {
            //Cập nhật trạng thái sự kiện sang hủy
            var @event = await _eventRepository.GetEventWithDetailByIdAsync(id);

            //Nếu ko tìm thấy hoặc sự kiện đã canceled =>
            if (@event == null || @event.Status.Equals(EventStatus.CANCELED.ToString()) || @event.Status.Equals(EventStatus.POST.ToString())) return false;
            @event.Status = EventStatus.CANCELED.ToString();

            //Hủy SponsorEvent
            var sponsorEventList = await _sponsorEventRepository.GetAllSponsorEventWithDetailCurrentEvent(@event.Id.ToString());
            foreach (var sponsorEvent in sponsorEventList)
            {
                await _sponsorEventRepository.DeleteAsync(sponsorEvent);
            }

            //Lấy thông tin các sponsor đã tài trợ cho sự kiện này => hoàn tiền
            var sponsorList = await _sponsorManagementRepository.GetAllSponsorManagementWithDetailCurrentEvent(@event.Id.ToString());
            foreach (var sponsor in sponsorList)
            {
                var user = await _userRepository.GetAccountByIdAsync(sponsor.SponsorId);
                user.CreditAmount += sponsor.ActualAmount;
                await _userRepository.UpdateAccountAsync(user);

                //Hủy hợp đồng
                await _sponsorManagementRepository.DeleteAsync(sponsor);

                //Tạo transaction
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountID = user.Id,
                    TransactionType = TransactionType.IN.ToString(),
                    TransactionDate = TimeUtils.GetTimeVietNam(),
                    Amount = sponsor.ActualAmount,
                    Description = $"FEventopia {user.UserName.ToUpper()}: '{@event.EventName}' event has been canceled: Refund +{sponsor.ActualAmount} | SponsorAgreementId: {sponsor.Id}.",
                    Status = true
                };
                await _transactionRepository.AddAsync(transaction);
            }
            @event.SponsorCapital = 0;

            //Lấy các eventDetail
            var eventDetailList = await _eventDetailRepository.GetAllEventDetailWithLocationById(@event.Id.ToString());
            foreach (var eventDetail in eventDetailList)
            {
                //Lấy thông tin user đã mua vé sự kiện này => hoàn tiền
                var ticketList = await _ticketRepository.GetAllTicketWithDetailCurrentEvent(eventDetail.Id.ToString());
                foreach (var ticket in ticketList)
                {
                    var user = await _userRepository.GetAccountByIdAsync(ticket.VisitorID);
                    user.CreditAmount += ticket.Transaction.Amount;
                    await _userRepository.UpdateAccountAsync(user);

                    //Hủy vé
                    await _ticketRepository.DeleteAsync(ticket);

                    //Tạo transaction
                    var transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        AccountID = user.Id,
                        TransactionType = TransactionType.IN.ToString(),
                        TransactionDate = TimeUtils.GetTimeVietNam(),
                        Amount = ticket.Transaction.Amount,
                        Description = $"FEventopia {user.UserName.ToUpper()}: '{@event.EventName}' event has been canceled: Refund +{ticket.Transaction.Amount} | TicketId: {ticket.Id}.",
                        Status = true
                    };
                    await _transactionRepository.AddAsync(transaction);
                };

                //Hoàn tiền mua stall
                var stallList = await _eventStallRepository.GetAllEventStallByEventDetailId(eventDetail.Id.ToString());
                foreach (var stall in stallList)
                {
                    var user = await _userRepository.GetAccountByIdAsync(stall.SponsorID);
                    user.CreditAmount += stall.Transaction.Amount;
                    await _userRepository.UpdateAccountAsync(user);

                    //Hủy stall
                    await _eventStallRepository.DeleteAsync(stall);

                    //Tạo transaction
                    var transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        AccountID = user.Id,
                        TransactionType = TransactionType.IN.ToString(),
                        TransactionDate = TimeUtils.GetTimeVietNam(),
                        Amount = stall.Transaction.Amount,
                        Description = $"FEventopia {user.UserName.ToUpper()}: '{@event.EventName}' event has been canceled: Refund +{stall.Transaction.Amount} | StallId: {stall.Id}.",
                        Status = true
                    };
                    await _transactionRepository.AddAsync(transaction);
                }

                await _eventDetailRepository.DeleteAsync(eventDetail);
            }
            @event.TicketSaleIncome = 0;
            @event.StallSaleIncome = 0;
            await _eventRepository.UpdateAsync(@event);
            return true;
        }

        public async Task<bool> DeleteEventAsync(string id)
        {
            var result = await _eventRepository.GetByIdAsync(id);
            if (result == null || !result.Status.Equals(EventStatus.INITIAL.ToString()))
            {
                return false;
            }
            return await _eventRepository.DeleteAsync(result);
        }

        public async Task<PageModel<EventModel>> GetAllEventAsync(PageParaModel pagePara, string? category, string? status)
        {
            var eventList = await _eventRepository.GetAllAsync();
            eventList.Sort((t1, t2) => t2.CreatedDate.CompareTo(t1.CreatedDate));
            var result = _mapper.Map<List<EventModel>>(eventList);
            if (!category.IsNullOrEmpty())
            {
                result = result.Where(e => e.Category.ToLower().Equals(category.ToLower())).ToList();
            }
            if (!status.IsNullOrEmpty())
            {
                result = result.Where(e => e.Status.ToLower().Equals(status.ToLower())).ToList();
            }
            return PageModel<EventModel>.ToPagedList(result,
                pagePara.PageNumber,
                pagePara.PageSize);
        }

        public async Task<EventModel> GetEventByIdAsync(string id)
        {
            var result = await _eventRepository.GetEventWithDetailByIdAsync(id);
            return _mapper.Map<EventModel>(result);
        }

        public async Task<EventOperatorModel> GetEventByIdOperatorAsync(string id)
        {
            var result = await _eventRepository.GetEventWithDetailByIdAsync(id);
            return _mapper.Map<EventOperatorModel>(result);
        }

        public async Task<EventOperatorModel> UpdateEventAsync(string id, EventProcessModel eventProcessModel)
        {
            var eventCurrent = await _eventRepository.GetByIdAsync(id);
            if (eventCurrent == null) 
            {
                return null;
            }

            if (eventCurrent.Status.Equals(EventStatus.EXECUTE.ToString()) || eventCurrent.Status.Equals(EventStatus.POST.ToString()) || eventCurrent.Status.Equals(EventStatus.CANCELED.ToString()))
            {
                return null;
            }

            var eventUpdate = _mapper.Map(eventProcessModel, eventCurrent);
            var result = await _eventRepository.UpdateAsync(eventUpdate);
            if (!result)
            {
                return null;
            }
            return _mapper.Map<EventOperatorModel>(eventUpdate);
        }

        public async Task<bool> UpdateEventNextPhaseAsync(string id)
        {
            var eventModel = await _eventRepository.GetEventWithDetailByIdAsync(id);
            if (eventModel == null)
            {
                return false;
            }
            //Lấy các eventDetail
            var eventDetailList = await _eventDetailRepository.GetAllEventDetailWithLocationById(eventModel.Id.ToString());
            EventStatus status = (EventStatus)System.Enum.Parse(typeof(EventStatus), eventModel.Status);
            switch (status)
            {
                case EventStatus.INITIAL:
                    eventModel.Status = EventStatus.FUNDRAISING.ToString();
                    break;
                case EventStatus.FUNDRAISING:
                    //Cập nhật số dư thực tế của sponsor
                    var sponsorList = await _sponsorManagementRepository.GetAllSponsorManagementWithDetailCurrentEvent(id);
                    foreach (var sponsor in sponsorList)
                    {
                        //Nếu sponsor chưa chuyển đủ như hợp đồng
                        if(!sponsor.Status.Equals(SponsorsManagementStatus.FULL.ToString()))
                        {
                            sponsor.SetAmount(sponsor.ActualAmount);
                            if (sponsor.Rank.Equals(SponsorRank.FAILEDCREDIT))
                            {
                                await _sponsorManagementRepository.DeleteAsync(sponsor);
                            } else await _sponsorManagementRepository.UpdateAsync(sponsor);
                        }
                    }
                    eventModel.Status = EventStatus.PREPARATION.ToString();
                    break;
                case EventStatus.PREPARATION:
                    //Nếu sự kiện chưa tạo eventDetails mà đã muốn mở bán vé, từ chối
                    if ( eventModel == null || eventModel.EventDetail.IsNullOrEmpty())
                    {
                        return false;
                    }
                    
                    foreach(var eventDetail in eventDetailList)
                    {
                        //Nếu các task của sự kiện chưa done => từ chối
                        var taskList = await _taskRepository.GetAllByEventDetailId(eventDetail.Id.ToString());
                        foreach (var task in taskList)
                        {
                            if (!task.Status.Equals(Enum.TaskStatus.DONE.ToString())) return false;
                        }
                    }
                    eventModel.Status = EventStatus.EXECUTE.ToString();
                    break;
                case EventStatus.EXECUTE:
                    foreach(var eventDetail in eventDetailList)
                    {
                        //Sự kiện chỉ được POST khi đã xong hết các EventDetail
                        if (TimeUtils.GetTimeVietNam() <= eventDetail.EndDate) return false;
                    }
                    eventModel.Status = EventStatus.POST.ToString();
                    break;
                default:
                    return false;
            }
            await _eventRepository.UpdateAsync(eventModel);
            return true;
        }

        public async Task<EventModel> GetEventByName(string name)
        {
            var result = await _eventRepository.GetEventByName(name);
            return _mapper.Map<EventModel>(result);
        }

        public async Task<List<EventModel>> SearchEventByName(string name)
        {
            var results = await _eventRepository.SearchEventByName(name);
            return _mapper.Map<List<EventModel>>(results);
        }

        public async Task<PageModel<EventModel>> GetAllEventForVisitorAsync(PageParaModel pagePara, string? category, string? status)
        {
            var eventList = await _eventRepository.GetAllEventForVisitorAsync();
            eventList.Sort((t1, t2) => t2.CreatedDate.CompareTo(t1.CreatedDate));
            var result = _mapper.Map<List<EventModel>>(eventList);
            if (!category.IsNullOrEmpty())
            {
                result = result.Where(e => e.Category.ToLower().Equals(category.ToLower())).ToList();
            }
            if (!status.IsNullOrEmpty())
            {
                result = result.Where(e => e.Status.ToLower().Equals(status.ToLower())).ToList();
            }
            return PageModel<EventModel>.ToPagedList(result,
                pagePara.PageNumber,
                pagePara.PageSize);
        }

        public async Task<List<EventModel>> GetAllAsync()
        {
            var result = await _eventRepository.GetAllAsync();
            result.Sort((t1, t2) => t2.CreatedDate.CompareTo(t1.CreatedDate));
            return _mapper.Map<List<EventModel>>(result);
        }
    }
}
