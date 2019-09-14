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
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MakeServicesAsync(IRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<VehicleMake> FindAsync(int id)
        {
            var make = await _repository.GetAsync<VehicleMake>(id);
            var models = await _repository.GetAllAsync<VehicleModel>();
            make.VehicleModels = models.Where(x => x.VehicleMakeId == make.Id).ToList();

            return make;
        }

        public async Task<IPagedList<IVehicleMake>> GetAsync(
            IFilteringParams filteringParams,
            IPagingParams pagingParams,
            ISortingParams sortingParams)
        {

            IQueryable<VehicleMake> makeList = await _repository.GetAllAsync<VehicleMake>();

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
                    await _unitOfWork.AddAsync<VehicleMake>(entity as VehicleMake);
                    return await _unitOfWork.SaveChangesAsync();
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
                    await _unitOfWork.UpdateAsync<VehicleMake>(entity as VehicleMake);

                    return await _unitOfWork.SaveChangesAsync();
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
                    await _unitOfWork.RemoveAsync<VehicleMake>(entity as VehicleMake);
                    
                    return await _unitOfWork.SaveChangesAsync();
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
            var list = await _repository.GetAllAsync<VehicleMake>();
            return list.OrderBy(x => x.Name).ToList();
        }

        public void Dispose()
        {
            _repository.Dispose();
            _unitOfWork.Dispose();
        }
    }
}

