using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Http;
using Autofac.Integration.WebApi;
using BrCms.Framework.Infrastructure;

namespace BrCms.Framework.WebApi2
{
    public class WebApiEngineContext
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

                BrEngineContext.Init(actions);
                //set dependency resolver
                //var configuration = Configuration;
                GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(BrEngineContext.Current);
            }
        }
    }
}