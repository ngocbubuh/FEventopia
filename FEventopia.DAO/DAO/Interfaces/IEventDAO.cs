using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO.Interfaces
{
    public interface IEventDAO : IGenericDAO<Event>
    {
        public Task<Event?> GetEventWithDetailByIdAsync(string id);
    }
}
