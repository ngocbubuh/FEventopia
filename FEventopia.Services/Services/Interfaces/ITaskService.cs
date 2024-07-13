using FEventopia.Services.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface ITaskService
    {
        public Task<PageModel<TaskModel>> GetAllByUsername(string username, PageParaModel pageParaModel);
        public Task<PageModel<TaskModel>> GetAllByAccountId(string staffId, PageParaModel pageParaModel);
        public Task<PageModel<TaskModel>> GetAllByEventDetailId(string eventDetailId, PageParaModel pageParaModel);
        public Task<List<TaskModel>> GetAllByEventDetailId(string eventDetailId);
        public Task<TaskModel> GetById(string taskId);
        public Task<TaskModel> CreateTask(TaskModel taskmodel);
        public Task<bool> UpdateTask(string taskid, TaskModel taskModel);
        public Task<bool> DeleteTask(string taskId);
        public Task<bool> UpdateTaskStatus(string taskid, TaskStatusModel taskmodel);
    }
}
