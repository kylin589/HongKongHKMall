using Autofac;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;

namespace BrCms.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 100; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            
        }
    }
}
