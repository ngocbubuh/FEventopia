using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        public Task<Event?> GetEventWithDetailByIdAsync(string id);
        public Task<Event> GetEventByName(string name);
    }
}
