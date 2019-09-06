using PagedList;
using Project.Service.Model;
using Project.Service.ParamContainers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public interface IModelServicesAsync : IServicesAsync<VehicleModel>
    {
        Task<IEnumerable<IVehicleModel>> GetAllByMakeAsync(int makeId);
        Task<IPagedList<IVehicleModel>> GetAsync(IFilteringParams filteringParams, 
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
