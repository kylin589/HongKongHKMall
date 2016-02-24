using System.Web.Routing;

namespace BrCms.Framework.Mvc.Routes
{
    public interface IRoutePublish
    {
        void Publish(RouteCollection routes);
        int Order { get; }
    }
}
