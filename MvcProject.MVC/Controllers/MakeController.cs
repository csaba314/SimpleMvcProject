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

        private readonly IMakeServicesAsync _makeService;
        private readonly IModelServicesAsync _modelService;
        private readonly IFilteringFactory _filteringFactory;
        private readonly ISortingFactory _sortingFactory;
        private readonly IPagingFactory _pagingFactory;
        private readonly IVehicleMake _vehicleMake;

        #endregion

        #region Constructor

        public MakeController(
            IMakeServicesAsync makeService, 
            IModelServicesAsync modelService,
            IFilteringFactory filteringFactory,
            ISortingFactory sortingFactory,
            IPagingFactory pagingFactory,
            IVehicleMake vehicleMake)
        {
            _makeService = makeService ?? throw new ArgumentNullException(nameof(IMakeServicesAsync));
            _modelService = modelService ?? throw new ArgumentNullException(nameof(IModelServicesAsync));
            _filteringFactory = filteringFactory ?? throw new ArgumentNullException(nameof(IFilteringFactory));
            _sortingFactory = sortingFactory ?? throw new ArgumentNullException(nameof(ISortingFactory));
            _pagingFactory = pagingFactory ?? throw new ArgumentNullException(nameof(IPagingFactory));
            _vehicleMake = vehicleMake ?? throw new ArgumentNullException(nameof(IVehicleMake));
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

            
            var filteringParams = _filteringFactory.Build(searchString, currentFilter);
            var pagingParams = _pagingFactory.Build(pageNumber, pageSize);
            var sortingParams = _sortingFactory.Build(sorting);

            if (id > 0)
            {
                var modelsList = await _modelService.GetAllByMakeAsync(id);
                model.ChildEntityList = modelsList.Select(x => Mapper.Map<VehicleModelDTO>(x));
            }

            var pagedDomainList = await _makeService.GetAllAsync(filteringParams, pagingParams, sortingParams);
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
            var newMake = _vehicleMake;
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