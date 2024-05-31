using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO.Interfaces
{
    public interface IUserDAO
    {
        public Task<List<Account>> GetAllAccountAsync();
        public Task<Account> GetAccountByUsernameAsync(string username);
        public Task<bool> UpdateAccountAsync(Account account);
    }
}
