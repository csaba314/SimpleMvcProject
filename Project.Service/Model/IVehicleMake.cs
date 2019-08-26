using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
