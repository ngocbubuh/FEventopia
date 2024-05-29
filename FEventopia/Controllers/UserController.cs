using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FEventopia.Controllers.Controllers
{
    [Route("user/management")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private string GetCurrentLogin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return AuthenToolSetting.GetCurrentUsername(identity);
        }

        [HttpGet("GetAllAccount")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllAccountAsync([FromQuery] PageParaModel pagePara)
        {
            try
            {
                var result = await _userService.GetAllAccountAsync(pagePara);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
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
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAccountByEmail")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllAccountByEmailAsync(string email, [FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _userService.GetAllAccountByEmailAsync(email, pageParaModel);
                if (result.IsNullOrEmpty())
                {
                    return NotFound();
                }
                else
                {
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
                
            } catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateAccountProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateAccountAsync(AccountProcessModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = GetCurrentLogin();
                    var result = await _userService.UpdateAccountAsync(username, model);
                    if (result)
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Update account successfully!"
                        };
                        return Ok(response);
                    }
                    else
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Update account fail!"
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
                return BadRequest();
            }
        }

        [HttpPut("UpdateAllAccount")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateAllAccountAsync(string username, AccountProcessModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userService.UpdateAccountAsync(username, model);
                    if (result)
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = $"Update {username}'s account successfully!"
                        };
                        return Ok(response);
                    }
                    else
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = $"Update {username}'s account fail!"
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
                return BadRequest();
            }
        }

        [HttpGet("AccountByUName")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAccountByUsernameAsync(string username)
        {
            //try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userService.GetAccountByUsernameAsync(username);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return ValidationProblem(ModelState);
                }
            }
            //catch
            //{
            //    return BadRequest();
            //}
        }

        [HttpDelete("UnactivateAccount")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UnactivateAccountAsync(string username)
        {
            try
            {
                var CurrentUsername = GetCurrentLogin();
                if (CurrentUsername != username)
                {
                    var result = await _userService.UnactivateAccountAsync(username);
                    if (result)
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Disable account successfully!"
                        };
                        return Ok(response);
                    }
                    else
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Disable account fail!"
                        };
                        return BadRequest(response);
                    }
                }
                else
                {
                    var response = new ResponseModel
                    {
                        Status = false,
                        Message = "Cannot disable ownself!"
                    };
                    return BadRequest(response);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetProfile")]
        [Authorize]
        public async Task<IActionResult> GetProfileAsync()
        {
            try
            {
                var username = GetCurrentLogin();
                var account = await _userService.GetAccountByUsernameAsync(username);
                return Ok(account);
            } catch 
            {
                return BadRequest();
            }
        }

        [HttpPatch("ReactivateAccount")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ReactivateAccountAsync(string username)
        {
            try
            {
                var result = await _userService.ActivateAccountAsync(username);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = true,
                        Message = "Re-activate account successfully!"
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel
                    {
                        Status = false,
                        Message = "Re-activate account fail!"
                    };
                    return Ok(response);
                }
                
            } catch 
            {
                return BadRequest();
            }
        }
    }
}
