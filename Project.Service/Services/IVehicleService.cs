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
        IVehicleMake GetVehicleMake(int id);
        IPagedList<IVehicleMake> GetAllVehicleMake(string searchString, string sorting, int pageSize, int pageNumber);
        IEnumerable<IVehicleMake> GetAllVehicleMake();

        void AddVehicleMake(IVehicleMake make);

        void RemoveVehicleMake(IVehicleMake make);
        #endregion


        #region Vehicle Model
        IVehicleModel GetVehicleModel(int id);
        IPagedList<IVehicleModel> GetAllVehicleModels(string searchString, string sorting, int pageSize, int pageNumber);
        IEnumerable<IVehicleModel> GetAllModelsByMake(int makeId);

        void AddVehicleModel(IVehicleModel model);

        void RemoveVehicleModel(IVehicleModel model);
        void RemoveVehicleModels(IEnumerable<IVehicleModel> modelList);
        #endregion

        int SaveChanges();

        IEnumerable<int> GetPageSizeParamList();
    }
}
