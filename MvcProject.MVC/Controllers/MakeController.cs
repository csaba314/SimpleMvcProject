using MvcProject.MVC.Models;
using Project.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.MVC.Controllers
{
    public class MakeController : Controller
    {

        private IVehicleService _service;

        public MakeController()
        {
            _service = new VehicleService();
        }

        // GET: /Make
        [HttpGet]
        public ActionResult Index(string searchString, string currentFilter,
                                  string sorting, int id = 0,
                                  int pageSize = 10, int pageNumber = 1)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = currentFilter;
            }

            var model = MakeIndexViewModel.GetModel();
            model.MakeList = _service.GetAllVehicleMake(searchString, sorting, pageSize, pageNumber);

            if (id > 0)
            {
                model.VehicleMake = _service.GetVehicleMake(id);
            }

            model.CurrentFilter = searchString;
            model.PageSize = pageSize;
            model.PageNumber = pageNumber;
            model.PageSizeDropdown = new SelectList(_service.GetPageSizeParamList());
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";


            return View(model);
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}