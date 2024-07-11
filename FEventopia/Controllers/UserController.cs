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
        private readonly IAuthenService _authenService;

        public UserController(IUserService userService, IAuthenService authenService)
        {
            _userService = userService;
            _authenService = authenService;
        }

        [HttpGet("GetAllAccount")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllAccountAsync([FromQuery] PageParaModel pagePara)
        {
            try
            {
                var result = await _userService.GetAllAccountAsync(pagePara);
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

        [HttpGet("GetAccountByEmail")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllAccountByEmailAsync(string email, [FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _userService.GetAllAccountByEmailAsync(email, pageParaModel);
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

        [HttpPut("UpdateAccountProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateAccountAsync(AccountProcessModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = _authenService.GetCurrentLogin;
                    var account = await _userService.GetAccountByUsernameAsync(username);
                    if (!model.Email.ToLower().Equals(account.Email.ToLower()))
                    {
                        var response = new ResponseModel
                        {
                            Status = false,
                            Message = "You cannot change email after confirmation!"
                        };
                        return BadRequest(response);
                    }
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
                            Status = false,
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
                throw;
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
                throw;
            }
        }

        [HttpGet("AccountByUName")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAccountByUsernameAsync(string username)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userService.GetAccountByUsernameAsync(username);
                    return Ok(result);
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

        [HttpDelete("UnactivateAccount")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UnactivateAccountAsync(string username)
        {
            try
            {
                var CurrentUsername = _authenService.GetCurrentLogin;
                if (!CurrentUsername.ToLower().Equals(username.ToLower()))
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
                throw;
            }
        }

        [HttpGet("GetProfile")]
        [Authorize]
        public async Task<IActionResult> GetProfileAsync()
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var account = await _userService.GetAccountByUsernameAsync(username);
                return Ok(account);
            } catch 
            {
                throw;
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
                throw;
            }
        }

        [HttpGet("GetAllStaffAccount")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR")]
        public async Task<IActionResult> GetAllStaffAccount()
        {
            try
            {
                var result = await _userService.GetAllStaffAccountAsync();
                return Ok(result);
            } catch
            {
                throw;
            }
        }

        [HttpGet("GetById")]
        [Authorize(Roles = "ADMIN, EVENTOPERATOR")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var result = await _userService.GetByIdAsync(id);
                return Ok(result);
            } catch
            {
                throw;
            }
        }
    }
}
