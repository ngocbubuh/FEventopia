using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet("GetLocationById")]
        public async Task<IActionResult> GetLocationById(string id)
        {
            try
            {
                var result = await _locationService.GetLocationById(id);
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpPost("AddLocation")]
        public async Task<IActionResult> CreateLocation(LocationProcessModel model)
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
        public async Task<IActionResult> UpdateLocation([Required] string id, LocationProcessModel location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _locationService.UpdateLocation(id, location);
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
        public async Task<IActionResult> DeleteLocation([Required] string id)
        {
            try
            {
                var result = await _locationService.DeleteLocation(id);
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
