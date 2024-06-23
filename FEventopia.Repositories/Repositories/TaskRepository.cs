using FEventopia.DAO.DAO;
using FEventopia.DAO.DAO.Interfaces;
using FEventopia.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = FEventopia.DAO.EntityModels.Task;

namespace FEventopia.Repositories.Repositories
{
    public class TaskRepository : GenericRepository<Task>, ITaskRepository
    {
        private readonly IGenericDAO<Task> _taskDAO;

        public TaskRepository(IGenericDAO<Task> genericDAO) : base(genericDAO)
        {
            _taskDAO = genericDAO;
        }

        public async Task<List<Task>> GetAllByAccountId(string staffid)
        {
            var tasks = await _taskDAO.GetAllAsync();
            return tasks.Where(t => t.StaffID.Equals(staffid)).ToList();
        }
    }
}
