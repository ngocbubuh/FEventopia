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
    public class TicketDAO : GenericDAO<Ticket>, ITicketDAO
    {
        private readonly FEventopiaDbContext _dbContext;
        public TicketDAO(FEventopiaDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Ticket>> GetAllTicketDetail()
        {
            return await _dbContext.Ticket.Include(t => t.EventDetail).ThenInclude(ed => ed.Location).Include(t => t.EventDetail).ThenInclude(ed => ed.Event).Include(t => t.Transaction)
                  .Where(t => !t.DeleteFlag).ToListAsync();
        }

        public async Task<Ticket?> GetTicketDetail(string id)
        {
            return await _dbContext.Ticket.Include(t => t.EventDetail).ThenInclude(ed => ed.Location).Include(t => t.EventDetail).ThenInclude(ed => ed.Event).Include(t => t.Transaction)
                  .FirstOrDefaultAsync(t => t.Id.ToString().ToLower().Equals(id.ToLower()) && !t.DeleteFlag);
        }
    }
}
