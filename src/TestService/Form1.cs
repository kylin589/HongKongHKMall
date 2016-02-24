using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrCms.Framework.Infrastructure;
using Autofac;
using HKTMall.Monitor;
using HKTHMall.Domain.AdminModel.Models.Orders;
using Simple.Data.RawSql;
using HKTHMall.Core.Extensions;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Services.Orders;
using HKTHMall.Domain.Models;
using HKTMall.Monitor.Services;
using HKTHMall.Core;
using BrCms.Framework.Logging;
using BrCms.Framework.Data;
using HKTHMall.Domain.Entities;
using System.Configuration;
using HKTHMall.Services.Sys;
using HKTHMall.Services.AC;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Services.Users;
using HKTHMall.Domain.CashBack;
using HKTHMall.Services;

namespace TestService
{
    public partial class Form1 : Form
    {
        private readonly static IOrderService orderService = BrEngineContext.Current.Resolve<IOrderService>();
        private readonly static IExceptionLogService exceptionLogService = BrEngineContext.Current.Resolve<IExceptionLogService>();
        private readonly static ILogger _logger = BrEngineContext.Current.Resolve<ILogger>();
        private readonly static IDatabaseHelper _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
        private readonly static IZJ_RebateService _rebateService = BrEngineContext.Current.Resolve<IZJ_RebateService>();
        private readonly static int bounsJobSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["BounsJobSeconds"]);
        private readonly static int earningsMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["earningsMinutes"]);   

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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
                        try
                        {
                            #region 订单表状态为已收货OrderStatus=5且退货标示0正常,2已处理（包括成功，失败）RefundFlag in{0,2}

                            if (orderModel.OrderStatus == (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.OutTimeReceiving)
                            {
                                var orderLog = listOrderTrackingLog.Find(orderTrackingLog => orderTrackingLog.OrderID == orderModel.OrderID && orderTrackingLog.OrderStatus == orderModel.OrderStatus);
                                if (orderLog != null)
                                {
                                    //DateTime endTime = orderLog.CreateTime.AddDays(earningsDays);
                                    DateTime endTime = orderLog.CreateTime.AddMinutes(earningsMinutes);
                                    if (endTime < DateTime.Now && orderModel.IsReward == 0)
                                    {
                                        if (orderModel.RefundFlag != 1)
                                        {
                                            BackMessage resultModel = _rebateService.GenerateList(orderModel);
                                            //if (resultModel.status!=1)
                                            //{
                                            //    //更改订单状态为已完成
                                            //   _database.Db.Order.UpdateByOrderID(OrderID: orderModel.OrderID, IsReward: 1, OrderStatus: (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.Completed);
                                            //   ///加入订单状态记录
                                            //    OrderTrackingLogModel orderTrackingLogTwo = new OrderTrackingLogModel();
                                            //    orderTrackingLogTwo.OrderID = orderModel.OrderID;
                                            //    orderTrackingLogTwo.OrderStatus = (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.Completed;
                                            //    orderTrackingLogTwo.TrackingContent = "已完成";
                                            //    orderTrackingLogTwo.CreateTime = DateTime.Now;
                                            //    orderTrackingLogTwo.CreateBy = "惠卡商城订单监控服务";
                                            //    _database.Db.OrderTrackingLog.Insert(orderTrackingLogTwo);

                                            _logger.Error(typeof(BounsJob), string.Format("处理订单【{0}】的会员购物订单返现订单生成成功", orderModel.OrderID));
                                            //}
                                            //else
                                            //{                                                   
                                            //    exceptionLogModel.HandleId = orderModel.OrderID;
                                            //    exceptionLogModel.Status = 1;
                                            //    exceptionLogModel.ResultType = 1;
                                            //    exceptionLogModel.Message = string.Format("处理订单【{0}】的会员购物订单返现订单生成失败,", orderModel.OrderID);
                                            //    exceptionLogService.Add(exceptionLogModel);
                                            //    _logger.Error(typeof(BounsJob), string.Format("处理订单【{0}】的会员购物订单返现订单生成失败,", orderModel.OrderID));
                                            //}
                                        }
                                    }
                                }
                                else
                                {
                                    //更新标志,表示该订单已经做了收益分成处理
                                    //_database.Db.Order.UpdateByOrderID(OrderID: orderModel.OrderID, IsReward: 1, OrderStatus: (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.Completed);
                                    exceptionLogModel.HandleId = orderModel.OrderID;
                                    exceptionLogModel.Status = 1;
                                    exceptionLogModel.ResultType = 1;
                                    exceptionLogModel.Message = "订单返现订单生成,订单【" + orderModel.OrderID + "】跟踪信息查询无结果,已转到无效订单";
                                    exceptionLogService.Add(exceptionLogModel);
                                    _logger.Error(typeof(OrderJob), "订单返现订单生成,订单【" + orderModel.OrderID + "】跟踪信息查询无结果,已转到无效订单");
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
