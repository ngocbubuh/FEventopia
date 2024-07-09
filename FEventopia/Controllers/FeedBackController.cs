using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FEventopia.Controllers.Controllers
{
    [Route("feedback/")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;

        public FeedBackController(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetByID (string id)
        {
            try
            {
                var result = await _feedBackService.GetFeedBackByID(id);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet("GetByEventDetail")]
        public async Task<IActionResult> GetByEventDetail (string eventDetailID, [FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _feedBackService.GetAllByEventDetail(eventDetailID,pageParaModel);
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

        [HttpPost]
        public async Task<IActionResult> AddFeedBack(FeedBackModel model)
        {
            try
            {
                var result = await _feedBackService.CreateFeedBack(model);
                return Ok(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpPut("UpdateFeedBack")]
        public async Task<IActionResult> UpdateFeedback([Required] string id, FeedBackModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _feedBackService.UpdateFeedBack(id, model);
                    if (result)
                    {
                        var response = new ResponseModel
                        {
                            Status = result,
                            Message = "Update feedback Successfully!"
                        };
                        return Ok(response);
                    }
                    else
                    {
                        var response = new ResponseModel
                        {
                            Status = result,
                            Message = "Update feedback Failed!"
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

        [HttpDelete]
        public async Task<IActionResult> DeleteFeedback([Required] string id)
        {
            try
            {
                var result = await _feedBackService.DeleteFeedBack(id);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Delete feedback Successfully!"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel
                    {
                        Status = result,
                        Message = "Delete feedback Failed!"
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
