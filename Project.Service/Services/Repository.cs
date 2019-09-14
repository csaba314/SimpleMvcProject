using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    internal class Repository : IRepository
    {

        private ProjectDbContext _context;

        public Repository(ProjectDbContext context)
        {
            _context = context;
        }

        public Task<TEntity> GetAsync<TEntity>(int id) where TEntity : class
        {
            return _context.Set<TEntity>().FindAsync(id);
        }

        public Task<DbSet<TEntity>> GetAllAsync<TEntity>() where TEntity : class
        {
            return Task.FromResult(_context.Set<TEntity>());
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
