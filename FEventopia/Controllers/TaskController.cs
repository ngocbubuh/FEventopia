using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using FEventopia.Services.Services;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using TaskStatus = FEventopia.Services.Enum.TaskStatus;

namespace FEventopia.Controllers.Controllers
{
    [Route("task")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("GetById")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR,CHECKINGSTAFF")]
        public async Task<IActionResult> GetByID(string id)
        {
            try
            {
                var result = await _taskService.GetById(id);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("GetAllByStaffId")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR,CHECKINGSTAFF")]
        public async Task<IActionResult> GetAllByStaffId(string staffId,[FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _taskService.GetAllByAccountId(staffId, pageParaModel);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("AddTask")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR")]
        public async Task<IActionResult> AddTask(TaskModel model)
        {
            try
            {
                var result = await _taskService.CreateTask(model);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpPut("UpdateTask")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR")]
        public async Task<IActionResult> UpdateTask([Required] string id, TaskModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskService.UpdateTask(id, model);
                    if (result)
                    {
                        var response = new ResponseModel
                        {
                            Status = result,
                            Message = "Update Task Successfully!"
                        };
                        return Ok(response);
                    }
                    else
                    {
                        var response = new ResponseModel
                        {
                            Status = result,
                            Message = "Update Task Failed!"
                        };
                        return BadRequest(response);
                    }
                }
                else
                {
                    return ValidationProblem(ModelState);
                }

            }
            catch
            {
                throw;
            }
        }

        [HttpPut("UpdateTaskStatus")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR,CHECKINGSTAFF")]
        public async Task<IActionResult> UpdateTaskStatus([Required] string id, TaskStatusModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //CHECK kiểu đúng lỏ luôn ba :)
                        var result = await _taskService.UpdateTaskStatus(id, model);
                        if (result)
                        {
                            var response = new ResponseModel
                            {
                                Status = result,
                                Message = "Update Task Status Successfully!"
                            };
                            return Ok(response);
                        }
                        else
                        {
                            var response = new ResponseModel
                            {
                                Status = result,
                                Message = "Update Task Status Failed!"
                            };
                            return BadRequest(response);
                        }                    
                }
                else
                {
                    return ValidationProblem(ModelState);
                }

            }
            catch
            {
                throw;
            }
        }


        //[HttpDelete("DeleteTask")]
        //public async Task<IActionResult> DeleteFeedback([Required] string id)
        //{
        //    try
        //    {
        //        var result = await _taskService.DeleteTask(id);
        //        if (result)
        //        {
        //            var response = new ResponseModel
        //            {
        //                Status = result,
        //                Message = "Delete task Successfully!"
        //            };
        //            return Ok(response);
        //        }
        //        else
        //        {
        //            var response = new ResponseModel
        //            {
        //                Status = result,
        //                Message = "Delete task Failed!"
        //            };
        //            return BadRequest(response);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

    }
}
