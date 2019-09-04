using Project.Service.Containers;
using Project.Service.Model;
using System.Collections.Generic;

namespace Project.Service.Services
{
    public interface IModelService : IService<VehicleModel>
    {
        IEnumerable<IVehicleModel> GetAllByMake(int makeId);
        IEnumerable<IVehicleModel> GetAll(IControllerParameters parameters);
        void Update(IVehicleModel entity);
        void RemoveRange(IEnumerable<IVehicleModel> entities);
        void Add(IVehicleModel model);
    }
}
