using System.Collections.Generic;
using System.Linq;
using System.Text;
using FEventopia.DAO.EntityModels;
using Task = FEventopia.DAO.EntityModels.Task;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface ITaskRepository : IGenericRepository<Task>
    {
        public Task<List<Task>>GetAllByAccountId(string staffid);
        public Task<List<Task>> GetAllByEventDetailId(string eventDetailId);

        //public Task<bool> UpdateTaskStatus(string taskid,string status);
    }
}
