﻿using PagedList;
using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.MVC.Models
{
    public class ModelIndexViewModel
    {
        public IPagedList<VehicleModelDTO> ModelList { get; set; }

        public VehicleModelDTO VehicleModel { get; set; }

        public string Sorting { get; set; }
        public string CurrentFilter { get; set; }
        public SelectList PageSizeDropdown { get; set; }

    }
}