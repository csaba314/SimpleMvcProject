using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Project.Service.Model;

namespace MvcProject.MVC.Models
{
    public class MakeIndexViewModel
    {
        public IPagedList<VehicleMakeDTO> MakeList { get; set; }
        public VehicleMakeDTO VehicleMake { get; set; }

        public IEnumerable<VehicleModelDTO> VehicleModels { get; set; }

        public string Sorting { get; set; }
        public string CurrentFilter { get; set; }
        public SelectList PageSizeDropdown { get; set; }
    }
}