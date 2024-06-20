using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.DbContext;
using FEventopia.DAO.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO
{
    public class SponsorManagementDAO : GenericDAO<SponsorManagement>, ISponsorManagementDAO
    {
        private readonly FEventopiaDbContext _context;
        public SponsorManagementDAO(FEventopiaDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<SponsorManagement>> GetAllSponsorManagementDetail()
        {
            return await _context.sponsorManagement.Include(sm => sm.Event).ToListAsync();
        }

        public async Task<SponsorManagement?> GetSponsorManagementDetailByPrimaryKey(string eventId, string accountId)
        {
            return await _context.sponsorManagement.Include(sm => sm.Event)
                .FirstOrDefaultAsync(t => accountId.ToLower().Equals(t.SponsorId.ToLower()) && eventId.ToLower().Equals(t.EventId.ToString().ToLower()));
        }

        public async Task<SponsorManagement?> GetSponsorManagementDetailById(string sponsorManagementId)
        {
            return await _context.sponsorManagement.Include(sm => sm.Event)
                .FirstOrDefaultAsync(t => sponsorManagementId.ToLower().Equals(t.Id.ToString().ToLower()));
        }
    }
}
