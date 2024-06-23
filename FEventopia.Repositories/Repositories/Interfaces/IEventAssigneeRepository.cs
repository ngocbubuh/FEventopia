using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface IEventAssigneeRepository : IGenericRepository<EventAssignee>
    {
        public Task<List<EventAssignee>> GetAllByEventDetail(string eventDetailId);
        public Task<EventAssignee> GetByED_AC(string accountid, string eventdetailid);
    }
}
