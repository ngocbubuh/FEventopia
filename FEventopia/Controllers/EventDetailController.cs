using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FEventopia.Controllers.Controllers
{
    [Route("event-details/")]
    [ApiController]
    public class EventDetailController : ControllerBase
    {
        private readonly IEventDetailService _eventDetailService;

        public EventDetailController(IEventDetailService eventService)
        {
            _eventDetailService = eventService;
        }

        [HttpGet("GetEventDetailByEventId")]
        public async Task<IActionResult> GetEventDetailByEventIdAsync(string eventId,[FromQuery] PageParaModel pageParaModel) 
        {
            try
            {
                var result = await _eventDetailService.GetAllEventDetailByEventIdAsync(eventId, pageParaModel);
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

        [HttpGet("GetEventDetailById")]
        public async Task<IActionResult> GetEventDetailByIdAsync(string id)
        {
            try
            {
                var result = await _eventDetailService.GetEventDetailByIdAsync(id);
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpGet("GetEventDetailById-Operator")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR")]
        public async Task<IActionResult> GetEventDetailByIdOperatorAsync(string id)
        {
            try
            {
                var result = await _eventDetailService.GetEventDetailOperatorByIdAsync(id);
                return Ok(result);
            } catch 
            {
                throw;
            }
        }

        [HttpPost("CreateEventDetail")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR")]
        public async Task<IActionResult> AddEventDetailAsync(EventDetailProcessModel processModel)
        {
            try
            {
                if (processModel.IsValidDate())
                {
                    var result = await _eventDetailService.AddEventDetailAsync(processModel);
                    return Ok(result);
                } else
                {
                    var repsonse = new ResponseModel { Status = false, Message = "Invalid Event Duration!" };
                    return BadRequest(repsonse);
                }
            } catch
            {
                throw;
            }
        }

        [HttpPut("UpdateEventDetail")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR")]
        public async Task<IActionResult> UpdateEventDetailAsync(string id, EventDetailProcessModel processModel)
        {
            try
            {
                var result = await _eventDetailService.UpdateEventDetailAsync(id, processModel);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest();
            } catch
            {
                throw;
            }
        }

        [HttpDelete("DeleteEventDetail")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR")]
        public async Task<IActionResult> DeleteEventDetailAsync(string id)
        {
            try
            {
                var result = await _eventDetailService.DeleteEventDetailAsync(id);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Delete successfully!"
                    };
                    return Ok(response);
                } else
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Delete failed!"
                    };
                    return BadRequest(response);
                } 

            } catch
            {
                throw;
            }
        }

    }
}
