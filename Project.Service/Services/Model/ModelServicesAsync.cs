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
    public class ModelServicesAsync : IModelServicesAsync
    {
        private readonly IUnitOfWork<VehicleModel> _modelUOW;
        private readonly IUnitOfWork<VehicleMake> _makeUOW;

        public ModelServicesAsync(IUnitOfWork<VehicleModel> modelServices, IUnitOfWork<VehicleMake> makeServices)
        {
            _modelUOW = modelServices;
            _makeUOW = makeServices;
        }

        public Task<VehicleModel> FindAsync(int id)
        {
            return _modelUOW.GetAsync(id);
        }

        public async Task<IEnumerable<IVehicleModel>> GetAllByMakeAsync(int makeId)
        {
            IQueryable<VehicleModel> list = await _modelUOW.GetAllAsync();
            return list.Where(m => m.VehicleMakeId == makeId).OrderBy(m => m.Name).ToList();
        }

        public async Task<IPagedList<IVehicleModel>> GetAsync(IFilteringParams filteringParams,
                                                                IPagingParams pagingParams,
                                                                ISortingParams sortingParams,
                                                                IOptions options)
        {
            IQueryable<VehicleModel> modelList = await _modelUOW.GetAllAsync();

            if (options.LoadMakesWithModel)
            {
                modelList = modelList.Include(m => m.VehicleMake);
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
                    entity.Abrv = await SetModelAbrv(entity.VehicleMakeId);
                    await _modelUOW.AddAsync(entity as VehicleModel);
                    return await _modelUOW.SaveChangesAsync();
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
                    entity.Abrv = await SetModelAbrv(entity.VehicleMakeId);
                    await _modelUOW.UpdateAsync(entity as VehicleModel);
                    return await _modelUOW.SaveChangesAsync();
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

        public async Task<int> UpdateRange(IEnumerable<IVehicleModel> entities)
        {
            if (entities is IEnumerable<VehicleModel>)
            {
                foreach (var item in entities)
                {
                    await _modelUOW.UpdateAsync(item as VehicleModel);
                }

                return await _modelUOW.SaveChangesAsync();
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
                    await _modelUOW.RemoveAsync(entity as VehicleModel);
                    return await _modelUOW.SaveChangesAsync();
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

        private async Task<string> SetModelAbrv(int makeId)
        {
            var parentEntity = await _makeUOW.GetAsync(makeId);
            return parentEntity.Abrv;

        }

        public void Dispose()
        {
            _modelUOW.Dispose();
            _makeUOW.Dispose();
        }
    }
}