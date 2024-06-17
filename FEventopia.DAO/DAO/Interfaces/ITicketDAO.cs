using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO.Interfaces
{
    public interface ITicketDAO : IGenericDAO<Ticket>
    {
        public Task<Ticket?> GetTicketDetail(string id);
        public Task<List<Ticket>> GetAllTicketDetail();
    }
}
