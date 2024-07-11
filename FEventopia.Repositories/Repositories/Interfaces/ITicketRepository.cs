using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        public Task<List<Ticket>> GetAllTicketWithDetail();
        public Task<List<Ticket>> GetAllTicketWithDetailCurrentEvent(string eventId);
        public Task<List<Ticket>> GetAllTicketWithDetailCurrentUser(string userId);
        public Task<Ticket?> GetTicketDetailById(string ticketId);
        public Task<List<Ticket>> GetAllTicketDetailCheckedInCurrentUser(string userId);
    }
}
