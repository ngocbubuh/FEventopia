using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories
{
    public class EventDetailRepository : GenericRepository<EventDetail>, IEventDetailRepository
    {
        private readonly IEventDetailDAO _eventDetailDAO;
        public EventDetailRepository(IGenericDAO<EventDetail> genericDAO, IEventDetailDAO eventDetailDAO) : base(genericDAO)
        {
            _eventDetailDAO = eventDetailDAO;
        }

        public async Task<List<EventDetail>> GetAllEventDetailWithLocationById(string id)
        {
            var result = await _eventDetailDAO.GetAllEventDetailWithLocation();
            return result.Where(ed => ed.EventID.ToString().ToLower().Equals(id.ToLower())).ToList();
        }

        public async Task<EventDetail?> GetEventDetailWithLocationById(string id)
        {
            return await _eventDetailDAO.GetEventDetailWithLocationById(id);
        }
    }
}
