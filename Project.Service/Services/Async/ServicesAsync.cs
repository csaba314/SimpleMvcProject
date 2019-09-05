using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services.Async
{
    public class ServicesAsync<TEntity> : IServicesAsync<TEntity> where TEntity : class
    {

        protected DbContext _context;

        public ServicesAsync(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException();
            }
            this._context = context;
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public Task<int> AddAsync(TEntity entity)
        {
            DbEntityEntry entityEntry = _context.Entry(entity);

            if (entityEntry.State != EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
            }
            else
            {
                _context.Set<TEntity>().Add(entity);
            }

            return Task.FromResult(1);
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            DbEntityEntry entityEntry = _context.Entry(entity);

            if (entityEntry.State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
            }

            entityEntry.State = EntityState.Modified;
            
            return Task.FromResult(1);
        }

        public Task<int> RemoveAsync(TEntity entity)
        {
            DbEntityEntry entityEntry = _context.Entry(entity);

            if (entityEntry.State != EntityState.Deleted)
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                _context.Set<TEntity>().Attach(entity);
                _context.Set<TEntity>().Remove(entity);
            }

            return Task.FromResult(1);
        }

        public Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            Task.Run(() =>_context.Set<TEntity>().RemoveRange(entities));

            return Task.FromResult(1);
        }

       
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
