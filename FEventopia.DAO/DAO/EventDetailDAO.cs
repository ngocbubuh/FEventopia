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
    public class EventDetailDAO : GenericDAO<EventDetail>, IEventDetailDAO
    {
        private readonly FEventopiaDbContext _dbContext;
        public EventDetailDAO(FEventopiaDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EventDetail>> GetAllEventDetailWithLocation()
        {
            return await _dbContext.EventDetail.Include(l => l.Location).ToListAsync();
        }

        public async Task<EventDetail?> GetEventDetailWithLocationById(string id)
        {
            return await _dbContext.EventDetail.Include(l => l.Location)
                .FirstOrDefaultAsync(e => id.ToLower().Equals(e.Id.ToString().ToLower()) && !e.DeleteFlag);
        }
    }
}
