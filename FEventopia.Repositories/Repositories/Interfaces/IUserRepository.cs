using FEventopia.DAO.EntityModels;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<Account>> GetAllAccountAsync();
        public Task<List<Account>> GetAllStaffAccount();
        public Task<Account> GetAccountByIdAsync(string id);
        public Task<Account> GetAccountByUsernameAsync(string username);
        public Task<List<Account>> GetAllAccountByEmailAsync(string email);
        public Task<bool> UpdateAccountAsync(Account account);
        public Task<bool> UnactivateAccountAsync(string username);
        public Task<bool> ActivateAccountAsync(string username);
    }
}
