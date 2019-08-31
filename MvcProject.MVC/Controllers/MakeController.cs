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
using PagedList;

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

            var model = new IndexViewModel<VehicleMakeDTO, VehicleModelDTO>();
            var list = _service.GetAllVehicleMake(_service.SetControllerParameters(sorting, searchString, pageSize, pageNumber));
            model.EntityList = DtoMapping.MapToVehicleMakeDTO(list, list.PageSize, list.PageNumber);

            //var model = new MakeIndexViewModel();
            //var list = _service.GetAllVehicleMake(_service.SetControllerParameters(sorting, searchString, pageSize, pageNumber));
            //model.MakeList = DtoMapping.MapToVehicleMakeDTO(list, list.PageSize, list.PageNumber);

            if (id > 0)
            {
                model.Entity = Mapper.Map<VehicleMakeDTO>(_service.GetVehicleMake(id));
                var modelsList = _service.GetAllModelsByMake(id);
                model.ChildEntityList = DtoMapping.MapToVehicleModelDTO(modelsList);
            }

            model.CurrentFilter = searchString;
            model.PageSizeDropdown = new SelectList(_service.GetPageSizeParamList());
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";
            model.Sorting = sorting;

            return View(model);
        }

        
        // GET: /Make/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new VehicleMakeDTO());
        }

        // POST: /Make/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, Abrv")] VehicleMakeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var newMake = new VehicleMake();
            Mapper.Map(model, newMake);
            _service.AddVehicleMake(newMake);
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
            
            return View(Mapper.Map<VehicleMakeDTO>(selectedMake));
        }

        //POST: /Make/Edit/1
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Name, Abrv")] VehicleMakeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var makeToUpdate = _service.GetVehicleMake(model.Id);
            Mapper.Map(model, makeToUpdate);
            _service.UpdateVehicleMake(makeToUpdate);
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

            if (selectedMake == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<VehicleMakeDTO>(selectedMake);
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