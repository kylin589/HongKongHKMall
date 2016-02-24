using BrCms.Framework.Infrastructure;

using HKTHMall.Services.Users;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

using log4net;
using HKTHMall.Domain.CashBack;

namespace HKTHMall.CashBack.Services
{
    /// <summary>
    /// 处理返现业务
    /// </summary>
    public class CashBackJob : IJob
    {
        private ILog log;
        private IZJ_RebateService RebateService;
        public CashBackJob() 
        {
            log = LogManager.GetLogger(typeof(CashBackJob));
            RebateService = BrEngineContext.Current.Resolve<IZJ_RebateService>();
        }

        public void Execute(IJobExecutionContext context)
        {
            RebateService.CashBackOrder();             
          
            log.Info(string.Format("{0}返现已完成", DateTime.Now));
        }
    }
}
