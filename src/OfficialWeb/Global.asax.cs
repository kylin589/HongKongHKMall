using BrCms.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OfficialWeb
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明,
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            BrMvcEngineContext.Init(null);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}