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
    public class EventAssigneeDAO : GenericDAO<EventAssignee>, IEventAssigneeDAO
    {
        private readonly FEventopiaDbContext _dbContext;
        public EventAssigneeDAO(FEventopiaDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EventAssignee>> GetAllEventAssigneeDetailByAccountId(string accountId)
        {
            return await _dbContext.EventAssignee.Include(es => es.EventDetail).ThenInclude(ed => ed.Event).Where(es => es.AccountId.ToLower().Equals(accountId.ToLower())).ToListAsync();
        }
    }
}
