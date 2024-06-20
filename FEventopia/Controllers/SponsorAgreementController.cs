using FEventopia.Controllers.ViewModels.RequestModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FEventopia.Controllers.Controllers
{
    [Route("agreement/")]
    [ApiController]
    public class SponsorAgreementController : ControllerBase
    {
        private readonly ISponsorManagementService _sponsorManagementService;
        private readonly IAuthenService _authenService;
        public SponsorAgreementController(ISponsorManagementService sponsorManagementService, IAuthenService authenService)
        {
            _sponsorManagementService = sponsorManagementService;
            _authenService = authenService;
        }

        [HttpGet("GetAllSponsorAgreement")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllSponsorAgreementAsync([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _sponsorManagementService.GetAllSponsorManagementWithDetailAsync(pageParaModel);
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

        [HttpGet("GetAllAgreementCurrentEvent")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR")]
        public async Task<IActionResult> GetAllSponsorAgreementCurrentEvent(string eventId, [FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _sponsorManagementService.GetAllSponsorManagementWithDetailCurrentEvent(eventId, pageParaModel);
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

        [HttpGet("GetAllAgreementCurrentUser")]
        [Authorize(Roles = "SPONSOR")]
        public async Task<IActionResult> GetAllSponsorAgreementCurrentUser([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _sponsorManagementService.GetAllSponsorManagementWithDetailCurrentUser(username, pageParaModel);
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

        [HttpGet("GetAgreementById")]
        [Authorize(Roles = "ADMIN, SPONSOR, EVENTOPERATOR")]
        public async Task<IActionResult> GetAgreementById(string id)
        {
            try
            {
                var result = await _sponsorManagementService.GetSponsorManagementDetailById(id);
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpPost("PledgeSponsoringEvent")]
        [Authorize(Roles = "SPONSOR")]
        public async Task<IActionResult> Pleged(PledgeSponsorRequestModel requestModel)
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _sponsorManagementService.AddSponsorManagementAsync(requestModel.EventId, requestModel.Amount, username);
                return Ok(result);
            } catch
            {
                throw;
            }
        }
    }
}
