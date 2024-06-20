using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FEventopia.Controllers.Controllers
{
    [Route("sponsor/")]
    [ApiController]
    public class SponsorEventController : ControllerBase
    {
        private readonly ISponsorEventService _sponsorEventService;
        private readonly IAuthenService _authenService;
        public SponsorEventController(ISponsorEventService sponsorEventService, IAuthenService authenService)
        {
            _sponsorEventService = sponsorEventService;
            _authenService = authenService;
        }

        [HttpGet("GetAllSponsorEvent")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllSponsorEvent([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _sponsorEventService.GetAllSponsorEventWithDetailAsync(pageParaModel);
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

        [HttpGet("GetAllSponsorEventCurrentEvent")]
        [Authorize(Roles = "EVENTOPERATOR, ADMIN")]
        public async Task<IActionResult> GetAllSponsorEventCurrentEvent([FromQuery] PageParaModel pageParaModel, string eventId)
        {
            try
            {
                var result = await _sponsorEventService.GetAllSponsorEventWithDetailCurrentEvent(eventId, pageParaModel);
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

        [HttpGet("GetAllSponsorEventCurrentUser")]
        [Authorize(Roles = "SPONSOR")]
        public async Task<IActionResult> GetAllSponsorEventCurrentUser([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _sponsorEventService.GetAllSponsorEventWithDetailCurrentUser(username, pageParaModel);
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

        [HttpGet("GetSponsorEventById")]
        [Authorize(Roles = "ADMIN, SPONSOR, EVENTOPERATOR")]
        public async Task<IActionResult> GetSponsorEventById(string id)
        {
            try
            {
                var result = await _sponsorEventService.GetSponsorEventDetailById(id);
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpPost("SponsoringEvent")]
        [Authorize(Roles = "SPONSOR")]
        public async Task<IActionResult> SponsoringEvent(SponsorEventProcessModel sponsorEventProcessModel)
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _sponsorEventService.AddSponsorEventAsync(sponsorEventProcessModel, username);
                return Ok(result);
            } catch
            {
                throw;
            }
        }
    }
}
