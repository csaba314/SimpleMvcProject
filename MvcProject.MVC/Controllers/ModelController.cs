﻿using AutoMapper;
using MvcProject.MVC.Models;
using PagedList;
using Project.Service.Model;
using Project.Service.Services;
using Project.Service.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcProject.MVC.PresentationService;
using MvcProject.MVC.Models.Factories;

namespace MvcProject.MVC.Controllers
{
    public class ModelController : Controller
    {
        private IMakeService _makeService;
        private IModelService _modelService;
        private IIndexViewModelFactory _indexViewModelFactory;
        private IDTOFactory _dtoFactory;
        private IDomainModelFactory _domainModelFactory;
        private IParamContainerBuilder _paramContainerBuilder;


        public ModelController(
            IMakeService makeService, 
            IModelService modelService,
            IIndexViewModelFactory indexViewModelFactory,
            IDTOFactory dtoFactory,
            IDomainModelFactory domainModelFactory,
            IParamContainerBuilder paramContainerBuilder)
        {
            _modelService = modelService;
            _makeService = makeService;
            _indexViewModelFactory = indexViewModelFactory;
            _dtoFactory = dtoFactory;
            _domainModelFactory = domainModelFactory;
            _paramContainerBuilder = paramContainerBuilder;
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

            var model = _indexViewModelFactory.ModelIndexViewModelInstance();

            model.ControllerParameters = _paramContainerBuilder.BuildControllerParameters(
                sorting, searchString, pageSize, pageNumber, _paramContainerBuilder.BuildLoadingOptions(true));

            var mappedList = _modelService.GetAll(model.ControllerParameters)
                                          .Select(x => Mapper.Map<VehicleModelDTO>(x));

            var list = mappedList.ToPagedList(pageNumber, pageSize);

            if (list.PageCount < list.PageNumber)
            {
                model.EntityList = mappedList.ToPagedList(1, pageSize);
            } else
            {
                model.EntityList = list;
            }


            model.ControllerParameters.CurrentFilter = searchString;
            ViewBag.PageSizeDropdown = new SelectList(PagingHelper.PageSizeDropdown);
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";
            ViewBag.MakeSorting = sorting == "make" ? "make_desc" : "make";
            model.ControllerParameters.Sorting = sorting;

            return View(model);
        }


        // GET: /Model/Create
        [HttpGet]
        public ActionResult Create()
        {
            var modelDTO = _dtoFactory.ModelDTOInstance();
            ViewBag.MakeDropdown = GetMakeDropDown();
            return View(modelDTO);
        }

        // POST: /Model/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, VehicleMakeId")] VehicleModelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var newModel = _domainModelFactory.ModelInstance();
            Mapper.Map(model, newModel);
            _modelService.Add(newModel);
            _modelService.SaveChanges();

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

            var selectedModel = _modelService.Get((int)id);

            if (selectedModel == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<VehicleModelDTO>(selectedModel);
            ViewBag.MakeDropdown = GetMakeDropDown();

            return View(model);
        }

        //POST: /Model/Edit/1
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Name, VehicleMakeId")] VehicleModelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var editedModel = _modelService.Get(model.Id);
            Mapper.Map(model, editedModel);
            _modelService.Update(editedModel);
            _modelService.SaveChanges();

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

            var selectedModel = _modelService.Get((int)id);

            if (selectedModel == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<VehicleModelDTO>(selectedModel));
        }

        //GET: /Model/Delete/1
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var modelToRemove = _modelService.Get(id);
            _modelService.Remove(modelToRemove);
            _modelService.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _modelService.Dispose();
            }
            base.Dispose(disposing);
        }

        private SelectList GetMakeDropDown()
        {
            return new SelectList(_makeService.GetAll(), "Id", "Name");
        }
    }
}