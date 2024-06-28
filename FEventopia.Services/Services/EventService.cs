using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ISponsorManagementRepository _sponsorManagementRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper, IUserRepository userRepository, ITicketRepository ticketRepository, ISponsorManagementRepository sponsorManagementRepository, ITransactionRepository transactionRepository)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _sponsorManagementRepository = sponsorManagementRepository;
            _transactionRepository = transactionRepository;
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
            @event.Status = EventStatus.CANCELED.ToString();
            await _eventRepository.UpdateAsync(@event);

            //Lấy thông tin user đã mua vé sự kiện này => hoàn tiền
            var ticketList = await _ticketRepository.GetAllTicketWithDetailCurrentEvent(@event.Id.ToString());
            foreach (var ticket in ticketList)
            {
                var user = await _userRepository.GetAccountByIdAsync(ticket.VisitorID);
                user.CreditAmount += ticket.Transaction.Amount;
                await _userRepository.UpdateAccountAsync(user);

                //Tạo transaction
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountID = user.Id,
                    TransactionType = TransactionType.IN.ToString(),
                    TransactionDate = TimeUtils.GetTimeVietNam(),
                    Amount = ticket.Transaction.Amount,
                    Description = $"FEventopia {user.UserName.ToUpper()}: '{@event.EventName}' event has been canceled: Refund -{ticket.Transaction.Amount}.",
                    Status = true
                };
                await _transactionRepository.AddAsync(transaction);
            };

            //Lấy thông tin các sponsor đã tài trợ cho sự kiện này => hoàn tiền
            var sponsorList = await _sponsorManagementRepository.GetAllSponsorManagementWithDetailCurrentEvent(@event.Id.ToString());
            foreach (var sponsor in sponsorList)
            {
                var user = await _userRepository.GetAccountByIdAsync(sponsor.SponsorId);
                user.CreditAmount += sponsor.ActualAmount;
                await _userRepository.UpdateAccountAsync(user);

                //Tạo transaction
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountID = user.Id,
                    TransactionType = TransactionType.IN.ToString(),
                    TransactionDate = TimeUtils.GetTimeVietNam(),
                    Amount = sponsor.ActualAmount,
                    Description = $"FEventopia {user.UserName.ToUpper()}: '{@event.EventName}' event has been canceled: Refund -{sponsor.ActualAmount}.",
                    Status = true
                };
                await _transactionRepository.AddAsync(transaction);
            }

            return true;
        }

        public async Task<bool> DeleteEventAsync(string id)
        {
            var result = await _eventRepository.GetByIdAsync(id);
            if (result == null)
            {
                return false;
            }
            return await _eventRepository.DeleteAsync(result);
        }

        public async Task<PageModel<EventModel>> GetAllEventAsync(PageParaModel pagePara)
        {
            var eventList = await _eventRepository.GetAllAsync();
            var result = _mapper.Map<List<EventModel>>(eventList);
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
            var eventModel = await _eventRepository.GetByIdAsync(id);
            if (eventModel == null)
            {
                return false;
            }
            switch (eventModel.Status.ToString())
            {
                case "INITIAL":
                    eventModel.Status = EventStatus.FUNDRAISING.ToString();
                    break;
                case "FUNDRAISING":
                    eventModel.Status = EventStatus.PREPARATION.ToString();
                    break;
                case "PREPARATION":
                    eventModel.Status = EventStatus.EXECUTE.ToString();
                    break;
                case "EXECUTE":
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

    }
}
