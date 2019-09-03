using Project.Service.Containers;
using Project.Service.Model;
using System.Collections.Generic;

namespace Project.Service.Services
{
    public interface IModelService : IService<IVehicleModel>
    {
        IEnumerable<IVehicleModel> GetAllByMake(int makeId);

        IEnumerable<IVehicleModel> GetAll(IControllerParameters parameters);
    }
}
