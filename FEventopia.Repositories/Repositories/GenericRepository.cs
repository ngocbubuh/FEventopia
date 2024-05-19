using FEventopia.Repositories.DbContext;
using FEventopia.Repositories.EntityModels.Base;
using FEventopia.Repositories.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly FEventopiaDbContext _dbContext;

        public GenericRepository(DbSet<TEntity> dbSet, FEventopiaDbContext dbContext)
        {
            _dbSet = dbSet;
            _dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity, string username)
        {
            entity.CreatedBy = username;
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(TEntity entity, string username)
        {
            entity.UpdatedBy = username;
            entity.DeleteFlag = true;
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await _dbSet.Where(p => p.Id == id).FirstAsync();
        }

        public async Task<bool> UpdateAsync(TEntity entity, string username)
        {
            entity.UpdatedBy = username;
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
