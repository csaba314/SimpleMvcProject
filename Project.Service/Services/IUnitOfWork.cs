using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public interface IUnitOfWork<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> GetAsync(int id);
        Task<DbSet<TEntity>> GetAllAsync();
        Task<int> AddAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> RemoveAsync(TEntity entity);
        Task<int> SaveChangesAsync();
    }
}
