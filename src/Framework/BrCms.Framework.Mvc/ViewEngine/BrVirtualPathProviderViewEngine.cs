using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace BrCms.Framework.Mvc.ViewEngine
{
    public abstract class BrVirtualPathProviderViewEngine : VirtualPathProviderViewEngine
    {
        private readonly string[] _emptyLocations = null;
        internal Func<string, string> GetExtensionThunk;
        protected BrVirtualPathProviderViewEngine()
        {
            this.GetExtensionThunk = VirtualPathUtility.GetExtension;
        }
        protected virtual string GetAreaName(RouteData routeData)
        {
            object obj2;
            if (routeData.DataTokens.TryGetValue("area", out obj2))
            {
                return (obj2 as string);
            }
            return this.GetAreaName(routeData.Route);
        }

        protected virtual string GetAreaName(RouteBase route)
        {
            var area = route as IRouteWithArea;
            if (area != null)
            {
                return area.Area;
            }
            var route2 = route as Route;
            if ((route2 != null) && (route2.DataTokens != null))
            {
                return (route2.DataTokens["area"] as string);
            }
            return null;
        }
        protected virtual List<ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats)
        {
            var list = new List<ViewLocation>();
            if (areaViewLocationFormats != null)
            {
                list.AddRange(areaViewLocationFormats.Select(str => new AreaAwareViewLocation(str)).Cast<ViewLocation>());
            }
            if (viewLocationFormats != null)
            {
                list.AddRange(viewLocationFormats.Select(str2 => new ViewLocation(str2)));
            }
            return list;
        }
        protected virtual bool IsSpecificPath(string name)
        {
            char ch = name[0];
            if (ch != '~')
            {
                return (ch == '/');
            }
            return true;
        }
        protected virtual string CreateCacheKey(string prefix, string name, string controllerName, string areaName, string theme)
        {
            return string.Format(CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:{5}", new object[] { base.GetType().AssemblyQualifiedName, prefix, name, controllerName, areaName, theme });
        }
        protected virtual string GetPath(ControllerContext controllerContext, string[] locations, string[] areaLocations, string locationsPropertyName, string name, string controllerName, string theme, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = this._emptyLocations;
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            string areaName = this.GetAreaName(controllerContext.RouteData);
            var strings = controllerContext.RouteData.DataTokens["namespaces"] as string[];
            if (strings != null)
            {
                var namespaces = strings[0];
                namespaces = namespaces.Substring(0, namespaces.LastIndexOf(".", StringComparison.Ordinal));

                var newLocations = locations.ToList();
                newLocations.Insert(0, "~/Modules/" + namespaces + "/Views/{1}/{0}.cshtml");
                newLocations.Insert(0, "~/Modules/" + namespaces + "/Views/Shared/{0}.cshtml");
                newLocations.Insert(0, "~/Themes/Default/Modules/" + areaName + "/Views/{1}/{0}.cshtml");
                newLocations.Insert(0, "~/Themes/Default/Modules/" + areaName + "/Views/Shared/{0}.cshtml");
                locations = newLocations.ToArray();
            }
            //little hack to get nop's admin area to be in /Administration/ instead of /Nop/Admin/ or Areas/Admin/
            if (!string.IsNullOrEmpty(areaName))
            {
                var newLocations = areaLocations.ToList();
                newLocations.Insert(0, "~/Themes/Default/Modules/" + areaName + "/Views/{1}/{0}.cshtml");
                newLocations.Insert(0, "~/Themes/Default/Modules/" + areaName + "/Views/Shared/{0}.cshtml");
                areaLocations = newLocations.ToArray();
            }

            bool flag = !string.IsNullOrEmpty(areaName);
            List<ViewLocation> viewLocations = this.GetViewLocations(locations, flag ? areaLocations : null);
            if (viewLocations.Count == 0)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Properties cannot be null or empty.", new object[] { locationsPropertyName }));
            }
            bool flag2 = this.IsSpecificPath(name);
            string key = this.CreateCacheKey(cacheKeyPrefix, name, flag2 ? string.Empty : controllerName, areaName, theme);
            if (useCache)
            {
                var cached = this.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, key);
                if (cached != null)
                {
                    return cached;
                }
            }
            if (!flag2)
            {
                return this.GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, theme, key, ref searchedLocations);
            }
            return this.GetPathFromSpecificName(controllerContext, name, key, ref searchedLocations);
        }
        protected virtual string GetPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string name, string controllerName, string areaName, string theme, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = string.Empty;
            searchedLocations = new string[locations.Count];
            for (int i = 0; i < locations.Count; i++)
            {
                string str2 = locations[i].Format(name, controllerName, areaName, theme);
                if (this.FileExists(controllerContext, str2))
                {
                    searchedLocations = this._emptyLocations;
                    virtualPath = str2;
                    this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
                    return virtualPath;
                }
                searchedLocations[i] = str2;
            }
            return virtualPath;
        }
        protected virtual bool FilePathIsSupported(string virtualPath)
        {
            if (this.FileExtensions == null)
            {
                return true;
            }
            string str = this.GetExtensionThunk(virtualPath).TrimStart(new char[] { '.' });
            return this.FileExtensions.Contains<string>(str, StringComparer.OrdinalIgnoreCase);
        }
        protected virtual string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = name;
            if (!this.FilePathIsSupported(name) || !this.FileExists(controllerContext, name))
            {
                virtualPath = string.Empty;
                searchedLocations = new string[] { name };
            }
            this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
            return virtualPath;
        }
        protected virtual string GetCurrentTheme()
        {
            var themeContext = BrMvcEngineContext.Current.Resolve<IBrMvcWorkContext>();
            return themeContext.WorkingThemeName;
        }

        protected virtual ViewEngineResult FindThemeableView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            string[] strArray;
            string[] strArray2;
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("View name cannot be null or empty.", "viewName");
            }
            var theme = this.GetCurrentTheme();
            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            string str2 = this.GetPath(controllerContext, this.ViewLocationFormats, this.AreaViewLocationFormats, "ViewLocationFormats", viewName, requiredString, theme, "View", useCache, out strArray);
            string str3 = this.GetPath(controllerContext, this.MasterLocationFormats, this.AreaMasterLocationFormats, "MasterLocationFormats", masterName, requiredString, theme, "Master", useCache, out strArray2);
            if (!string.IsNullOrEmpty(str2) && (!string.IsNullOrEmpty(str3) || string.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(this.CreateView(controllerContext, str2, str3), this);
            }
            if (strArray2 == null)
            {
                strArray2 = new string[0];
            }
            return new ViewEngineResult(strArray.Union<string>(strArray2));

        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return this.FindThemeableView(controllerContext, viewName, masterName, useCache);
        }
    }
    public class AreaAwareViewLocation : ViewLocation
    {
        public AreaAwareViewLocation(string virtualPathFormatString)
            : base(virtualPathFormatString)
        {
        }

        public override string Format(string viewName, string controllerName, string areaName, string theme)
        {
            return string.Format(CultureInfo.InvariantCulture, this._virtualPathFormatString, viewName, controllerName, areaName, theme);
        }
    }

    public class ViewLocation
    {
        protected readonly string _virtualPathFormatString;

        public ViewLocation(string virtualPathFormatString)
        {
            this._virtualPathFormatString = virtualPathFormatString;
        }

        public virtual string Format(string viewName, string controllerName, string areaName, string theme)
        {
            return string.Format(CultureInfo.InvariantCulture, this._virtualPathFormatString, viewName, controllerName, theme);
        }
    }
}
