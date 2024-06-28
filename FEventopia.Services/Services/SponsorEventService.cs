using AutoMapper;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Enum;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEventopia.Repositories.Repositories;
using FEventopia.DAO.EntityModels;
using FEventopia.Services.Utils;

namespace FEventopia.Services.Services
{
    public class SponsorEventService : ISponsorEventService
    {
        private readonly ISponsorEventRepository _sponsorEventRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ISponsorManagementRepository _sponsorManagementRepository;
        private readonly IMapper _mapper;

        public SponsorEventService(ISponsorEventRepository sponsorEventRepository, IMapper mapper, IEventRepository eventRepository, IUserRepository userRepository, ITransactionRepository transactionRepository, ISponsorManagementRepository sponsorManagementRepository)
        {
            _sponsorEventRepository = sponsorEventRepository;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _sponsorManagementRepository = sponsorManagementRepository;
        }

        public async Task<SponsorEventModel> AddSponsorEventAsync(SponsorEventProcessModel sponsorEventProcessModel, string username)
        {
            //Lấy event, nếu event ko tồn tại => lỗi
            var @event = await _eventRepository.GetByIdAsync(sponsorEventProcessModel.EventId.ToString());
            if (@event == null) { return null; }

            //Nếu sự kiện chưa mở nhận tài trợ hoặc đã qua đợt tài trợ => Hủy
            if (!@event.Status.Equals(EventStatus.FUNDRAISING.ToString())) { return null; }

            //Lấy account user đang login => Lấy accountId, trừ tiền trong tài khoản
            var account = await _userRepository.GetAccountByUsernameAsync(username);

            //Nếu tiền trong tài khoản ko đủ => ko được tài trợ tiếp
            if (account.CreditAmount < sponsorEventProcessModel.Amount) { return null; }

            //Lấy agreement, chưa hứa chuyển => Hứa đi rồi cho chuyển :3
            var agreement = await _sponsorManagementRepository.GetSponsorManagementDetailByPrimaryKey(@event.Id.ToString(), account.Id.ToString());
            if (agreement == null) { return null; }

            //Cập nhật số dư tài khoản
            account.CreditAmount -= sponsorEventProcessModel.Amount;
            await _userRepository.UpdateAccountAsync(account);

            //Cập nhật tiền nhận tài trợ của sự kiện
            @event.TicketSaleIncome += sponsorEventProcessModel.Amount;
            await _eventRepository.UpdateAsync(@event);

            //Ghi nhận giao dịch, cập nhật Agreement
            //Nếu chuyển lần đầu, cập nhật thành Partial
            if (agreement.Status.Equals(SponsorsManagementStatus.PENDING.ToString()))
            {
                agreement.Status = SponsorsManagementStatus.PARTIAL.ToString();
            }
            
            //Cập nhật số tiền đã tài trợ thực tế
            agreement.ActualAmount += sponsorEventProcessModel.Amount;

            //Nếu số tiền thực tế lớn hơn hoặc bằng => Cập nhật Status thành đủ
            if (agreement.ActualAmount.Equals(agreement.PledgeAmount) || agreement.ActualAmount > agreement.PledgeAmount)
            {
                agreement.Status = SponsorsManagementStatus.FULL.ToString();
            }
            await _sponsorManagementRepository.UpdateAsync(agreement);

            //Tạo transaction OUT, ghi nhận giao dịch
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountID = account.Id,
                TransactionType = TransactionType.OUT.ToString(),
                TransactionDate = TimeUtils.GetTimeVietNam(),
                Amount = sponsorEventProcessModel.Amount,
                Description = $"FEventopia {username.ToUpper()}: Sponsor for '{@event.EventName}' event -{sponsorEventProcessModel.Amount}.",
                Status = true
            };
            await _transactionRepository.AddAsync(transaction);

            //Tạo Sponsor Event
            var sponsorEvent = new SponsorEvent
            {
                Id = Guid.NewGuid(),
                EventID = @event.Id,
                SponsorID = account.Id,
                TransactionID = transaction.Id,
            };
            await _sponsorEventRepository.AddAsync(sponsorEvent);

            var result = await _sponsorEventRepository.GetSponsorEventDetailById(sponsorEvent.Id.ToString());
            return _mapper.Map<SponsorEventModel>(result);
        }

        public async Task<PageModel<SponsorEventModel>> GetAllSponsorEventWithDetailAsync(PageParaModel pageParaModel)
        {
            var sponsorEvents = await _sponsorEventRepository.GetAllSponsorEvent();
            var result = _mapper.Map<List<SponsorEventModel>>(sponsorEvents);
            return PageModel<SponsorEventModel>.ToPagedList(result, 
                pageParaModel.PageNumber, 
                pageParaModel.PageSize);
        }

        public async Task<PageModel<SponsorEventModel>> GetAllSponsorEventWithDetailCurrentEvent(string eventId, PageParaModel pageParaModel)
        {
            var sponsorEvents = await _sponsorEventRepository.GetAllSponsorEventWithDetailCurrentEvent(eventId);
            var result = _mapper.Map<List<SponsorEventModel>>(sponsorEvents);
            return PageModel<SponsorEventModel>.ToPagedList(result,
                pageParaModel.PageNumber,
                pageParaModel.PageSize);
        }

        public async Task<PageModel<SponsorEventModel>> GetAllSponsorEventWithDetailCurrentUser(string username, PageParaModel pageParaModel)
        {
            var user = await _userRepository.GetAccountByUsernameAsync(username);
            var sponsorEvents = await _sponsorEventRepository.GetAllSponsorEventWithDetailCurrentUser(user.Id);
            var result = _mapper.Map<List<SponsorEventModel>>(sponsorEvents);
            return PageModel<SponsorEventModel>.ToPagedList(result,
                pageParaModel.PageNumber,
                pageParaModel.PageSize);
        }

        public async Task<SponsorEventModel?> GetSponsorEventDetailById(string sponsorEventId)
        {
            var result = await _sponsorEventRepository.GetSponsorEventDetailById(sponsorEventId);
            return _mapper.Map<SponsorEventModel?>(result);
        }
    }
}
