using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly IGenericDAO<Transaction> _transactionDAO;
        public TransactionRepository(IGenericDAO<Transaction> genericDAO) : base(genericDAO)
        {
            _transactionDAO = genericDAO;
        }

        public async Task<List<Transaction>> GetAllByUserId(string userId)
        {
            var result = await _transactionDAO.GetAllAsync();
            return result.Where(t => userId.ToLower().Equals(t.AccountID.ToString().ToLower())).ToList();
        }
    }
}
