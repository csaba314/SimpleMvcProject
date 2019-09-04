using Project.Service.Containers;
using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Project.Service.Services
{
    public class ModelService : Service<VehicleModel>, IModelService
    {
        private ProjectDbContext context { get { return _context as ProjectDbContext; } }

        public ModelService(ProjectDbContext context) : base(context)
        {

        }

       
        public IEnumerable<IVehicleModel> GetAllByMake(int makeId)
        {
            return context.VehicleModels
                .Where(x => x.VehicleMakeId == makeId)
                .ToList();
        }

        public IEnumerable<IVehicleModel> GetAll(IControllerParameters parameters)
        {
            IQueryable<VehicleModel> modelList;

            if (!parameters.Options.LoadMakesWithModel)
            {
                modelList = context.VehicleModels;
            }
            else
            {
                modelList = context.VehicleModels.Include(m => m.VehicleMake);
            }

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
        
        public void Add(IVehicleModel model)
        {
            if (model is VehicleModel)
            {
                model.Abrv = context.VehicleMakes.Find(model.VehicleMakeId).Abrv;
                base.Add(model as VehicleModel);
            }
            else
            {
                throw new ArgumentException();
            }

        }

        public void Update(IVehicleModel model)
        {
            if (model is VehicleModel)
            {
                model.Abrv = context.VehicleMakes.Find(model.VehicleMakeId).Abrv;
                base.Update(model as VehicleModel);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void RemoveRange(IEnumerable<IVehicleModel> models)
        {
            if (models is IEnumerable<VehicleModel>)
            {
                base.RemoveRange(models as IEnumerable<VehicleModel>);

            }
        }
    }
}
