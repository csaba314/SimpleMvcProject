using Project.Service.Containers;
using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services.Async
{
    public interface IMakeServicesAsync : IServicesAsync<VehicleMake>
    {
        Task<IEnumerable<IVehicleMake>> GetAllAsync(IControllerParameters parameters);
        Task<int> UpdateAsync(IVehicleMake entity);
        Task<int> AddAsync(IVehicleMake entity);
    }
}
