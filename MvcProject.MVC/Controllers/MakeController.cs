using Project.MVC.Models;
using Project.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Project.Service.Model;
using Project.MVC.PresentationService;
using System.Threading.Tasks;
using Project.Common.ParamContainers;
using PagedList;

namespace Project.MVC.Controllers
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

            var model = new IndexViewModel<VehicleMakeDTO, VehicleModelDTO>();

            
            var filteringParams = _paramsFactory.FilteringParamsInstance(searchString, currentFilter);
            var pagingParams = _paramsFactory.PagingParamsInstance(pageNumber, pageSize);
            var sortingParams = _paramsFactory.SortingParamsInstance(sorting);
            var options = _paramsFactory.OptionsInstance("VehicleModel");

            IPagedList<IVehicleMake> pagedDomainList;

            if (id > 0)
            {
                pagedDomainList = await _makeService.GetAsync(filteringParams, pagingParams, sortingParams, options);
                var modelsList = await _modelService.GetAllByMakeAsync(id);
                model.ChildEntityList = modelsList.Select(x => Mapper.Map<VehicleModelDTO>(x));
            }
            pagedDomainList = await _makeService.GetAsync(filteringParams, pagingParams, sortingParams, options);

            var mappedList = Mapper.Map<IEnumerable<VehicleMakeDTO>>(pagedDomainList);

            model.EntityList = new StaticPagedList<VehicleMakeDTO>(mappedList, pagedDomainList.GetMetaData());

            SetMessage();

            ViewBag.PageSizeDropdown = new SelectList(PagingHelper.PageSizeDropdown);
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";

            model.CurrentFilter = searchString;
            model.Sorting = sorting;
            model.Id = id;

            return View(model);
        }

        #endregion

        #region Details
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = Mapper.Map<VehicleMakeDTO>(await _makeService.FindAsync((int)id));

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        #endregion

        #region Create
        // GET: /Make/Create
        [HttpGet]
        public ActionResult Create()
        {
            var makeDTO = new VehicleMakeDTO();

            SetMessage();
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
            var newMake = new VehicleMake();
            Mapper.Map(model, newMake);

            try
            {
                await _makeService.AddAsync(newMake);
                TempData["Message"] = $"Make: {model.Name} ({model.Abrv}) successfully added to the database.";
            }
            catch (Exception e)
            {
                TempData["Message"] = $"Something went wrong: {e.Message}";
                return View(model);
            }

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

            SetMessage();
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

            try
            {
                await _makeService.UpdateAsync(makeToUpdate);
                TempData["Message"] = $"Make: {model.Name} ({model.Abrv}) successfully updated.";
            }
            catch (Exception e)
            {
                TempData["Message"] = $"Something went wrong: {e.Message}";
                return View(model);
            }
            
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
            SetMessage();
            return View(model);
        }

        //GET: /Make/Delete/
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var makeToRemove = await _makeService.FindAsync(id);

            try
            {
                await _makeService.RemoveAsync(makeToRemove);
                TempData["Message"] = $"Make: {makeToRemove.Name} ({makeToRemove.Abrv}) successfully deleted.";
            }
            catch (Exception e)
            {
                TempData["Message"] = $"Something went wrong: {e.Message}";
                return View(id);
            }

            return RedirectToAction("Index");
        }
        #endregion

        private void SetMessage()
        {
            TempData["Message"] = TempData["Message"] ?? string.Empty;
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