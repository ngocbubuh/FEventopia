using FEventopia.Services.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<string> SignInAsync(string username, string password);
        public Task<bool> SignUpAsync(string name, string email, string phoneNum, string userName, string password);
        public Task<bool> SignUpInternalAsync(string name, string email, string userName, Role role);
        public Task<bool> ChangePasswordAsync(string username, string currentPassword, string newPassword);
        public Task<string> SendConfirmEmailAsync(string username);
        public Task<bool> SendConfirmEmailAsync(string username, string token);
        public Task<bool> ConfirmEmailAsync(string token, string username);
    }
}
