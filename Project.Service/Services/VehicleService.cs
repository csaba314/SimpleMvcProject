using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Project.Service.Model;

namespace Project.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private ProjectDbContext _context;

        public VehicleService()
        {
            _context = ProjectDbContext.GetDbContext();
        }

        public static VehicleService GetInstance()
        {
            return new VehicleService();
        }

        #region Vehicle Make CRUD
        public VehicleMake GetVehicleMake(int id)
        {
            return _context.VehicleMakes.Find(id);
        }

        public IPagedList<VehicleMake> GetAllVehicleMake(string searchString, string sorting, int pageSize, int pageNumber)
        {
            IQueryable<VehicleMake> makeList = _context.VehicleMakes;

            // Filtering
            if (!String.IsNullOrEmpty(searchString))
            {
                makeList = FilterRecords(makeList, searchString) as IQueryable<VehicleMake>;
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

        public IPagedList<VehicleModel> GetAllVehicleModels(string searchString, string sorting, int pageSize, int pageNumber)
        {
            IQueryable<VehicleModel> modelList = _context.VehicleModels;

            // Filtering
            if (!String.IsNullOrEmpty(searchString))
            {
                modelList = FilterRecords(modelList, searchString) as IQueryable<VehicleModel>;
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
            _context.VehicleModels.Add(model);
        }

        public void RemoveVehicleModel(VehicleModel model)
        {
            _context.VehicleModels.Remove(model);
        }
        #endregion

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }


        private IQueryable<IVehicle> FilterRecords(IQueryable<IVehicle> list, string searchString)
        {
            return list.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
        }
    }
}
