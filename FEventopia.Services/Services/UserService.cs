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

        public async Task<bool> ActivateAccountAsync(string id)
        {
            return await _userRepository.ActivateAccountAsync(id);
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

        public async Task<AccountModel> GetByIdAsync(string id)
        {
            return _mapper.Map<AccountModel>(await _userRepository.GetAccountByIdAsync(id));
        }

        public async Task<List<AccountModel>> GetAllStaffAccountAsync()
        {
            var resultList = await _userRepository.GetAllStaffAccount();
            return _mapper.Map<List<AccountModel>>(resultList);
        }

        public async Task<bool> UnactivateAccountAsync(string id)
        {
            return await _userRepository.UnactivateAccountAsync(id);
        }

        public async Task<bool> UpdateAccountAsync(string id, AccountProcessModel accountModel)
        {
            var acc = await _userRepository.GetAccountByIdAsync(id);
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
