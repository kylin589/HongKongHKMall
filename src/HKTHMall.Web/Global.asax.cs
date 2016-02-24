using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using BrCms.Framework.Mvc;
using System;
using System.Web.Security;
using HKTHMall.Core;
using HKTHMall.Web.Common;
using System.IO;
using System.Collections.Generic;

namespace HKTHMall.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            BrMvcEngineContext.Init(null);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

    

        #region 火狐浏览器使用uploadify上传失败解决方案
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            /* we guess at this point session is not already retrieved by application so we recreate cookie with the session id... */
            try
            {
                string session_param_name = "ASPSESSID";
                string session_cookie_name = "ASP.NET_SessionId";

                if (HttpContext.Current.Request.Form[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form[session_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString[session_param_name]);
                }
            }
            catch
            {
            }

            try
            {
                string auth_param_name = "AUTHID";
                string auth_cookie_name = FormsAuthentication.FormsCookieName;

                if (HttpContext.Current.Request.Form[auth_param_name] != null)
                {
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form[auth_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[auth_param_name] != null)
                {
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.QueryString[auth_param_name]);
                }

            }
            catch
            {
            }
        }

        private void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (null == cookie)
            {
                cookie = new HttpCookie(cookie_name);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        } 
        #endregion

        ////捕捉404错误 自定义到404错误页面 当然也可以添加别的错误自定义
        protected void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();
            var code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;
            //如果不是HttpException记录错误信息
            if (code == 404)
            {
                //此处邮件或日志记录错误信息
                Response.Clear();
                Server.ClearError();
                string path = Request.Path;
                //log4net.ILog log = log4net.LogManager.GetLogger(this.GetType());
               // log.Error(string.Format("错误发生地址:{0}\n", path));
                string Extension = Path.GetExtension(path);
                if (!boolPicType(Path.GetExtension(path)))
                {
                    //if (Request.UrlReferrer != null)
                    //{
                    //    Context.Response.Redirect(Request.UrlReferrer.ToString());
                    //}
                    //else
                    //{
                    //    Context.Response.Redirect("/NotFound.html");
                    //}
                    Context.Response.Redirect("/NotFound.html");
                }
            }
        }

        /// <summary>
        /// 判断是否重定向的例外（对图片不存在的例外）
        /// </summary>
        /// <param name="picExtension"></param>
        /// <returns></returns>
        public static bool boolPicType(string picExtension)
        {
            bool res = false;
            picExtension = picExtension.ToLower();
            List<string> extList = new List<string>() { ".jpg", ".jpeg", ".gif", ".png" };
            foreach (var Item in extList)
            {
                if (picExtension == Item)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }
    }
}