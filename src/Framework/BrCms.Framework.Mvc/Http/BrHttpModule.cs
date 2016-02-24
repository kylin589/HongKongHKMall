using System;
using System.Web;

namespace BrCms.Framework.Mvc.Http
{
    public class BrHttpModule : IHttpModule
    {
        public void Dispose()
        {
            //throw new System.NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += ReUrl_BeginRequest;
        }

        /// <summary>
        /// 重写Url
        /// </summary>
        /// <param name="sender">事件的源</param>
        /// <param name="e">包含事件数据的 EventArgs</param>
        private static void ReUrl_BeginRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            var requestPath = context.Request.Path.ToLower();
            if (requestPath.Contains("CrystalReports".ToLower()))
            {
                context.RewritePath("WebForm/CrystalReports.aspx");
            }
        }
    }
}
