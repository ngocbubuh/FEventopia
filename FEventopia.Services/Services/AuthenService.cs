using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class AuthenService : IAuthenService
    {
        public AuthenService(IHttpContextAccessor httpContextAccessor) 
        {
            var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var username = AuthenToolSetting.GetCurrentUsername(identity);
            GetCurrentLogin = username;
        }
        public string GetCurrentLogin { get; }
    }
}
