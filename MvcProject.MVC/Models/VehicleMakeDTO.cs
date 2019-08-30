using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Project.Service.Model;

namespace MvcProject.MVC.Models
{
    public class VehicleMakeDTO
    {
        public int Id { get; set; }

        [Display(Name="Make Name"),
            StringLength(50),
            Required]
        public string Name { get; set; }

        [Display(Name= "Abbreviation"),
            StringLength(10)]
        public string Abrv { get; set; }

        public IEnumerable<IVehicleModel> VehicleModels { get; set; }
    }
}