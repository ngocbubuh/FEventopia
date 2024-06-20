using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface ISponsorManagementRepository : IGenericRepository<SponsorManagement>
    {
        public Task<List<SponsorManagement>> GetAllSponsorManagementDetail();
        public Task<List<SponsorManagement>> GetAllSponsorManagementWithDetailCurrentEvent(string eventId);
        public Task<List<SponsorManagement>> GetAllSponsorManagementWithDetailCurrentUser(string userId);
        public Task<SponsorManagement?> GetSponsorManagementDetailByPrimaryKey(string eventId, string accountId);
        public Task<SponsorManagement?> GetSponsorManagementDetailById(string id);
    }
}
