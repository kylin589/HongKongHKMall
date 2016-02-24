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

namespace HKTMall.Monitor.Services
{
    public class PurchaseOrderJob : IJob
    {
        private readonly static ILogger _logger = BrEngineContext.Current.Resolve<ILogger>();
        private readonly static IDatabaseHelper _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
        private readonly static IExceptionLogService exceptionLogService = BrEngineContext.Current.Resolve<IExceptionLogService>();
        private readonly static int purchaseOrderJobSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["PurchaseOrderJobSeconds"]);
        public void Execute(IJobExecutionContext context)
        {
            PurchaseOrder purchaseOrder = null;
            PurchaseOrderDetails purchaseOrderDetails = null;
            ExceptionLogModel exceptionLogModel = new ExceptionLogModel();
            exceptionLogModel.ServiceName = "订单拆分采购单服务";
            exceptionLogModel.CreateBy = "系统服务";
            decimal costPrice = 0;

            try
            {
                _logger.Error(typeof(PurchaseOrderJob), "订单拆分采购单服务开始***********************************");
                //待拆分订单
                string sql = string.Format(@"select t2.OrderID from OrderDetails t2 where t2.OrderID in(select t1.OrderID from [Order] t1 where t1.OrderStatus=3 and t1.IsPurchase=0)
 and t2.SupplierId>0 group by t2.OrderID order by t2.OrderID");
                List<dynamic> sources = _database.RunSqlQuery(x => x.ToResultSets(sql))[0];
                List<OrderModel> list = sources.ToEntity<OrderModel>();
                //待拆分供应商订单信息
                string sqlOrder = string.Format(@"select t2.OrderID,t2.SupplierId from OrderDetails t2 where t2.OrderID in(select t1.OrderID from [Order] t1 where t1.OrderStatus=3 and t1.IsPurchase=0)
 and t2.SupplierId>0 group by t2.OrderID,t2.SupplierId order by t2.OrderID");
                List<dynamic> sourcesOrder = _database.RunSqlQuery(x => x.ToResultSets(sqlOrder))[0];
                List<OrderModel> listOrder = sourcesOrder.ToEntity<OrderModel>();
                //待拆分订单详情信息
                string sqlOrderDetails = string.Format(@"select t2.OrderID,t2.OrderDetailsID,t2.ProductId,t2.ProductName,t2.CostPrice,t2.SalesPrice,t2.DiscountInfo,t2.Quantity,t2.Unit,t2.SKU_ProducId,t2.SkuName,t2.SupplierId from OrderDetails t2 where t2.SupplierId>0 and t2.OrderID in(select t1.OrderID from [Order] t1 where t1.OrderStatus=3 and t1.IsPurchase=0)");
                List<dynamic> sourcesOrderDetails = _database.RunSqlQuery(x => x.ToResultSets(sqlOrderDetails))[0];
                List<OrderDetailsModel> listOrderDetails = sourcesOrderDetails.ToEntity<OrderDetailsModel>();

                if (listOrder != null && listOrder.Count > 0)
                {
                    foreach (OrderModel order in list)
                    {
                        using (var bt = _database.Db.BeginTransaction())
                        {
                            try
                            {
                                var tempListOrder = from query in listOrder where query.OrderID == order.OrderID select query;
                                if (tempListOrder != null)
                                {
                                    foreach (OrderModel orderModel in tempListOrder)
                                    {
                                        costPrice = 0;
                                        //供应商采购单主体信息
                                        purchaseOrder = new PurchaseOrder();
                                        purchaseOrder.PurchaseOrderId = MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString();
                                        purchaseOrder.OrderID = orderModel.OrderID;
                                        purchaseOrder.SupplierId = orderModel.SupplierId;
                                        purchaseOrder.CreateBy = "系统服务";
                                        purchaseOrder.CreateTime = DateTime.Now;
                                        purchaseOrder.status = 1;
                                        purchaseOrder.PurchaseAmount = costPrice;
                                        purchaseOrder.RealPurchaseAmount = costPrice;
                                        _database.Db.PurchaseOrder.Insert(purchaseOrder);
                                        //供应商采购单详情
                                        var tempList = from detailsQuery in listOrderDetails where detailsQuery.OrderID == orderModel.OrderID && detailsQuery.SupplierId == orderModel.SupplierId select detailsQuery;
                                        if (tempList != null)
                                        {
                                            foreach (OrderDetailsModel om in tempList)
                                            {
                                                purchaseOrderDetails = new PurchaseOrderDetails();
                                                purchaseOrderDetails.OrderDetailsID = om.OrderDetailsID;
                                                purchaseOrderDetails.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                                                purchaseOrderDetails.ProductId = om.ProductId;
                                                purchaseOrderDetails.ProductName = om.ProductName;
                                                purchaseOrderDetails.CostPrice = om.CostPrice;
                                                purchaseOrderDetails.SalesPrice = om.SalesPrice;
                                                purchaseOrderDetails.DiscountInfo = om.DiscountInfo;
                                                purchaseOrderDetails.Quantity = om.Quantity;
                                                purchaseOrderDetails.returnedQty = 0;
                                                purchaseOrderDetails.RealQty = om.Quantity;
                                                purchaseOrderDetails.Unit = om.Unit;
                                                purchaseOrderDetails.SKU_ProducId = om.SKU_ProducId;
                                                purchaseOrderDetails.SkuName = om.SkuName;
                                                costPrice += om.CostPrice * om.Quantity;
                                                _database.Db.PurchaseOrderDetails.Insert(purchaseOrderDetails);
                                            }
                                        }
                                        _database.Db.PurchaseOrder.UpdateByPurchaseOrderId(PurchaseOrderId: purchaseOrder.PurchaseOrderId, PurchaseAmount: costPrice, RealPurchaseAmount: costPrice);
                                    }
                                }
                                //订单待拆分状态更改 
                                _database.Db.Order.UpdateByOrderID(OrderID: order.OrderID, IsPurchase: 1);
                                bt.Commit();
                            }
                            catch (Exception ex)
                            {
                                bt.Rollback();
                                exceptionLogModel.HandleId = order.OrderID;
                                exceptionLogModel.Status = 1;
                                exceptionLogModel.ResultType = 1;
                                exceptionLogModel.Message = "【订单拆分采购单服务foreach(订单ID：" + order.OrderID + ")】" + ex.Message;
                                exceptionLogService.Add(exceptionLogModel);
                                _logger.Error(typeof(PurchaseOrderJob), "【订单拆分采购单服务foreach】" + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    _logger.Error(typeof(ProductRuleJob), "【订单拆分采购单服务】本次未处理任何数据.");
                }
                exceptionLogModel.HandleId = "0";
                exceptionLogModel.Status = 2;
                exceptionLogModel.ResultType = 2;
                exceptionLogModel.Message = "运行正常";
                exceptionLogService.Add(exceptionLogModel);
                _logger.Error(typeof(PurchaseOrderJob), "订单拆分采购单服务服务结束**********************************");
            }
            catch (Exception ex)
            {
                exceptionLogModel.HandleId = "0";
                exceptionLogModel.Status = 1;
                exceptionLogModel.ResultType = 1;
                exceptionLogModel.Message = "【订单拆分采购单服务运行环节报错】" + ex.Message;
                exceptionLogService.Add(exceptionLogModel);
                _logger.Error(typeof(PurchaseOrderJob), "【订单拆分采购单服务运行环节报错】" + ex.Message);
            }
        }


        public static void Create()
        {
            IJobDetail purchaseOrderJob = JobBuilder.Create<PurchaseOrderJob>()
                                                            .WithIdentity("订单拆分采购单任务", "PurchaseOrderJobGroup")
                                                            .Build();
            ITrigger orderTrigger = TriggerBuilder.Create()
                .WithIdentity(Guid.NewGuid().ToString(), "PurchaseOrderJobName")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(purchaseOrderJobSeconds)
                    .RepeatForever())
                .Build();
            HKTMallMonitorService.S_scheduler.ScheduleJob(purchaseOrderJob, orderTrigger);
            _logger.Error(typeof(PurchaseOrderJob), "订单拆分采购单服务启动");
        }
    }
}
