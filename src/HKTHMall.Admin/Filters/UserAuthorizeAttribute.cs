using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Admin.common;
using HKTHMall.Core;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;

namespace HKTHMall.Admin.Filters
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly Authority _Authority = BrEngineContext.Current.Resolve<Authority>();
        private readonly IAC_FunctionService _FunctionService = BrEngineContext.Current.Resolve<IAC_FunctionService>();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //如果存在身份信息 
            if (UserInfo.CurrentUserID == 0)
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
                    filterContext.Result = new RedirectResult("/AC_User/login");
                }
            }
            else
            {
                var controller = filterContext.RouteData.Values["controller"].ToString();
                var action = filterContext.RouteData.Values["action"].ToString();
                List<AC_FunctionModel> funList = (this._FunctionService.GetAC_FunList()).Data;
                var tempList = new List<AC_FunctionModel>();
                tempList = funList.Where(t => t.Action == action && t.Controller == controller).ToList();
                if (tempList.Count > 0)
                {
                    var funcNum = 0;
                    funcNum = tempList.First().FunctionID;
                    if (!this._Authority.CheckAction(funcNum))
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest()) // AJAX请求,返回status标识未登录
                        {
                            filterContext.Result = new JsonResult
                            {
                                Data = new {status = "NOT_AUTHORIZED"},
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }
                        else
                        {
                            var Content = new ContentResult();
                            Content.Content = "<script type='text/javascript'>alert('Unauthorized access!');history.go(-1);</script>";
                            filterContext.Result = Content;
                        }
                    }
                }
            }
        }
    }
}