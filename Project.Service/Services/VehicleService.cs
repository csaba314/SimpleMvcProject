using System;
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

        public VehicleMake GetVehicleMake(int id)
        {
            return _context.VehicleMakes.Find(id);
        }

        public IEnumerable<VehicleMake> GetAllVehicleMake()
        {
            return _context.VehicleMakes.ToList();
        }

        public IPagedList<VehicleMake> GetAllVehicleMake(string searchString, string sorting, int pageSize, int pageNumber)
        {
            IQueryable<VehicleMake> makeList = _context.VehicleMakes;

            // Filtering
            if (!String.IsNullOrEmpty(searchString))
            {
                makeList = makeList.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            }

            // Sorting and Paging
            switch (sorting)
            {
                case "name_desc":
                    return makeList.OrderByDescending(x => x.Name).ToPagedList(pageNumber, pageSize);
                case "id":
                    return makeList.OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize);
                case "id_desc":
                    return makeList.OrderByDescending(x => x.Id).ToPagedList(pageNumber, pageSize);
                case "abrv":
                    return makeList.OrderBy(x => x.Abrv).ToPagedList(pageNumber, pageSize);
                case "abrv_desc":
                    return makeList.OrderByDescending(x => x.Abrv).ToPagedList(pageNumber, pageSize);
                default:
                    return makeList.OrderBy(x => x.Name).ToPagedList(pageNumber, pageSize);
            }
        }

        public void AddVehicleMake(VehicleMake make)
        {
            _context.VehicleMakes.Add(make);
        }

        public void RemoveVehicleMake(VehicleMake make)
        {
            _context.VehicleMakes.Remove(make);
        }
        #endregion




        #region Vehicle Model CRUD

        public VehicleModel GetVehicleModel(int id)
        {
            return _context.VehicleModels.Find(id);
        }

        public IEnumerable<VehicleModel> GetAllModelsByMake(int makeId)
        {
            return _context.VehicleModels.Where(m => m.VehicleMakeId == makeId).ToList();
        }

        public IPagedList<VehicleModel> GetAllVehicleModels(string searchString, string sorting, int pageSize, int pageNumber)
        {
            IQueryable<VehicleModel> modelList = _context.VehicleModels.Include(m => m.VehicleMake);

            // Filtering
            if (!String.IsNullOrEmpty(searchString))
            {
                modelList = modelList.Where(x => x.VehicleMake.Name.ToLower().Contains(searchString.ToLower()));
            }

            // Sorting and Paging
            switch (sorting)
            {
                case "name_desc":
                    return modelList.OrderByDescending(x => x.Name).ToPagedList(pageNumber, pageSize);
                case "id":
                    return modelList.OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize);
                case "id_desc":
                    return modelList.OrderByDescending(x => x.Id).ToPagedList(pageNumber, pageSize);
                case "abrv":
                    return modelList.OrderBy(x => x.Abrv).ToPagedList(pageNumber, pageSize);
                case "abrv_desc":
                    return modelList.OrderByDescending(x => x.Abrv).ToPagedList(pageNumber, pageSize);
                case "make":
                    return modelList.OrderBy(x => x.VehicleMake.Name).ToPagedList(pageNumber, pageSize);
                case "make_desc":
                    return modelList.OrderByDescending(x => x.VehicleMake.Name).ToPagedList(pageNumber, pageSize);
                default:
                    return modelList.OrderBy(x => x.Name).ToPagedList(pageNumber, pageSize);
            }
        }

        public void AddVehicleModel(VehicleModel model)
        {
            model.Abrv = _context.VehicleMakes.Find(model.VehicleMakeId).Abrv;
            _context.VehicleModels.Add(model);
        }

        public void RemoveVehicleModel(VehicleModel model)
        {
            _context.VehicleModels.Remove(model);
        }

        public void RemoveVehicleModels(IEnumerable<VehicleModel> modelList)
        {
            _context.VehicleModels.RemoveRange(modelList);
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
    }
}
