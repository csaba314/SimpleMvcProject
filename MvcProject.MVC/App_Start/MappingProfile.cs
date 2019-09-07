using AutoMapper;
using Project.Service.Model;
using Project.MVC.Models;

namespace Project.MVC.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<VehicleMakeDTO, IVehicleMake>().ForMember(m => m.Id, opt => opt.Ignore());
            Mapper.CreateMap<IVehicleMake, VehicleMakeDTO>();

            Mapper.CreateMap<VehicleModelDTO, IVehicleModel>().ForMember(m => m.Id, opt => opt.Ignore());
            Mapper.CreateMap<IVehicleModel, VehicleModelDTO>();
        }
    }
}