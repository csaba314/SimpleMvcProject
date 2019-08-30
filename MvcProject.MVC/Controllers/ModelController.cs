using AutoMapper;
using MvcProject.MVC.Models;
using Project.Service.Model;
using Project.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                                  string sorting, int pageSize = 10, int pageNumber = 1)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = currentFilter;
            }

            var model = new ModelIndexViewModel();
            model.ModelList = _service.GetAllVehicleModels(_service.SetControllerParameters(sorting, searchString, pageSize, pageNumber));

            model.CurrentFilter = searchString;
            model.PageSizeDropdown = new SelectList(_service.GetPageSizeParamList());
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";
            ViewBag.MakeSorting = sorting == "make" ? "make_desc" : "make";
            model.Sorting = sorting;

            return View(model);
        }


        // GET: /Model/Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new ModelCreateEditViewModel();
            model.MakeDropdown = GetMakeDropDown();
            return View(model);
        }

        // POST: /Model/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, VehicleMakeId")] ModelCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _service.AddVehicleModel(Mapper.Map<IVehicleModel>(model));
            _service.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Model/Edit/1
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedModel = _service.GetVehicleModel((int)id);

            if (selectedModel == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<ModelCreateEditViewModel>(selectedModel);
            model.MakeDropdown = GetMakeDropDown();

            return View(model);
        }

        //POST: /Model/Edit/1
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Name, VehicleMakeId")] ModelCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var editedModel = _service.GetVehicleModel(model.Id);
            Mapper.Map(model, editedModel);
            editedModel.Abrv = _service.GetVehicleMake(editedModel.VehicleMakeId).Abrv;
            _service.SaveChanges();

            return RedirectToAction("Index");
        }

        //GET: /Model/Delete/1
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedModel = _service.GetVehicleModel((int)id);

            if (selectedModel == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<ModelCreateEditViewModel>(selectedModel));
        }

        //GET: /Model/Delete/1
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var modelToRemove = _service.GetVehicleModel(id);
            _service.RemoveVehicleModel(modelToRemove);
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

        private SelectList GetMakeDropDown()
        {
            return new SelectList(_service.GetAllVehicleMake(), "Id", "Name");
        }
    }
}