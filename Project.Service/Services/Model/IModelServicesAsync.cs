using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PagedList;
using Project.Common.ParamContainers;
using Project.Service.Model;

namespace Project.Service.Services
{
    public interface IModelServicesAsync : IDisposable
    {
        Task<int> AddAsync(IVehicleModel entity);
        Task<VehicleModel> FindAsync(int id);
        Task<IEnumerable<IVehicleModel>> GetAllByMakeAsync(int makeId);
        Task<IPagedList<IVehicleModel>> GetAsync(IFilteringParams filteringParams, IPagingParams pagingParams, ISortingParams sortingParams, IOptions options);
        Task<int> RemoveAsync(IVehicleModel entity);
        Task<int> UpdateAsync(IVehicleModel entity);
        Task<int> UpdateRange(IEnumerable<IVehicleModel> entities);
    }
}