using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Net.WebSockets;

namespace FEventopia.Controllers.Controllers
{
    [Route("event/")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("GetAllEvent")]
        [Authorize(Roles = "SPONSOR, CHECKINGSTAFF, EVENTOPERATOR, ADMIN")]
        public async Task<IActionResult> GetAllEventAsync([FromQuery] PageParaModel pageParaModel, string? category, string? status) 
        {
            try
            {
                var result = await _eventService.GetAllEventAsync(pageParaModel, category, status);
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

        [HttpGet("GetEventById")]
        public async Task<IActionResult> GetEventById(string id) 
        {
            try
            {
                var result = await _eventService.GetEventByIdAsync(id);
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpGet("GetEventById-Operator")]
        [Authorize(Roles = "EVENTOPERATOR, ADMIN")]
        public async Task<IActionResult> GetEventByIdOperatorAsync(string id) 
        {
            try
            {
                var result = await _eventService.GetEventByIdOperatorAsync(id);
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpPost("CreateEvent")]
        [Authorize(Roles = "EVENTOPERATOR, ADMIN")]
        public async Task<IActionResult> AddEventAsync(EventProcessModel model, [Required(ErrorMessage = "Event Category required!")] EventCategory category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Category = category.ToString();
                    var result = await _eventService.AddEventAsync(model);
                    return Ok(result);
                } else
                {
                    return ValidationProblem(ModelState);
                }
            } catch
            {
                throw;
            }
        }

        [HttpPut("UpdateEvent")]
        [Authorize(Roles = "EVENTOPERATOR, ADMIN")]
        public async Task<IActionResult> UpdateEventAsync(string id, EventProcessModel model, [Required(ErrorMessage = "Event Category required!")] EventCategory category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Category = category.ToString();
                    var result = await _eventService.UpdateEventAsync(id, model);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                    return BadRequest();
                } else
                {
                    return ValidationProblem(ModelState);
                }
            } catch
            {
                throw;
            }
        }

        [HttpPut("UpdateEventNextPhase")]
        [Authorize(Roles = "EVENTOPERATOR, ADMIN")]
        public async Task<IActionResult> UpdateEventNextPhase(string id)
        {
            try
            {
                var result = await _eventService.UpdateEventNextPhaseAsync(id);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Move to next phase successfully!"
                    };
                    return Ok(response);
                } else
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Move to next phase failed!"
                    };
                    return BadRequest(response);
                }
            } catch
            {
                throw;
            }
        }

        [HttpDelete("DeleteEvent")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR")]
        public async Task<IActionResult> DeleteEventAsync(string id)
        {
            try
            {
                var result = await _eventService.DeleteEventAsync(id);
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

        [HttpGet("GetEventByName")]
        public async Task<IActionResult> GetEventByName(string name)
        {
            try
            {
                var result = await _eventService.GetEventByName(name);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("SearchEventByName")]
        public async Task<IActionResult> SearchEventByName(string name)
        {
            try
            {
                var results = await _eventService.SearchEventByName(name);
                return Ok(results);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("GetAllEventForVisitor")]
        public async Task<IActionResult> GetAllEventForVisitor([FromQuery] PageParaModel pageParaModel, string? category, string? status)
        {
            try
            {
                var result = await _eventService.GetAllEventForVisitorAsync(pageParaModel, category, status);
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
    }
}
