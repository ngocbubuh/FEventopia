using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface IEventStallRepository : IGenericRepository<EventStall>
    {
        public Task<List<EventStall>> GetBySponsorIDAsync(string sponsorID);
        public Task<List<EventStall>> GetByEventStallNumber(string eventStallNumber);
    }
}
