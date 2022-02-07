using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Data
{
    public abstract class RepositoryBase<TEntity>
        where TEntity : class
    {

        public static ApplicationContext _dbContext;

        protected RepositoryBase(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public static async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public static async Task<long> CountAsync()
        {
            return await _dbContext.Set<TEntity>().CountAsync();
        }

        public static async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().Where(predicate).CountAsync();
        }

        public static async Task<int> DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public static IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate != null ? _dbContext.Set<TEntity>().Where(predicate) : _dbContext.Set<TEntity>();
        }

        public static DbSet<TEntity> GetDbSet()
        {
            return _dbContext.Set<TEntity>();
        }

        public static TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
            {
                return _dbContext.Set<TEntity>().FirstOrDefault(predicate);
            }

            return null;
        }

        public static async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
            {
                return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
            }

            return null;
        }

        public static bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
            {
                return _dbContext.Set<TEntity>().Any(predicate);
            }

            return false;
        }

        public static async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
            {
                return await _dbContext.Set<TEntity>().AnyAsync(predicate);
            }

            return false;
        }


        public static async Task<ICollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
            {
                var query = _dbContext.Set<TEntity>().Where(predicate);

                return await query.ToListAsync();
            }

            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public static IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public static async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public static async Task<TEntity> GetByIdAsync(Object id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public static async Task<TEntity> UpdateAsync(TEntity entity, Object id)
        {
            if (entity == null)
                return null;

            var existing = await _dbContext.Set<TEntity>().FindAsync(id);
            if (existing == null)
                return null;

            _dbContext.Entry(existing).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return existing;
        }

        public static async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.AddRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public static async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
}
