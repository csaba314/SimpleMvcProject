using Project.Service.Containers;
using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Project.Service.Services.Async
{
    public class ModelServicesAsync : ServicesAsync<VehicleModel>, IModelServicesAsync
    {
        public ProjectDbContext Context { get { return _context as ProjectDbContext; } }

        public ModelServicesAsync(ProjectDbContext dbcontext) : base(dbcontext)
        {

        }

        public async Task<IEnumerable<IVehicleModel>> GetAllByMakeAsync(int makeId)
        {
            return await Task.Run(()=> GetAllAsync().Result.Where(m => m.VehicleMakeId == makeId).ToList());
        }

        public async Task<IEnumerable<IVehicleModel>> GetAllAsync(IControllerParameters parameters)
        {
            IQueryable<VehicleModel> modelList;

            if (!parameters.Options.LoadMakesWithModel)
            {
                modelList = Context.VehicleModels;
            }
            else
            {
                modelList = Context.VehicleModels.Include(m => m.VehicleMake);
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

            return await modelList.ToListAsync();
        }


        public async Task<int> AddAsync(IVehicleModel entity)
        {
            if (entity is VehicleModel)
            {
                entity.Abrv = SetModelAbrv(entity.VehicleMakeId);
                return await base.AddAsync(entity as VehicleModel);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<int> UpdateAsync(IVehicleModel entity)
        {
            if (entity is VehicleModel)
            {
                entity.Abrv = SetModelAbrv(entity.VehicleMakeId);
                return await base.UpdateAsync(entity as VehicleModel);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<int> RemoveRangeAsync(IEnumerable<IVehicleModel> entities)
        {
            if (entities is IEnumerable<VehicleModel>)
            {
                return await base.RemoveRangeAsync(entities as IEnumerable<VehicleModel>);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private string SetModelAbrv(int makeId)
        {
            return Context.VehicleMakes.Find(makeId).Abrv;
        }
    }

}
