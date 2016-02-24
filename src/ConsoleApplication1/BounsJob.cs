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
using HKTHMall.Services.Users;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.CashBack;
using OrderDetails = HKTHMall.Domain.CashBack.OrderDetails;

namespace HKTHMall.CashBack.Services
{
    public class BounsJob : IJob
    {
        private readonly static IOrderService orderService = BrEngineContext.Current.Resolve<IOrderService>();
        private readonly static IExceptionLogService exceptionLogService = BrEngineContext.Current.Resolve<IExceptionLogService>();
        private readonly static ILogger _logger = BrEngineContext.Current.Resolve<ILogger>();
        private readonly static IParameterSetService _parameterSetService = BrEngineContext.Current.Resolve<IParameterSetService>();
        private readonly static IDatabaseHelper _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
        private readonly static IZJ_RebateService _rebateService = BrEngineContext.Current.Resolve<IZJ_RebateService>();
        private readonly static int bounsJobSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["BounsJobSeconds"]);
        private readonly static int earningsDays = Convert.ToInt32(ConfigurationManager.AppSettings["earningsDays"]);   //收益分润分钟数
        private readonly static long companyAccountParamenterID = Convert.ToInt64(ConfigurationManager.AppSettings["companyAccountParamenterID"]);  //公司虚拟帐户系统参数ID
        public void Execute(IJobExecutionContext context)
        {
            ExceptionLogModel exceptionLogModel = new ExceptionLogModel();
            exceptionLogModel.ServiceName = "会员购物订单返现订单生成服务";
            exceptionLogModel.CreateBy = "系统服务";

            try
            {
                _logger.Error(typeof(BounsJob), "会员购物订单返现订单生成服务开始***********************************");
                //订单信息
                string sqlOrder = string.Format(@"select OrderID,OrderStatus,DelayDays,OrderAmount,CostAmount,UserID,RefundFlag from [Order] where OrderStatus=5 and RefundFlag in(0,2)");
                List<dynamic> sourcesOrder = _database.RunSqlQuery(x => x.ToResultSets(sqlOrder))[0];
                List<OderModel> listOrder = sourcesOrder.ToEntity<OderModel>();
                //订单跟踪信息
                string sqlOrderTrackingLog = string.Format(@"select OrderID,OrderStatus,CreateTime from OrderTrackingLog where OrderID in(select OrderID from [Order] where OrderStatus=5)");
                List<dynamic> sourcesOrderTrackingLog = _database.RunSqlQuery(x => x.ToResultSets(sqlOrderTrackingLog))[0];
                List<OrderTrackingLogModel> listOrderTrackingLog = sourcesOrderTrackingLog.ToEntity<OrderTrackingLogModel>();

                if (listOrder != null && listOrder.Count > 0)
                {
                    foreach (OderModel orderModel in listOrder)
                    {
                        YH_User currUser = (YH_User)_database.Db.YH_User.FindByUserID(orderModel.UserID);
                        if (currUser != null)
                        {
                            try
                            {
                                #region 订单表状态为已收货OrderStatus=5且退货标示0正常,2已处理（包括成功，失败）RefundFlag in{0,2}

                                if (orderModel.OrderStatus == (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.OutTimeReceiving)
                                {
                                    var orderLog = listOrderTrackingLog.Find(orderTrackingLog => orderTrackingLog.OrderID == orderModel.OrderID && orderTrackingLog.OrderStatus == orderModel.OrderStatus);
                                    if (orderLog != null)
                                    {
                                        //DateTime endTime = orderLog.CreateTime.AddDays(earningsDays);
                                        DateTime endTime = orderLog.CreateTime.AddMinutes(earningsDays);
                                        if (endTime < DateTime.Now && orderModel.IsReward == 0)
                                        {
                                            if (orderModel.RefundFlag !=1)
                                            {
                                                BackMessage resultModel = _rebateService.GenerateList(orderModel);


                                                if (resultModel.status==1)
                                                {
                                                    _logger.Error(typeof(BounsJob), string.Format("处理订单【{0}】的返现订单生成成功", orderModel.OrderID));
                                                }
                                                else
                                                {                                                   
                                                    exceptionLogModel.HandleId = orderModel.OrderID;
                                                    exceptionLogModel.Status = 1;
                                                    exceptionLogModel.ResultType = 1;
                                                    exceptionLogModel.Message = string.Format("处理订单【{0}】的会员购物订单返现订单生成失败,", orderModel.OrderID);
                                                    exceptionLogService.Add(exceptionLogModel);
                                                    _logger.Error(typeof(BounsJob), string.Format("处理订单【{0}】的会员购物订单返现订单生成失败,", orderModel.OrderID));
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        exceptionLogModel.HandleId = orderModel.OrderID;
                                        exceptionLogModel.Status = 1;
                                        exceptionLogModel.ResultType = 1;
                                        exceptionLogModel.Message = "订单返现订单生成,订单【" + orderModel.OrderID + "】跟踪信息查询无结果";
                                        exceptionLogService.Add(exceptionLogModel);
                                        _logger.Error(typeof(BounsJob), "订单返现订单生成,订单【" + orderModel.OrderID + "】跟踪信息查询无结果");
                                    }
                                }

                                #endregion
                            }
                            catch (Exception ex)
                            {
                                exceptionLogModel.HandleId = orderModel.OrderID;
                                exceptionLogModel.Status = 1;
                                exceptionLogModel.ResultType = 1;
                                exceptionLogModel.Message = "【会员购物订单返现订单生成算法foreach】" + ex.Message;
                                exceptionLogService.Add(exceptionLogModel);
                                _logger.Error(typeof(BounsJob), "【会员购物订单返现订单生成算法foreach】" + ex.Message);
                            }
                        }
                        else
                        {
                            exceptionLogModel.HandleId = orderModel.OrderID;
                            exceptionLogModel.Status = 1;
                            exceptionLogModel.ResultType = 1;
                            exceptionLogModel.Message = string.Format("处理订单【{0}】的订单返现订单生成失败,获取订单用户【{1}】信息失败", orderModel.OrderID, orderModel.UserID);
                            exceptionLogService.Add(exceptionLogModel);
                            _logger.Error(typeof(BounsJob), string.Format("处理订单【{0}】的订单返现订单生成失败,获取订单用户【{1}】信息失败", orderModel.OrderID, orderModel.UserID));
                        }
                    }
                }
                else
                {
                    _logger.Error(typeof(BounsJob), "【会员购物订单返现订单生成服务】本次未处理任何数据.");
                }
                exceptionLogModel.HandleId = "0";
                exceptionLogModel.Status = 2;
                exceptionLogModel.ResultType = 2;
                exceptionLogModel.Message = "运行正常";
                exceptionLogService.Add(exceptionLogModel);
                _logger.Error(typeof(BounsJob), "会员购物订单返现订单生成服务结束**********************************");
            }
            catch (Exception ex)
            {
                exceptionLogModel.HandleId = "0";
                exceptionLogModel.Status = 1;
                exceptionLogModel.ResultType = 1;
                exceptionLogModel.Message = "【会员购物订单返现订单生成服务运行环节报错】" + ex.Message;
                exceptionLogService.Add(exceptionLogModel);
                _logger.Error(typeof(BounsJob), "【会员购物订单返现订单生成服务运行环节报错】" + ex.Message);
            }
        }

      
    }
}
