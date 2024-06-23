using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface IEventDetailRepository : IGenericRepository<EventDetail>
    {
        public Task<List<EventDetail>> GetAllEventDetailWithLocationById(string id);
        public Task<EventDetail?> GetEventDetailWithLocationById(string id);
        public Task<List<EventDetail>> GetAllEventDetailAtLocation(string locationId, DateTime startDate, DateTime endDate);
    }
}
