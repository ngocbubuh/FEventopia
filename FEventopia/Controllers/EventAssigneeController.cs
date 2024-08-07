﻿using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FEventopia.Controllers.Controllers
{
    [Route("eventassignee/")]
    [ApiController]
    public class EventAssigneeController : ControllerBase
    {
        private readonly IEventAssigneeService _eventAssigneeService;
        private readonly IAuthenService _authenService;

        public EventAssigneeController(IEventAssigneeService eventAssigneeService, IAuthenService authenService)
        {
            this._eventAssigneeService = eventAssigneeService;
            this._authenService = authenService;
        }

        [HttpGet("GetAllByCurrentEvent")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR, CHECKINGSTAFF")]
        public async Task<IActionResult> GetAllByEventDetailId(string eventdetailid)
        {
            try
            {
                var result = await _eventAssigneeService.GetAllByEventDetailId(eventdetailid);
                return Ok(result);
            }
            catch
            { 
                throw;
            }
        }

        [HttpGet("GetAllAssigneeDetailCurrentUser")]
        [Authorize(Roles = "CHECKINGSTAFF")]
        public async Task<IActionResult> GetAllDetailCurrentUser([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _eventAssigneeService.GetAllAsigneeDetailByUsername(username, pageParaModel);
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
            } catch
            {
                throw;
            }
        }

        [HttpGet("GetAllByCurrentUser")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR, CHECKINGSTAFF")]
        public async Task<IActionResult> GetAllByAcocuntUsername([FromQuery] PageParaModel pagePara)
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _eventAssigneeService.GetAllByAccountUsername(username, pagePara);
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
            } catch
            {
                throw;
            }
        }

        [HttpGet("GetAllByAccountUsername")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR")]
        public async Task<IActionResult> GetAllByAccountId([FromQuery] PageParaModel pagePara, string username)
        {
            try
            {
                var result = await _eventAssigneeService.GetAllByAccountUsername(username, pagePara);
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

        //[HttpGet("GetById")]
        //[Authorize(Roles = "ADMIN,EVENTOPERATOR")]
        //public async Task<IActionResult> GetById(string id, [FromQuery] PageParaModel pageParaModel)
        //{
        //    try
        //    {
        //        var results = await _eventAssigneeService.GetById(id, pageParaModel);
        //        return Ok(results);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        [HttpPost("AddEventAssignee")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR")]
        public async Task<IActionResult> AddEventAssignee(string accountId, string eventDetailId)
        {
            try
            {
                var result = await _eventAssigneeService.AddEventAssignee(accountId, eventDetailId);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = true,
                        Message = "Add Event Assignee Successfully!"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel
                    {
                        Status = false,
                        Message = "Add Event Assignee Failed!"
                    };
                    return BadRequest(response);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("AddRangeEventAssignee")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR")]
        public async Task<IActionResult> AddRangeEventAssignee(List<string> accountId, string eventDetailId)
        {
            try
            {
                var result = await _eventAssigneeService.AddRangeEventAssignee(accountId, eventDetailId);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = true,
                        Message = "Add Range Event Assignee Successfully!"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel
                    {
                        Status = false,
                        Message = "Add Range Event Assignee Failed!"
                    };
                    return BadRequest(response);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR")]
        public async Task<IActionResult> DeleteEventAssignee(string eventDetailId, string accountId)
        {
            try
            {
                var result = await _eventAssigneeService.DeleteEventAssignee(eventDetailId,accountId);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Delete Event Assigne Successfully!"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Delete Event Assigne Failed!"
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
