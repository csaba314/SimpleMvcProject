using MvcProject.MVC.Models;
using Project.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Project.Service.Model;

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

            var model = new MakeIndexViewModel();
            model.MakeList = _service.GetAllVehicleMake(searchString, sorting, pageSize, pageNumber);

            if (model.MakeList.PageCount < model.MakeList.PageNumber)
            {
                model.MakeList = _service.GetAllVehicleMake(searchString, sorting, pageSize, 1);
            }


            if (id > 0)
            {
                model.VehicleMake = _service.GetVehicleMake(id);
                model.VehicleModels = _service.GetAllModelsByMake(id);
            }

            model.CurrentFilter = searchString;
            model.PageSizeDropdown = new SelectList(_service.GetPageSizeParamList());
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";

            return View(model);
        }

        
        // GET: /Make/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new MakeCreateEditViewModel());
        }

        // POST: /Make/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, Abrv")] MakeCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _service.AddVehicleMake(Mapper.Map<VehicleMake>(model));
            _service.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Make/Edit/1
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedMake = _service.GetVehicleMake((int)id);

            if (selectedMake == null)
            {
                return HttpNotFound();
            }
            
            return View(Mapper.Map<MakeCreateEditViewModel>(selectedMake));
        }

        //POST: /Make/Edit/1
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Name, Abrv")] MakeCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Mapper.Map(model, _service.GetVehicleMake(model.Id));
            _service.SaveChanges();

            return RedirectToAction("Index");
        }

        //GET: /Make/Delete/1
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedMake = _service.GetVehicleMake((int)id);

            if (selectedMake is null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<MakeCreateEditViewModel>(selectedMake);
            model.VehicleModels = _service.GetAllModelsByMake(model.Id);

            return View(model);
        }

        //GET: /Make/Delete/
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var modelList = _service.GetAllModelsByMake(id);
            _service.RemoveVehicleModels(modelList);

            var makeToRemove = _service.GetVehicleMake(id);
            _service.RemoveVehicleMake(makeToRemove);

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