using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using MvcProject.MVC.Models;
using MvcProject.MVC.Models.Factories;
using Project.Service.Containers;
using Project.Service.Model;
using Project.Service.Services;

namespace MvcProject.MVC.App_Start
{
    public static class DIContainerConfig
    {
        public static void Configure()
        {
            // create ContainerBuilder
            var builder = new Autofac.ContainerBuilder();

            // Register MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                .Where(c => c.Name.EndsWith("Controller"));

            builder.RegisterType<ProjectDbContext>().AsSelf();

            builder.RegisterType<IndexViewModel<VehicleMakeDTO, VehicleModelDTO>>().AsSelf();
            builder.RegisterType<IndexViewModel<VehicleModelDTO, string>>().AsSelf();
            builder.RegisterType<VehicleMakeDTO>().AsSelf();
            builder.RegisterType<VehicleModelDTO>().AsSelf();

            builder.RegisterType<DomainModelFactory>().As<IDomainModelFactory>();

            builder.RegisterType<ControllerParameters>().As<IControllerParameters>();
            builder.RegisterType<Options>().As<ILoadingOptions>();
            builder.RegisterType<ParamContainerBuilder>().As<IParamContainerBuilder>();

            // register all types in the assembly where MakeService is located
            builder.RegisterAssemblyTypes(typeof(MakeService).Assembly)
                // where type name ends with "Service" and namespace ends with "Services"
                .Where(t => t.Name.EndsWith("Service") && t.Namespace.EndsWith("Services"))
                // map to interface that is implemented by type where interface name is "I" + type name
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            // register all domain model objects
            builder.RegisterAssemblyTypes(typeof(VehicleMake).Assembly)
                .Where(t => t.Name.StartsWith("Vehicle") && t.Namespace.EndsWith("Model"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterAssemblyTypes(typeof(DTOFactory).Assembly)
                .Where(t => t.Name.EndsWith("Factory") && t.Namespace.EndsWith("Factories"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            // build the container
            var container = builder.Build();

            // set the dependency resolver for MVC controllers to use autofac 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}