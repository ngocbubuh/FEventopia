using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.DbContext;
using FEventopia.DAO.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO
{
    public class EventDAO : GenericDAO<Event>, IEventDAO
    {
        private readonly FEventopiaDbContext _dbContext;
        public EventDAO(FEventopiaDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Event?> GetEventWithDetailByIdAsync(string id)
        {
            return await _dbContext.Event
            .Include(e => e.EventDetail.Where(ed => !ed.DeleteFlag && ed.EventID.ToString().ToLower().Equals(id.ToLower())))
            .ThenInclude(l => l.Location)
            .FirstOrDefaultAsync(e => id.ToLower().Equals(e.Id.ToString().ToLower()) && !e.DeleteFlag);
        }
    }
}
