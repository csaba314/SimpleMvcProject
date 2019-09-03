using System;
using System.Collections.Generic;
using System.Linq;
using Project.Service.Containers;
using Project.Service.Model;

namespace Project.Service.Services
{
    public class MakeService : Service<IVehicleMake>, IMakeService
    {

        private ProjectDbContext context { get { return _context as ProjectDbContext; } }
        private IModelService _modelService;

        public MakeService(ProjectDbContext context, IModelService modelService) : base(context)
        {
            this._modelService = modelService;
        } 

        public IEnumerable<IVehicleMake> GetAllVehicleMake(IControllerParameters parameters)
        {
            IQueryable<VehicleMake> makeList = context.VehicleMakes;

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

        public override void Update(IVehicleMake entity)
        {
            base.Update(entity);
            var childModels = context.VehicleModels.Where(x => x.VehicleMakeId == entity.Id);
            foreach (var item in childModels)
            {
                item.Abrv = entity.Abrv;
                _modelService.Update(item);
            }
        }
    }
}
