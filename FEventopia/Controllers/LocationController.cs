using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FEventopia.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLocation()
        {
            try
            {
                var result = await _locationService.GetAllLocation();
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }
        [HttpGet("GetLocationByName")]
        public async Task<IActionResult> GetLocationByName(string name)
        {
            try
            {
                var result = await _locationService.GetLocationByName(name);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }
        [HttpPost("AddLocation")]
        public async Task<IActionResult> CreateLocation(LocationModel model)
        {
            try
            {
                var result = await _locationService.CreateLocation(model);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpPut("UpdateLocation")]
        public async Task<IActionResult> UpdateLocation(LocationModel location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _locationService.UpdateLocation(location);
                    if (result)
                    {
                        var response = new ResponseModel
                        {
                            Status = result,
                            Message = "Update location Successfully!"
                        };
                        return Ok(response);
                    }
                    else
                    {
                        var response = new ResponseModel
                        {
                            Status = result,
                            Message = "Update location Failed!"
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

        [HttpDelete("DeleteLocation")]
        public async Task<IActionResult> DeleteLocation(string name)
        {
            try
            {
                var result = await _locationService.DeleteLocation(name);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Delete Location Successfully!"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Delete Location Failed!"
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
