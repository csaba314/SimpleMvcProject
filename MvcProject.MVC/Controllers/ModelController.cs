using AutoMapper;
using Project.MVC.Models;
using Project.Service.Model;
using Project.Service.Services;
using System;
using System.Net;
using System.Web.Mvc;
using Project.MVC.PresentationService;
using System.Threading.Tasks;
using Project.Common.ParamContainers;

namespace Project.MVC.Controllers
{
    public class ModelController : Controller
    {
        #region Private Properties

        private IMakeServicesAsync _makeService;
        private IModelServicesAsync _modelService;
        private IParamsFactory _paramsFactory;

        #endregion

        #region Constructor

        public ModelController(
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
        // GET: /Model
        [HttpGet]
        public async Task<ActionResult> Index(string searchString, string currentFilter,
                                  string sorting, int pageSize = 10, int pageNumber = 1)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = currentFilter;
            }

            var model = new IndexViewModel<VehicleModelDTO, string>();

            var filteringParams = _paramsFactory.FilteringParamsInstance(searchString, currentFilter);
            var pagingParams = _paramsFactory.PagingParamsInstance(pageNumber, pageSize);
            var sortingParams = _paramsFactory.SortingParamsInstance(sorting);
            var options = _paramsFactory.OptionsInstance(true);

            var pagedDomainList = await _modelService.GetAsync(filteringParams, pagingParams, sortingParams, options);

            if (pagedDomainList.PageNumber > pagedDomainList.PageCount)
            {
                model.PageNumber = 1;
                pagedDomainList = await _modelService.GetAsync(filteringParams, pagingParams, sortingParams, options);
            }

            model.EntityList = PagedListMapper.ToMappedPagedList<IVehicleModel, VehicleModelDTO>(pagedDomainList);

            SetMessage();

            model.CurrentFilter = searchString;
            ViewBag.PageSizeDropdown = new SelectList(PagingHelper.PageSizeDropdown);
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";
            ViewBag.MakeSorting = sorting == "make" ? "make_desc" : "make";
            model.Sorting = sorting;

            return View(model);
        }

        #endregion

        #region Create

        // GET: /Model/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var modelDTO = new VehicleModelDTO();
            ViewBag.MakeDropdown = await GetMakeDropDownAsync();
            SetMessage();
            return View(modelDTO);
        }

        // POST: /Model/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Name, VehicleMakeId")] VehicleModelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var newModel = DependencyResolver.Current.GetService<IVehicleModel>();
            Mapper.Map(model, newModel);

            try
            {
                await _modelService.AddAsync(newModel);
                TempData["Message"] = $"Model: {model.Name} successfully added to the database.";
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
        // GET: /Model/Edit/1
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedModel = await _modelService.FindAsync((int)id);

            if (selectedModel == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<VehicleModelDTO>(selectedModel);
            ViewBag.MakeDropdown = await GetMakeDropDownAsync();

            SetMessage();
            return View(model);
        }

        //POST: /Model/Edit/1
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id, Name, VehicleMakeId")] VehicleModelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var editedModel = await _modelService.FindAsync(model.Id);
            Mapper.Map(model, editedModel);

            try
            {
                await _modelService.UpdateAsync(editedModel);
                TempData["Message"] = $"Model: {model.Name} successfully updated.";
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
        //GET: /Model/Delete/1
        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedModel = await _modelService.FindAsync((int)id);

            if (selectedModel == null)
            {
                return HttpNotFound();
            }

            SetMessage();
            return View(Mapper.Map<VehicleModelDTO>(selectedModel));
        }

        //GET: /Model/Delete/1
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var modelToRemove = await _modelService.FindAsync(id);

            try
            {
                await _modelService.RemoveAsync(modelToRemove);
                TempData["Message"] = $"Model: {modelToRemove.Name} successfully deleted.";
            }
            catch (Exception e)
            {
                TempData["Message"] = $"Something went wrong: {e.Message}";
                return View(id);
            }

            return RedirectToAction("Index");
        }
        #endregion


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _modelService.Dispose();
                _makeService.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<SelectList> GetMakeDropDownAsync()
        {
            return new SelectList(await _makeService.GetMakeDropdown(), "Id", "Name");
        }

        private void SetMessage()
        {
            TempData["Message"] = TempData["Message"] ?? string.Empty;
        }
    }
}