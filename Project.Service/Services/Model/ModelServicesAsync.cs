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
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ModelServicesAsync(IRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public Task<VehicleModel> FindAsync(int id)
        {
            return _repository.GetAsync<VehicleModel>(id);
        }

        public async Task<IEnumerable<IVehicleModel>> GetAllByMakeAsync(int makeId)
        {
            IQueryable<VehicleModel> list = await _repository.GetAllAsync<VehicleModel>();
            return list.Where(m => m.VehicleMakeId == makeId).OrderBy(m => m.Name).ToList();
        }

        public async Task<IPagedList<IVehicleModel>> GetAsync(IFilteringParams filteringParams,
                                                                IPagingParams pagingParams,
                                                                ISortingParams sortingParams,
                                                                IOptions options)
        {
            IQueryable<VehicleModel> modelList = await _repository.GetAllAsync<VehicleModel>();

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
                    entity.Abrv = SetModelAbrv(entity.Name);
                    await _unitOfWork.AddAsync<VehicleModel>(entity as VehicleModel);
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

        public async Task<int> UpdateAsync(IVehicleModel entity)
        {
            if (entity is VehicleModel)
            {
                try
                {
                    entity.Abrv = SetModelAbrv(entity.Name);
                    await _unitOfWork.UpdateAsync<VehicleModel>(entity as VehicleModel);
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

        public async Task<int> UpdateRange(IEnumerable<IVehicleModel> entities)
        {
            if (entities is IEnumerable<VehicleModel>)
            {
                foreach (var item in entities)
                {
                    await _unitOfWork.UpdateAsync<VehicleModel>(item as VehicleModel);
                }

                return await _unitOfWork.SaveChangesAsync();
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
                    await _unitOfWork.RemoveAsync<VehicleModel>(entity as VehicleModel);
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

        private string SetModelAbrv(string name)
        {

            return "VM-" + name.Substring(0, 2);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _repository.Dispose();
        }
    }
}