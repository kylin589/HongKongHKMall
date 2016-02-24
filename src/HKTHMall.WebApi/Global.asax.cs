using System.Web;
using System.Web.Http;
using BrCms.Framework.WebApi2;
using HKTHMall.Core;
using HKTHMall.WebApi.Common;
using System;
using System.Web.Routing;
using System.Web.Mvc;


namespace HKTHMall.WebApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            
            WebApiEngineContext.Init(null);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Em_Start();
        }
        /// <summary>
        /// 启动EM本地服务
        /// </summary>
        public void Em_Start()
        {
            if (!Settings.IsEnableEM)
            {
                Logger.Write("EmLog", "SJ启动帐号系统的开关已关闭,没启动帐号系统！");
            }
            else
            {
                //启动帐号系统中间件 
                try
                {
                    Logger.Write("EmLog", System.DateTime.Now.ToString() + " : " + "SJ-开始启动帐号系统中间件和连接...");
                    //启动EM本地服务
                    HKSJ.MidMessage.Core.EMLauncherRun.EMLauncherStartUp();
                    Logger.Write("EmLog", "SJ-成功启动帐号系统中间件.OK");
                }
                catch (Exception ex)
                {
                    Logger.Write("EmLog", "SJ-启动帐号系统中间件失败.False" + ex.ToString());
                }
            }
        }
    }
}