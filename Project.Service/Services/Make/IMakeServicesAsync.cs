using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PagedList;
using Project.Common.ParamContainers;
using Project.Service.Model;

namespace Project.Service.Services
{
    public interface IMakeServicesAsync : IDisposable
    {
        Task<int> AddAsync(IVehicleMake entity);
        Task<IVehicleMake> FindAsync(int id);
        Task<IPagedList<IVehicleMake>> GetAllAsync(IFilteringParams filteringParams, IPagingParams pagingParams, ISortingParams sortingParams);
        Task<IEnumerable<IVehicleMake>> GetMakeDropdown();
        Task<int> RemoveAsync(IVehicleMake entity);
        Task<int> UpdateAsync(IVehicleMake entity);
    }
}