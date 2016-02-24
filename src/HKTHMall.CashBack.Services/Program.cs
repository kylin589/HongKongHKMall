using BrCms.Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.CashBack.Services
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            BrEngineContext.Init(null);
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new RunService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
