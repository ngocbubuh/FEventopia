﻿using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;

namespace FEventopia.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDAO _userDAO;

        public UserRepository(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task<bool> ActivateAccountAsync(string username)
        {
            var acc = await _userDAO.GetAccountByUsernameAsync(username);
            acc.DeleteFlag = false;
            return await _userDAO.UpdateAccountAsync(username, acc);
        }

        public async Task<Account> GetAccountByUsernameAsync(string username)
        {
            return await _userDAO.GetAccountByUsernameAsync(username);
        }

        public async Task<List<Account>> GetAllAccountAsync()
        {
            return await _userDAO.GetAllAccountAsync();
        }

        public async Task<List<Account>> GetAllAccountByEmailAsync(string email)
        {
            var acc = await _userDAO.GetAllAccountAsync();
            return acc.Where(a => email.Equals(a.Email)).ToList();
        }

        public async Task<bool> UnactivateAccountAsync(string username)
        {
            var acc = await _userDAO.GetAccountByUsernameAsync(username);
            acc.DeleteFlag = true;
            return await _userDAO.UpdateAccountAsync(username, acc);
        }

        public async Task<bool> UpdateAccountAsync(string username, Account account)
        {
            return await _userDAO.UpdateAccountAsync(username, account);
        }
    }
}
