using System.Collections.Generic;

namespace Project.Service.Model
{
    public class VehicleMake : IVehicleMake
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }

        public IEnumerable<VehicleModel> VehicleModels { get; set; }
    }
}
