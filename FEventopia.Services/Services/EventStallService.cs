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
    public class EventStallService : IEventStallService
    {
        private readonly IEventStallRepository _eventStallRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventStallService(IEventStallRepository eventStallRepository, IUserRepository userRepository, 
            ITransactionRepository transactionRepository, IEventDetailRepository eventDetailRepository, IEventRepository eventRepository, IMapper mapper)
        {
            _eventStallRepository = eventStallRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;

            _mapper = mapper;
        }

        public async Task<EventStallModel> CreateEventStall(string eventDetailId, string username, string stallnumber)
        {
            //lay event detail => lay stallopenforsale => kiem tra con` stall hay ko => ko con => chim cut
            var eventdetail = await _eventDetailRepository.GetByIdAsync(eventDetailId);
            if (eventdetail == null) { return null; }

            if (!(eventdetail.StallForSaleInventory > 0)) { return null; }

            //Nếu sự kiện đã bắt đầu, ko được mua
            if (TimeUtils.GetTimeVietNam() >= eventdetail.StartDate) return null;

            //lay event => event co status khac execute => false
            var @event = await _eventRepository.GetByIdAsync(eventdetail.EventID.ToString());
            if (!@event.Status.Equals(EventStatus.EXECUTE.ToString()))
            {
                return null;
            }

            //lay account dang login
            var account = await _userRepository.GetAccountByUsernameAsync(username);

            //validate stall number
            var stall = await _eventStallRepository.GetByEventStallNumber(stallnumber);
            if(stall != null) { return null; }

            //neu tien trong tai khoan ko du => chim cut
            if (account.CreditAmount < eventdetail.StallPrice) {return null;}

            //Nếu sự kiện chưa mở bán stall => false
            if (!@event.Status.Equals(EventStatus.EXECUTE.ToString()))
            {
                return null;
            }

            //Cập nhật số dư tài khoản
            account.CreditAmount -= eventdetail.StallPrice;
            await _userRepository.UpdateAccountAsync(account);

            //cap nhat so luong stall trong eventdetail
            eventdetail.StallForSaleInventory -= 1;
            await _eventDetailRepository.UpdateAsync(eventdetail);

            //Cập nhật doanh thu stall
            @event.StallSaleIncome += eventdetail.StallPrice;
            await _eventRepository.UpdateAsync(@event);

            //cap nhat transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountID = account.Id,
                TransactionType = TransactionType.OUT.ToString(),
                TransactionDate = TimeUtils.GetTimeVietNam(),
                Amount = eventdetail.StallPrice,
                Description = $"FEventopia {username.ToUpper()}: Purchase {@event.EventName} Stall -{eventdetail.StallPrice}.",
                Status = true
            };
            await _transactionRepository.AddAsync(transaction);

            //tao event stall - luu vao database
            var eventstall = new EventStall
            {
                Id = Guid.NewGuid(),
                SponsorID = account.Id,
                EventDetailID = eventdetail.Id,
                TransactionID = transaction.Id,     
                StallNumber = stallnumber,
            };
            await _eventStallRepository.AddAsync(eventstall);

            var result = await _eventStallRepository.GetByIdAsync(eventstall.Id.ToString());
            return _mapper.Map<EventStallModel>(result);
        }

        public async Task<List<EventStallModel>> GetAllEventStall(PageParaModel pageParaModel)
        {
            var eventStalls =  await _eventStallRepository.GetAllAsync();
            var result = _mapper.Map<List<EventStallModel>>(eventStalls);
            return PageModel<EventStallModel>.ToPagedList(result, pageParaModel.PageNumber, pageParaModel.PageSize);
        }

        public async Task<List<EventStallModel>> GetEventStallBySponsorID(string username, PageParaModel pageParaModel)
        {
            var user = await _userRepository.GetAccountByUsernameAsync(username);
            var eventstalls = await _eventStallRepository.GetBySponsorIDAsync(user.Id);
            var result = _mapper.Map<List<EventStallModel>>(eventstalls);
            return PageModel<EventStallModel>.ToPagedList(result, pageParaModel.PageNumber, pageParaModel.PageSize);
        }

        public async Task<List<EventStallModel>> GetAllByStallNumber(string stallnumber, PageParaModel pageParaModel)
        {
            var eventstalls = await _eventStallRepository.GetByEventStallNumber(stallnumber);
            var result = _mapper.Map<List<EventStallModel>>(eventstalls);
            return PageModel<EventStallModel>.ToPagedList(result, pageParaModel.PageNumber, pageParaModel.PageSize);
        }

        public async Task<EventStallModel> GetEventStallById(string stallid)
        {
            var result = await _eventStallRepository.GetEventStallByIdWithDetail(stallid);
            return _mapper.Map<EventStallModel>(result);
        }
    }
}
