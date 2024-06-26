using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FEventopia.Controllers.Controllers
{
    [Route("event-analysis")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;

        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        [HttpGet("{eventId}", Name = "eventId")]
        [Authorize(Roles = "EVENTOPERATOR, ADMIN")]
        public async Task<IActionResult> GetEventAnalysis(Guid eventId)
        {
            try
            {
                var result = await _analysisService.GetEventAnalysis(eventId.ToString());
                return Ok(result);
            } catch
            {
                throw;
            }
        }

    }
}
