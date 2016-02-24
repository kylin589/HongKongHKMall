using HKTHMall.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using Simple.Data.RawSql;
using Simple.Data;
using HKTHMall.Core.Extensions;
using HKTHMall.Core;
using BrCms.Framework.Data;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTMall.Monitor.Services;
using Autofac;
using HKTHMall.Services.Orders;
using System.Configuration;
using BrCms.Framework.Logging;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Services.AC;
using HKTHMall.Domain.AdminModel.Models.AC;

namespace HKTMall.Monitor
{
    public class ProductRuleJob : IJob
    {
        private readonly static ILogger _logger = BrEngineContext.Current.Resolve<ILogger>();
        private readonly static IExceptionLogService exceptionLogService = BrEngineContext.Current.Resolve<IExceptionLogService>();
        private readonly static IDatabaseHelper _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
        private readonly static int ProductRuleJobSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["ProductRuleJobSeconds"]);

        public void Execute(IJobExecutionContext context)
        {
            ExceptionLogModel exceptionLogModel = new ExceptionLogModel();
            exceptionLogModel.ServiceName = "商品促销到期实时监控服务";
            exceptionLogModel.CreateBy = "系统服务";
            try
            {
                _logger.Error(typeof(ProductRuleJob), "商品促销到期实时监控服务开始***********************************");
                //商品促销过期信息
                string sqlProductRule = string.Format(@"select ProductRuleId from ProductRule where SalesRuleId>1 and EndDate<GETDATE()");
                List<dynamic> sourcesProductRule = _database.RunSqlQuery(x => x.ToResultSets(sqlProductRule))[0];
                List<ProductRuleModel> listProductRule = sourcesProductRule.ToEntity<ProductRuleModel>();

                if (listProductRule != null && listProductRule.Count > 0)
                {
                    foreach (ProductRuleModel productRuleModel in listProductRule)
                    {
                        try
                        {
                            int flag1 = _database.Db.ProductRule.UpdateByProductRuleId(ProductRuleId: productRuleModel.ProductRuleId, SalesRuleId: 1);
                            if (flag1 > 0)
                            {
                                _logger.Error(typeof(ProductRuleJob), string.Format("处理商品促销【{0}】到期成功", productRuleModel.ProductRuleId));
                            }
                            else
                            {
                                exceptionLogModel.HandleId = productRuleModel.ProductRuleId.ToString();
                                exceptionLogModel.Status = 1;
                                exceptionLogModel.ResultType = 1;
                                exceptionLogModel.Message = string.Format("处理商品促销【{0}】到期失败", productRuleModel.ProductRuleId);
                                exceptionLogService.Add(exceptionLogModel);
                                _logger.Error(typeof(ProductRuleJob), string.Format("处理商品促销【{0}】到期失败", productRuleModel.ProductRuleId));
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionLogModel.HandleId = productRuleModel.ProductRuleId.ToString();
                            exceptionLogModel.Status = 1;
                            exceptionLogModel.ResultType = 1;
                            exceptionLogModel.Message = string.Format("处理商品促销【{0}】到期失败foreach", productRuleModel.ProductRuleId);
                            exceptionLogService.Add(exceptionLogModel);
                            _logger.Error(typeof(ProductRuleJob), ex.Message);
                        }
                    }
                }
                else
                {
                    _logger.Error(typeof(ProductRuleJob), "【商品促销到期实时监控服务】本次未处理任何数据.");
                }
                exceptionLogModel.HandleId = "0";
                exceptionLogModel.Status = 2;
                exceptionLogModel.ResultType = 2;
                exceptionLogModel.Message = "运行正常";
                exceptionLogService.Add(exceptionLogModel);
                _logger.Error(typeof(ProductRuleJob), "处理商品促销到期服务结束**********************************");
            }
            catch (Exception ex)
            {
                exceptionLogModel.HandleId = "0";
                exceptionLogModel.Status = 1;
                exceptionLogModel.ResultType = 1;
                exceptionLogModel.Message = "【商品促销到期实时监控服务运行环节报错】" + ex.Message;
                exceptionLogService.Add(exceptionLogModel);
                _logger.Error(typeof(ProductRuleJob), ex.Message);
            }
        }

        public static void Create()
        {
            IJobDetail productRuleJob = JobBuilder.Create<ProductRuleJob>()
                                                            .WithIdentity("商品促销", "ProductRuleJobGroup")
                                                            .Build();
            ITrigger productRuleTrigger = TriggerBuilder.Create()
                .WithIdentity(Guid.NewGuid().ToString(), "ProductRuleJobName")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(ProductRuleJobSeconds)
                    .RepeatForever())
                .Build();
            HKTMallMonitorService.S_scheduler.ScheduleJob(productRuleJob, productRuleTrigger);
            _logger.Error(typeof(ProductRuleJob), "商品促销到期实时监控服务启动");
        }
    }
}
