using Autofac;
using Project.Service.Model;
using Project.Service.Services;
using System.Linq;


namespace Project.Service
{
    public class DIModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register all domain model objects
            builder.RegisterAssemblyTypes(typeof(VehicleMake).Assembly)
                .Where(t => t.Name.StartsWith("Vehicle") && t.Namespace.EndsWith("Model"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            // Register all Async Services
            builder.RegisterAssemblyTypes(typeof(MakeServicesAsync).Assembly)
                .Where(t => t.Name.EndsWith("Async") && t.Namespace.EndsWith("Services"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            // Register DbContext
            builder.RegisterType<ProjectDbContext>().AsSelf();

            // Register generic Services 
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<Repository>().As<IRepository>().InstancePerRequest();
        }
    }
}
