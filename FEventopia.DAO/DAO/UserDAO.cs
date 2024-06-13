using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.DbContext;
using FEventopia.DAO.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace FEventopia.DAO.DAO
{
    public class UserDAO : IUserDAO
    {
        private readonly FEventopiaDbContext _context;

        public UserDAO(FEventopiaDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountByIdAsync(string id)
        {
            return await _context.Account.FirstOrDefaultAsync(p => id.ToLower().Equals(p.Id.ToString().ToLower())) ?? null;
        }

        public async Task<Account> GetAccountByUsernameAsync(string username)
        {
            return await _context.Account.FirstAsync(p => username.ToLower().Equals(p.UserName.ToLower())) ?? null;
        }

        public async Task<List<Account>> GetAllAccountAsync()
        {
            return await _context.Account.ToListAsync() ?? new List<Account>();
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            _context.Account.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}