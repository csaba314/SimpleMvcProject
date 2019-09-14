using Autofac;
using Project.Common.ParamContainers;
using System.Linq;

namespace Project.Common
{
    public class DIModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register parameters
            builder.RegisterAssemblyTypes(typeof(Options).Assembly)
                .Where(t => t.Name.EndsWith("Params") && t.Namespace.EndsWith("ParamContainers"))
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));

            builder.RegisterType<Options>().As<IOptions>();
            builder.RegisterType<ParamsFactory>().As<IParamsFactory>();
        }
    }
}
