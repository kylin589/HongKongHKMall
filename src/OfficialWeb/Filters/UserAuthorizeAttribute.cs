using BrCms.Framework.Infrastructure;
using HKTHMall.Services.AC;
using OfficialWeb.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OfficialWeb.Filters
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //如果存在身份信息 
            if (SuppliersLogin.CurrentSuppliersID == 0)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest()) // AJAX请求,返回status标识未登录
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new { status = "NoLanding", Messages = new List<string> { "Please login!" } },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Login/index");
                }
            }
            
        }
    }
}