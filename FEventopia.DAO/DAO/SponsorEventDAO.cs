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
    public class SponsorEventDAO : GenericDAO<SponsorEvent>, ISponsorEventDAO
    {
        private readonly FEventopiaDbContext _dbContext;
        public SponsorEventDAO(FEventopiaDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SponsorEvent>> GetAllSponsorEventDetail()
        {
            return await _dbContext.SponsorEvent.Include(se => se.Event).Include(se => se.Transaction)
                .ToListAsync();
        }

        public async Task<SponsorEvent?> GetSponsorEventDetail(string id)
        {
            return await _dbContext.SponsorEvent.Include(se => se.Event).Include(se => se.Transaction).
                FirstOrDefaultAsync(t => t.Id.ToString().ToLower().Equals(id.ToLower()) && !t.DeleteFlag);
        }
    }
}
