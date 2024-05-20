using FEventopia.Repositories.EntityModels;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<Account>> GetAllAccountAsync();
        public Task<Account> GetAccountByUsernameAsync(string username);
        public Task<List<Account>> GetAllAccountByEmailAsync(string email);
        public Task<bool> UpdateAccountAsync(string username, Account account);
        public Task<bool> UnactivateAccountAsync(string username);
        public Task<bool> ActivateAccountAsync(string username);
    }
}
