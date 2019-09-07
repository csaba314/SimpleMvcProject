using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Project.MVC.Models;
using Project.Service.Model;
using Project.Common;
using Project.Service;


namespace Project.MVC.App_Start
{
    public static class DIContainerConfig
    {
        public static void Configure()
        {
            // create ContainerBuilder
            var builder = new Autofac.ContainerBuilder();

            // Register MVC controllers. (MvcApplication is the name of the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                .Where(c => c.Name.EndsWith("Controller"));

            builder.RegisterType<ProjectDbContext>().AsSelf();

            // Register viewModels and DTOs
            builder.RegisterType<IndexViewModel<VehicleMakeDTO, VehicleModelDTO>>().AsSelf();
            builder.RegisterType<IndexViewModel<VehicleModelDTO, string>>().AsSelf();
            builder.RegisterType<VehicleMakeDTO>().AsSelf();
            builder.RegisterType<VehicleModelDTO>().AsSelf();

            // register Common module
            builder.RegisterModule<CommonModule>();
            // register Service module
            builder.RegisterModule<ServiceModule>();

            var container = builder.Build();

            // set the dependency resolver for MVC controllers to use AutofacDependencyResolver 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}