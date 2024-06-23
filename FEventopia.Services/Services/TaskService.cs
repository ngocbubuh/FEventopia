using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using FEventopia.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FEventopia.Services.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<TaskModel> CreateTask(TaskModel taskmodel)
        {
            var task = _mapper.Map<DAO.EntityModels.Task>(taskmodel);
            task.Status = Enum.TaskStatus.TODO.ToString();
            var result = await _taskRepository.AddAsync(task);
            return _mapper.Map<TaskModel>(result);
        }   

        public async Task<bool> DeleteTask(string taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            return await _taskRepository.DeleteAsync(task);          
        }

        public async Task<PageModel<TaskModel>> GetAllByAccountId(string staffId, PageParaModel pageParaModel)
        {
            var tasks = await _taskRepository.GetAllByAccountId(staffId);
            var result = _mapper.Map<List<TaskModel>>(tasks);
            return PageModel<TaskModel>.ToPagedList(result, pageParaModel.PageNumber, pageParaModel.PageSize);
        }

        public async Task<TaskModel> GetById(string taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            return _mapper.Map<TaskModel>(task);
        }

        public async Task<bool> UpdateTask(string taskid, TaskModel taskModel)
        {
            var task = await _taskRepository.GetByIdAsync(taskid);
            if(task == null)
            { 
                return false;
            }
            var result = _mapper.Map(taskModel,task);
            return await _taskRepository.UpdateAsync(result);
        }

        public async Task<bool> UpdateTaskStatus(string taskid, TaskStatusModel taskmodel)
        {
            var task = await _taskRepository.GetByIdAsync(taskid);
            if (task == null)
            {
                return false;
            }
            //var result = _mapper.Map(taskmodel, task);
            task.Status = taskmodel.Status.ToString();
            return await _taskRepository.UpdateAsync(task);
        }
    }
}
