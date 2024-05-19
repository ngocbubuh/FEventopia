﻿using FEventopia.Controllers.ViewModels.ResponseModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using FEventopia.Controllers.ViewModels.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
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
        public async Task<IActionResult> GetAccountByEmailAsync (string email, [FromQuery] PageParaModel pageParaModel)
        {
            try
            {
                var result = await _userService.GetAllAccountByEmailAsync(email, pageParaModel);
                if (result == null)
                {
                    return NotFound();
                } else
                {
                    return Ok(result);
                }
            } catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateAccountInfo")]
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

        [HttpGet("AccountByUName")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAccountByUsernameAsync(string username)
        {
            try
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
            catch
            {
                return BadRequest();
            }
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
        public async Task<IActionResult> UpdateAllAccountAsync(string username, AccountProcessModel accountModel)
        {
            try
            {
                var result = await _userService.UpdateAccountAsync(username, accountModel);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = true,
                        Message = "Update account successfully!"
                    };
                    return Ok(response);
                } else 
                {
                    var response = new ResponseModel
                    {
                        Status = true,
                        Message = "Update account fail!"
                    };
                    return BadRequest(response); 
                }
                
            } catch 
            {
                return BadRequest();
            }
        }
    }
}
