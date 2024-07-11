using FEventopia.Services.BussinessModels;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IUserService
    {
        public Task<AccountModel> GetByIdAsync(string id);
        public Task<PageModel<AccountModel>> GetAllAccountAsync(PageParaModel pagePara);
        public Task<List<AccountModel>> GetAllStaffAccountAsync();
        public Task<AccountModel> GetAccountByUsernameAsync(string username);
        public Task<PageModel<AccountModel>> GetAllAccountByEmailAsync(string email, PageParaModel pagePara);
        public Task<bool> UpdateAccountAsync(string username, AccountProcessModel account);
        public Task<bool> UnactivateAccountAsync(string username);
        public Task<bool> ActivateAccountAsync(string username);
    }
}
