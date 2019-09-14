using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Project.MVC.Models;
using Project.Service.Model;



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

            // register Common DIModule
            builder.RegisterModule<Project.Common.DIModule>();
            // register Service DIModule
            builder.RegisterModule<Project.Service.DIModule>();

            var container = builder.Build();

            // set the dependency resolver for MVC controllers to use AutofacDependencyResolver 
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}