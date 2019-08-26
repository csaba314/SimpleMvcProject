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
        public IPagedList<IVehicleMake> MakeList { get; set; }
        public IVehicleMake VehicleMake { get; set; }

        public string Sorting { get; set; }
        public string CurrentFilter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public SelectList PageSizeDropdown { get; set; }
    }
}