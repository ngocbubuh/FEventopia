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
    public class EventStallRepository : GenericRepository<EventStall>, IEventStallRepository
    {
        private readonly IEventStallDAO _eventStallDAO;

        public EventStallRepository(IEventStallDAO genericDAO) : base(genericDAO)
        {
            _eventStallDAO = genericDAO;
        }

        public async Task<List<EventStall>> GetByEventStallNumber(string eventStallNumber)
        {
            var eventStalls = await _eventStallDAO.GetAllEventStallWithDetail();
            return eventStalls.Where(e => e.StallNumber.Equals(eventStallNumber)).ToList();
        }

        public async Task<List<EventStall>> GetBySponsorIDAsync(string sponsorID)
        {
            var eventstalls = await _eventStallDAO.GetAllEventStallWithDetail();
            return eventstalls.Where(e => e.SponsorID.Equals(sponsorID)).ToList();
        }

        public async Task<EventStall> GetEventStallByIdWithDetail(string id)
        {
            return await _eventStallDAO.GetEventStallWithDetailById(id);
        }
    }
}
