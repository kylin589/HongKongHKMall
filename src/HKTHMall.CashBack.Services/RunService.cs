using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
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
using log4net;
using System.Threading;

namespace HKTHMall.CashBack.Services
{
    public partial class RunService : ServiceBase
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private ILog log = LogManager.GetLogger("logger-name");
        private IScheduler _scheduler;
        public RunService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread.Sleep(10000);
            try
            {    
                int hour = int.Parse(ConfigurationManager.AppSettings["Hour"]);
                int minute = int.Parse(ConfigurationManager.AppSettings["Minute"]);
                ISchedulerFactory sf = new StdSchedulerFactory();//执行者  
                _scheduler = sf.GetScheduler();
             
                IJobDetail job1 = JobBuilder.Create<CashBackJob>()
                   .WithIdentity("CashBackJob", "group1").Build();
                //执行返现操作 每天固定时间执行
                //ITrigger trigger1 = TriggerBuilder.Create()
                //       .WithDailyTimeIntervalSchedule(a => a.WithIntervalInHours(24).OnEveryDay()
                //       .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hour, minute))).Build();

                //test
                ITrigger trigger1 = TriggerBuilder.Create()
                       .WithDailyTimeIntervalSchedule(a => a.WithIntervalInMinutes(1).OnEveryDay() 
                       ).Build();

                _scheduler.ScheduleJob(job1, trigger1);
                _scheduler.Start();
                log.Info("Quartz服务成功启动");
            }
            catch (Exception ex)
            {
                log.Error("Quartz服务成功失败", ex);
            }
        }

        protected override void OnStop()
        {
            _scheduler.Shutdown();
            log.Info("Quartz服务成功关闭");
        }
    }
}
