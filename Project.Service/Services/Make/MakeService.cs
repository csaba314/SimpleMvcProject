using System;
using System.Collections.Generic;
using System.Linq;
using Project.Service.Containers;
using Project.Service.Model;

namespace Project.Service.Services
{
    public class MakeService : Service<VehicleMake>, IMakeService
    {

        private ProjectDbContext context { get { return _context as ProjectDbContext; } }
        private IModelService _modelService;

        public MakeService(ProjectDbContext context, IModelService modelService) : base(context)
        {
            this._modelService = modelService;
        } 

        public IEnumerable<IVehicleMake> GetAll(IControllerParameters parameters)
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

        public void Update(IVehicleMake entity)
        {
            if (entity is VehicleMake)
            {
                base.Update(entity as VehicleMake);

                // get the list of unmodified child entities
                var modifiedChildModels = context.VehicleModels.Where(x => x.VehicleMakeId == entity.Id).ToList();

                foreach (var item in modifiedChildModels)
                {
                    // set the new property value for each item in the list
                    item.Abrv = entity.Abrv;

                    // select the existing child entity from the parrent entity
                    var existingChild = entity.VehicleModels.Where(c => c.Id == item.Id).SingleOrDefault();

                    if (existingChild != null)
                    {
                        // set the new values to the child entity in the parrent entity collection
                        context.Entry(existingChild).CurrentValues.SetValues(item);
                    }
                }
            }
        }

        public void Add(IVehicleMake make)
        {
            if (make is VehicleMake)
            {
                base.Add(make as VehicleMake);
            }
        }
    }
}
