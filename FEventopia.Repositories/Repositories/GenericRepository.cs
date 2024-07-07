using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.EntityModels.Base;
using FEventopia.Repositories.Repositories.Interfaces;

namespace FEventopia.Repositories.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        private readonly IGenericDAO<TEntity> _genericDAO;

        public GenericRepository(IGenericDAO<TEntity> genericDAO)
        {
            this._genericDAO = genericDAO;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            return await _genericDAO.AddAsync(entity);
        }

        public async Task<bool> AddRangeAsync(List<TEntity> entities)
        {
            return await _genericDAO.AddRangeAsync(entities);
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await _genericDAO.DeleteAsync(entity);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _genericDAO.GetAllAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await _genericDAO.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await _genericDAO.UpdateAsync(entity);
        }
    }
}
