using FEventopia.Repositories.DbContext;
using FEventopia.Repositories.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FEventopia.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FEventopiaDbContext _context;

        public UserRepository(FEventopiaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ActivateAccountAsync(string username)
        {
            var acc = await _context.Account.FirstAsync(p => p.UserName == username);
            if (acc == null)
            {
                return false;
            }
            else
            {
                acc.DeleteFlag = false;
                _context.Account.Update(acc);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Account> GetAccountByUsernameAsync(string username)
        {
            return await _context.Account.FirstAsync(p => p.UserName == username);
        }

        public async Task<List<Account>> GetAllAccountAsync()
        {
            return await _context.Account.ToListAsync();
        }

        public async Task<List<Account>> GetAllAccountByEmailAsync(string email)
        {
            return await _context.Account.Where(p => p.Email == email).ToListAsync();
        }

        public async Task<bool> UnactivateAccountAsync(string username)
        {
            var acc = await _context.Account.FirstAsync(p => p.UserName == username);
            if (acc == null)
            {
                return false;
            }
            else
            {
                acc.DeleteFlag = true;
                _context.Account.Update(acc);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateAccountAsync(string username, Account account)
        {
            if (username == account.UserName)
            {
                _context.Account.Update(account);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
