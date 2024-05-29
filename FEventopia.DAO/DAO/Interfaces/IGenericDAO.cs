using FEventopia.DAO.EntityModels.Base;

namespace FEventopia.DAO.DAO.Interfaces
{
    public interface IGenericDAO<TEntity> where TEntity : EntityBase
    {
        public Task<List<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(string id);
        public Task<TEntity> AddAsync(TEntity entity, string username);
        public Task<bool> UpdateAsync(TEntity entity, string username);
        public Task<bool> DeleteAsync(TEntity entity, string username);
    }
}
