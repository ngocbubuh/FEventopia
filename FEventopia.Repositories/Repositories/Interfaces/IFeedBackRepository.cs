using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface IFeedBackRepository : IGenericRepository<Feedback>
    {
        public Task<List<Feedback>> GetAllByEventDetail(string eventDetailId);
    }
}
