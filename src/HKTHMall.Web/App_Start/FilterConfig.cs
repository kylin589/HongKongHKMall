using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTMall.Web;


namespace HKTHMall.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new ActionFilter());
            filters.Add(new ExceptionLogAttribute());//注册全局错误过滤器
        }
    }
}