using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.DbContext;
using FEventopia.DAO.EntityModels.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO
{
    public class GenericDAO<TEntity> : IGenericDAO<TEntity> where TEntity : EntityBase
    {
        protected DbSet<TEntity> _dbSet;
        private readonly FEventopiaDbContext _dbContext;

        public GenericDAO(FEventopiaDbContext dbContext)
        {
            _dbSet = dbContext.Set<TEntity>();
            _dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            entity.DeleteFlag = true;
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await _dbSet.FirstOrDefaultAsync(p => id.ToLower().Equals(p.Id.ToString().ToLower()));
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddRangeAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
