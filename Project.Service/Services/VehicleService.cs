﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Project.Service.Model;
using System.Data.Entity;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private ProjectDbContext _context;

        public VehicleService()
        {
            _context = ProjectDbContext.GetDbContext();
        }


        #region Vehicle Make CRUD

        public IVehicleMake GetVehicleMake(int id)
        {
            return _context.VehicleMakes.Find(id);
        }

        public IEnumerable<IVehicleMake> GetAllVehicleMake()
        {
            return _context.VehicleMakes.ToList();
        }

        public IPagedList<IVehicleMake> GetAllVehicleMake(ControllerParameters parameters)
        {
            IQueryable<VehicleMake> makeList = _context.VehicleMakes;

            // Filtering
            if (!String.IsNullOrEmpty(parameters.SearchString))
            {
                makeList = makeList.Where(x => x.Name.ToLower().Contains(parameters.SearchString.ToLower()));
            }

            switch (parameters.Sorting)
            {
                case "name_desc":
                    makeList = makeList.OrderByDescending(x => x.Name);
                    break;
                case "id":
                    makeList = makeList.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    makeList = makeList.OrderByDescending(x => x.Id);
                    break;
                case "abrv":
                    makeList = makeList.OrderBy(x => x.Abrv);
                    break;
                case "abrv_desc":
                    makeList = makeList.OrderByDescending(x => x.Abrv);
                    break;
                default:
                    makeList = makeList.OrderBy(x => x.Name);
                    break;
            }

            var list = makeList.ToPagedList(parameters.PageNumber, parameters.PageSize);

            if (list.PageCount < list.PageNumber)
            {
                return makeList.ToPagedList(1, parameters.PageSize);
            }

            return list;
        }

        public void AddVehicleMake(IVehicleMake make)
        {
            if (make is VehicleMake)
            {
                _context.VehicleMakes.Add(make as VehicleMake);
            }
            else
            {
                throw new ArgumentException();
            }

        }

        public void RemoveVehicleMake(IVehicleMake make)
        {
            if (make is VehicleMake)
            {
                _context.VehicleMakes.Remove(make as VehicleMake);
            }
            else
            {
                throw new ArgumentException();
            }

        }
        #endregion




        #region Vehicle Model CRUD

        public IVehicleModel GetVehicleModel(int id)
        {
            return _context.VehicleModels.Find(id);
        }

        public IEnumerable<IVehicleModel> GetAllModelsByMake(int makeId)
        {
            return _context.VehicleModels.Where(m => m.VehicleMakeId == makeId).ToList();
        }

        public IPagedList<IVehicleModel> GetAllVehicleModels(ControllerParameters parameters)
        {
            IQueryable<VehicleModel> modelList = _context.VehicleModels.Include(m => m.VehicleMake);

            // Filtering
            if (!String.IsNullOrEmpty(parameters.SearchString))
            {
                modelList = modelList.Where(x => x.VehicleMake.Name.ToLower().Contains(parameters.SearchString.ToLower()));
            }

            // Sorting and Paging
            switch (parameters.Sorting)
            {
                case "name_desc":
                    modelList = modelList.OrderByDescending(x => x.Name);
                    break;
                case "id":
                    modelList = modelList.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    modelList = modelList.OrderByDescending(x => x.Id);
                    break;
                case "abrv":
                    modelList = modelList.OrderBy(x => x.Abrv);
                    break;
                case "abrv_desc":
                    modelList = modelList.OrderByDescending(x => x.Abrv);
                    break;
                case "make":
                    modelList = modelList.OrderBy(x => x.VehicleMake.Name);
                    break;
                case "make_desc":
                    modelList = modelList.OrderByDescending(x => x.VehicleMake.Name);
                    break;
                default:
                    modelList = modelList.OrderBy(x => x.Name);
                    break;
            }

            var list = modelList.ToPagedList(parameters.PageNumber, parameters.PageSize);

            if (list.PageCount < list.PageNumber)
            {
                return modelList.ToPagedList(1, parameters.PageSize);
            }

            return list;
        }

        public void AddVehicleModel(IVehicleModel model)
        {
            model.Abrv = _context.VehicleMakes.Find(model.VehicleMakeId).Abrv;

            if (model is VehicleModel)
            {
                _context.VehicleModels.Add(model as VehicleModel);
            }
            else
            {
                throw new ArgumentException();
            }

        }

        public void RemoveVehicleModel(IVehicleModel model)
        {
            if (model is VehicleModel)
            {
                _context.VehicleModels.Remove(model as VehicleModel);

            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void RemoveVehicleModels(IEnumerable<IVehicleModel> modelList)
        {
            if (modelList is IEnumerable<VehicleModel>)
            {
                _context.VehicleModels.RemoveRange(modelList as IEnumerable<VehicleModel>);
            }
            else
            {
                throw new ArgumentException();
            }

        }
        #endregion

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<int> GetPageSizeParamList()
        {
            return new List<int> { 5, 10, 20, 40 };
        }

        public ControllerParameters SetControllerParameters(string sorting, string searchString, int pageSize, int pageNumber)
        {
            var parameters = new ControllerParameters
            {
                Sorting = sorting,
                SearchString = searchString,
                PageSize = pageSize,
                PageNumber = pageNumber
            };


            return parameters;
        }
    }
}
