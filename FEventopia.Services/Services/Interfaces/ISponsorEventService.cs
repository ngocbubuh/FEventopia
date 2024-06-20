using FEventopia.Services.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface ISponsorEventService
    {
        public Task<PageModel<SponsorEventModel>> GetAllSponsorEventWithDetailAsync(PageParaModel pageParaModel);
        public Task<PageModel<SponsorEventModel>> GetAllSponsorEventWithDetailCurrentEvent(string eventId, PageParaModel pageParaModel);
        public Task<PageModel<SponsorEventModel>> GetAllSponsorEventWithDetailCurrentUser(string userId, PageParaModel pageParaModel);
        public Task<SponsorEventModel?> GetSponsorEventDetailById(string sponsorEventId);
        public Task<SponsorEventModel> AddSponsorEventAsync(SponsorEventProcessModel sponsorEventProcessModel, string username);
    }
}
