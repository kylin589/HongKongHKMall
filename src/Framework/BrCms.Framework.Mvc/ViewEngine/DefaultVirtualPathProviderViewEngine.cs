using System;
using System.Linq;
using System.Web.Mvc;

namespace BrCms.Framework.Mvc.ViewEngine
{
    public abstract class DefaultVirtualPathProviderViewEngine : VirtualPathProviderViewEngine
    {
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            object areaName;
            if (controllerContext.RouteData.DataTokens.TryGetValue("area",out areaName))
            {
                var strings = controllerContext.RouteData.DataTokens["namespaces"] as string[];
                if (strings != null)
                {

                    var namespaces = strings[0];
                    namespaces = namespaces.Substring(0, namespaces.LastIndexOf(".", StringComparison.Ordinal));
                    var newViewLocationFormats = this.ViewLocationFormats.ToList();
                    var newMasterLocationFormats = this.MasterLocationFormats.ToList();

                    newViewLocationFormats.Insert(0, "~/Modules/" + namespaces + "/Views/{1}/{0}.cshtml");
                    newViewLocationFormats.Insert(0, "~/Modules/" + namespaces + "/Views/Shared/{0}.cshtml");
                    newMasterLocationFormats.Insert(0, "~/Modules/" + namespaces + "/Views/{1}/{0}.cshtml");
                    newMasterLocationFormats.Insert(0, "~/Modules/" + namespaces + "/Views/Shared/{0}.cshtml");

                    newViewLocationFormats.Insert(0, "~/Themes/Default/Modules/" + areaName + "/Views/{1}/{0}.cshtml");
                    newViewLocationFormats.Insert(0, "~/Themes/Default/Modules/" + areaName + "/Views/Shared/{0}.cshtml");
                    newMasterLocationFormats.Insert(0, "~/Themes/Default/Modules/" + areaName + "/Views/{1}/{0}.cshtml");
                    newMasterLocationFormats.Insert(0, "~/Themes/Default/Modules/" + areaName + "/Views/Shared/{0}.cshtml");

                    this.ViewLocationFormats = newViewLocationFormats.ToArray();
                    this.MasterLocationFormats = newMasterLocationFormats.ToArray();
                }
            }
            else
            {

                this.ViewLocationFormats = new[]
                {
                    "~/Themes/Default/Views/{1}/{0}.cshtml",
                    "~/Themes/Default/Views/Shared/{0}.cshtml",
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml"                    
                };
                this.MasterLocationFormats = new[]
                {
                    "~/Themes/Default/Views/{1}/{0}.cshtml",
                    "~/Themes/Default/Views/Shared/{0}.cshtml",
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml"                    
                };
            }
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            object areaName;
            if (controllerContext.RouteData.DataTokens.TryGetValue("area", out areaName))
            {
                var strings = controllerContext.RouteData.DataTokens["namespaces"] as string[];
                if (strings != null)
                {
                    var namespaces = strings[0];
                    namespaces = namespaces.Substring(0, namespaces.LastIndexOf(".", StringComparison.Ordinal));

                    var newPartialViewLocationFormats = this.PartialViewLocationFormats.ToList();

                    newPartialViewLocationFormats.Insert(0, "~/Modules/" + namespaces + "/Views/{1}/{0}.cshtml");
                    newPartialViewLocationFormats.Insert(0, "~/Modules/" + namespaces + "/Views/Shared/{0}.cshtml");                    
                    newPartialViewLocationFormats.Insert(0, "~/Themes/Default/Modules/" + areaName + "/Views/{1}/{0}.cshtml");
                    newPartialViewLocationFormats.Insert(0, "~/Themes/Default/Modules/" + areaName + "/Views/Shared/{0}.cshtml");

                    this.PartialViewLocationFormats = newPartialViewLocationFormats.ToArray();
                }
            }
            else
            {
                this.PartialViewLocationFormats = new[]
                {
                    "~/Themes/Default/Views/{1}/{0}.cshtml",
                    "~/Themes/Default/Views/Shared/{0}.cshtml",
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml"                    
                };
            }
            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }
    }
}
