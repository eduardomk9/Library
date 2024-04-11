using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Configuration;

using Core.Repositories;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories
{
    public class GenericEnterpriseRepository<T>(GenericEnterpriseContext dbContext) : IGenericEnterpriseRepository<T> where T : class
    {

        protected GenericEnterpriseContext _dbContext = dbContext;
        protected DbSet<T> _dbSet = dbContext.Set<T>();

        public async Task<T?> GetById(int Id)
        {
            _dbContext.ChangeTracker.Clear();
            var entity = await _dbSet.FindAsync(Id);



            if (entity != null)
                _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            _dbContext.ChangeTracker.Clear();
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<int> Create(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<T> CreateScope(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CreateRangeAsync(IList<T> entities)
        {
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            await _dbContext.BulkInsertAsync(entities, options => options.BatchSize = 1000);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> CreateOrUpdateRangeAsync(IList<T> entities)
        {
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            await _dbContext.BulkInsertOrUpdateAsync(entities, options => options.BatchSize = 1000);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> prPredicate)
        {
            var linq = _dbSet
                .Where(prPredicate)
                .AsQueryable()
                .AsNoTracking();
            IEnumerable<T> result = await linq.ToListAsync();
            return result;
        }
        public async Task<IEnumerable<T>> GetAll(Func<IQueryable<T>, IQueryable<T>> includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (includes != null)
            {
                query = includes(query);
            }
            IEnumerable<T> results = await query.ToListAsync();
            foreach (T result in results)
            {
                _dbContext.Entry(result).State = EntityState.Detached;
            }
            return results;
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> prPredicate, Func<IQueryable<T>, IQueryable<T>> includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (includes != null)
            {
                query = includes(query);
            }
            IEnumerable<T> results = await query.Where(prPredicate).ToListAsync();
            foreach (T result in results)
            {
                _dbContext.Entry(result).State = EntityState.Detached;
            }
            return results;
        }

        public async Task<T?> Get(Expression<Func<T, bool>> prPredicate)
        {
            _dbContext.ChangeTracker.Clear();
            var linq = _dbSet
                .Where(prPredicate)
                .AsQueryable();
            T? result = await linq.FirstOrDefaultAsync();

            if (result != null)
                _dbContext.Entry(result).State = EntityState.Detached;

            return result;
        }
        public async Task<T?> Get(Expression<Func<T, bool>> prPredicate, Func<IQueryable<T>, IQueryable<T>> includes)
        {
            _dbContext.ChangeTracker.Clear();
            var linq = _dbSet.Where(prPredicate);

            if (includes != null)
            {
                linq = includes(linq);
            }

            T? result = await linq.FirstOrDefaultAsync();

            if (result != null)
                _dbContext.Entry(result).State = EntityState.Detached;

            return result;
        }

        public async Task<T> Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<ICollection<T>> UpdateRange(ICollection<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
            return entities;
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(T entity)
        {
            _dbContext.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(Expression<Func<T, bool>> prPredicate)
        {
            var entityRange = _dbSet
                .Where(prPredicate)
                .AsEnumerable();
            _dbSet.RemoveRange(entityRange);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRange(IEnumerable<T> entities)
        {
            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task<List<T>> CreateRangeEntities(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
            return entities;
        }
    }
}