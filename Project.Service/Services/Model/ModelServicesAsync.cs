using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Project.Common.ParamContainers;
using PagedList;

namespace Project.Service.Services
{
    public class ModelServicesAsync : ServicesAsync<VehicleModel>, IModelServicesAsync
    {
        public ProjectDbContext Context { get { return _context as ProjectDbContext; } }

        public ModelServicesAsync(ProjectDbContext dbcontext) : base(dbcontext)
        {

        }

        public async Task<IVehicleModel> FindAsync(int id)
        {
            return await base.GetAsync(id);
        }

        public async Task<IEnumerable<IVehicleModel>> GetAllByMakeAsync(int makeId)
        {
            IQueryable<VehicleModel> list = await GetAllAsync();
            return list.Where(m => m.VehicleMakeId == makeId).OrderBy(m => m.Name).ToList();
        }

        public async Task<IPagedList<IVehicleModel>> GetAsync(IFilteringParams filteringParams,
                                                                IPagingParams pagingParams,
                                                                ISortingParams sortingParams,
                                                                IOptions options)
        {
            IQueryable<VehicleModel> modelList = await base.GetAllAsync();

            if (!options.LoadMakesWithModel)
            {
                modelList = Context.VehicleModels;
            }
            else
            {
                modelList = Context.VehicleModels.Include(m => m.VehicleMake);
            }

            // Filtering
            if (!String.IsNullOrEmpty(filteringParams.SearchString))
            {
                modelList = modelList.Where(x => x.VehicleMake.Name.ToLower().Contains(filteringParams.SearchString.ToLower()));
            }

            // Sorting
            switch (sortingParams.Sorting)
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

            var pagedList = modelList.ToPagedList(pagingParams.PageNumber, pagingParams.PageSize);

            if (pagedList.PageCount < pagedList.PageNumber)
            {
                modelList.ToPagedList(1, pagingParams.PageSize);
            }
            return pagedList;
        }
    


        public async Task<int> AddAsync(IVehicleModel entity)
        {
            if (entity is VehicleModel)
            {
                try
                {
                    entity.Abrv = SetModelAbrv(entity.VehicleMakeId);
                    await base.AddAsync(entity as VehicleModel);
                    return await base.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw e;
                }
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
                try
                {
                    entity.Abrv = SetModelAbrv(entity.VehicleMakeId);
                    await base.UpdateAsync(entity as VehicleModel);
                    return await base.SaveChangesAsync();
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<int> RemoveAsync(IVehicleModel entity)
        {
            if (entity is VehicleModel)
            {
                try
                {
                    await base.RemoveAsync(entity as VehicleModel);
                    return await base.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw e;
                }
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

