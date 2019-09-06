using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.MVC.Models
{
    public class VehicleModelDTO
    {
        public int Id { get; set; }

        [Required, Display(Name = "Vehicle Model"), StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Abbreviation"), StringLength(10)]
        public string Abrv { get; set; }

        [Required, Display(Name = "Vehicle Make")]
        public int VehicleMakeId { get; set; }

        public VehicleMakeDTO VehicleMake { get; set; }
    }
}
