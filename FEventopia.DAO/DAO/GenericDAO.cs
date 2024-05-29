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
        private readonly DbSet<TEntity> _dbSet;
        private readonly FEventopiaDbContext _dbContext;

        public GenericDAO(DbSet<TEntity> dbSet, FEventopiaDbContext dbContext)
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
            return await _dbSet.FirstOrDefaultAsync(p => id.Equals(p.Id));
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
