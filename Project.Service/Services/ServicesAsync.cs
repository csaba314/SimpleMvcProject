﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Project.Service.Services
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

        public Task<DbSet<TEntity>> GetAllAsync()
        {
            return Task.FromResult(_context.Set<TEntity>());
        }

        public Task<int> AddAsync(TEntity entity)
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            try
            {
                DbEntityEntry entityEntry = _context.Entry(entity);

                if (entityEntry.State == EntityState.Detached)
                {
                    _context.Set<TEntity>().Attach(entity);
                }

                entityEntry.State = EntityState.Modified;

                return Task.FromResult(1);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Task<int> RemoveAsync(TEntity entity)
        {
            try
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
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
            
        }
    }
}
