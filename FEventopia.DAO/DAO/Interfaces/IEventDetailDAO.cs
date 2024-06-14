using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO.Interfaces
{
    public interface IEventDetailDAO : IGenericDAO<EventDetail>
    {
        public Task<List<EventDetail>> GetAllEventDetailWithLocation();
        public Task<EventDetail?> GetEventDetailWithLocationById(string id);
    }
}
