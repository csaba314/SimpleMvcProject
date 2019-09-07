using PagedList;
using Project.Service.Model;
using Project.Common.ParamContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service.Services
{
    public class MakeServicesAsync : ServicesAsync<VehicleMake>, IMakeServicesAsync
    {
        private ProjectDbContext Context { get { return _context as ProjectDbContext; } }

        public MakeServicesAsync(ProjectDbContext context) : base(context)
        {
        }

        public async Task<IVehicleMake> FindAsync(int id)
        {
            return await base.GetAsync(id);

        }

        public async Task<IPagedList<IVehicleMake>> GetAsync(
            IFilteringParams filteringParams, 
            IPagingParams pagingParams,
            ISortingParams sortingParams)
        {

            IQueryable<VehicleMake> makeList = await GetAllAsync();
            
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
                    await base.AddAsync(entity as VehicleMake);
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

        public async Task<int> UpdateAsync(IVehicleMake entity)
        {
            if (entity is VehicleMake)
            {
                try
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
                    await base.UpdateAsync(entity as VehicleMake);
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

        public async Task<int> RemoveAsync(IVehicleMake entity)
        {
            if (entity is VehicleMake)
            {
                try
                {
                    await base.RemoveAsync(entity as VehicleMake);
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

        public async Task<IEnumerable<IVehicleMake>> GetMakeDropdown()
        {
            var list = await base.GetAllAsync();
            return list.ToList();
        }
    }
}
