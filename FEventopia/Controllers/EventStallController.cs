using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FEventopia.Controllers.Controllers
{
    //[Route("api/[controller]")]
    [Route("eventstall/")]
    [ApiController]
    public class EventStallController : ControllerBase
    {
        private readonly IEventStallService _eventStallService;
        private readonly IAuthenService _authenService;

        public EventStallController(IEventStallService eventStallService, IAuthenService authenService)
        {
            _eventStallService = eventStallService;
            _authenService = authenService;
        }

        [HttpGet("GetAllEventStall")]
        public async Task<IActionResult> GetAll([FromQuery] PageParaModel pageParaModel) 
        {
            try
            {
                var result = await _eventStallService.GetAllEventStall(pageParaModel);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("GetEventStallBySponsorUsername")]
        public async Task<IActionResult>GetBySponsorUsername(string username, [FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _eventStallService.GetEventStallBySponsorID(username, pageParaModel);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("GetEventStallCurrentUser")]
        public async Task<IActionResult> GetByCurrentUser([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _eventStallService.GetEventStallBySponsorID(username, pageParaModel);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("GetByEventStallNumber")]
        public async Task<IActionResult> Get(string stallnumber, [FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _eventStallService.GetAllByStallNumber(stallnumber, pageParaModel);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("GetStallById")]
        public async Task<IActionResult> GetStallById(string id)
        {
            try
            {
                var result = await _eventStallService.GetEventStallById(id);
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpPost("AddEventStall")]
        public async Task<IActionResult> AddEventStall(string eventDetailId, string username, string stallnumber)
        {
            try
            {
                var result = await _eventStallService.CreateEventStall(eventDetailId, username, stallnumber);
                return Ok(result);
            }
             catch
            {
                throw;
            }
        }
    }
}
