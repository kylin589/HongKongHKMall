using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HKTHMall.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute("category1", "home/category.html", new { controller = "home", action = "category" });
            routes.MapRoute("NotFound", "NotFound.html", new { controller = "home", action = "NotFound" });
            routes.MapRoute("search", "home/search.html", new { controller = "home", action = "search" });
            routes.MapRoute("index", "search/index.html", new { controller = "search", action = "index" });
            routes.MapRoute("indexall", "home/indexall.html", new { controller = "home", action = "indexall" });
            routes.MapRoute("indexfirst", "home/indexfirst.html", new { controller = "home", action = "indexfirst" });
            routes.MapRoute("login", "login", new { controller = "login", action = "index" });
            //routes.MapRoute("index", "{sellerkey}", new { controller = "Home", action = "sellerindex" });
            routes.MapRoute("shopping", "home/shopping/{id}.html", new { controller = "Home", action = "shopping" });
            routes.MapRoute("showshopping", "home/shopping/{show}/{id}.html", new { controller = "Home", action = "shopping" });
            routes.MapRoute("newslist", "News.html", new { controller = "News", action = "Index" });
            routes.MapRoute("newsdetails", "News/{id}.html", new { controller = "News", action = "Details" });
            //routes.MapRoute("snapshot", "home/snapshot/{id}.html", new { controller = "Home", action = "snapshot" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
