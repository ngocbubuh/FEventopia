using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FEventopia.Services.Utils;

namespace FEventopia.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository,
                                  IUserRepository userRepository,
                                  IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<TransactionModel> AddTransactionByVNPAYAsync(double amount, string username)
        {
            var user = await _userRepository.GetAccountByUsernameAsync(username);
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountID = user.Id,
                TransactionType = TransactionType.IN.ToString(),
                TransactionDate = TimeUtils.GetTimeVietNam(),
                Amount = amount,
                Description = $"FEventopia {username.ToUpper()}: Recharge +{amount}.",
            };
            var result = await _transactionRepository.AddAsync(transaction);
            return _mapper.Map<TransactionModel>(result);
        }

        public async Task<TransactionModel> AddTransactionRefundByVNPAYAsync(double amount, string username)
        {
            var user = await _userRepository.GetAccountByUsernameAsync(username);
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountID = user.Id,
                TransactionType = TransactionType.OUT.ToString(),
                TransactionDate = TimeUtils.GetTimeVietNam(),
                Amount = amount,
                Description = $"FEventopia {username.ToUpper()}: Withdrawal -{amount}.",
            };
            var result = await _transactionRepository.AddAsync(transaction);
            return _mapper.Map<TransactionModel>(result);
        }

        public async Task<PageModel<TransactionModel>> GetAllTransactionByUsernameAsync(string username, PageParaModel pagePara)
        {
            var user = await _userRepository.GetAccountByUsernameAsync(username);
            var transactionList = await _transactionRepository.GetAllByUserId(user.Id);
            var result = _mapper.Map<List<TransactionModel>>(transactionList);
            return PageModel<TransactionModel>.ToPagedList(result,
                pagePara.PageNumber,
                pagePara.PageSize);
        }

        public async Task<PageModel<TransactionModel>> GetAllTransactionsAsync(PageParaModel pagePara)
        {
            var transactionList = await _transactionRepository.GetAllAsync();
            var result = _mapper.Map<List<TransactionModel>>(transactionList);
            return PageModel<TransactionModel>.ToPagedList(result,
                pagePara.PageNumber,
                pagePara.PageSize);
        }

        public async Task<TransactionModel> UpdateTransactionByVNPAYStatusAsync(VnPayModel model)
        {
            if(model.vnp_ResponseCode == "00")
            {
                var transaction = await _transactionRepository.GetByIdAsync(model.vnp_TxnRef);
                transaction.Status = true;
                await _transactionRepository.UpdateAsync(transaction);
                
                //Update credit for user
                var user = await _userRepository.GetAccountByIdAsync(transaction.AccountID);
                user.CreditAmount += double.Parse(model.vnp_Amount)/100;
                await _userRepository.UpdateAccountAsync(user);
                return _mapper.Map<TransactionModel>(transaction);
            } else
            {
                var transaction = await _transactionRepository.GetByIdAsync(model.vnp_TxnRef);
                return _mapper.Map<TransactionModel>(transaction);
            }
        }
    }
}
