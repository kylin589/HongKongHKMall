using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Mvc.Routes;
//using BrCms.Framework.Mvc.ViewEngine;

namespace BrCms.Framework.Mvc
{
    public class BrMvcEngineContext
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Init(IList<Action<Type>> actions)
        {
            if (BrEngineContext.Current == null)
            {
                if (actions == null)
                {
                    actions = new List<Action<Type>>();
                }
                actions.Add(t =>
                {
                    //IRoutePublish
                    var routes = new List<IRoutePublish>();
                    if (typeof (IRoutePublish).IsAssignableFrom(t))
                    {
                        routes.Add((IRoutePublish) Activator.CreateInstance(t));
                    }
                    if (routes.Any())
                    {
                        routes.OrderBy(x => x.Order).ToList().ForEach(m => m.Publish(RouteTable.Routes));
                    }
                });

                BrEngineContext.Init(actions);
                //set dependency resolver
                DependencyResolver.SetResolver(new AutofacDependencyResolver(BrEngineContext.Current));
                //remove all view engines
                //ViewEngines.Engines.Clear();
                //except the themeable razor view engine we use
                //ViewEngines.Engines.Add(new DefaultViewEngine());
            }
        }

        public static ILifetimeScope Current
        {
            get
            {
                try
                {
                    return AutofacDependencyResolver.Current.RequestLifetimeScope ??
                           BrEngineContext.Current.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
                }
                catch
                {
                    return BrEngineContext.Current.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
                }
            }
        }
    }
}
