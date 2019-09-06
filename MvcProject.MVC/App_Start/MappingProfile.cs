using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Project.Service.DTO;
using Project.Service.Model;

namespace MvcProject.MVC.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<VehicleMakeDTO, IVehicleMake>().ForMember(m => m.Id, opt => opt.Ignore());
            Mapper.CreateMap<IVehicleMake, VehicleMakeDTO>();

            Mapper.CreateMap<VehicleModelDTO, IVehicleModel>().ForMember(m => m.Id, opt => opt.Ignore());
            Mapper.CreateMap<IVehicleModel, VehicleModelDTO>();

            Mapper.CreateMap<VehicleModel, VehicleModelDTO>();
            Mapper.CreateMap<VehicleMake, VehicleMakeDTO>();

        }
    }
}