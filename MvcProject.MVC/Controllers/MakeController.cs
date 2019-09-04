using MvcProject.MVC.Models;
using Project.Service.Services;
using Project.Service.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Project.Service.Model;
using PagedList;
using MvcProject.MVC.PresentationService;
using Autofac;
using MvcProject.MVC.Models.Factories;

namespace MvcProject.MVC.Controllers
{
    public class MakeController : Controller
    {

        private IMakeService _makeService;
        private IModelService _modelService;
        private IDomainModelFactory _domainModelFactory;
        private IIndexViewModelFactory _indexViewModelFactory;
        private IParamContainerBuilder _paramContainerBuilder;

        public MakeController(
            IMakeService makeService, 
            IModelService modelService,
            IDomainModelFactory domainModelFactory,
            IIndexViewModelFactory indexViewModelFactory,
            IParamContainerBuilder paramContainerBuilder)
        {
            _modelService = modelService;
            _makeService = makeService;
            _domainModelFactory = domainModelFactory;
            _indexViewModelFactory = indexViewModelFactory;
            _paramContainerBuilder = paramContainerBuilder;
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

            var model = _indexViewModelFactory.MakeIndexViewModelInstance();
            model.ControllerParameters = _paramContainerBuilder.BuildControllerParameters(sorting, searchString, pageSize, pageNumber);

            var mappedList = _makeService.GetAll(model.ControllerParameters)
                                         .Select(x => Mapper.Map<VehicleMakeDTO>(x));

            var list = mappedList.ToPagedList(pageNumber, pageSize);

            if (list.PageCount < list.PageNumber)
            {
                model.EntityList = mappedList.ToPagedList(1, pageSize);
            }
            else
            {
                model.EntityList = list;
            }

            
            if (id > 0)
            {
                model.Entity = Mapper.Map<VehicleMakeDTO>(_makeService.Get(id));
                var modelsList = _modelService.GetAllByMake(id);
                model.ChildEntityList = modelsList.Select(x => Mapper.Map<VehicleModelDTO>(x));
            }            

            ViewBag.PageSizeDropdown = new SelectList(PagingHelper.PageSizeDropdown);
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";

            model.ControllerParameters.CurrentFilter = searchString;
            model.ControllerParameters.Sorting = sorting;

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
            var newMake = _domainModelFactory.MakeInstance();
            Mapper.Map(model, newMake);
            _makeService.Add(newMake);
            _makeService.SaveChanges();

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

            var selectedMake = _makeService.Get((int)id);

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
            var makeToUpdate = _makeService.Get(model.Id);
            Mapper.Map(model, makeToUpdate);
            _makeService.Update(makeToUpdate);
            _makeService.SaveChanges();

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

            var selectedMake = _makeService.Get((int)id);

            if (selectedMake == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<VehicleMakeDTO>(selectedMake);
            model.VehicleModels = Mapper.Map<IEnumerable<VehicleModelDTO>>(_modelService.GetAllByMake(model.Id));

            return View(model);
        }

        //GET: /Make/Delete/
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var modelList = _modelService.GetAllByMake(id);
            _modelService.RemoveRange(modelList);

            var makeToRemove = _makeService.Get(id);
            _makeService.Remove(makeToRemove);

            _makeService.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _makeService.Dispose();
                _modelService.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}