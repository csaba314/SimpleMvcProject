using PagedList;
using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public interface IVehicleService : IDisposable
    {
        #region Vehicle Make
        VehicleMake GetVehicleMake(int id);
        IPagedList<VehicleMake> GetAllVehicleMake(string searchString, string sorting, int pageSize, int pageNumber);

        void AddVehicleMake(VehicleMake make);

        void RemoveVehicleMake(VehicleMake make);
        #endregion


        #region Vehicle Model
        VehicleModel GetVehicleModel(int id);
        IPagedList<VehicleModel> GetAllVehicleModels(string searchString, string sorting, int pageSize, int pageNumber);

        void AddVehicleModel(VehicleModel model);

        void RemoveVehicleModel(VehicleModel model);
        #endregion

        int SaveChanges();

        IEnumerable<int> GetPageSizeParamList();
    }
}
