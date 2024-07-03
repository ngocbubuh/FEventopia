using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public TicketService(ITicketRepository ticketRepository, IMapper mapper, IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, ITransactionRepository transactionRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public async Task<TicketModel> AddTicketAsync(string eventDetailId, string username)
        {
            //Lấy eventDetail => lấy giá vé, trừ số lượng vé
            var eventDetail = await _eventDetailRepository.GetByIdAsync(eventDetailId);
            if (eventDetail == null) { return null; }

            //Nếu sự kiện đã bắt đầu, ko được mua
            if (TimeUtils.GetTimeVietNam() >= eventDetail.StartDate) return null;

            //Lấy event => Lấy event name, cập nhật tiền bán vé của sự kiện
            var @event = await _eventRepository.GetByIdAsync(eventDetail.EventID.ToString());

            //Nếu sự kiện chưa mở bán vé => false
            if (!@event.Status.Equals(EventStatus.EXECUTE.ToString()))
            {
                return null;
            }

            //Lấy account user đang login => Lấy accountId, trừ tiền trong tài khoản
            var account = await _userRepository.GetAccountByUsernameAsync(username);

            //Nếu tiền trong tài khoản ko đủ => ko được đặt tiếp
            if (account.CreditAmount < eventDetail.TicketPrice) { return null; }

            //Nếu số lượng vé ko còn => ko được đặt tiếp
            if (eventDetail.TicketForSaleInventory == 0) { return null; }

            //Cập nhật số dư tài khoản
            account.CreditAmount -= eventDetail.TicketPrice;
            await _userRepository.UpdateAccountAsync(account);

            //Cập nhật số lượng vé
            eventDetail.TicketForSaleInventory -= 1;
            await _eventDetailRepository.UpdateAsync(eventDetail);

            //Cập nhật doanh thu bán vé
            @event.TicketSaleIncome += eventDetail.TicketPrice;
            await _eventRepository.UpdateAsync(@event);

            //Tạo transaction OUT, ghi nhận giao dịch
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountID = account.Id,
                TransactionType = TransactionType.OUT.ToString(),
                TransactionDate = TimeUtils.GetTimeVietNam(),
                Amount = eventDetail.TicketPrice,
                Description = $"FEventopia {username.ToUpper()}: Purchase {@event.EventName} Ticket -{eventDetail.TicketPrice}.",
                Status = true
            };
            await _transactionRepository.AddAsync(transaction);

            //Tạo Ticket, lưu vào database
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                EventDetailID = eventDetail.Id, 
                TransactionID = transaction.Id,
                VisitorID = account.Id,
            };
            await _ticketRepository.AddAsync(ticket);

            var result = await _ticketRepository.GetTicketDetailById(ticket.Id.ToString());
            return _mapper.Map<TicketModel>(result);
        }

        public async Task<bool> CheckInAsync(string ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            ticket.CheckInStatus = true;
            return await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task<PageModel<TicketModel>> GetAllTicketWithDetailAsync(PageParaModel pageParaModel)
        {
            var ticketList = await _ticketRepository.GetAllTicketWithDetail();
            var result = _mapper.Map<List<TicketModel>>(ticketList);
            return PageModel<TicketModel>.ToPagedList(result,
                pageParaModel.PageNumber,
                pageParaModel.PageSize);
        }

        public async Task<PageModel<TicketModel>> GetAllTicketWithDetailCurrentEvent(string eventId, PageParaModel pageParaModel)
        {
            var ticketList = await _ticketRepository.GetAllTicketWithDetailCurrentEvent(eventId);
            ticketList.Sort((t1, t2) => t2.CreatedDate.CompareTo(t1.CreatedDate));
            if (ticketList == null) { return null; }
            var result = _mapper.Map<List<TicketModel>>(ticketList);
            return PageModel<TicketModel>.ToPagedList(result,
                pageParaModel.PageNumber,
                pageParaModel.PageSize);
        }

        public async Task<PageModel<TicketModel>> GetAllTicketWithDetailCurrentUser(string username, PageParaModel pageParaModel)
        {
            var account = await _userRepository.GetAccountByUsernameAsync(username);
            var ticketList = await _ticketRepository.GetAllTicketWithDetailCurrentUser(account.Id);
            ticketList.Sort((t1, t2) => t2.CreatedDate.CompareTo(t1.CreatedDate));
            var result = _mapper.Map<List<TicketModel>>(ticketList);
            return PageModel<TicketModel>.ToPagedList(result,
                pageParaModel.PageNumber,
                pageParaModel.PageSize);
        }

        public async Task<TicketModel?> GetTicketDetailById(string ticketId)
        {
            var result = await _ticketRepository.GetTicketDetailById(ticketId);
            if (result == null) { return null; }
            return _mapper.Map<TicketModel>(result);
        }
    }
}
