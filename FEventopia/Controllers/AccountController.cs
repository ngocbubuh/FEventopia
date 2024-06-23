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

namespace FEventopia.Controllers.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMailService mailService;
        private readonly UserManager<Account> accountManager;
        private readonly SignInManager<Account> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAuthenService authenService;

        public AccountController(
            IMailService mailService,
            UserManager<Account> accountManager,
            SignInManager<Account> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            IAuthenService authenService)
        {
            this.mailService = mailService;
            this.accountManager = accountManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.authenService = authenService;
        }

        private string CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            _ = int.TryParse(configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: TimeUtils.GetTimeVietNam().AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string token, string username)
        {
            try
            {
                var user = await accountManager.FindByNameAsync(username);
                if (user != null)
                {
                    var result = await accountManager.ConfirmEmailAsync(user, token);
                    if (result.Succeeded)
                    {
                        var response = new ResponseModel
                        {
                            Status = true,
                            Message = "Email Verification Successfully!"
                        };
                        //var urlParameter = response.ToUrlParameters();
                        //return Redirect("https://feventopia.vercel.app/confirmEmail?" + urlParameter);
                        return Ok(response);
                    }
                    else
                    {
                        var response = new ResponseModel
                        {
                            Status = false,
                            Message = "Email Verification Failed!"
                        };
                        //var urlParameter = response.ToUrlParameters();
                        //return Redirect("https://feventopia.vercel.app/confirmEmail?" + urlParameter);
                        return BadRequest(response);
                    }
                }
                else
                {
                    var response = new ResponseModel
                    {
                        Status = false,
                        Message = "User does not existed!"
                    };
                    //var urlParameter = response.ToUrlParameters();
                    //return Redirect("https://feventopia.vercel.app/confirmEmail?" + urlParameter);
                    return BadRequest(response);
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
                    var acc = authenService.GetCurrentLogin;
                    if (acc != null)
                    {
                        var response = new ResponseModel
                        {
                            Status = false,
                            Message = "You are logged in!"
                        };
                        return BadRequest(response);
                    }
                    var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var account = await accountManager.FindByNameAsync(model.Username);

                        //If account has been disable, then no login
                        if (!account.DeleteFlag)
                        {
                            var roles = await accountManager.GetRolesAsync(account);

                            var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Username),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                            foreach (var role in roles)
                            {
                                authClaims.Add(new Claim(ClaimTypes.Role, role));
                            }

                            var token = CreateToken(authClaims);

                            Response.Headers["JSON-Web-Token"] = token;

                            var response = new ResponseModel
                            {
                                Status = true,
                                Message = "Login Successfully!"
                            };
                            return Ok(response);
                        }
                        else
                        {
                            var response = new ResponseModel
                            {
                                Status = false,
                                Message = "Login Failed! Your account has been disable!"
                            };
                            return BadRequest(response);
                        }
                    }
                    else
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
                    var userExist = await accountManager.FindByNameAsync(model.Username);
                    if (userExist == null)
                    {
                        var user = new Account
                        {
                            Name = model.Name,
                            PhoneNumber = model.PhoneNumber,
                            Email = model.Email,
                            UserName = model.Username,
                            Role = Role.VISITOR.ToString()
                        };
                        //Create user in database
                        var result = await accountManager.CreateAsync(user, model.Password);

                        //Add role
                        if (result.Succeeded)
                        {
                            if (!await roleManager.RoleExistsAsync(Role.VISITOR.ToString()))
                            {
                                await roleManager.CreateAsync(new IdentityRole(Role.VISITOR.ToString()));
                            }
                            if (await roleManager.RoleExistsAsync(Role.VISITOR.ToString()))
                            {
                                await accountManager.AddToRoleAsync(user, Role.VISITOR.ToString());
                            }

                            //Send confirmation email
                            var tokenConfirm = accountManager.GenerateEmailConfirmationTokenAsync(user);

                            var confirmationURL = Url.Action(nameof(ConfirmEmailAsync), "Account", new { token = tokenConfirm.Result, username = model.Username }, Request.Scheme);

                            var messageRequest = new MailRequestSetting
                            {
                                ToEmail = model.Email,
                                Subject = "FEventopia Confirmation Email",
                                Body = ConfirmationEmail.EmailContent(user.Name, confirmationURL)
                            };

                            await mailService.SendEmailAsync(messageRequest);

                            //Response
                            var response = new ResponseModel
                            {
                                Status = true,
                                Message = "Sign Up Successfully!"
                            };
                            return Ok(response);
                        }
                        else
                        {
                            string errorMessage = string.Empty;
                            foreach (var error in result.Errors)
                            {
                                errorMessage = error.Description;
                            }
                            var response = new ResponseModel
                            {
                                Status = false,
                                Message = errorMessage
                            };
                            return BadRequest(response);
                        }
                    }
                    else
                    {
                        var response = new ResponseModel
                        {
                            Status = false,
                            Message = "Username already existed!"
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

        [HttpPost("SignUp-Inernal")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> SignUpInternal(SignUpInternalRequestModel model, Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userExist = await accountManager.FindByNameAsync(model.Username);
                    if (userExist == null)
                    {
                        var user = new Account
                        {
                            Name = model.Name,
                            UserName = model.Username,
                            Role = role.ToString()
                        };

                        //Create user in database
                        var result = await accountManager.CreateAsync(user, model.Password);

                        //Add role
                        if (result.Succeeded)
                        {
                            if (role == Role.VISITOR)
                            {
                                if (!await roleManager.RoleExistsAsync(Role.VISITOR.ToString()))
                                {
                                    await roleManager.CreateAsync(new IdentityRole(Role.VISITOR.ToString()));
                                }
                                if (await roleManager.RoleExistsAsync(Role.VISITOR.ToString()))
                                {
                                    await accountManager.AddToRoleAsync(user, Role.VISITOR.ToString());
                                }
                            }
                            if (role == Role.SPONSOR)
                            {
                                if (!await roleManager.RoleExistsAsync(Role.SPONSOR.ToString()))
                                {
                                    await roleManager.CreateAsync(new IdentityRole(Role.SPONSOR.ToString()));
                                }
                                if (await roleManager.RoleExistsAsync(Role.SPONSOR.ToString()))
                                {
                                    await accountManager.AddToRoleAsync(user, Role.SPONSOR.ToString());
                                }
                            }
                            else if (role == Role.EVENTOPERATOR)
                            {
                                if (!await roleManager.RoleExistsAsync(Role.EVENTOPERATOR.ToString()))
                                {
                                    await roleManager.CreateAsync(new IdentityRole(Role.EVENTOPERATOR.ToString()));
                                }
                                if (await roleManager.RoleExistsAsync(Role.EVENTOPERATOR.ToString()))
                                {
                                    await accountManager.AddToRoleAsync(user, Role.EVENTOPERATOR.ToString());
                                }
                            }
                            else if (role == Role.CHECKINGSTAFF)
                            {
                                if (!await roleManager.RoleExistsAsync(Role.CHECKINGSTAFF.ToString()))
                                {
                                    await roleManager.CreateAsync(new IdentityRole(Role.CHECKINGSTAFF.ToString()));
                                }
                                if (await roleManager.RoleExistsAsync(Role.CHECKINGSTAFF.ToString()))
                                {
                                    await accountManager.AddToRoleAsync(user, Role.CHECKINGSTAFF.ToString());
                                }
                            }
                            else if (role == Role.ADMIN)
                            {
                                if (!await roleManager.RoleExistsAsync(Role.ADMIN.ToString()))
                                {
                                    await roleManager.CreateAsync(new IdentityRole(Role.ADMIN.ToString()));
                                }
                                if (await roleManager.RoleExistsAsync(Role.ADMIN.ToString()))
                                {
                                    await accountManager.AddToRoleAsync(user, Role.ADMIN.ToString());
                                }
                            }

                            //Response
                            var response = new ResponseModel
                            {
                                Status = true,
                                Message = $"Sign Up {role} Account Successfully!"
                            };
                            return Ok(response);
                        }
                        else
                        {
                            string errorMessage = string.Empty;
                            foreach (var error in result.Errors)
                            {
                                errorMessage = error.Description;
                            }
                            var response = new ResponseModel
                            {
                                Status = false,
                                Message = errorMessage
                            };
                            return Unauthorized(response);
                        }
                    }
                    else
                    {
                        var response = new ResponseModel
                        {
                            Status = false,
                            Message = "Username already existed!"
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
                    var username = authenService.GetCurrentLogin;
                    var account = await accountManager.FindByNameAsync(username);
                    if (account == null)
                    {
                        var response = new ResponseModel
                        {
                            Status = false,
                            Message = "Account doesn't exist!"
                        };
                        return BadRequest(response);
                    }
                    else
                    {
                        var result = await accountManager.ChangePasswordAsync(account, model.CurrentPassword, model.NewPassword);

                        string errorMessage = string.Empty;
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                errorMessage = error.Description;
                            }

                            var response = new ResponseModel
                            {
                                Status = false,
                                Message = errorMessage
                            };
                            return Unauthorized(response);
                        }
                        else
                        {
                            var response = new ResponseModel
                            {
                                Status = true,
                                Message = "Change Password Successfully!"
                            };
                            return Ok(response);
                        }
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
                var username = authenService.GetCurrentLogin;
                var user = await accountManager.FindByNameAsync(username);

                if (user.Email == null)
                {
                    var response = new ResponseModel
                    {
                        Status = false,
                        Message = "Please update your email!"
                    };
                    return BadRequest(response);
                }
                else if (user.EmailConfirmed == true)
                {
                    var response = new ResponseModel
                    {
                        Status = false,
                        Message = "You are already confirmed your email!"
                    };
                    return BadRequest(response);
                }
                else
                {
                    var tokenConfirm = accountManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationURL = Url.Action(nameof(ConfirmEmailAsync), "Account", new { token = tokenConfirm.Result, username = user.UserName }, Request.Scheme);

                    var messageRequest = new MailRequestSetting
                    {
                        ToEmail = user.Email,
                        Subject = "FEventopia Confirmation Email",
                        Body = ConfirmationEmail.EmailContent(user.Name, confirmationURL)
                    };

                    await mailService.SendEmailAsync(messageRequest);
                    var response = new ResponseModel
                    {
                        Status = true,
                        Message = "Confirmation Email Sent!"
                    };
                    return Ok(response);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
