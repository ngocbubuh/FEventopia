using FEventopia.Services.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface ISponsorManagementService
    {
        public Task<PageModel<SponsorManagementModel>> GetAllSponsorManagementWithDetailAsync(PageParaModel pageParaModel);
        public Task<PageModel<SponsorManagementModel>> GetAllSponsorManagementWithDetailCurrentEvent(string eventId, PageParaModel pageParaModel);
        public Task<PageModel<SponsorManagementModel>> GetAllSponsorManagementWithDetailCurrentUser(string userId, PageParaModel pageParaModel);
        public Task<SponsorManagementModel?> GetSponsorManagementDetailById(string sponsorManagementId);
        public Task<SponsorManagementModel> GetSponsorManagementDetailByPrimaryKey(string eventId, string userId);
        public Task<SponsorManagementModel> AddSponsorManagementAsync(Guid eventId, double amount, string username);
    }
}
