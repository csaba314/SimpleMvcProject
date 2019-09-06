using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.DTO
{
    public class VehicleMakeDTO
    {
        public int Id { get; set; }

        [Display(Name = "Make Name"),
            StringLength(50),
            Required]
        public string Name { get; set; }

        [Display(Name = "Abbreviation"),
            StringLength(10)]
        public string Abrv { get; set; }

        public IEnumerable<VehicleModelDTO> VehicleModels { get; set; }
    }
}
