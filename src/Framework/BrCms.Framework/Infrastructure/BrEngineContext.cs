using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using BrCms.Core.Infrastructure;
using BrCms.Framework.Logging;
//using ca
//using ILogger = BrCms.Framework.Logging.ILogger;

namespace BrCms.Framework.Infrastructure
{
    public class BrEngineContext
    {
        private static IContainer _current;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Init(IList<Action<Type>> actions)
        {
            if (_current == null)
            {
                _current = new ContainerBuilder().Build();
                var builder = new ContainerBuilder();

                builder.RegisterType<Logger>().As<ILogger>();

                builder.RegisterType<WebAppTypeFinder>().As<ITypeFinder>();
                builder.Update(_current);

                var typeFinder = _current.Resolve<ITypeFinder>();

                var drInstances = new List<IDependencyRegistrar>();

                var stInstances = new List<IStartupTask>();

                typeFinder.GetAssemblies()
                    .ToList().ForEach(a =>
                    {

                        //todo 此处代码有待优化
                        //if (a.FullName == "Couchbase, Version=1.3.12.0, Culture=neutral, PublicKeyToken=05e9c6b5a9ec94c2"
                        //    || a.FullName == "HKSJ.MidMessage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")//ID:中间件报错部分,屏蔽即可！ modified by michael in 20150709
                        //{
                        //    return;
                        //}
                        try
                        {
                            var types = a.GetTypes();
                            foreach (var t in types)
                            {
                                if (!t.IsClass) continue;

                                //IDependencyRegistrar
                                if (typeof(IDependencyRegistrar).IsAssignableFrom(t))
                                {
                                    drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(t));
                                }

                                //IStartupTask
                                if (typeof(IStartupTask).IsAssignableFrom(t))
                                {
                                    stInstances.Add((IStartupTask)Activator.CreateInstance(t));
                                }

                                //初始化程序
                                if (actions != null)
                                {
                                    actions.ToList().ForEach(action => { action(t); });
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                    });

                builder = new ContainerBuilder();

                builder.RegisterAssemblyTypes(typeFinder.GetAssemblies().ToArray())
                    .Where(t => t.IsClass && typeof(IDependency).IsAssignableFrom(t))
                    .EnableInterfaceInterceptors()
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope()
                    ;

                builder.Update(_current);

                if (drInstances.Any())
                {
                    builder = new ContainerBuilder();

                    drInstances.OrderBy(x => x.Order)
                        .ToList()
                        .ForEach(m => m.Register(builder, typeFinder));

                    builder.Update(_current);
                }

                if (stInstances.Any())
                {
                    stInstances.OrderBy(x => x.Order).ToList().ForEach(m => m.Execute());
                }
            }
        }
        public static IContainer Current
        {
            get
            {
                return _current;
            }
        }
    }
}
