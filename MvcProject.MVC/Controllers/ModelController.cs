using AutoMapper;
using MvcProject.MVC.Models;
using Project.Service.Model;
using Project.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.MVC.Controllers
{
    public class ModelController : Controller
    {
        private IVehicleService _service;

        public ModelController()
        {
            _service = new VehicleService();
        }

        // GET: /Model
        [HttpGet]
        public ActionResult Index(string searchString, string currentFilter,
                                  string sorting, int id = 0,
                                  int pageSize = 10, int pageNumber = 1)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = currentFilter;
            }

            var model = new ModelIndexViewModel();
            model.ModelList = _service.GetAllVehicleModels(searchString, sorting, pageSize, pageNumber);

            if (id > 0)
            {
                model.VehicleModel = _service.GetVehicleModel(id);
            }

            model.CurrentFilter = searchString;
            model.PageSize = pageSize;
            model.PageNumber = pageNumber;
            model.PageSizeDropdown = new SelectList(_service.GetPageSizeParamList());
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";
            ViewBag.MakeSorting = sorting == "make" ? "make_desc" : "make";

            return View(model);
        }


        // GET: /Model/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new ModelCreateEditViewModel();
            model.MakeDropdown = new SelectList(_service.GetAllVehicleMake(), "Id", "Name");
            return View(model);
        }

        // POST: /Model/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, Abrv, VehicleMakeId")] ModelCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _service.AddVehicleModel(Mapper.Map<VehicleModel>(model));
            _service.SaveChanges();

            return RedirectToAction("Index");
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