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
    public class SponsorManagementRepository : GenericRepository<SponsorManagement>, ISponsorManagementRepository
    {
        private readonly ISponsorManagementDAO _sponsorManagementDAO;
        public SponsorManagementRepository(IGenericDAO<SponsorManagement> genericDAO, ISponsorManagementDAO sponsorManagementDAO) : base(genericDAO)
        {
            _sponsorManagementDAO = sponsorManagementDAO;
        }

        public async Task<List<SponsorManagement>> GetAllSponsorManagementDetail()
        {
            return await _sponsorManagementDAO.GetAllSponsorManagementDetail();
        }

        public async Task<List<SponsorManagement>> GetAllSponsorManagementWithDetailCurrentEvent(string eventId)
        {
            var result = await _sponsorManagementDAO.GetAllSponsorManagementDetail();
            return result.Where(se => eventId.ToLower().Equals(se.EventId.ToString().ToLower())).ToList();
        }

        public async Task<List<SponsorManagement>> GetAllSponsorManagementWithDetailCurrentUser(string userId)
        {
            var result = await _sponsorManagementDAO.GetAllSponsorManagementDetail();
            return result.Where(se => userId.ToLower().Equals(se.SponsorId.ToString().ToLower())).ToList();
        }

        public async Task<SponsorManagement?> GetSponsorManagementDetailById(string id)
        {
            return await _sponsorManagementDAO.GetSponsorManagementDetailById(id);
        }

        public async Task<SponsorManagement?> GetSponsorManagementDetailByPrimaryKey(string eventId, string accountId)
        {
            return await _sponsorManagementDAO.GetSponsorManagementDetailByPrimaryKey(eventId, accountId);
        }
    }
}
