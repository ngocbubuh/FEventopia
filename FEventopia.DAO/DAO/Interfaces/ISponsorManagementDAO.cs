using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO.Interfaces
{
    public interface ISponsorManagementDAO : IGenericDAO<SponsorManagement>
    {
        public Task<SponsorManagement?> GetSponsorManagementDetailByPrimaryKey(string eventId, string accountId);
        public Task<SponsorManagement?> GetSponsorManagementDetailById(string sponsorManagementId);
        public Task<List<SponsorManagement>> GetAllSponsorManagementDetail();
    }
}
