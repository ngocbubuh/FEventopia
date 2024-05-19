using FEventopia.Repositories.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        public Task<List<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(string id);
        public Task<TEntity> AddAsync(TEntity entity, string username);
        public Task<bool> UpdateAsync(TEntity entity, string username);
        public Task<bool> DeleteAsync(TEntity entity, string username);
    }
}
