using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface ITransactionService
    {
        public Task<TransactionModel> AddTransactionByVNPAYAsync(double amount, string username);
        public Task<TransactionModel> AddTransactionRefundByVNPAYAsync(double amount, string username);
        public Task<TransactionModel> UpdateTransactionByVNPAYStatusAsync (VnPayModel model);
        public Task<PageModel<TransactionModel>> GetAllTransactionsAsync(PageParaModel pageParaModel);
        public Task<PageModel<TransactionModel>> GetAllTransactionByUsernameAsync(string username, PageParaModel pageParaModel);
        public Task<PageModel<TransactionModel>> GetAllTransactionAccountIdAsync(string id, PageParaModel pageParaModel);
    }
}
