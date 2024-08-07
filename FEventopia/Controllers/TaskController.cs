﻿using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using FEventopia.Services.Services;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IAuthenService _authenService;
        public TaskController(ITaskService taskService, IAuthenService authenService)
        {
            _taskService = taskService;
            _authenService = authenService;
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

        [HttpGet("GetAllByEventDetailId")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR,CHECKINGSTAFF")]
        public async Task<IActionResult> GetAllByEventDetailId(string eventDetailId)
        {
            try
            {
                var result = await _taskService.GetAllByEventDetailId(eventDetailId);
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpGet("GetAllByStaffUsername")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR,CHECKINGSTAFF")]
        public async Task<IActionResult> GetAllByStaffUsername(string username,[FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _taskService.GetAllByUsername(username, pageParaModel);
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("GetAllByCurrentUser")]
        [Authorize(Roles = "CHECKINGSTAFF")]
        public async Task<IActionResult> GetAllByCurrentUser([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _taskService.GetAllByUsername(username, pageParaModel);
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("AddTask")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR")]
        public async Task<IActionResult> AddTask(TaskProcessModel model)
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
        public async Task<IActionResult> UpdateTask([Required] string id, TaskProcessModel model)
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
        public async Task<IActionResult> UpdateTaskStatus([Required] string id,[FromQuery] TaskStatusModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
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


        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DeleteFeedback([Required] string taskId)
        {
            try
            {
                var result = await _taskService.DeleteTask(taskId);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Delete task Successfully!"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Delete task Failed!"
                    };
                    return BadRequest(response);
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
