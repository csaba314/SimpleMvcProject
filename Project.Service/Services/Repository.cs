using Project.Service.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    internal class Repository : IRepository
    {

        private readonly ProjectDbContext _context;

        public Repository(ProjectDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(ProjectDbContext));
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
