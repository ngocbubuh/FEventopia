﻿using FEventopia.DAO.DAO.Interfaces;
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

        public async Task<Account> GetAccountByUsernameAsync(string username)
        {
            return await _context.Account.FirstAsync(p => username.Equals(p.UserName));
        }

        public async Task<List<Account>> GetAllAccountAsync()
        {
            return await _context.Account.ToListAsync();
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