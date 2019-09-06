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
using System.Threading.Tasks;

namespace MvcProject.MVC.Controllers
{
    public class MakeController : Controller
    {
        #region Private Properties

        private IMakeServicesAsync _makeService;
        private IModelServicesAsync _modelService;
        private IParamContainerBuilder _paramContainerBuilder;

        #endregion

        #region Constructor

        public MakeController(
            IMakeServicesAsync makeService, 
            IModelServicesAsync modelService,
            IParamContainerBuilder paramContainerBuilder)
        {
            _modelService = modelService;
            _makeService = makeService;
            _paramContainerBuilder = paramContainerBuilder;
        }
        #endregion

        #region Index
        // GET: /Make
        [HttpGet]
        public async Task<ActionResult> Index(string searchString, string currentFilter,
                                  string sorting, int id = 0,
                                  int pageSize = 10, int pageNumber = 1)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = currentFilter;
            }

            var model = DependencyResolver.Current.GetService<IndexViewModel<VehicleMakeDTO, VehicleModelDTO>>();

               model.ControllerParameters = _paramContainerBuilder.BuildControllerParameters(sorting, searchString, pageSize, pageNumber);

            var mappedList = await _makeService.GetAllAsync(model.ControllerParameters);

            var list = mappedList.Select(x => Mapper.Map<VehicleMakeDTO>(x)).ToPagedList(pageNumber, pageSize);

            if (list.PageCount < list.PageNumber)
            {
                model.EntityList = mappedList.Select(x => Mapper.Map<VehicleMakeDTO>(x)).ToPagedList(1, pageSize);
            }
            else
            {
                model.EntityList = list;
            }

            
            if (id > 0)
            {
                model.Entity = Mapper.Map<VehicleMakeDTO>(await _makeService.GetAsync(id));
                var modelsList = await _modelService.GetAllByMakeAsync(id);
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

        #endregion

        #region Create
        // GET: /Make/Create
        [HttpGet]
        public ActionResult Create()
        {
            var makeDTO = DependencyResolver.Current.GetService<VehicleMakeDTO>();
            return View(makeDTO);
        }

        // POST: /Make/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Name, Abrv")] VehicleMakeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var newMake = DependencyResolver.Current.GetService<IVehicleMake>();
            Mapper.Map(model, newMake);
            await _makeService.AddAsync(newMake);
            await _makeService.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        // GET: /Make/Edit/1
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedMake = await _makeService.GetAsync((int)id);

            if (selectedMake == null)
            {
                return HttpNotFound();
            }
            
            return View(Mapper.Map<VehicleMakeDTO>(selectedMake));
        }

        //POST: /Make/Edit/1
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id, Name, Abrv")] VehicleMakeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var makeToUpdate = await _makeService.GetAsync(model.Id);
            Mapper.Map(model, makeToUpdate);
            await _makeService.UpdateAsync(makeToUpdate);
            await _makeService.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        //GET: /Make/Delete/1
        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedMake = await _makeService.GetAsync((int)id);

            if (selectedMake == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<VehicleMakeDTO>(selectedMake);
            model.VehicleModels = Mapper.Map<IEnumerable<VehicleModelDTO>>(await _modelService.GetAllByMakeAsync(model.Id));

            return View(model);
        }

        //GET: /Make/Delete/
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var modelList = await _modelService.GetAllByMakeAsync(id);
            await _modelService.RemoveRangeAsync(modelList);

            var makeToRemove = await _makeService.GetAsync(id);
            await _makeService.RemoveAsync(makeToRemove);

            await _makeService.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion


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