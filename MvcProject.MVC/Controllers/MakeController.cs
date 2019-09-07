using MvcProject.MVC.Models;
using Project.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Project.Service.Model;
using MvcProject.MVC.PresentationService;
using System.Threading.Tasks;
using Project.Service.ParamContainers;

namespace MvcProject.MVC.Controllers
{
    public class MakeController : Controller
    {
        #region Private Properties

        private IMakeServicesAsync _makeService;
        private IModelServicesAsync _modelService;
        private IParamsFactory _paramsFactory;

        #endregion

        #region Constructor

        public MakeController(
            IMakeServicesAsync makeService, 
            IModelServicesAsync modelService,
            IParamsFactory paramsFactory)
        {
            _modelService = modelService;
            _makeService = makeService;
            _paramsFactory = paramsFactory;
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

            ViewModelManager.SetParams(ref model, _paramsFactory, searchString, currentFilter, sorting, pageSize, pageNumber);


            var pagedDomainList = await _makeService.GetAsync(model.FilteringParams, model.PagingParams, model.SortingParams);

            if (pagedDomainList.PageNumber > pagedDomainList.PageCount)
            {
                model.PagingParams.PageNumber = 1;
                pagedDomainList = await _makeService.GetAsync(model.FilteringParams, model.PagingParams, model.SortingParams);
            }

            model.EntityList = PagedListMapper.ToMappedPagedList<IVehicleMake, VehicleMakeDTO>(pagedDomainList);


            if (id > 0)
            {
                model.Entity = Mapper.Map<VehicleMakeDTO>(await _makeService.FindAsync(id));
                var modelsList = await _modelService.GetAllByMakeAsync(id);
                model.ChildEntityList = modelsList.Select(x => Mapper.Map<VehicleModelDTO>(x));
            }            

            ViewBag.PageSizeDropdown = new SelectList(PagingHelper.PageSizeDropdown);
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";

            model.FilteringParams.CurrentFilter = searchString;
            model.SortingParams.Sorting = sorting;

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
            string message = string.Empty;

            await _makeService.AddAsync(newMake);

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

            var selectedMake = await _makeService.FindAsync((int)id);

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
            var makeToUpdate = await _makeService.FindAsync(model.Id);
            Mapper.Map(model, makeToUpdate);

            await _makeService.UpdateAsync(makeToUpdate);

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

            var selectedMake = await _makeService.FindAsync((int)id);

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
            var makeToRemove = await _makeService.FindAsync(id);

            await _makeService.RemoveAsync(makeToRemove);

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