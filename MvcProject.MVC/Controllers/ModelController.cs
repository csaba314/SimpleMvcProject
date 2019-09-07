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

            var model = DependencyResolver.Current.GetService<IndexViewModel<VehicleModelDTO, string>>();

            ViewModelManager.SetParams(ref model, _paramsFactory, searchString, currentFilter, sorting, pageSize, pageNumber, true);


            var pagedDomainList = await _modelService.GetAsync(model.FilteringParams, model.PagingParams, model.SortingParams, model.Options);

            if (pagedDomainList.PageNumber > pagedDomainList.PageCount)
            {
                model.PagingParams.PageNumber = 1;
                pagedDomainList = await _modelService.GetAsync(model.FilteringParams, model.PagingParams, model.SortingParams, model.Options);
            }

            model.EntityList = PagedListMapper.ToMappedPagedList<IVehicleModel, VehicleModelDTO>(pagedDomainList);



            model.FilteringParams.CurrentFilter = searchString;
            ViewBag.PageSizeDropdown = new SelectList(PagingHelper.PageSizeDropdown);
            ViewBag.IdSorting = sorting == "id" ? "id_desc" : "id";
            ViewBag.NameSorting = string.IsNullOrEmpty(sorting) ? "name_desc" : "";
            ViewBag.AbrvSorting = sorting == "abrv" ? "abrv_desc" : "abrv";
            ViewBag.MakeSorting = sorting == "make" ? "make_desc" : "make";
            model.SortingParams.Sorting = sorting;

            return View(model);
        }

        #endregion

        #region Create

        // GET: /Model/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var modelDTO = DependencyResolver.Current.GetService<VehicleModelDTO>();
            ViewBag.MakeDropdown = await GetMakeDropDownAsync();
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

            await _modelService.AddAsync(newModel);

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

            await _modelService.UpdateAsync(editedModel);

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
            return View(Mapper.Map<VehicleModelDTO>(selectedModel));
        }

        //GET: /Model/Delete/1
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var modelToRemove = await _modelService.FindAsync(id);

            await _modelService.RemoveAsync(modelToRemove);

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
    }
}