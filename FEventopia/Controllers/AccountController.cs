using FEventopia.DAO.EntityModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using FEventopia.Services.Enum;
using FEventopia.Controllers.ViewModels.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FEventopia.Controllers.ViewModels.RequestModels;
using FEventopia.Services.Utils;
using Newtonsoft.Json.Linq;

namespace FEventopia.Controllers.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenService _authenService;

        public AccountController(IAuthenService authenService,
            IAccountService accountService)
        {
            this._authenService = authenService;
            this._accountService = accountService;
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string token, string username)
        {
            try
            {
                var result = await _accountService.ConfirmEmailAsync(token, username);
                if (result)
                {
                    var response = new ResponseModel
                    {
                        Status = true,
                        Message = "Email Verification Successfully!"
                    };
                    var urlParameter = response.ToUrlParameters();
                    return Redirect("https://feventopia.vercel.app/confirmEmail?" + urlParameter);
                } else
                {
                    var response = new ResponseModel
                    {
                        Status = false,
                        Message = "User does not existed!"
                    };
                    var urlParameter = response.ToUrlParameters();
                    return Redirect("https://feventopia.vercel.app/confirmEmail?" + urlParameter);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync(SignInRequestModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.SignInAsync(model.Username, model.Password);
                    if (!result.IsNullOrEmpty())
                    {
                        Response.Headers["JSON-Web-Token"] = result;

                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Login Successfully!"
                        };
                        return Ok(response);
                    } else
                    {
                        var response = new ResponseModel
                        {
                            Status = false,
                            Message = "Login Failed! Wrong Email or Password!"
                        };
                        return Unauthorized(response);
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

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpAsync(SignUpRequestModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result= await _accountService.SignUpAsync(model.Name, model.Email, model.PhoneNumber, model.Username, model.Password);
                    if (result)
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Sign Up Successfully!"
                        };
                        return Ok(response);
                    } else
                    {
                        var response = new ResponseModel
                        {
                            Status = false,
                            Message = "Sign Up Failed!"
                        };
                        return BadRequest(response);
                    }
                }
                else
                {
                    return ValidationProblem(ModelState);
                }
            }
            catch { throw; }
        }

        [HttpPost("SignUp-Inernal")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SignUpInternalAsync(SignUpInternalRequestModel model, Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.SignUpInternalAsync(model.Name, model.Email, model.Username, role);
                    if (result)
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = $"Sign Up {role} Account Successfully!"
                        };
                        return Ok(response);
                    } else
                    {
                        var response = new ResponseModel
                        {
                            Status = false,
                            Message = $"Sign Up {role} Account Failed!"
                        };
                        return Unauthorized(response);
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

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequestModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = _authenService.GetCurrentLogin;
                    var result = await _accountService.ChangePasswordAsync(username, model.CurrentPassword, model.NewPassword);
                    if (result)
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Change Password Successfully!"
                        };
                        return Ok(response);
                    } else
                    {
                        var response = new ResponseModel
                        {
                            Status = false,
                            Message = "Change Password Failed!"
                        };
                        return Unauthorized(response);
                    }
                }
                else
                {
                    return ValidationProblem(ModelState);
                }
            }
            catch { throw; }
        }

        [HttpPost("SendConfirmationEmail")]
        [Authorize]
        public async Task<IActionResult> SendConfirmEmailAsync()
        {
            try
            {
                var username = _authenService.GetCurrentLogin;
                var result = await _accountService.SendConfirmEmailAsync(username);
                if (!result.IsNullOrEmpty())
                {
                    var confirmationURL = Url.Action(nameof(ConfirmEmailAsync), "Account", new { token = result, username = username }, Request.Scheme);

                    var emailSent = await _accountService.SendConfirmEmailAsync(username, confirmationURL);
                    
                    if (emailSent)
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Confirmation Email Sent!"
                        };
                        return Ok(response);
                    } else
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Confirmation Email Send Failed!"
                        };
                        return BadRequest(response);
                    }
                    
                } else
                {
                    var response = new ResponseModel
                    {
                        Status = true,
                        Message = "Confirmation Email Send Failed!"
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
