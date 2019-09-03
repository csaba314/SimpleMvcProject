using System.Collections.Generic;

namespace Project.Service.Model
{
    public interface IVehicleMake
    {
        int Id { get; set; }
        string Name { get; set; }
        string Abrv { get; set; }
        IEnumerable<VehicleModel> VehicleModels { get; set; }
    }
}
