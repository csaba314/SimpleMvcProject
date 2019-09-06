using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Project.Service.ParamContainers;
using PagedList;
using Project.Service.DTO;

namespace Project.Service.Services
{
    public class ModelServicesAsync : ServicesAsync<VehicleModel>, IModelServicesAsync
    {
        public ProjectDbContext Context { get { return _context as ProjectDbContext; } }

        public ModelServicesAsync(ProjectDbContext dbcontext) : base(dbcontext)
        {

        }

        public async Task<IEnumerable<IVehicleModel>> GetAllByMakeAsync(int makeId)
        {
            IQueryable<VehicleModel> list = await GetAllAsync();
            return list.Where(m => m.VehicleMakeId == makeId).ToList();
        }

        public async Task<IPagedList<VehicleModelDTO>> GetAsync(IFilteringParams filteringParams,
                                                                IPagingParams pagingParams,
                                                                ISortingParams sortingParams,
                                                                IOptions options)
        {
            IQueryable<VehicleModel> modelList = await base.GetAllAsync();

            if (options.LoadMakesWithModel)
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

            var dtoList = modelList.Select(x => AutoMapper.Mapper.Map<VehicleModelDTO>(x));

            var pagedList = dtoList.ToPagedList(pagingParams.PageNumber, pagingParams.PageSize);

            if (pagedList.PageCount < pagedList.PageNumber)
            {
                dtoList.ToPagedList(1, pagingParams.PageSize);
            }
            return pagedList;
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

        public async Task<IVehicleModel> FindAsync(int id)
        {
            return await base.GetAsync(id);
        }

        public async Task<int> RemoveAsync(IVehicleModel entity)
        {
            if (entity is VehicleModel)
            {
                return await base.RemoveAsync(entity as VehicleModel);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}

