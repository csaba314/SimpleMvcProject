using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Model
{
    public interface IVehicleModel : IVehicle
    {
        int VehicleMakeId { get; set; }
        VehicleMake VehicleMake { get; set; }
    }
}
