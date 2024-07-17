using Azure;
using FEventopia.DAO.EntityModels;
using FEventopia.Services.Enum;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using FEventopia.Services.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMailService mailService;
        private readonly UserManager<Account> accountManager;
        private readonly SignInManager<Account> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountService(
            IMailService mailService,
            UserManager<Account> accountManager,
            SignInManager<Account> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager)
        {
            this.mailService = mailService;
            this.accountManager = accountManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
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

        public async Task<bool> ChangePasswordAsync(string username, string currentPassword, string newPassword)
        {
            var account = await accountManager.FindByNameAsync(username);
            if (account == null)
            {
                return false;
            }
            else
            {
                var result = await accountManager.ChangePasswordAsync(account, currentPassword, newPassword);
                if (!result.Succeeded)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public async Task<bool> ConfirmEmailAsync(string token, string username)
        {
            var user = await accountManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await accountManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<string> SendConfirmEmailAsync(string username)
        {
            var user = await accountManager.FindByNameAsync(username);

            if (user.Email == null)
            {
                return null;
            }
            else
            {
                return await accountManager.GenerateEmailConfirmationTokenAsync(user);
            }
        }

        public async Task<string> SignInAsync(string username, string password)
        {
            var result = await signInManager.PasswordSignInAsync(username, password, false, false);
            if (result.Succeeded)
            {
                var account = await accountManager.FindByNameAsync(username);

                //If account has been disable, then no login
                if (!account.DeleteFlag)
                {
                    var roles = await accountManager.GetRolesAsync(account);

                    var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                    foreach (var role in roles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    return CreateToken(authClaims);
                }
                else
                {
                    return string.Empty;
                }
            } else
            {
                return string.Empty;
            }
        }

        public async Task<bool> SignUpAsync(string name, string email, string phoneNum, string username, string password)
        {
            var userExist = await accountManager.FindByNameAsync(username);
            if (userExist == null)
            {
                var user = new Account
                {
                    Name = name,
                    PhoneNumber = phoneNum,
                    Email = email,
                    UserName = username,
                    Role = Role.VISITOR.ToString()
                };
                //Create user in database
                var result = await accountManager.CreateAsync(user, password);

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
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SignUpInternalAsync(string name, string email, string username, Role role)
        {
            var userExist = await accountManager.FindByNameAsync(username);
            if (userExist == null)
            {
                var user = new Account
                {
                    Name = name,
                    UserName = username,
                    Email = email,
                    Role = role.ToString()
                };

                string password = PasswordUtils.GenerateRandomPassword();

                //Create user in database
                var result = await accountManager.CreateAsync(user, password);

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

                    var messageRequest = new MailRequestSetting
                    {
                        ToEmail = email,
                        Subject = "FEventopia Welcome Email",
                        Body = AccountEmail.EmailContent(name, username, password)
                    };
                    await mailService.SendEmailAsync(messageRequest);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SendConfirmEmailAsync(string username, string token)
        {
            var user = await accountManager.FindByNameAsync(username);
            var messageRequest = new MailRequestSetting
            {
                ToEmail = user.Email,
                Subject = "FEventopia Confirmation Email",
                Body = ConfirmationEmail.EmailContent(user.Name, token)
            };

            await mailService.SendEmailAsync(messageRequest);
            return true;
        }
    }
}
