using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Model
{
    public interface IVehicleModel
    {
        int Id { get; set; }
        string Name { get; set; }
        string Abrv { get; set; }
        int VehicleMakeId { get; set; }
        VehicleMake VehicleMake { get; set; }
    }
}
