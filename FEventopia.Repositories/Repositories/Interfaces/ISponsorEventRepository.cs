using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface ISponsorEventRepository : IGenericRepository<SponsorEvent>
    {
        public Task<List<SponsorEvent>> GetAllSponsorEvent();
        public Task<List<SponsorEvent>> GetAllSponsorEventWithDetailCurrentEvent(string eventId);
        public Task<List<SponsorEvent>> GetAllSponsorEventWithDetailCurrentUser(string userId);
        public Task<SponsorEvent?> GetSponsorEventDetailById(string sponsorEventId);
    }
}
