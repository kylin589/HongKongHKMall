using BrCms.Framework.Infrastructure;
using HKTHMall.CashBack.Services;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BrCms.Framework.Logging;
using log4net;
using System.Collections.Specialized;
using HKTHMall.Services.Common.MultiLangKeys;




namespace ConsoleApplication1
{
    public class Program
    {


        public static void Main(string[] args)
        {
            BrEngineContext.Init(null);
            ILog log = LogManager.GetLogger("logger-name");

            IScheduler _scheduler = null;
            try
            {
                DateTimeOffset runTime = DateTimeOffset.UtcNow;
                DateTimeOffset.TryParse(DateTime.Now.ToShortDateString(), out runTime);
                int hour = int.Parse(ConfigurationManager.AppSettings["Hour"]);
                int minute = int.Parse(ConfigurationManager.AppSettings["Minute"]);
                ISchedulerFactory sf = new StdSchedulerFactory();//执行者  
                _scheduler = sf.GetScheduler();


                IJobDetail job1 = JobBuilder.Create<CashBackJob>()
                   .WithIdentity("CashBackJob", "group1").Build();
                //执行返现操作 每天固定时间执行
                ITrigger trigger1 = TriggerBuilder.Create()
                      .StartNow()
                       .WithDailyTimeIntervalSchedule(
                         a => a.WithIntervalInHours(24).OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(hour, minute)))
                         .Build();

                IJobDetail bounsJob = JobBuilder.Create<BounsJob>()
                                                             .WithIdentity("会员购物订单返现订单生成服务", "BounsJobGroup")
                                                             .Build();
                ITrigger orderTrigger = TriggerBuilder.Create()
                    .WithIdentity(Guid.NewGuid().ToString(), "BounsJobName")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(1)
                        .RepeatForever())
                    .Build();
                 _scheduler.ScheduleJob(bounsJob, orderTrigger);               

                //_scheduler.ScheduleJob(job1, trigger1);
                _scheduler.Start();
                log.Info("Quartz服务成功启动");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
