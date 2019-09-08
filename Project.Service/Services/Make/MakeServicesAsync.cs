using PagedList;
using Project.Service.Model;
using Project.Common.ParamContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public class MakeServicesAsync : IMakeServicesAsync
    {
        private readonly IModelServicesAsync _modelServices;
        private readonly IUnitOfWork<VehicleMake> _makeUOW;

        public MakeServicesAsync(IModelServicesAsync modelServices, IUnitOfWork<VehicleMake> makeServices)
        {
            _modelServices = modelServices;
            _makeUOW = makeServices;
        }

        public Task<VehicleMake> FindAsync(int id)
        {
            return _makeUOW.GetAsync(id);

        }

        public async Task<IPagedList<IVehicleMake>> GetAsync(
            IFilteringParams filteringParams,
            IPagingParams pagingParams,
            ISortingParams sortingParams)
        {

            IQueryable<VehicleMake> makeList = await _makeUOW.GetAllAsync();

            // Filtering
            if (!String.IsNullOrEmpty(filteringParams.SearchString))
            {
                makeList = makeList.Where(x => x.Name.ToLower().Contains(filteringParams.SearchString.ToLower()));
            }

            // Sorting
            switch (sortingParams.Sorting)
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
            var pagedList = makeList.ToPagedList(pagingParams.PageNumber, pagingParams.PageSize);

            if (pagedList.PageCount < pagedList.PageNumber)
            {
                makeList.ToPagedList(1, pagingParams.PageSize);
            }
            return pagedList;
        }

        public async Task<int> AddAsync(IVehicleMake entity)
        {
            if (entity is VehicleMake)
            {
                try
                {
                    await _makeUOW.AddAsync(entity as VehicleMake);
                    return await _makeUOW.SaveChangesAsync();
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

        public async Task<int> UpdateAsync(IVehicleMake entity)
        {
            if (entity is VehicleMake)
            {
                try
                {
                    // get the list of unmodified child entities
                    var modifiedChildModels = await _modelServices.GetAllByMakeAsync(entity.Id);

                    foreach (var item in modifiedChildModels)
                    {
                        // change each child entity
                        item.Abrv = entity.Abrv;
                    }
                    // update all child entities
                    await _modelServices.UpdateRange(modifiedChildModels);

                    // update parent entity
                    await _makeUOW.UpdateAsync(entity as VehicleMake);

                    return await _makeUOW.SaveChangesAsync();
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

        public async Task<int> RemoveAsync(IVehicleMake entity)
        {
            
            if (entity is VehicleMake)
            {
                try
                {
                    await _makeUOW.RemoveAsync(entity as VehicleMake);
                    
                    return await _makeUOW.SaveChangesAsync();
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

        public async Task<IEnumerable<IVehicleMake>> GetMakeDropdown()
        {
            var list = await _makeUOW.GetAllAsync();
            return list.OrderBy(x => x.Name).ToList();
        }

        public void Dispose()
        {
            _modelServices.Dispose();
            _makeUOW.Dispose();
        }
    }
}

