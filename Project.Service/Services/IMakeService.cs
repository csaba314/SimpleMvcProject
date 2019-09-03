using Project.Service.Containers;
using Project.Service.Model;
using System.Collections.Generic;


namespace Project.Service.Services
{
    public interface IMakeService : IService<IVehicleMake>
    {
        IEnumerable<IVehicleMake> GetAllVehicleMake(IControllerParameters parameters);
    }
}
