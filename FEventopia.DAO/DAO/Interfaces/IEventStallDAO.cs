using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO.Interfaces
{
    public interface IEventStallDAO : IGenericDAO<EventStall>
    {
        public Task<List<EventStall>> GetAllEventStallWithDetail();
        public Task<EventStall?> GetEventStallWithDetailById(string id);
    }
}
