using AutoMapper;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;

namespace FEventopia.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> ActivateAccountAsync(string username)
        {
            return await _userRepository.ActivateAccountAsync(username);
        }

        public async Task<AccountModel> GetAccountByUsernameAsync(string username)
        {
            var resultAcc = await _userRepository.GetAccountByUsernameAsync(username);
            return _mapper.Map<AccountModel>(resultAcc);
        }

        public async Task<PageModel<AccountModel>> GetAllAccountAsync(PageParaModel pagePara)
        {
            var resultList = await _userRepository.GetAllAccountAsync();
            var result = _mapper.Map<List<AccountModel>>(resultList);
            return PageModel<AccountModel>.ToPagedList(result,
                pagePara.PageNumber,
                pagePara.PageSize);
        }

        public async Task<PageModel<AccountModel>> GetAllAccountByEmailAsync(string email, PageParaModel pagePara)
        {
            var resultList = await _userRepository.GetAllAccountByEmailAsync(email);
            var result = _mapper.Map<List<AccountModel>>(resultList);
            return PageModel<AccountModel>.ToPagedList(result,
                pagePara.PageNumber,
                pagePara.PageSize);
        }

        public async Task<List<AccountModel>> GetAllStaffAccountAsync()
        {
            var resultList = await _userRepository.GetAllStaffAccount();
            return _mapper.Map<List<AccountModel>>(resultList);
        }

        public async Task<bool> UnactivateAccountAsync(string username)
        {
            return await _userRepository.UnactivateAccountAsync(username);
        }

        public async Task<bool> UpdateAccountAsync(string username, AccountProcessModel accountModel)
        {
            var acc = await _userRepository.GetAccountByUsernameAsync(username);
            if (acc != null)
            {
                var account = _mapper.Map(accountModel, acc);
                return await _userRepository.UpdateAccountAsync(account);
            } else
            {
                return false;
            }
        }
    }
}
