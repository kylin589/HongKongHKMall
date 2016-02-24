using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using Autofac;

namespace HKTMall.Web
{
    public class ExceptionLogAttribute : HandleErrorAttribute, IExceptionFilter
    {
        ILogger log = BrEngineContext.Current.Resolve<ILogger>();
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
                return;
            base.OnException(filterContext);

            // 记录异常消息
            Exception ex = filterContext.Exception;
            string exUrl = HttpContext.Current.Request.RawUrl;//错误发生地址

           // log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
            log.Error(typeof(ExceptionLogAttribute), string.Format("引发异常的方法：{0}\n错误信息：{1}\n错误堆栈：{2}\n错误发生地址：{3}\n", ex.TargetSite, ex.Message, ex.StackTrace, exUrl));
            filterContext.ExceptionHandled = true;
            // 跳转至Error页面
            //filterContext.HttpContext.Response.Redirect("/Home/Error");
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Area", "" }, { "controller", "Home" }, { "action", "NotFound" } });


        }
    }
}