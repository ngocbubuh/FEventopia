using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface ITicketService
    {
        public Task<PageModel<TicketModel>> GetAllTicketWithDetailAsync(PageParaModel pageParaModel);
        public Task<PageModel<TicketModel>> GetAllTicketWithDetailCurrentEvent(string eventId, PageParaModel pageParaModel);
        public Task<PageModel<TicketModel>> GetAllTicketWithDetailCurrentUser(string userId, PageParaModel pageParaModel);
        public Task<TicketModel?> GetTicketDetailById(string ticketId);
        public Task<TicketModel> AddTicketAsync(string eventDetailId, string username);
        public Task<bool> CheckInAsync(string ticketId);
    }
}
