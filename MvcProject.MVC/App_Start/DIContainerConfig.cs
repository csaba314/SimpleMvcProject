using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using MvcProject.MVC.Models;
using Project.Service.Model;
using Project.Common.ParamContainers;
using Project.Service.Services;


namespace MvcProject.MVC.App_Start
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

            // Register parameters
            builder.RegisterAssemblyTypes(typeof(Options).Assembly)
                .Where(t => t.Name.EndsWith("Params") && t.Namespace.EndsWith("ParamContainers"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));
            builder.RegisterType<Options>().As<IOptions>();
            builder.RegisterType<ParamsFactory>().As<IParamsFactory>();

            // Register all domain model objects
            builder.RegisterAssemblyTypes(typeof(VehicleMake).Assembly)
                .Where(t => t.Name.StartsWith("Vehicle") && t.Namespace.EndsWith("Model"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            // Register all Async Services
            builder.RegisterAssemblyTypes(typeof(MakeServicesAsync).Assembly)
                .Where(t => t.Name.EndsWith("Async") && t.Namespace.EndsWith("Services"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            // build the container
            var container = builder.Build();

            // set the dependency resolver for MVC controllers to use AutofacDependencyResolver 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}