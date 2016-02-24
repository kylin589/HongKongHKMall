using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Admin.common;

namespace HKTHMall.Services
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 0; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
           // builder.RegisterType<ServiceIInterceptor>();
            builder.RegisterType<Authority>();
        }
    }
}
