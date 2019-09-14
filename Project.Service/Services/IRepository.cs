using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public interface IRepository : IDisposable
    {
        Task<TEntity> GetAsync<TEntity>(int id) where TEntity : class;
        Task<DbSet<TEntity>> GetAllAsync<TEntity>() where TEntity : class;
    }
}