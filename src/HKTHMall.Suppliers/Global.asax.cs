using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrCms.Framework.Mvc;
using FluentValidation.Mvc;

namespace HKTHMall.Suppliers
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            BrMvcEngineContext.Init(null);
            FluentValidationModelValidatorProvider.Configure();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}