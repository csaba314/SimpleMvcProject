using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public interface IUnitOfWork : IDisposable 
    {
        Task<int> AddAsync<TEntity>(TEntity entity) where TEntity : class;
        Task<int> UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
        Task<int> RemoveAsync<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync();
    }
}
