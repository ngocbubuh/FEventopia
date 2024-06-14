using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private readonly IEventDAO _eventDAO;
        public EventRepository(IGenericDAO<Event> genericDAO, IEventDAO eventDAO) : base(genericDAO)
        {
            _eventDAO = eventDAO;
        }

        public async Task<Event?> GetEventWithDetailByIdAsync(string id)
        {
            return await _eventDAO.GetEventWithDetailByIdAsync(id);
        }
    }
}
