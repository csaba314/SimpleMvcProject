using AutoMapper;
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
using System.Threading.Tasks;

namespace MvcProject.MVC.Controllers
{
    public class ModelController : Controller
    {
        #region Private Properties

        private IMakeServicesAsync _makeService;
        private IModelServicesAsync _modelService;
        private IIndexViewModelFactory _indexViewModelFactory;
        private IDTOFactory _dtoFactory;
        private IDomainModelFactory _domainModelFactory;
        private IParamContainerBuilder _paramContainerBuilder;

        #endregion

        #region Constructor

        public ModelController(
            IMakeServicesAsync makeService,
            IModelServicesAsync modelService,
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

            var model = _indexViewModelFactory.ModelIndexViewModelInstance();

            model.ControllerParameters = _paramContainerBuilder.BuildControllerParameters(
                sorting, searchString, pageSize, pageNumber, _paramContainerBuilder.BuildLoadingOptions(true));

            var mappedList = await _modelService.GetAllAsync(model.ControllerParameters);

            var list = mappedList.Select(x => Mapper.Map<VehicleModelDTO>(x)).ToPagedList(pageNumber, pageSize);

            if (list.PageCount < list.PageNumber)
            {
                model.EntityList = mappedList.Select(x => Mapper.Map<VehicleModelDTO>(x)).ToPagedList(1, pageSize);
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
        #endregion

        #region Create

        // GET: /Model/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var modelDTO = _dtoFactory.ModelDTOInstance();
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
            var newModel = _domainModelFactory.ModelInstance();
            Mapper.Map(model, newModel);
            await _modelService.AddAsync(newModel);
            await _modelService.SaveChangesAsync();

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

            var selectedModel = await _modelService.GetAsync((int)id);

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

            var editedModel = await _modelService.GetAsync(model.Id);
            Mapper.Map(model, editedModel);
            await _modelService.UpdateAsync(editedModel);
            await _modelService.SaveChangesAsync();

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

            var selectedModel = await _modelService.GetAsync((int)id);

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
            var modelToRemove = await _modelService.GetAsync(id);
            await _modelService.RemoveAsync(modelToRemove);
            await _modelService.SaveChangesAsync();

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
            return new SelectList(await _makeService.GetAllAsync(), "Id", "Name");
        }
    }
}