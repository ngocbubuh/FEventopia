using FEventopia.Controllers.ViewModels.RequestModels;
using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using FEventopia.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace FEventopia.Controllers.Controllers
{
    [Route("ticket/")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly IAuthenService _authenService;
        private readonly IMailService _mailService;

        public TicketController(ITicketService ticketService, IAuthenService authenService, IMailService mailService, IUserService userService)
        {
            _ticketService = ticketService;
            _authenService = authenService;
            _mailService = mailService;
            _userService = userService;
        }

        [HttpPost("BuyTicket")]
        [Authorize(Roles = "VISITOR")]
        public async Task<IActionResult> AddTicketAsync(BuyTicketRequestModel buyTicketRequestModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = _authenService.GetCurrentLogin;
                    var account = await _userService.GetAccountByUsernameAsync(username);
                    //Kiểm tra account có đủ tiền hay ko
                    if(account.CreditAmount < buyTicketRequestModel.TotalPrice)
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Insufficient credit!"
                        };
                        return BadRequest(response);
                    }
                    else
                    {
                        bool bookingFlag = false;
                        foreach (var item in buyTicketRequestModel.TicketRequests)
                        {
                            //Với mỗi số lượng mà loại vé đó đặt
                            for (var i = 1; i <= item.Quantity; i++)
                            {
                                var result = await _ticketService.AddTicketAsync(item.EventDetailId.ToString(), username);
                                if (result != null)
                                {
                                    //Gửi mail vẽ với mỗi vé tạo thành công
                                    var messageRequest = new MailRequestSetting
                                    {
                                        ToEmail = buyTicketRequestModel.EmailAddress,
                                        Subject = "FEventopia Ticket Confirmation",
                                        Body = TicketEmail.EmailContent(result, account, HttpUtility.UrlEncode(result.Id))
                                    };
                                    await _mailService.SendEmailAsync(messageRequest);
                                } else
                                {
                                    bookingFlag = true;
                                }
                            }
                        }
                        if (bookingFlag)
                        {
                            var response = new ResponseModel
                            {
                                Status = false,
                                Message = "1 or all ticket booking request failed, please check your email and inventory for more details!"
                            };
                            return StatusCode(208, response);
                        }
                        else
                        {
                            var response = new ResponseModel
                            {
                                Status = true,
                                Message = "Booking Ticket Successfully!"
                            };
                            return Ok(response);
                        }
                    }
                } else
                {
                    return ValidationProblem(ModelState);
                }
            } catch
            {
                throw;
            }
        }

        [HttpGet("Checkin")]
        [Authorize(Roles = "CHECKINGSTAFF, EVENTOPERATOR")]
        public async Task<IActionResult> Checkin(string ticketId, string eventDetailId)
        {
            try
            {
                //Thêm quét sự kiện hiện tại để chỉ checkin thành công đúng vé của sự kiện đó
                var result = await _ticketService.CheckInAsync(ticketId, eventDetailId);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = true,
                        Message = "Check-in successfully!"
                    };
                    return Ok(response);
                } else
                {
                    var response = new ResponseModel
                    {
                        Status = false,
                        Message = "Check-in failed: Ticket has been checked!"
                    };
                    return BadRequest(response);
                }
            } catch
            {
                throw;
            }
        }

        [HttpGet("GetAllOwnTicketInfo")]
        [Authorize(Roles = "VISITOR")]
        public async Task<IActionResult> GetAllOwnTicketInfo([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _ticketService.GetAllTicketWithDetailCurrentUser(username, pageParaModel);
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

        [HttpGet("GetEventAllTicketInfo")]
        [Authorize(Roles = "EVENTOPERATOR, ADMIN")]
        public async Task<IActionResult> GetAllEventTicketInfo([FromQuery] PageParaModel pageParaModel, string eventId)
        {
            try
            {
                var result = await _ticketService.GetAllTicketWithDetailCurrentEvent(eventId, pageParaModel);
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

        [HttpGet("GetTicketInfo")]
        [Authorize]
        public async Task<IActionResult> GetTicketInfo(string ticketId)
        {
            try
            {
                var result = await _ticketService.GetTicketDetailById(ticketId);
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpGet("GetAllTicket")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllTicket([FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _ticketService.GetAllTicketWithDetailAsync(pageParaModel);
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
    }
}
