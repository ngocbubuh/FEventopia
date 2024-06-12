using FEventopia.DAO.EntityModels.Base;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        public Task<List<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(string id);
        public Task<TEntity> AddAsync(TEntity entity);
        public Task<bool> UpdateAsync(TEntity entity);
        public Task<bool> DeleteAsync(TEntity entity);
    }
}
