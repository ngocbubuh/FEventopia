using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories;
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
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, IMapper mapper, IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _userRepository = userRepository;
        }

        public async Task<TaskModel> CreateTask(TaskModel taskmodel)
        {
            var eventdetail = await _eventDetailRepository.GetByIdAsync(taskmodel.EventDetailId.ToString());
            if (eventdetail == null) { return null; }

            //Nếu sự kiện đã tới giai đoạn EXECUTE trở đi => Hủy
            var @event = await _eventRepository.GetByIdAsync(eventdetail.EventID.ToString());
            if (!@event.Status.Equals(EventStatus.PREPARATION.ToString())) return null;

            var task = _mapper.Map<DAO.EntityModels.Task>(taskmodel);
            task.Status = Enum.TaskStatus.TODO.ToString();
            var result = await _taskRepository.AddAsync(task);
            return _mapper.Map<TaskModel>(result);
        }   

        public async Task<bool> DeleteTask(string taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            var eventdetail = await _eventDetailRepository.GetByIdAsync(task.EventDetailID.ToString());
            if (eventdetail == null) { return false; }

            //Nếu sự kiện đã tới giai đoạn EXECUTE trở đi => Hủy
            var @event = await _eventRepository.GetByIdAsync(eventdetail.EventID.ToString());
            if (@event.Status.Equals(EventStatus.EXECUTE.ToString()) || @event.Status.Equals(EventStatus.POST.ToString())) return false;

            return await _taskRepository.DeleteAsync(task);          
        }

        public async Task<PageModel<TaskModel>> GetAllByAccountId(string staffId, PageParaModel pageParaModel)
        {
            var tasks = await _taskRepository.GetAllByAccountId(staffId);
            var result = _mapper.Map<List<TaskModel>>(tasks);
            return PageModel<TaskModel>.ToPagedList(result, pageParaModel.PageNumber, pageParaModel.PageSize);
        }

        public async Task<PageModel<TaskModel>> GetAllByEventDetailId(string eventDetailId, PageParaModel pageParaModel)
        {
            var tasks = await _taskRepository.GetAllByEventDetailId(eventDetailId);
            var result = _mapper.Map<List<TaskModel>>(tasks);
            return PageModel<TaskModel>.ToPagedList(result, pageParaModel.PageNumber, pageParaModel.PageSize);
        }

        public async Task<List<TaskModel>> GetAllByEventDetailId(string eventDetailId)
        {
            var tasks = await _taskRepository.GetAllByEventDetailId(eventDetailId);
            return _mapper.Map<List<TaskModel>>(tasks);
        }

        public async Task<PageModel<TaskModel>> GetAllByUsername(string username, PageParaModel pageParaModel)
        {
            var account = await _userRepository.GetAccountByUsernameAsync(username);
            var tasks = await _taskRepository.GetAllByAccountId(account.Id);
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
            if (task == null)
            {
                return false;
            }

            var eventdetail = await _eventDetailRepository.GetByIdAsync(task.EventDetailID.ToString());
            if (eventdetail == null) { return false; }

            //Nếu sự kiện đã tới giai đoạn EXECUTE trở đi => Hủy
            var @event = await _eventRepository.GetByIdAsync(eventdetail.EventID.ToString());
            if (@event.Status.Equals(EventStatus.EXECUTE.ToString()) || @event.Status.Equals(EventStatus.POST.ToString())) return false;

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

            //Nếu sự kiện đã done, ko cho cập nhật nữa
            if (task.Status.Equals(Enum.TaskStatus.DONE.ToString()))
            {
                return false;
            }

            //Nếu sự kiện cập nhật sang done, thì phải có Actual Cost Input
            if (taskmodel.Status.Equals(Enum.TaskStatus.DONE.ToString()))
            {
                if (!taskmodel.ActualCost.HasValue) { return false; }
            }

            //var result = _mapper.Map(taskmodel, task);
            task.Status = taskmodel.Status.ToString();
            return await _taskRepository.UpdateAsync(task);
        }
    }
}