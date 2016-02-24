using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.WebApi.Areas.Huifen
{
    public class HuifenAreaNew : AreaRegistration 
    {
        public override string AreaName
        {
            get
            {
                return "Huifen";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Huifen_default44",
                "Huifen/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}