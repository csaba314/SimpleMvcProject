using System;
using System.Collections.Generic;

namespace Project.Service.Services
{
    public interface IService<TEntity> : IDisposable where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        void SaveChanges();
    }
}
