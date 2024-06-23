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

        public EventStallController(IEventStallService eventStallService)
        {
            _eventStallService = eventStallService;
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

        [HttpGet("GetEventBySponsorID")]
        public async Task<IActionResult>GetBySponsorID(string id, [FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _eventStallService.GetEventStallBySponsorID(id, pageParaModel);
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
