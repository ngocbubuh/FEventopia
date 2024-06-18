using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly ITicketDAO _ticketDAO;
        public TicketRepository(IGenericDAO<Ticket> genericDAO, ITicketDAO ticketDAO) : base(genericDAO)
        {
            _ticketDAO = ticketDAO;
        }

        public async Task<List<Ticket>> GetAllTicketWithDetail()
        {
            return await _ticketDAO.GetAllTicketDetail();
        }

        public async Task<List<Ticket>> GetAllTicketWithDetailCurrentEvent(string eventId)
        {
            var result = await _ticketDAO.GetAllTicketDetail();
            return result.Where(t => t.EventDetailID.ToString().ToLower().Equals(eventId.ToLower())).ToList();
        }

        public async Task<List<Ticket>> GetAllTicketWithDetailCurrentUser(string userId)
        {
            var result = await _ticketDAO.GetAllTicketDetail();
            return result.Where(t => t.VisitorID.ToString().ToLower().Equals(userId.ToLower())).ToList();
        }

        public async Task<Ticket?> GetTicketDetailById(string ticketId)
        {
            return await _ticketDAO.GetTicketDetail(ticketId);
        }
    }
}
