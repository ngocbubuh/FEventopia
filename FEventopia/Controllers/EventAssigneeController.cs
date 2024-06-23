using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace FEventopia.Controllers.Controllers
{
    [Route("eventassignee/")]
    [ApiController]
    public class EventAssigneeController : ControllerBase
    {
        private readonly IEventAssigneeService _eventAssigneeService;

        public EventAssigneeController(IEventAssigneeService eventAssigneeService)
        {
            this._eventAssigneeService = eventAssigneeService;
        }

        [HttpGet("GetAllByEventDetailId")]
        [Authorize(Roles = "ADMIN,EVENTOPERATOR")]
        public async Task<IActionResult> GetAllByEventDetailId(string eventdetailid, [FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var results = await _eventAssigneeService.GetAllByEventDetailId(eventdetailid, pageParaModel);
                return Ok(results);
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
        public async Task<IActionResult> AddEventStall(string accountId, string eventDetailId)
        {
            try
            {
                var result = await _eventAssigneeService.AddEventAssignee(accountId, eventDetailId);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpDelete]
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
