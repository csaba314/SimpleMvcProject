﻿using PagedList;
using Project.Service.Model;
using Project.Common.ParamContainers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public interface IMakeServicesAsync : IServicesAsync<VehicleMake>
    {
        Task<IPagedList<IVehicleMake>> GetAsync(IFilteringParams filteringParams, IPagingParams pagingParams, ISortingParams sortingParams);
        Task<int> UpdateAsync(IVehicleMake entity);
        Task<int> AddAsync(IVehicleMake entity);
        Task<IVehicleMake> FindAsync(int id);
        Task<int> RemoveAsync(IVehicleMake entity);
        Task<IEnumerable<IVehicleMake>> GetMakeDropdown();
    }
}
