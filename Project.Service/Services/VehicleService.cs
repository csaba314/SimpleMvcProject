using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        #region Vehicle Make
        public VehicleMake GetVehicleMake(int id)
        {
            return _context.VehicleMakes.Find(id);
        }

        public IEnumerable<VehicleMake> GetAllVehicleMake()
        {
            return _context.VehicleMakes;
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




        #region Vehicle Model
        public VehicleModel GetVehicleModel(int id)
        {
            return _context.VehicleModels.Find(id);
        }

        public IEnumerable<VehicleModel> GetAllVehicleModels()
        {
            return _context.VehicleModels;
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
    }
}
