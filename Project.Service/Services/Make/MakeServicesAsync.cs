using Project.Service.Containers;
using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public class MakeServicesAsync : ServicesAsync<VehicleMake>, IMakeServicesAsync
    {
        private ProjectDbContext Context { get { return _context as ProjectDbContext; } }

        public MakeServicesAsync(ProjectDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<IVehicleMake>> GetAllAsync(IControllerParameters parameters)
        {
            var makeList = await GetAllAsync();

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

            return makeList;
        }

        public async Task<int> AddAsync(IVehicleMake entity)
        {
            if (entity is VehicleMake)
            {
                return await base.AddAsync(entity as VehicleMake);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<int> UpdateAsync(IVehicleMake entity)
        {
            if (entity is VehicleMake)
            {
                // get the list of unmodified child entities
                var modifiedChildModels = Context.VehicleModels.Where(x => x.VehicleMakeId == entity.Id).ToList();

                foreach (var item in modifiedChildModels)
                {
                    // set the new property value for each item in the list
                    item.Abrv = entity.Abrv;

                    // select the existing child entity from the parrent entity
                    var existingChild = entity.VehicleModels.Where(c => c.Id == item.Id).SingleOrDefault();

                    if (existingChild != null)
                    {
                        // set the new values to the child entity in the parrent entity collection
                        Context.Entry(existingChild).CurrentValues.SetValues(item);
                    }
                }
                return await base.UpdateAsync(entity as VehicleMake);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
