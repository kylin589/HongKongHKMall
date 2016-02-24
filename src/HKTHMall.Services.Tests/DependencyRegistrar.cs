using Autofac;
using BrCms.Framework;
using BrCms.Framework.Infrastructure;

namespace HKTHMall.Services.Tests
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 0; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<StubHostEnvironment>().As<IHostEnvironment>();
        }
    }
}
