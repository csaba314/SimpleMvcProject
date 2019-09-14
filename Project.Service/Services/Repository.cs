using Project.Service.Model;
using System.Data.Entity;
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
