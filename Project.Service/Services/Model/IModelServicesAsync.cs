using PagedList;
using Project.Service.DTO;
using Project.Service.Model;
using Project.Service.ParamContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public interface IModelServicesAsync : IServicesAsync<VehicleModel>
    {
        Task<IEnumerable<IVehicleModel>> GetAllByMakeAsync(int makeId);
        Task<IPagedList<VehicleModelDTO>> GetAsync(IFilteringParams filteringParams, 
                                                  IPagingParams pagingParams, 
                                                  ISortingParams sortingParams, 
                                                  IOptions options);
        Task<int> UpdateAsync(IVehicleModel entity);
        Task<int> RemoveRangeAsync(IEnumerable<IVehicleModel> entities);
        Task<int> AddAsync(IVehicleModel entity);
        Task<IVehicleModel> FindAsync(int id);
        Task<int> RemoveAsync(IVehicleModel entity);
    }
}
