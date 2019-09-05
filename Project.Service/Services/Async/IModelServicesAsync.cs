using Project.Service.Containers;
using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services.Async
{
    interface IModelServicesAsync : IServicesAsync<VehicleModel>
    {
        Task<IEnumerable<IVehicleModel>> GetAllByMakeAsync(int makeId);
        Task<IEnumerable<IVehicleModel>> GetAllAsync(IControllerParameters parameters);
        Task<int> UpdateAsync(IVehicleModel entity);
        Task<int> RemoveRangeAsync(IEnumerable<IVehicleModel> entities);
        Task<int> AddAsync(IVehicleModel entity);
    }
}
