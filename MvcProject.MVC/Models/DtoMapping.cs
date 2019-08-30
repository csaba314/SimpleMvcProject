using Project.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PagedList;

namespace MvcProject.MVC.Models
{
    static class DtoMapping
    {
        public static IEnumerable<VehicleMakeDTO> MapToVehicleMakeDTO(IEnumerable<IVehicleMake> makeList)
        {
            var dtoList = new List<VehicleMakeDTO>();

            foreach (var item in makeList)
            {
                dtoList.Add(Mapper.Map<VehicleMakeDTO>(item));
            }

            return dtoList;
        }

        public static IPagedList<VehicleMakeDTO> MapToVehicleMakeDTO(IEnumerable<IVehicleMake> makeList, int? pageSize, int? pageNumber)
        {
            var list = MapToVehicleMakeDTO(makeList);
            if (pageSize == null)
            {
                throw new ArgumentException();
            }
            if (pageNumber == null)
            {
                return list.ToPagedList(1, (int)pageSize);
            }
            return list.ToPagedList((int) pageNumber, (int)pageSize);
        }


        public static IEnumerable<VehicleModelDTO> MapToVehicleModelDTO(IEnumerable<IVehicleModel> modelList)
        {
            var dtoList = new List<VehicleModelDTO>();

            foreach (var item in modelList)
            {
                dtoList.Add(Mapper.Map<VehicleModelDTO>(item));
            }
            return dtoList;
        }

        public static IPagedList<VehicleModelDTO> MapToVehicleModelDTO(IEnumerable<IVehicleModel> modelList, int? pageSize, int? pageNumber)
        {
            var list = MapToVehicleModelDTO(modelList);

            if (pageSize == null)
            {
                throw new ArgumentException();
            }
            if (pageNumber == null)
            {
                return list.ToPagedList(1, (int)pageSize);
            }
            return list.ToPagedList((int)pageNumber, (int)pageSize);
        }

    }
}