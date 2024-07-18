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
    public class SponsorEventRepository : GenericRepository<SponsorEvent>, ISponsorEventRepository
    {
        private readonly ISponsorEventDAO _sponsorEventDAO;
        public SponsorEventRepository(IGenericDAO<SponsorEvent> genericDAO, ISponsorEventDAO eventDAO) : base(genericDAO)
        {
            _sponsorEventDAO = eventDAO;
        }

        public async Task<List<SponsorEvent>> GetAllSponsorEvent()
        {
            return await _sponsorEventDAO.GetAllSponsorEventDetail();
        }

        public async Task<List<SponsorEvent>> GetAllSponsorEventWithDetailCurrentEvent(string eventId)
        {
            var result = await _sponsorEventDAO.GetAllSponsorEventDetail();
            return result.Where(se => eventId.ToLower().Equals(se.EventID.ToString().ToLower())).OrderByDescending(se => se.CreatedDate).ToList();
        }

        public async Task<List<SponsorEvent>> GetAllSponsorEventWithDetailCurrentUser(string userId)
        {
            var result = await _sponsorEventDAO.GetAllSponsorEventDetail();
            return result.Where(se => userId.ToLower().Equals(se.SponsorID.ToString().ToLower())).OrderByDescending(se => se.CreatedDate).ToList();
        }

        public async Task<SponsorEvent?> GetSponsorEventDetailById(string sponsorEventId)
        {
            return await _sponsorEventDAO.GetSponsorEventDetail(sponsorEventId);
        }
    }
}
