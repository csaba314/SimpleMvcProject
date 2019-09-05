using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services.Async
{
    public interface IServicesAsync<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<int> AddAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> RemoveAsync(TEntity entity);
        Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities);

        Task<int> SaveChangesAsync();
    }
}
