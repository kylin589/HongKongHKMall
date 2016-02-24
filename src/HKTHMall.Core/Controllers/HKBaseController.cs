using System.Web.Mvc;

namespace HKTHMall.Core.Controllers
{
    public class HKBaseController : Controller
    {

        public HKBaseController()
        {
            ViewBag.RootImage = ToolUtil.getFilePath("ImagePath");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //if (filterContext.HttpContext.Session["UserInfo"] == null)
            //{
            //    if (filterContext.HttpContext.Request.Url.Segments.Length > 2)
            //    {
            //        if (filterContext.HttpContext.Request.Url.Segments[2].ToLower().Trim() != "islogin" && filterContext.HttpContext.Request.Url.Segments[2].ToLower().Trim() != "login")
            //        {
            //            //跳转到登陆页
            //            filterContext.Result = new RedirectResult("/AC_User/login");
            //        }
            //    }
            //    else
            //    {
            //        //跳转到登陆页
            //        filterContext.Result = new RedirectResult("/AC_User/login");
            //    }
            //}
            //base.OnActionExecuting(filterContext);

        }
    }
}
