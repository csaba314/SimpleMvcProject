using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MvcProject.MVC.Models;
using Project.Service.Model;

namespace MvcProject.MVC.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<MakeCreateEditViewModel, VehicleMake>().ForMember(m => m.Id, opt => opt.Ignore());
            Mapper.CreateMap<VehicleMake, MakeCreateEditViewModel>();
        }
    }
}