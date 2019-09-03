using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Project.Service.Services
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;

        public Service(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException();
            }
            this._context = context;
        }

        // Get
        public virtual TEntity Get(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        // Add
        public virtual void Add(TEntity entity)
        {
            DbEntityEntry entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                _context.Set<TEntity>().Add(entity);
            }
            else
            {
                entry.State = EntityState.Added;
            }
        }
        
        // Remove
        public virtual void Remove(TEntity entity)
        {
            DbEntityEntry entry = _context.Entry(entity);

            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                _context.Set<TEntity>().Attach(entity);
                _context.Set<TEntity>().Remove(entity);
            }
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        // Update
        public virtual void Update(TEntity entity)
        {
            DbEntityEntry entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        // Save
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
