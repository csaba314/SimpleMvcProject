using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public interface IVehicleService
    {
        #region Vehicle Make
        VehicleMake GetVehicleMake(int id);
        IEnumerable<VehicleMake> GetAllVehicleMake();

        void AddVehicleMake(VehicleMake make);

        void RemoveVehicleMake(VehicleMake make);
        #endregion


        #region Vehicle Model
        VehicleModel GetVehicleModel(int id);
        IEnumerable<VehicleModel> GetAllVehicleModels();

        void AddVehicleModel(VehicleModel model);

        void RemoveVehicleModel(VehicleModel model);
        #endregion

        int SaveChanges();
    }
}
