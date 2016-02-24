using System.Linq;
using Autofac;
using Autofac.Integration.WebApi;
using BrCms.Framework.Infrastructure;

namespace BrCms.Framework.WebApi2
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 100; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterApiControllers(typeFinder.GetAssemblies().ToArray());
        }
    }
}
