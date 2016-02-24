using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using HKTHMall.Services.Orders;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace HKTMall.Monitor.Services
{
    public partial class HKTMallMonitorService : ServiceBase
    {
        internal static IScheduler S_scheduler;
        ILogger _logger = BrEngineContext.Current.Resolve<ILogger>();

        public HKTMallMonitorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                string projectName = ConfigurationManager.AppSettings["ProjectName"];
                S_scheduler = StdSchedulerFactory.GetDefaultScheduler();
                S_scheduler.Start();
                OrderJob.Create();
                ProductRuleJob.Create();
                BounsJob.Create();
                //PurchaseOrderJob.Create();
                _logger.Error(typeof(HKTMallMonitorService), string.Format("{0}监控服务启动成功", projectName));
            }
            catch (Exception ex)
            {
                _logger.Error(typeof(HKTMallMonitorService), ex.Message);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
