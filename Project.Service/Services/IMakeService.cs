using Project.Service.Containers;
using Project.Service.Model;
using System.Collections.Generic;


namespace Project.Service.Services
{
    public interface IMakeService : IService<VehicleMake>
    {
        IEnumerable<IVehicleMake> GetAll(IControllerParameters parameters);
        void Update(IVehicleMake entity);
        void Add(IVehicleMake make);
    }
}
