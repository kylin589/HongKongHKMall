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
using HKTHMall.Domain.Entities;
using HKTHMall.Services.Sys;
using HKTHMall.Services.AC;
using HKTHMall.Domain.AdminModel.Models.AC;

namespace HKTMall.Monitor
{
    /// <summary>
    /// 订单监控任务业务处理
    /// Created By zhoub 20150722
    /// </summary>
    public class OrderJob : IJob
    {
        private readonly static IOrderService orderService = BrEngineContext.Current.Resolve<IOrderService>();
        private readonly static IExceptionLogService exceptionLogService = BrEngineContext.Current.Resolve<IExceptionLogService>();
        private readonly static ILogger _logger = BrEngineContext.Current.Resolve<ILogger>();
        private readonly static IParameterSetService _parameterSetService = BrEngineContext.Current.Resolve<IParameterSetService>();
        private readonly static IDatabaseHelper _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
        private readonly static int OrderJobSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["OrderJobSeconds"]);             //处理订单超期支付、订单过期服务间隔时间(秒)
        private readonly static int payMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["payMinutes"]);                       //超期支付分钟数
        private readonly static int delayDays = Convert.ToInt32(ConfigurationManager.AppSettings["delayDays"]);                        //收货天数
        private readonly static long companyAccountParamenterID = Convert.ToInt64(ConfigurationManager.AppSettings["companyAccountParamenterID"]);  //公司虚拟帐户系统参数ID
        private readonly static int languageID = 3;
        public void Execute(IJobExecutionContext context)
        {
            ExceptionLogModel exceptionLogModel = new ExceptionLogModel();
            exceptionLogModel.ServiceName = "处理订单超期支付、超期收货服务";
            exceptionLogModel.CreateBy = "系统服务";
            
            try
            {
                _logger.Error(typeof(OrderJob), "处理订单超期支付、超期收货服务开始***********************************");
                //订单信息
                string sqlOrder = string.Format(@"select OrderID,OrderStatus,DelayDays,OrderAmount,CostAmount,UserID from [Order] where OrderStatus in(2,4)");
                List<dynamic> sourcesOrder = _database.RunSqlQuery(x => x.ToResultSets(sqlOrder))[0];
                List<OrderModel> listOrder = sourcesOrder.ToEntity<OrderModel>();
                //订单跟踪信息
                string sqlOrderTrackingLog = string.Format(@"select OrderID,OrderStatus,CreateTime from OrderTrackingLog where OrderID in(select OrderID from [Order] where OrderStatus in(2,4))");
                List<dynamic> sourcesOrderTrackingLog = _database.RunSqlQuery(x => x.ToResultSets(sqlOrderTrackingLog))[0];
                List<OrderTrackingLogModel> listOrderTrackingLog = sourcesOrderTrackingLog.ToEntity<OrderTrackingLogModel>();

                if (listOrder != null && listOrder.Count > 0)
                {
                    //公司虚拟帐户ID
                    long companyAccountUserID = Convert.ToInt64(_parameterSetService.GetParametePValueById(companyAccountParamenterID).Data);
                    foreach (OrderModel orderModel in listOrder)
                    {
                        try
                        {
                            #region 处理订单的超期支付==正常两天后自动取消订单

                            if (orderModel.OrderStatus == (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.Obligation)
                            {
                                //订单详情
                                //List<OrderDetailsModel> listOrderDetails = _database.Db.OrderDetails.All().Where(_database.Db.OrderDetails.OrderID == orderModel.OrderID).ToList<OrderDetailsModel>();
                                //获取订单跟踪创建时间
                                var orderLog = listOrderTrackingLog.Find(orderTrackingLog => orderTrackingLog.OrderID == orderModel.OrderID && orderTrackingLog.OrderStatus == orderModel.OrderStatus);
                                if (orderLog != null)
                                {
                                    DateTime orderTrackingTime = listOrderTrackingLog.Find(orderTrackingLog => orderTrackingLog.OrderID == orderModel.OrderID && orderTrackingLog.OrderStatus == orderModel.OrderStatus).CreateTime;
                                    DateTime endTime = orderTrackingTime;

                                    #region 支付提醒
                                    //bool isExplosionOrder = false;
                                    //bool isExplosionOrder = listOrderDetails.Count(a => a.ActivityType == 1) > 0;
                                    //if (isExplosionOrder)
                                    //{
                                    //    #region 发送爆款订单支付提醒短信
                                    //    //if (orderModel.PaySmsStatus == 1 && orderTrackingTime.AddMinutes(explosionOrderPaySmsMinutes) < DateTime.Now)
                                    //    //{
                                    //    //    string smsContent = string.Format(explosionOrderPayRemind, orderModel.OrderNumber, explosionPayMinutes);
                                    //    //    PinMall.BLL.SM.PhoneMsg smsResult = PinMall.BLL.SM.SendPhoneMsg.SendPhoneMsgActive(orderModel.ReceiverPhone, smsContent, 8);
                                    //    //    if (!smsResult.IsMessage)
                                    //    //    {
                                    //    //        HKMallMonitorService.S_logger.Error(string.Format("处理爆款订单【{0}】的支付提醒短信失败,短信发送失败,{1}", orderModel.OrderNumber, smsResult.Msg));
                                    //    //    }
                                    //    //    orderModel.PaySmsStatus = smsResult.IsMessage ? 2 : 3;
                                    //    //    orderModel.UpdateDT = DateTime.Now;
                                    //    //    orderModel.UpdateBy = "惠卡商城订单监控服务";
                                    //    //    if (orderModel.Update() < 1)
                                    //    //    {
                                    //    //        HKMallMonitorService.S_logger.Error(string.Format("处理爆款订单【{0}】的支付提醒短信失败,数据库执行失败", orderModel.OrderNumber));
                                    //    //    }
                                    //    //}
                                    //    #endregion

                                    //    //endTime = orderTrackingTime.AddMinutes(explosionPayMinutes);
                                    //}
                                    //else
                                    //{
                                    //    #region 发送订单支付提醒短信
                                    //    //if (orderModel.PaySmsStatus == 1 && orderTrackingTime.AddMinutes(orderPaySmsMinutes) < DateTime.Now)
                                    //    //{
                                    //    //    string smsContent = string.Format(orderPayRemind, orderModel.OrderNumber);
                                    //    //    PinMall.BLL.SM.PhoneMsg smsResult = PinMall.BLL.SM.SendPhoneMsg.SendPhoneMsgActive(orderModel.ReceiverPhone, smsContent, 9);
                                    //    //    if (!smsResult.IsMessage)
                                    //    //    {
                                    //    //        HKMallMonitorService.S_logger.Error(string.Format("处理订单【{0}】的支付提醒短信失败,短信发送失败,{1}", orderModel.OrderNumber, smsResult.Msg));
                                    //    //    }
                                    //    //    orderModel.PaySmsStatus = smsResult.IsMessage ? 2 : 3;
                                    //    //    orderModel.UpdateDT = DateTime.Now;
                                    //    //    orderModel.UpdateBy = "惠卡商城订单监控服务";
                                    //    //    if (orderModel.Update() < 1)
                                    //    //    {
                                    //    //        HKMallMonitorService.S_logger.Error(string.Format("处理订单【{0}】的支付提醒短信失败,数据库执行失败", orderModel.OrderNumber));
                                    //    //    }
                                    //    //}
                                    //    #endregion


                                    //}
                                    #endregion
                                    endTime = orderTrackingTime.AddMinutes(payMinutes);
                                    if (endTime < DateTime.Now)
                                    {
                                        #region 还原库存

                                        try
                                        {
                                            SearchOrderDetailView view = new SearchOrderDetailView();
                                            view.OrderStatus = orderModel.OrderStatus;
                                            view.OrderID = orderModel.OrderID;
                                            view.LanguageID = languageID;
                                            ResultModel model = orderService.CancelOrderBy(view);
                                            if (model.IsValid)
                                            {
                                                _logger.Error(typeof(OrderJob), string.Format("处理订单【{0}】的还原库存成功", orderModel.OrderID));
                                            }
                                            else
                                            {
                                                exceptionLogModel.HandleId = orderModel.OrderID;
                                                exceptionLogModel.Status = 1;
                                                exceptionLogModel.ResultType = 1;
                                                exceptionLogModel.Message = string.Format("处理订单【{0}】的还原库存失败【{1}】", orderModel.OrderID, model.Messages);
                                                exceptionLogService.Add(exceptionLogModel);
                                                _logger.Error(typeof(OrderJob), string.Format("处理订单【{0}】的还原库存失败【{1}】", orderModel.OrderID, model.Messages));
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            exceptionLogModel.HandleId = orderModel.OrderID;
                                            exceptionLogModel.Status = 1;
                                            exceptionLogModel.ResultType = 1;
                                            exceptionLogModel.Message = "【还原库存】" + ex.Message;
                                            exceptionLogService.Add(exceptionLogModel);
                                            _logger.Error(typeof(OrderJob), "【还原库存】" + ex.Message);
                                        }

                                        #endregion
                                    }
                                }
                                else {
                                    exceptionLogModel.HandleId = orderModel.OrderID;
                                    exceptionLogModel.Status = 1;
                                    exceptionLogModel.ResultType = 1;
                                    exceptionLogModel.Message = "处理订单的超期支付,订单【" + orderModel.OrderID + "】跟踪信息查询无结果";
                                    exceptionLogService.Add(exceptionLogModel);
                                    _logger.Error(typeof(OrderJob), "处理订单的超期支付,订单【" + orderModel.OrderID + "】跟踪信息查询无结果");
                                }
                            }

                            #endregion

                            #region 处理订单的超期收货==正常7天后自动收货操作

                            if (orderModel.OrderStatus == (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.WaitReceiving)
                            {
                                var orderLog = listOrderTrackingLog.Find(orderTrackingLog => orderTrackingLog.OrderID == orderModel.OrderID && orderTrackingLog.OrderStatus == orderModel.OrderStatus);
                                if (orderLog != null)
                                {
                                    DateTime endTime = orderLog.CreateTime;
                                    endTime = endTime.AddDays(delayDays + orderModel.DelayDays);
                                    if (endTime < DateTime.Now)
                                    {
                                        using (var bt = _database.Db.BeginTransaction())
                                        {
                                            try
                                            {
                                                //更新订单状态
                                                int flag1 = _database.Db.Order.UpdateByOrderID(OrderID: orderModel.OrderID, OrderStatus: (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.OutTimeReceiving);
                                                //记录订单跟踪信息
                                                //订单收货超时记录
                                                OrderTrackingLogModel orderTrackingLogOne = new OrderTrackingLogModel();
                                                orderTrackingLogOne.OrderID = orderModel.OrderID;
                                                orderTrackingLogOne.OrderStatus = (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.OutTimeReceiving;
                                                orderTrackingLogOne.TrackingContent = "收货超时";
                                                orderTrackingLogOne.CreateTime = DateTime.Now.AddMilliseconds(-500);
                                                orderTrackingLogOne.CreateBy = "惠卡商城订单监控服务";
                                                _database.Db.OrderTrackingLog.Insert(orderTrackingLogOne);
                                                ////订单确认收货记录
                                                //OrderTrackingLogModel orderTrackingLogTwo = new OrderTrackingLogModel();
                                                //orderTrackingLogTwo.OrderID = orderModel.OrderID;
                                                //orderTrackingLogTwo.OrderStatus = (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.Completed;
                                                //orderTrackingLogTwo.TrackingContent = "确认收货";
                                                //orderTrackingLogTwo.CreateTime = DateTime.Now;
                                                //orderTrackingLogTwo.CreateBy = "惠卡商城订单监控服务";
                                                //_database.Db.OrderTrackingLog.Insert(orderTrackingLogTwo);

                                                //平台打款给卖家
                                                bool flag3 = orderService.AddAmountNoTran(orderModel.OrderID, orderModel.MerchantID, orderModel.CostAmount, 5, "订单收货超时,自动收货打款", "惠卡商城订单监控服务").IsValid;
                                                //平台账户扣款
                                                bool flag4 = orderService.CutAmountNoTran(orderModel.OrderID, companyAccountUserID, orderModel.CostAmount, 5, "结算货款（商家账号:" + orderModel.MerchantID.ToString() + "）", "惠卡商城订单监控服务").IsValid;

                                                //短信提醒卖家(todo)

                                                if (flag1 > 0 && flag3 && flag4)
                                                {
                                                    bt.Commit();
                                                    exceptionLogModel.HandleId = orderModel.OrderID;
                                                    exceptionLogModel.Status = 1;
                                                    exceptionLogModel.ResultType = 1;
                                                    exceptionLogModel.Message = string.Format("处理订单【{0}】的超期收货成功", orderModel.OrderID);
                                                    exceptionLogService.Add(exceptionLogModel);
                                                    _logger.Error(typeof(OrderJob), string.Format("处理订单【{0}】的超期收货成功", orderModel.OrderID));
                                                }
                                                else
                                                {
                                                    bt.Rollback();
                                                    exceptionLogModel.HandleId = orderModel.OrderID;
                                                    exceptionLogModel.Status = 1;
                                                    exceptionLogModel.ResultType = 1;
                                                    exceptionLogModel.Message = string.Format("处理订单【{0}】的超期收货失败,数据库事务回滚", orderModel.OrderID);
                                                    exceptionLogService.Add(exceptionLogModel);
                                                    _logger.Error(typeof(OrderJob), string.Format("处理订单【{0}】的超期收货失败,数据库事务回滚", orderModel.OrderID));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                bt.Rollback();
                                                exceptionLogModel.HandleId = orderModel.OrderID;
                                                exceptionLogModel.Status = 1;
                                                exceptionLogModel.ResultType = 1;
                                                exceptionLogModel.Message = "【超期收货】" + ex.Message;
                                                exceptionLogService.Add(exceptionLogModel);
                                                _logger.Error(typeof(OrderJob), "【超期收货】" + ex.Message);
                                            }
                                        }
                                    }
                                }
                                else {
                                    exceptionLogModel.HandleId = orderModel.OrderID;
                                    exceptionLogModel.Status = 1;
                                    exceptionLogModel.ResultType = 1;
                                    exceptionLogModel.Message = "处理订单的超期收货,订单【" + orderModel.OrderID + "】跟踪信息查询无结果";
                                    exceptionLogService.Add(exceptionLogModel);
                                    _logger.Error(typeof(OrderJob), "处理订单的超期收货,订单【" + orderModel.OrderID + "】跟踪信息查询无结果");
                                }
                            }

                            #endregion
                        }
                        catch (Exception ex)
                        {
                            exceptionLogModel.HandleId = orderModel.OrderID;
                            exceptionLogModel.Status = 1;
                            exceptionLogModel.ResultType = 1;
                            exceptionLogModel.Message = "【处理订单超期支付、超期收货服务foreach操作步骤】" + ex.Message;
                            exceptionLogService.Add(exceptionLogModel);
                            _logger.Error(typeof(OrderJob), "【处理订单超期支付、超期收货服务foreach操作步骤】" + ex.Message);
                        }
                    }
                }
                else {
                    _logger.Error(typeof(ProductRuleJob), "【处理订单超期支付、超期收货服务】本次未处理任何数据.");
                }
                exceptionLogModel.HandleId = "0";
                exceptionLogModel.Status = 2;
                exceptionLogModel.ResultType = 2;
                exceptionLogModel.Message = "运行正常";
                exceptionLogService.Add(exceptionLogModel);
                _logger.Error(typeof(OrderJob), "处理订单超期支付、超期收货服务结束**********************************");
            }
            catch (Exception ex)
            {
                exceptionLogModel.HandleId = "0";
                exceptionLogModel.Status = 1;
                exceptionLogModel.ResultType = 1;
                exceptionLogModel.Message = "【处理订单超期支付、超期收货服务运行环节报错】" + ex.Message;
                exceptionLogService.Add(exceptionLogModel);
                _logger.Error(typeof(OrderJob), "【处理订单超期支付、超期收货服务运行环节报错】" + ex.Message);
            }
        }

        public static void Create()
        {
            IJobDetail orderJob = JobBuilder.Create<OrderJob>()
                                                            .WithIdentity("订单监控任务", "JobGroup")
                                                            .Build();
            ITrigger orderTrigger = TriggerBuilder.Create()
                .WithIdentity(Guid.NewGuid().ToString(), "simpleName")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(OrderJobSeconds)
                    .RepeatForever())
                .Build();
            HKTMallMonitorService.S_scheduler.ScheduleJob(orderJob, orderTrigger);
            _logger.Error(typeof(OrderJob), "处理订单超期支付、超期收货服务启动");
        }
    }
}
