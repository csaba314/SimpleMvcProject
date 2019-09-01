using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Project.Service.Model;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

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

        public IEnumerable<IVehicleMake> GetAllVehicleMake(ControllerParameters parameters)
        {
            IQueryable<VehicleMake> makeList = _context.VehicleMakes;

            // Filtering
            if (!String.IsNullOrEmpty(parameters.SearchString))
            {
                makeList = makeList.Where(x => x.Name.ToLower().Contains(parameters.SearchString.ToLower()));
            }

            // Sorting
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

            return makeList.ToList();
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

        public void UpdateVehicleMake(IVehicleMake make)
        {
            if (make is VehicleMake)
            {

                DbEntityEntry makeEntry = _context.Entry(make);

                if (makeEntry.State == EntityState.Detached)
                {
                    _context.VehicleMakes.Attach((VehicleMake)make);
                }

                makeEntry.State = EntityState.Modified;

                var childModels = GetAllModelsByMake(make.Id);
                foreach (var item in childModels)
                {
                    item.Abrv = make.Abrv;
                    UpdateVehicleModel(item);
                }
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

        public IEnumerable<IVehicleModel> GetAllVehicleModels(ControllerParameters parameters)
        {
            IQueryable<VehicleModel> modelList = _context.VehicleModels.Include(m => m.VehicleMake);

            // Filtering
            if (!String.IsNullOrEmpty(parameters.SearchString))
            {
                modelList = modelList.Where(x => x.VehicleMake.Name.ToLower().Contains(parameters.SearchString.ToLower()));
            }

            // Sorting
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

            return modelList.ToList();
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


        public void UpdateVehicleModel(IVehicleModel model)
        {
            if (model is VehicleModel)
            {
                model.Abrv = GetVehicleMake(model.VehicleMakeId).Abrv;

                DbEntityEntry modelEntry = _context.Entry(model);

                if (modelEntry.State == EntityState.Detached)
                {
                    _context.VehicleModels.Attach((VehicleModel)model);
                }

                modelEntry.State = EntityState.Modified;

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
