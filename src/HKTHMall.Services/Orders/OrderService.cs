using System.Configuration;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using BrCms.Framework.Collections;
using HKTHMall.Core;
using HKTHMall.Core.Sql;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Domain.WebModel.Models.Shipment;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;
using HKTHMall.Domain.WebModel.Models.SKU;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Services.Orders.Impl;
using HKTHMall.Services.Products;
using HKTHMall.Services.ShoppingCart.Impl;
using HKTHMall.Services.Users;
using HKTHMall.Services.YHUser;
using Simple.Data;
using System;
using HKTHMall.Core.Extensions;
using Simple.Data.RawSql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HKTHMall.Services.SKU.Impl;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.AdminModel.Models;
using System.Collections;
using HKTHMall.Services.Common;
using HKTHMall.Domain.WebModel.Models.YH;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Services.Sys;
using HKTHMall.Services.Shipment;
using HKTHMall.Domain.AdminModel.Models.Shipment;



namespace HKTHMall.Services.Orders
{
    /// <summary>
    /// 订单服务类
    /// </summary>
    public class OrderService : BaseService, IOrderService
    {
        /// <summary>
        /// 系统参数设置对象业务处理类
        /// </summary>
        private ParameterSetService parameterSetService;
        /// <summary>
        /// 商品服务实体
        /// </summary>
        private ProductService productService;

        /// <summary>
        /// 商品库存实体
        /// </summary>
        private SKU_ProductService skuProductService;

        /// <summary>
        /// 订单跟踪日志服务实体
        /// </summary>
        private OrderTrackingLogService orderTrackingLogService;

        /// <summary>
        /// 购物车服务类
        /// </summary>
        private ShoppingCartService shoppingCartService;

        /// <summary>
        /// 支付单服务类
        /// </summary>
        private PaymentOrderService paymentOrderService;

        /// <summary>
        /// 用户服务
        /// </summary>
        private YH_UserService userService;

        private PurchaseOrderSerivce purchaseOrderSerivce;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="skuProductService"></param>
        /// <param name="orderTrackingLogService"></param>
        /// <param name="shoppingCartService"></param>
        /// <param name="orderDetailsService"></param>
        /// <param name="paymentOrderOrdersService"></param>
        /// <param name="orderAddressService"></param>
        /// <param name="paymentOrderService"></param>
        /// <param name="userService"></param>
        /// <param name="purchaseOrderSerivce"></param>
        public OrderService(ProductService productService, SKU_ProductService skuProductService, OrderTrackingLogService orderTrackingLogService, ShoppingCartService shoppingCartService
            , PaymentOrderService paymentOrderService, YH_UserService userService, PurchaseOrderSerivce purchaseOrderSerivce, ParameterSetService parameterSetService)
        {
            this.productService = productService;
            this.skuProductService = skuProductService;
            this.orderTrackingLogService = orderTrackingLogService;
            this.shoppingCartService = shoppingCartService;
            this.paymentOrderService = paymentOrderService;
            this.userService = userService;
            this.purchaseOrderSerivce = purchaseOrderSerivce;
            this.parameterSetService = parameterSetService;
        }

        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="model">用户搜索模型</param>
        /// <returns>用户列表数据</returns>
        public ResultModel GetPagingOrder(SearchOrderModel model)
        {
            var tb = _database.Db.Order;
            var whereExpr = tb.OrderID.Like("%" + (model.OrderID != null ? model.OrderID.ToString().Trim() : model.OrderID) + "%");

            if (model.UserID > 0)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.UserID == model.UserID, SimpleExpressionType.And);
            }
            if (model.Phone != null)
            {
                whereExpr = new SimpleExpression(whereExpr, _database.Db.YH_User.Phone == model.Phone.Trim(), SimpleExpressionType.And);
            }
            if (model.Email != null)
            {
                whereExpr = new SimpleExpression(whereExpr, _database.Db.YH_User.Email.Like("%" + model.Email.Trim() + "%"), SimpleExpressionType.And);
            }

            if (model.ShopName != null)
            {
                whereExpr = new SimpleExpression(whereExpr,
                    _database.Db.YH_MerchantInfo.ShopName.Like("%" + model.ShopName.Trim() + "%"), SimpleExpressionType.And);
            }
            if (model.NickName != null)
            {
                whereExpr = new SimpleExpression(whereExpr,
                    _database.Db.YH_User.NickName.Like("%" + model.NickName.Trim() + "%"), SimpleExpressionType.And);
            }
            if (model.OrderStatus != 0)
            {
                whereExpr = new SimpleExpression(whereExpr, _database.Db.Order.OrderStatus == model.OrderStatus,
                    SimpleExpressionType.And);
            }

            if (model.StartPaidDate != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.OrderDate >= model.StartPaidDate,
                    SimpleExpressionType.And);
            }
            if (model.EndPaidDate != null)
            {
                whereExpr = new SimpleExpression(whereExpr,
                    tb.OrderDate < Convert.ToDateTime(model.EndPaidDate).AddDays(1), SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<OrderModel>(
                        _database.Db.Order.All()
                            .LeftJoin(_database.Db.YH_User)
                            .On(_database.Db.YH_User.UserID == _database.Db.Order.UserID)
                            .LeftJoin(_database.Db.YH_MerchantInfo)
                            .On(_database.Db.YH_MerchantInfo.MerchantID == _database.Db.Order.MerchantID)
                            .Select(
                                _database.Db.Order.OrderID, _database.Db.Order.TotalAmount,
                                _database.Db.Order.OrderStatus, _database.Db.Order.PaidDate,
                                _database.Db.Order.OrderDate, _database.Db.YH_MerchantInfo.ShopName,
                                _database.Db.YH_MerchantInfo.Tel, _database.Db.YH_User.NickName,
                                _database.Db.Order.PayChannel, _database.Db.Order.ExpressMoney,
                                 _database.Db.YH_User.Email,
                                _database.Db.YH_User.Phone)
                            .Where(whereExpr).OrderByDescending(_database.Db.Order.OrderDate), model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 更新订单状态
        /// zhoub 20150713
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ResultModel UpdateOrderStatus(string orderId, int status)
        {
            var result = new ResultModel();
            result.Data = _database.Db.Order.UpdateByOrderID(OrderID: orderId, OrderStatus: status);
            return result;
        }

        /// <summary>
        /// 更新订单状态
        /// 黄主霞 2016-01-19
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="RefundFlag">0正常，1退款中，2已处理（包括成功，失败）</param>
        /// <returns></returns>
        public ResultModel UpdateRefundFlag(string orderId, int RefundFlag)
        {
            var result = new ResultModel();
            result.Data = _database.Db.Order.UpdateByOrderID(OrderID: orderId, RefundFlag: RefundFlag);
            return result;
        }

        /// <summary>
        /// 更新订单快递单号
        /// zhoub 20150909
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="expressOrder"></param>
        /// <returns></returns>
        public ResultModel UpdateExpressOrder(string orderId, string expressOrder)
        {
            var result = new ResultModel();
            result.Data = _database.Db.Order.UpdateByOrderID(OrderID: orderId, ExpressOrder: expressOrder);
            return result;
        }

        /// <summary>
        /// 订单分页详情
        /// zhoub 20150713
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel GetPagingOrderDetails(long orderId, int languageID)
        {
            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<OrderDetailsModel>(
                        _database.Db.OrderDetails.All()
                            .LeftJoin(_database.Db.ProductPic).On(_database.Db.ProductPic.ProductID == _database.Db.OrderDetails.ProductID && _database.Db.ProductPic.Flag == 1)
                            .LeftJoin(_database.Db.OrderDetails_lang).On(_database.Db.OrderDetails_lang.OrderDetailsID == _database.Db.OrderDetails.OrderDetailsID && _database.Db.OrderDetails_lang.LanguageID == languageID)
                            .LeftJoin(_database.Db.ZJ_RebateInfo).On(_database.Db.ZJ_RebateInfo.OrderDetailsID == _database.Db.OrderDetails.OrderDetailsID)
                            .Select(_database.Db.OrderDetails.ProductId, _database.Db.OrderDetails_lang.ProductName,
                                _database.Db.OrderDetails.SkuName, _database.Db.OrderDetails.SalesPrice,
                                _database.Db.OrderDetails.Quantity, _database.Db.OrderDetails.DiscountInfo,
                                _database.Db.OrderDetails.SubTotal, _database.Db.ProductPic.PicUrl,
                                _database.Db.OrderDetails.RetateDays, _database.Db.OrderDetails.ReateRedio,
                                _database.Db.ZJ_RebateInfo.StartTime
                                )
                            .Where(_database.Db.OrderDetails.OrderID == orderId), 0, 100)
            };
            return result;
        }

        /// <summary>
        /// 订单详情
        /// zhoub 20150714
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ResultModel GetOrderDetails(long orderId)
        {
            var result = new ResultModel
            {
                Data =
                    _database.Db.Order.All()
                        .LeftJoin(_database.Db.OrderAddress)
                        .On(_database.Db.OrderAddress.OrderID == _database.Db.Order.OrderID)
                        .Select(_database.Db.Order.OrderID, _database.Db.Order.OrderDate, _database.Db.Order.OrderStatus, _database.Db.Order.ExpressMoney, _database.Db.OrderAddress.THAreaID,
                            _database.Db.OrderAddress.DetailsAddress, _database.Db.OrderAddress.Receiver, _database.Db.OrderAddress.Email, _database.Db.OrderAddress.Phone,
                            _database.Db.OrderAddress.Mobile, _database.Db.Order.PayType, _database.Db.Order.PayChannel, _database.Db.Order.MerchantID, _database.Db.Order.TotalAmount)
                        .Where(_database.Db.Order.OrderID == orderId)
                        .ToList<OrderModel>()
            };

            return result;
        }
        /// <summary>
        /// 获取订单明细
        /// 刘文宁20160116
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ResultModel GetOrderDetailsByPaymentOrderId(string PaymentOrderId)
        {
            var result = new ResultModel
            {
                Data =
                    _database.Db.Order.All()
                        .LeftJoin(_database.Db.OrderDetails)
                        .On(_database.Db.OrderDetails.OrderID == _database.Db.Order.OrderID)
                        .LeftJoin(_database.Db.PaymentOrder_Orders)
                        .On(_database.Db.PaymentOrder_Orders.OrderID == _database.Db.Order.OrderID)
                        .Select(
                        _database.Db.Order.OrderID,
                        _database.Db.Order.TotalAmount,
                        _database.Db.OrderDetails.OrderDetailsID,
                        _database.Db.OrderDetails.ProductName,
                        _database.Db.OrderDetails.ProductId,
                        _database.Db.OrderDetails.SalesPrice,
                        _database.Db.OrderDetails.RetateDays,
                        _database.Db.OrderDetails.ReateRedio,
                        _database.Db.orderDetails.SubTotal
                        )
                        .Where(_database.Db.PaymentOrder_Orders.PaymentOrderId == PaymentOrderId)
                        .ToList<OrderDetailsForPayResultView>()

            };
            return result;
        }

        /// <summary>
        /// 根据支付单Id获取为付款的订单id集合
        /// </summary>
        /// <author>樊利民</author>
        /// <param name="paymentOrderId">支付单Id</param>
        /// <returns>Data:List`string`</returns>
        public ResultModel GetOrderIdByPaymentOrderId(string paymentOrderId)
        {
            string querySql = @"SELECT  a.OrderID
                                FROM    dbo.PaymentOrder_Orders AS a
                                        INNER JOIN dbo.[Order] AS b ON a.OrderID = b.OrderID
                                WHERE   a.PaymentOrderID = @PaymentOrderID
                                        AND b.OrderStatus = @OrderStatus";

            List<string> orderIds = _dataDapper.Query<string>(querySql,
                 new { PaymentOrderID = paymentOrderId, OrderStatus = (int)OrderEnums.OrderStatus.Obligation }).ToList();
            return new ResultModel()
            {
                Data = orderIds
            };
        }

        /// <summary>
        /// 根据订单ID获取订单信息
        /// zhoub 20150716
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ResultModel GetOrderByOrderID(string orderId)
        {
            var result = new ResultModel
            {

                Data =
                    _database.Db.Order.All()
                        .LeftJoin(_database.Db.Complaints)
                        .On(_database.Db.Complaints.OrderID == _database.Db.Order.OrderID)
                        .Select(_database.Db.Order.UserID, _database.Db.Order.OrderID, _database.Db.Order.ExpressMoney, _database.Db.Order.TotalAmount, _database.Db.Order.PaidDate, _database.Db.Order.ComplaintStatus, _database.Db.Order.MerchantID,
                           _database.Db.Complaints.Content)
                        .Where(_database.Db.Order.OrderID == orderId)
                        .ToList<OrderModel>()[0]
            };
            //var result = new ResultModel { Data = _database.Db.Order.FindByOrderID(orderId) };
            return result;
        }

        /// <summary>
        /// 订单投诉状态更改
        /// zhoub 20150716
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ResultModel UpdateOrderComplaintStatus(string orderId, int status)
        {
            var result = new ResultModel();
            result.Data = _database.Db.Order.UpdateByOrderID(OrderID: orderId, ComplaintStatus: status);
            return result;
        }
        /// <summary>
        /// 更改邮费
        /// </summary>
        /// <param name="model"></param>
        /// <param name="oldExpressFee"></param>
        /// <returns></returns>
        public ResultModel UpdateOrderExpressMoney(OrderModel model, decimal oldExpressFee)
        {
            var result = new ResultModel() { IsValid = false };
            //开启事务
            var trans = _database.Db.BeginTransaction();
            try
            {
                //更新订单邮费
                result.Data = trans.Order.UpdateByOrderID(OrderID: model.OrderID, ExpressMoney: model.ExpressMoney, TotalAmount: model.TotalAmount);
                //创建邮费更改记录
                var orderExpressFeeLog = new OrderExpressFeeLogModel()
                {
                    OrderID = model.OrderID,
                    OrderStatus = model.OrderStatus,
                    OldExpressFee = oldExpressFee,
                    NewExpressFee = model.ExpressMoney,
                    ExpressFeeContent = "更改邮费",
                    CreateBy = "客服",
                    CreateTime = DateTime.Now
                };
                trans.OrderExpressFeeLog.Insert(orderExpressFeeLog);
                //创建订单更改记录
                var orderTrackingLog = new OrderTrackingLogView()
                {
                    OrderID = model.OrderID,
                    OrderStatus = model.OrderStatus,
                    TrackingContent = "更改邮费",
                    CreateBy = "客服",
                    CreateTime = DateTime.Now
                };
                trans.OrderTrackingLog.Insert(orderTrackingLog);

                trans.Commit();
                result.IsValid = true;
                result.Messages.Add("Confirm receipt, operation success");//更改快递费,操作成功
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            result.IsValid = true;
            return result;
        }
        /// <summary>
        /// 根据用户ID 订单状态 获取待付款/待收货订单数目
        /// </summary>
        /// <returns></returns>
        public ResultModel GetOrderByUserIDStatus(long userId, int status)
        {
            var result = new ResultModel()
            {
                Data =
                    _database.Db.Order.FindAll(_database.Db.Order.OrderStatus == status &&
                                               _database.Db.Order.UserID == userId).ToList<OrderModel>()
            };

            return result;
        }

        /// <summary>
        /// 根据商家ID、状态，获取商家信息  wuyf  2015-9-9
        /// </summary>
        /// <returns></returns>
        public ResultModel GetYH_MerchantInfoByMerchantID(long MerchantID, int AuditStatus)
        {
            //_database.Db.YH_MerchantInfo.AuditStatus == AuditStatus
            var result = new ResultModel()
            {
                Data =
                    _database.Db.YH_MerchantInfo.FindAll(_database.Db.YH_MerchantInfo.MerchantID == MerchantID).ToList<YH_MerchantInfoView>()
            };

            return result;
        }

        /// <summary>
        /// 根据用户ID获取待评价订单数目
        /// </summary>
        /// <returns></returns>
        public ResultModel GetOrderUnComment(long userId)
        {
            //var tb = _database.Db.Order;
            ResultModel result = new ResultModel();
            //dynamic pc;
            //var query = tb
            //    .Query()
            //    .LeftJoin(_database.Db.OrderDetails, out pc)
            //    .On(pc.OrderID == tb.OrderID)
            //    .Select(
            //        pc.OrderID, pc.Iscomment
            //    )
            //    .Where(_database.Db.OrderDetails.Iscomment == 0).Where(_database.Db.OrderDetails.IsReturn == 0)
            //    .Where(_database.Db.Order.OrderStatus == 5).Where(tb.UserID == userId);
            //result.Data = query.ToList<OrderDetailsModel>();
            var sql = @"select distinct o.OrderID from [Order] o 
                        left join 
                        OrderDetails od on od.OrderID=o.OrderID
                        where o.OrderStatus in(5,6) and od.IsReturn in(0,3) and od.Iscomment=0 and o.UserID=" + userId;
            List<dynamic> sources = _database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            result.Data = sources.ToEntity<OrderDetailsModel>();
            result.IsValid = true;
            return result;
        }

        /// <summary>
        /// 根据订单详情ID查询数据
        /// zhoub 20150720
        /// </summary>
        /// <param name="orderDetailsID"></param>
        /// <returns></returns>
        public ResultModel GetOrderDetailsById(long orderDetailsID, long userId, int languageID)
        {
            var det = _database.Db.OrderDetails;
            var ord = _database.Db.Order;
            var mer = _database.Db.YH_MerchantInfo;
            var pic = _database.Db.ProductPic;
            var ordLang = _database.Db.OrderDetails_lang;

            var result = new ResultModel
            {
                Data = det.All().LeftJoin(ord).On(det.OrderID == ord.OrderID)
                    .LeftJoin(mer).On(mer.MerchantID == ord.MerchantID)
                    .LeftJoin(pic).On(pic.ProductID == det.ProductId && pic.Flag == 1)
                    .LeftJoin(ordLang).On(ordLang.OrderDetailsID == det.OrderDetailsID && ordLang.LanguageID == languageID)
                    .Select(ordLang.ProductName, det.SkuName, det.SalesPrice, det.Quantity, det.OrderID,
                        det.SubTotal, ord.ExpressMoney, ord.TotalAmount, ord.OrderDate, mer.ShopName, pic.PicUrl,
                        det.ProductId, det.OrderDetailsID)
                    .Where(det.OrderDetailsID == orderDetailsID && ord.UserID == userId)
                    .ToList<OrderDetailsModel>()
            };
            return result;
        }

        /// <summary>
        /// 获取订单详情数据
        /// </summary>
        /// <param name="model">订单搜索模型</param>
        /// <returns>订单详情数据</returns>
        public ResultModel GetOrderDetailIntoWebBy(HKTHMall.Domain.WebModel.Models.Orders.SearchOrderDetailView model)
        {

            var whereExpr = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (!string.IsNullOrEmpty(model.OrderID))
            {
                whereExpr = new SimpleExpression(whereExpr,
                    new SimpleExpression(_database.Db.Order.OrderID, model.OrderID, SimpleExpressionType.Equal),
                    SimpleExpressionType.And);
            }
            if (model.UserID.HasValue)
            {
                whereExpr = new SimpleExpression(whereExpr,
                    new SimpleExpression(_database.Db.Order.UserID, model.UserID, SimpleExpressionType.Equal),
                    SimpleExpressionType.And);
            }
            if (model.MerchantID.HasValue)
            {
                whereExpr = new SimpleExpression(whereExpr,
                    new SimpleExpression(_database.Db.Order.MerchantID, model.MerchantID, SimpleExpressionType.Equal),
                    SimpleExpressionType.And);
            }
            if (model.OrderStatus.HasValue)
            {
                whereExpr = new SimpleExpression(whereExpr,
                    new SimpleExpression(_database.Db.Order.OrderStatus, model.OrderStatus, SimpleExpressionType.Equal),
                    SimpleExpressionType.And);
            }


            var result = new ResultModel() { IsValid = false };
            var queryResult = _database.Db.Order.Query()
                .Join(_database.Db.OrderAddress.As("OrderAddressView"), OrderID: _database.Db.Order.OrderID)
                .Join(_database.Db.YH_MerchantInfo.As("YH_MerchantInfoView"), MerchantID: _database.Db.Order.MerchantID);
            queryResult = queryResult.WithOne(queryResult.OrderAddressView)
                .WithOne(queryResult.YH_MerchantInfoView)
                .Where(whereExpr)
                .FirstOrDefault();

            HKTHMall.Domain.WebModel.Models.Orders.OrderView orderModel = queryResult;
            if (orderModel != null)
            {
                string sql =
                    string.Format(@"SELECT   a.SKU_ProducId,a.Iscomment,a.OrderID,a.ProductId,a.Quantity,d.ProductName,a.SkuName,a.SalesPrice,c.PicUrl,a.IsReturn,a.OrderDetailsID FROM OrderDetails AS a
                            INNER JOIN dbo.Product AS b
                            ON a.ProductId =b.ProductId
                            LEFT JOIN(SELECT * FROM dbo.ProductPic WHERE Flag=1) AS c
                            ON b.ProductId=c.ProductID
                            INNER JOIN(SELECT * FROM OrderDetails_lang WHERE LanguageID={0}) AS d
                             ON d.OrderDetailsID=a.OrderDetailsID
                            WHERE a.OrderID='{1}'", model.LanguageID.Value, orderModel.OrderID);

                if (model.Iscomment.HasValue)
                {
                    sql += string.Format(" AND a.Iscomment={0}", model.Iscomment.Value);
                }





                List<dynamic> sources = _database.RunSqlQuery(x => x.ToResultSets(sql))[0];
                orderModel.OrderDetailViews = sources.ToEntity<OrderDetailsView>();
                result.IsValid = true;
            }
            result.Data = orderModel;
            return result;
        }

        /// <summary>
        /// 分页获取订单列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel GetPagingOrdersIntoWeb(HKTHMall.Domain.WebModel.Models.Orders.SearchOrderView model)
        {

            SimpleExpression whereExpr = null;
            switch (model.d)
            {
                case OrderEnums.TimeSpanType.All:
                    whereExpr = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
                    break;
                case OrderEnums.TimeSpanType.HalfOfMonth:
                    whereExpr = new SimpleExpression(_database.Db.Order.OrderDate, DateTime.Now.Date.AddDays(-15),
                        SimpleExpressionType.GreaterThanOrEqual);
                    break;
                case OrderEnums.TimeSpanType.ThreeMonths:
                    whereExpr = new SimpleExpression(_database.Db.Order.OrderDate, DateTime.Now.Date.AddMonths(-3),
                        SimpleExpressionType.GreaterThanOrEqual);
                    break;
                case OrderEnums.TimeSpanType.Earlier:
                    whereExpr = new SimpleExpression(_database.Db.Order.OrderDate, DateTime.Now.Date.AddMonths(-3),
                        SimpleExpressionType.LessThanOrEqual);
                    break;
            }

            if (model.s != OrderEnums.OrderStatus.All)
            {
                whereExpr = new SimpleExpression(whereExpr,
                    new SimpleExpression(_database.Db.Order.OrderStatus, (int)model.s, SimpleExpressionType.Equal),
                    SimpleExpressionType.And);
            }


            whereExpr = new SimpleExpression(whereExpr,
                new SimpleExpression(_database.Db.Order.UserID, model.UserID, SimpleExpressionType.Equal),
                SimpleExpressionType.And);

            var result = new ResultModel() { IsValid = false };
            //if (model.Iscomment==100)
            //{
            //whereExpr = new SimpleExpression(whereExpr,
            //new SimpleExpression(_database.Db.OrderDetails.Iscomment, 0, SimpleExpressionType.Equal),
            //SimpleExpressionType.And);
            //whereExpr = new SimpleExpression(whereExpr,
            //new SimpleExpression(_database.Db.OrderDetails.IsReturn, 0, SimpleExpressionType.Equal),
            //SimpleExpressionType.And);


            //dynamic yhm,od;

            //var queryResult = _database.Db.Order.Query()
            //.LeftJoin(_database.Db.YH_MerchantInfo, out yhm).On(yhm.MerchantID == _database.Db.Order.MerchantID)
            //.LeftJoin(_database.Db.OrderDetails, out od).On(od.OrderID == _database.Db.Order.OrderID)
            //.Select(
            //    _database.Db.Order.OrderID,
            //    _database.Db.Order.OrderDate,
            //     _database.Db.Order.OrderStatus
            //    )
            //.Where(whereExpr).OrderByOrderDateDescending();
            //result.Data = new SimpleDataPagedList<OrderView>(queryResult, model.page - 1, model.pageSize);
            //var sql =string.Format(@"select  distinct top {0} o.OrderID,o.OrderDate,o.OrderStatus from [Order] o 
            //left join 
            //OrderDetails od on od.OrderID=o.OrderID
            //where o.OrderStatus in(5,6) and od.IsReturn=0 and od.Iscomment=0
            //and o.OrderID not in (select  distinct top {1} o.OrderID from [Order] o 
            //left join 
            //OrderDetails od on od.OrderID=o.OrderID
            //where o.OrderStatus in(5,6) and od.IsReturn=0 and od.Iscomment=0) and o.UserID={2}", model.pageSize, (model.page - 1) * model.pageSize, model.UserID);
            //List<dynamic> sources = _database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            //result.Data = sources.ToEntity<OrderDetailsModel>();
            //result.IsValid = true;
            //}
            //else
            //{
            var queryResult = _database.Db.Order.Query()
            .Join(_database.Db.YH_MerchantInfo.As("YH_MerchantInfoView"), MerchantID: _database.Db.Order.MerchantID);
            queryResult = queryResult
                .WithOne(queryResult.YH_MerchantInfoView)
                .Where(whereExpr).OrderByOrderDateDescending();
            result.Data = new SimpleDataPagedList<OrderView>(queryResult, model.page - 1, model.pageSize);
            //}


            result.IsValid = true;


            if (result.Data != null && result.Data.TotalCount > 0)
            {
                List<OrderView> orderViews = result.Data;
                List<string> orderIds = orderViews.Select(x => "'" + x.OrderID + "'").ToList();

                if (orderIds != null && orderIds.Count > 0)
                {
                    string orderIdString = string.Join(",", orderIds);
                    string sql =
                        string.Format(@"SELECT a.SKU_ProducId,a.Iscomment,a.OrderDetailsID,a.IsReturn,a.OrderID,a.ProductId,a.Quantity,d.ProductName,a.SkuName,a.SalesPrice,c.PicUrl FROM OrderDetails AS a
                            INNER JOIN dbo.Product AS b
                            ON a.ProductId =b.ProductId
                            INNER JOIN(SELECT * FROM ProductPic WHERE Flag=1) AS c
                            ON b.ProductId=c.ProductID
                            INNER JOIN(SELECT * FROM OrderDetails_lang WHERE LanguageID={0}) AS d
                            ON d.OrderDetailsID=a.OrderDetailsID
                            WHERE a.OrderID in({1})", model.LanguageID, orderIdString);



                    List<dynamic> sources = _database.RunSqlQuery(x => x.ToResultSets(sql))[0];
                    List<OrderDetailsView> orderDetails = sources.ToEntity<OrderDetailsView>();
                    if (orderDetails != null && orderDetails.Count > 0)
                    {
                        foreach (OrderView orderView in orderViews)
                        {
                            orderView.OrderDetailViews =
                                orderDetails.Where(x => x.OrderId == orderView.OrderID).OrderBy(x => x.OrderId).ToList();
                        }
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// 分页获取待评价订单列表
        /// zhoub 20150817
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel GetPagingEvaluationOrdersIntoWeb(HKTHMall.Domain.WebModel.Models.Orders.SearchOrderView model)
        {
            int[] orderStatusArray = new int[] { 5, 6 };
            SimpleExpression whereExpr = null;
            var result = new ResultModel() { IsValid = false };
            List<OrderModel> resultOrder = base._database.Db.OrderDetails.All()
                .Join(base._database.Db.Order).On(base._database.Db.Order.OrderID == base._database.Db.OrderDetails.OrderID)
                .Select(base._database.Db.OrderDetails.OrderID.Distinct())
                .Where(base._database.Db.Order.UserID == model.UserID && base._database.Db.OrderDetails.Iscomment == 0).ToList<OrderModel>();

            switch (model.d)
            {
                case OrderEnums.TimeSpanType.All:
                    whereExpr = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
                    break;
                case OrderEnums.TimeSpanType.HalfOfMonth:
                    whereExpr = new SimpleExpression(_database.Db.Order.OrderDate, DateTime.Now.Date.AddDays(-15),
                        SimpleExpressionType.GreaterThanOrEqual);
                    break;
                case OrderEnums.TimeSpanType.ThreeMonths:
                    whereExpr = new SimpleExpression(_database.Db.Order.OrderDate, DateTime.Now.Date.AddMonths(-3),
                        SimpleExpressionType.GreaterThanOrEqual);
                    break;
                case OrderEnums.TimeSpanType.Earlier:
                    whereExpr = new SimpleExpression(_database.Db.Order.OrderDate, DateTime.Now.Date.AddMonths(-3),
                        SimpleExpressionType.LessThanOrEqual);
                    break;
            }

            if (resultOrder != null && resultOrder.Count > 0)
            {
                whereExpr = new SimpleExpression(whereExpr, new SimpleExpression(_database.Db.Order.OrderID, resultOrder.Select(x => x.OrderID).ToArray(), SimpleExpressionType.Equal), SimpleExpressionType.And);
            }
            else
            {
                whereExpr = new SimpleExpression(whereExpr, new SimpleExpression(_database.Db.Order.OrderID, "0000000000", SimpleExpressionType.Equal), SimpleExpressionType.And);
            }

            //订单状态为 5 确认收货 6 已完成
            whereExpr = new SimpleExpression(whereExpr, new SimpleExpression(_database.Db.Order.OrderStatus, orderStatusArray, SimpleExpressionType.Equal), SimpleExpressionType.And);

            whereExpr = new SimpleExpression(whereExpr,
                new SimpleExpression(_database.Db.Order.UserID, model.UserID, SimpleExpressionType.Equal), SimpleExpressionType.And);

            var queryResult = _database.Db.Order.Query()
                .Join(_database.Db.YH_MerchantInfo.As("YH_MerchantInfoView"), MerchantID: _database.Db.Order.MerchantID);

            queryResult = queryResult
                .WithOne(queryResult.YH_MerchantInfoView)
                .Where(whereExpr).OrderByOrderDateDescending();

            result.Data = new SimpleDataPagedList<OrderView>(queryResult, model.page - 1, model.pageSize);
            result.IsValid = true;

            if (result.Data != null && result.Data.TotalCount > 0)
            {
                List<OrderView> orderViews = result.Data;
                List<string> orderIds = orderViews.Select(x => "'" + x.OrderID + "'").ToList();

                if (orderIds != null && orderIds.Count > 0)
                {
                    string orderIdString = string.Join(",", orderIds);
                    string sql =
                        string.Format(@"SELECT a.SKU_ProducId,a.Iscomment,a.OrderDetailsID,a.IsReturn,a.OrderID,a.ProductId,a.Quantity,d.ProductName,a.SkuName,a.SalesPrice,c.PicUrl FROM OrderDetails AS a
                            INNER JOIN dbo.Product AS b
                            ON a.ProductId =b.ProductId
                            INNER JOIN(SELECT * FROM ProductPic WHERE Flag=1) AS c
                            ON b.ProductId=c.ProductID
                            INNER JOIN(SELECT * FROM OrderDetails_lang WHERE LanguageID={0}) AS d
                            ON d.OrderDetailsID=a.OrderDetailsID
                            WHERE a.OrderID in({1})", model.LanguageID, orderIdString);

                    List<dynamic> sources = _database.RunSqlQuery(x => x.ToResultSets(sql))[0];
                    List<OrderDetailsView> orderDetails = sources.ToEntity<OrderDetailsView>();
                    if (orderDetails != null && orderDetails.Count > 0)
                    {
                        foreach (OrderView orderView in orderViews)
                        {
                            orderView.OrderDetailViews =
                                orderDetails.Where(x => x.OrderId == orderView.OrderID).OrderBy(x => x.OrderId).ToList();
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="searchModel">条件模型</param>
        /// <returns>结果</returns>
        public ResultModel CancelOrderBy(SearchOrderDetailView searchModel)
        {
            var result = new ResultModel();

            //获取订单详情
            var orderResult = this.GetOrderDetailIntoWebBy(searchModel);

            //支付单
            PaymentOrderView paymentOrder = base._database.Db.PaymentOrder.Query()
                                            .Join(base._database.Db.PaymentOrder_Orders, PaymentOrderID: base._database.Db.PaymentOrder.PaymentOrderID)
                                            .Where(base._database.Db.PaymentOrder_Orders.OrderID == searchModel.OrderID)
                                            .FirstOrDefault<PaymentOrderView>();

            //没有找到该订单
            if (!orderResult.IsValid
                || orderResult.Data == null
                //订单处于待发货，不是货到付款的订单，不允许取消
                || ((int)OrderEnums.OrderStatus.WaitDeliver == orderResult.Data.OrderStatus && (int)OrderEnums.PayChannel.COD != paymentOrder.PayChannel)
                //订单不处于待付款，不允许取消
                || ((int)OrderEnums.OrderStatus.Obligation != orderResult.Data.OrderStatus && (int)OrderEnums.PayChannel.COD != paymentOrder.PayChannel)
                )
            {
                result.IsValid = false;
                result.Messages.Add("The order was not found");//没有找到该订单
                return result;
            }

            //sql语句集合
            List<string> sqls = new List<string>();

            foreach (OrderDetailsView orderDetail in orderResult.Data.OrderDetailViews)
            {
                //还原商品库存
                sqls.Add(
                    productService.GenerateUpdateStockQuantitySql(new ProductView()
                    {
                        ProductId = orderDetail.ProductId,
                        StockQuantity = orderDetail.Quantity
                    }));

                //还原SKU商品库存
                sqls.Add(
                    skuProductService.GenerateUpdateStockSql(new SKU_ProductView()
                    {
                        SKU_ProducId = orderDetail.SKU_ProducId,
                        Stock = orderDetail.Quantity
                    }));
            }

            //更新订单状态
            sqls.Add(
                this.GenerateUpdateOrderStatusSql(new OrderView()
                {
                    OrderID = searchModel.OrderID,
                    OrderStatus = (int)OrderEnums.OrderStatus.Canceled
                }));

            //插入跟踪日志
            var tempOrderTrackingLog = new OrderTrackingLogView()
            {
                OrderID = searchModel.OrderID,
                OrderStatus = (int)OrderEnums.OrderStatus.Canceled,
                TrackingContent = "取消订单",
                CreateBy = searchModel.UserID == orderResult.Data.UserID ? "用户" : "系统",
                CreateTime = DateTime.Now
            };
            sqls.Add(orderTrackingLogService.GenerateInsertSql(tempOrderTrackingLog));

            //取消支付单
            sqls.Add(paymentOrderService.GenerateUpdateStatusSql(new PaymentOrderView()
            {
                Flag = (int)OrderEnums.PaymentFlag.Cancel,
                PaymentOrderID = paymentOrder.PaymentOrderID
            }));

            string sql = SqlTransactionUtil.GenerateTransSql(sqls);
            var queryResult = _database.RunSqlQuery(x => x.ToResultSets(sql));
            dynamic source = queryResult[0][0];
            result.IsValid = source.Count > 0;

            if (result.IsValid)
            {
                result.Messages.Add("Cancel order");

                //只有货到付款，才能取消供应商订单
                if (orderResult.Data.PayChannel == (int)OrderEnums.PayChannel.COD)
                {
                    PurchaseOrderModel model = new PurchaseOrderModel();
                    model.OrderID = searchModel.OrderID;
                    model.CancelDate = DateTime.Now;
                    model.CancelUser = UserInfo.CurrentUserID == 0 ? "用户" : UserInfo.CurrentUserName;
                    model.status = 5;
                    purchaseOrderSerivce.UpdateByOrderId(model);
                }

            }
            return result;
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="searchModel">条件模型（需提供OrderID,[UserId]可选）</param>
        /// <returns>操作结果</returns>
        public ResultModel OutTimeReceivingOrder(SearchOrderDetailView searchModel)
        {
            var result = new ResultModel()
            {
                IsValid = false,
            };
            //订单Id,订单状态
            SimpleExpression whereExpr = new SimpleExpression(base._database.Db.Order.OrderID, searchModel.OrderID, SimpleExpressionType.Equal);

            whereExpr = new SimpleExpression(whereExpr, new SimpleExpression(base._database.Db.Order.OrderStatus, searchModel.OrderStatus, SimpleExpressionType.Equal), SimpleExpressionType.And);

            if (searchModel.UserID.HasValue)
            {
                whereExpr = new SimpleExpression(whereExpr, new SimpleExpression(base._database.Db.Order.UserID, searchModel.UserID, SimpleExpressionType.Equal), SimpleExpressionType.And);
            }

            //订单
            OrderView orderResult = base._database.Db.Order.Find(whereExpr);

            //没有找到该订单
            if (orderResult == null)
            {
                result.IsValid = false;
                result.Messages.Add("The order was not found");//没有找到该订单
                return result;
            }

            //开启事务
            var trans = _database.Db.BeginTransaction();
            try
            {

                trans.Order.UpdateByOrderID(OrderID: orderResult.OrderID, OrderStatus: (int)OrderEnums.OrderStatus.OutTimeReceiving);

                var orderTrackingLog = new OrderTrackingLogView()
                {
                    OrderID = searchModel.OrderID,
                    OrderStatus = (int)OrderEnums.OrderStatus.OutTimeReceiving,
                    TrackingContent = searchModel.UserID.HasValue ? "确认收货" : (string.IsNullOrEmpty(UserInfo.CurrentUserName) ? "超时收货" : "后台确认收货"),
                    //周博 edit 20150910
                    CreateBy = searchModel.UserID.HasValue ? "用户" : (string.IsNullOrEmpty(UserInfo.CurrentUserName) ? "系统" : UserInfo.CurrentUserName),
                    CreateTime = DateTime.Now
                };

                trans.OrderTrackingLog.Insert(orderTrackingLog);
                trans.Commit();
                result.IsValid = true;
                result.Messages.Add("Confirm receipt, operation success");//确认收货,操作成功
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            return result;
        }

        #region 生成订单相关

        /// <summary>
        /// 生成普通订单
        /// </summary>
        /// <param name="addOrderInfoView">新增订单信息</param>
        /// <returns>结果</returns>
        public ResultModel GenerateNormalOrder(AddOrderInfoView addOrderInfoView)
        {

            var result = new ResultModel();
            if (addOrderInfoView != null)
            {

                //获取购物车中的相关信息
                var comInfos = shoppingCartService.getGoodsGroupByCom(1.ToString(), addOrderInfoView.LanguageId,
                    addOrderInfoView.UserId.ToString()).Data;
                result = this.GenerateOrder(addOrderInfoView, comInfos);
            }
            return result;

        }

        /// <summary>
        /// 生成立即购买订单
        /// </summary>
        /// <param name="addOrderInfoView">新增订单信息</param>
        /// <returns>结果</returns>
        public ResultModel GenerateOutrightOrder(AddOrderInfoView addOrderInfoView)
        {

            List<string> productIds = new List<string>();           //商品集合
            List<string> skuProductIds = new List<string>();        //skuid集合

            List<AddOrderInfoView.GoodsView> goods = new List<AddOrderInfoView.GoodsView>();
            addOrderInfoView.MerchantViews.ForEach(x =>
            {
                goods.AddRange(x.Goods);
            });
            if (goods != null && goods.Count > 0)
            {
                productIds = goods.Select(x => x.ProductID).ToList();
                skuProductIds = goods.Select(x => x.SkuNumber).ToList();
            }
            //获取购买的商品集合
            List<GoodsInfoModel> tempGoodInfos = shoppingCartService.GetGoodsInfo(productIds, skuProductIds,
                addOrderInfoView.LanguageId).Data;

            var orderInfos = tempGoodInfos.GroupBy(x => new { x.ComId, x.ComName }).Select(x => new ComInfo()
            {
                ComId = x.Key.ComId,
                ComName = x.Key.ComName,
                Goods = tempGoodInfos.Where(y => y.ComId == x.Key.ComId).ToList()
            }).ToList();

            foreach (var orderInfo in orderInfos)
            {
                foreach (var good in orderInfo.Goods)
                {
                    good.Count += int.Parse(goods.FirstOrDefault(x => x.SkuNumber == good.SkuNumber).ProductNumber);
                }
            }


            return this.GenerateOrder(addOrderInfoView, orderInfos);
        }

        /// <summary>
        /// 根据订单号,用户ID获取订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultModel GetOrderStatus(long orderId, long userId)
        {
            var orderStatus = _database.Db.Order.All().Select(_database.Db.Order.orderStatus).Where(_database.Db.Order.OrderID == orderId && _database.Db.Order.UserID == userId).ToScalarOrDefault();
            ResultModel result = new ResultModel()
            {
                Data = orderStatus
            };
            return result;
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="addOrderInfoView">添加订单参数</param>
        /// <param name="orderInfos">订单信息</param>
        /// <returns>操作结果</returns>
        private ResultModel GenerateOrder(AddOrderInfoView addOrderInfoView, List<ComInfo> orderInfos)
        {
            var resultModel = new ResultModel()
            {
                IsValid = false,
                Status = (int)OrderEnums.GenerateOrderFailType.Fail
            };

            resultModel.Messages = new List<string>()
            {
                CultureHelper.GetAPPLangSgring("FAILURE", addOrderInfoView.LanguageId)
            };



            //List<string> orderIds = new List<string>();
            List<string> sqls = new List<string>();


            if (orderInfos != null)
            {
                //前台提交过来的商家Id集合,需要根据商家分订单
                //var merchantIDs = addOrderInfoView.MerchantViews.Select(x => x.MerchantID).ToList();

                //收货地址
                UserAddressView userAddress =
                    base._database.Db.UserAddress.Find(_database.Db.UserAddress.UserAddressId ==
                                                       addOrderInfoView.ReceiverAddressId &&
                                                       _database.Db.UserAddress.UserID == addOrderInfoView.UserId);
                if (userAddress == null)
                {
                    resultModel.Status = (int)OrderEnums.GenerateOrderFailType.NotAddress;
                    resultModel.Messages = new List<string>() { CultureHelper.GetAPPLangSgring("NOT_ADDRESS", addOrderInfoView.LanguageId) };
                    return resultModel;
                }

                //取出所有商品
                List<GoodsInfoModel> goodsInfos = new List<GoodsInfoModel>();
                orderInfos.ForEach(x =>
                {
                    if (x.Goods != null)
                    {
                        goodsInfos.AddRange(x.Goods);
                    }
                });


                //没有商品
                if (orderInfos.Count == 0)
                {
                    resultModel.Status = (int)OrderEnums.GenerateOrderFailType.NotStock;
                    resultModel.Messages = new List<string>() { CultureHelper.GetAPPLangSgring("MONEY_SHOPPINGCART_INSUFFICIENTINVENTORY", addOrderInfoView.LanguageId) };
                    return resultModel;
                }


                //是否存在库存不足的商品
                if (goodsInfos.Count(x => x.StockQuantity == 0 || x.StockQuantity - x.Count < 0) > 0)
                {
                    resultModel.Status = (int)OrderEnums.GenerateOrderFailType.NotStock;
                    resultModel.Messages = new List<string>() { CultureHelper.GetAPPLangSgring("MONEY_SHOPPINGCART_INSUFFICIENTINVENTORY", addOrderInfoView.LanguageId) };
                    return resultModel;
                }

                //已上架
                int upShelvesStatus = (int)ProductStatus.HasUpShelves;


                //是否存在已下架的商品
                if (goodsInfos.Count(x => x.Status == upShelvesStatus) != goodsInfos.Count)
                {

                    resultModel.Status = (int)OrderEnums.GenerateOrderFailType.UnShelve;
                    resultModel.Messages = new List<string>() { CultureHelper.GetAPPLangSgring("MONEY_SHOPPINGCART_COMMODITIESHAVESHELVES", addOrderInfoView.LanguageId) };
                    return resultModel;
                }



                //商品语言
                List<Product_LangView> productLangViews = base._database.Db.Product_Lang
                    .FindAll(base._database.Db.Product_Lang.ProductId == goodsInfos.Select(x => x.GoodsId).ToArray())
                    .Select(base._database.Db.Product_Lang.Id,
                            base._database.Db.Product_Lang.ProductId,
                            base._database.Db.Product_Lang.ProductName,
                            base._database.Db.Product_Lang.LanguageID
                            ).ToList<Product_LangView>();

                #region 支付单

                //支付单
                PaymentOrderView paymentOrderView = new PaymentOrderView()
                {
                    PaymentOrderID = addOrderInfoView.PaymentOrderId,
                    CreateDT = DateTime.Now,
                    Flag = (int)OrderEnums.PaymentFlag.NonPaid,
                    PayChannel = addOrderInfoView.PayChannel,
                    UserID = addOrderInfoView.UserId,
                    PayType = addOrderInfoView.PaidType
                };

                #endregion


                //不同商家,不同订单,同一支付
                foreach (ComInfo orderInfo in orderInfos)
                {


                    var tempMerchant =
                        addOrderInfoView.MerchantViews.FirstOrDefault(x => x.MerchantID == orderInfo.ComId);

                    //订单
                    var tempOrder = new OrderView()
                    {
                        OrderID = MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString(),
                        OrderStatus = (int)OrderEnums.OrderStatus.Obligation,
                        ComplaintStatus = 0,
                        IsDisplay = 1,
                        IsReward = 0,
                        MerchantID = long.Parse(orderInfo.ComId),
                        OrderDate = DateTime.Now,
                        OrderSource = addOrderInfoView.OrderSource,
                        PayType = addOrderInfoView.PayType,
                        PayChannel = addOrderInfoView.PayChannel,
                        Remark = tempMerchant == null ? "" : tempMerchant.Remark,
                        UserID = addOrderInfoView.UserId,
                        PayDays = 0,
                        DelayDays = 0,
                        Vouchers = 0, //抵用金
                        RefundFlag = 0
                    };
                    //tempOrder.TotalAmount = orderInfo.Goods.Sum(x => x.GoodsUnits * x.Count); //订单总费用
                    tempOrder.OrderAmount = orderInfo.Goods.Sum(x => x.GoodsUnits * x.Count); //商品总费用
                    tempOrder.ExpressMoney = this.GetOrderExpressMoney(orderInfo, userAddress.UserAddressId, tempOrder.OrderAmount.Value).Data;      //订单运费
                    tempOrder.TotalAmount = tempOrder.OrderAmount+tempOrder.ExpressMoney;//orderInfo.Goods.Sum(x => x.GoodsUnits * x.Count) + tempOrder.ExpressMoney; //订单总费用
                    
                    tempOrder.CostAmount = orderInfo.Goods.Sum(x => x.PurchasePrice * x.Count); //商品总成本费用


                    //订单详情
                    int isAllWebsite = Convert.ToInt32(this.parameterSetService.GetParametePValueById(7529218804).Data);
                    int? rebateDays = null;
                    decimal? rebateRatio = null;
                    foreach (GoodsInfoModel goods in orderInfo.Goods)
                    {
                        if (isAllWebsite == 0)
                        {
                            rebateDays = goods.RebateDays;
                            rebateRatio = goods.RebateRatio;
                        }
                        else
                        {
                            rebateDays = this.parameterSetService.GetParametePValueById(7529218793).Data != null ? Convert.ToInt32(this.parameterSetService.GetParametePValueById(7529218793).Data) : null;
                            rebateRatio = this.parameterSetService.GetParametePValueById(7529218877).Data != null ? decimal.Parse(this.parameterSetService.GetParametePValueById(7529218877).Data) : null;
                        }
                        var tempOrderDetail = new OrderDetailsViewT1()
                        {
                            CostPrice = goods.PurchasePrice,
                            Iscomment = 0,
                            IsReturn = 0,
                            OrderDetailsID = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                            OrderID = tempOrder.OrderID,
                            ProductId = long.Parse(goods.GoodsId),
                            ProductName = goods.GoodsName,
                            Quantity = goods.Count,
                            SalesPrice = goods.GoodsUnits,
                            ProductSnapshotID = 0,
                            SKU_ProducId = long.Parse(goods.SkuNumber),
                            SkuName = goods.ValueStr,
                            SubTotal = goods.GoodsUnits * goods.Count,
                            SupplierId = goods.SupplierId,
                            RebateDays = rebateDays,
                            RebateRatio = rebateRatio
                        };
                        //构建订单详情sql
                        sqls.Add(this.GenerateInsertOrderDetailSql(tempOrderDetail));


                        #region 商品名（多语言）

                        //获取当前商品的语言包
                        List<Product_LangView> currentProductLangViews =
                            productLangViews.Where(x => x.ProductId == long.Parse(goods.GoodsId)).ToList();

                        foreach (var currentProductLangView in currentProductLangViews)
                        {
                            //构建订单详情语言包
                            sqls.Add(this.GenerateInsertOrderDetails_langSql(new HKTHMall.Domain.WebModel.Models.Orders.OrderDetails_lang()
                            {
                                OrderDetails_langId = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                                OrderDetailsID = tempOrderDetail.OrderDetailsID,
                                ProductId = tempOrderDetail.ProductId,
                                ProductName = currentProductLangView.ProductName,
                                LanguageID = currentProductLangView.LanguageID
                            }));


                        }

                        #endregion

                        //构建更新库存sql
                        sqls.Add(
                            skuProductService.GenerateUpdateStockSql(new SKU_ProductView()
                            {
                                Stock = -tempOrderDetail.Quantity,
                                SKU_ProducId = tempOrderDetail.SKU_ProducId
                            }));

                        //构建更新商品库存sql 如果是货到付款还需更新销售量
                        sqls.Add(
                            productService.GenerateUpdateStockQuantitySql(new ProductView()
                            {
                                StockQuantity = -tempOrderDetail.Quantity,
                                ProductId = tempOrderDetail.ProductId,
                                SaleCount = 0
                            }));

                        //构建删除购物车sql
                        sqls.Add(
                            shoppingCartService.GenerateDeleteSql(new ShoppingCartModel()
                            {
                                UserID = addOrderInfoView.UserId,
                                SKU_ProducId = tempOrderDetail.SKU_ProducId,

                            }));

                    }


                    //统计费用
                    paymentOrderView.ProductAmount += tempOrder.TotalAmount.Value;

                    //订单地址
                    OrderAddressView orderAddressView = new OrderAddressView()
                    {
                        DetailsAddress = userAddress.DetailsAddress,
                        Email = userAddress.Email,
                        Mobile = userAddress.Mobile,
                        OrderAddressId = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                        OrderID = tempOrder.OrderID,
                        Phone = userAddress.Phone,
                        PostalCode = userAddress.PostalCode,
                        Receiver = userAddress.Receiver,
                        THAreaID = userAddress.THAreaID
                    };

                    //构建订单地址sql
                    sqls.Add(this.GenerateInsertOrderAddressSql(orderAddressView));

                    OrderTrackingLogView orderTrackingLogView = new OrderTrackingLogView()
                    {
                        CreateBy = "用户",
                        CreateTime = DateTime.Now,
                        OrderID = tempOrder.OrderID,
                        OrderStatus = (int)OrderEnums.OrderStatus.Obligation,
                        TrackingContent = "提交订单"
                    };

                    //构建订单跟踪日志
                    sqls.Add(orderTrackingLogService.GenerateInsertSql(orderTrackingLogView));

                    //订单、支付单关联数据
                    PaymentOrder_OrdersView paymentOrderOrdersView = new PaymentOrder_OrdersView()
                    {
                        OrderID = tempOrder.OrderID,
                        PaymentOrderID = paymentOrderView.PaymentOrderID,
                        RelateID = MemCacheFactory.GetCurrentMemCache().Increment("commonId")
                    };
                    //构建订单、支付单关联
                    sqls.Add(paymentOrderService.GenerateInsertPaymentOrder_OrdersSql(paymentOrderOrdersView));

                    //构建订单sql
                    sqls.Insert(0, this.GenerateInsertSql(tempOrder));

                }

                //构建支付单sql
                sqls.Insert(0, paymentOrderService.GenerateInsertSql(paymentOrderView));

                string sql = SqlTransactionUtil.GenerateTransSql(sqls);

                //执行sql
                var queryResult = _database.RunSqlQuery(x => x.ToResultSets(sql));
                dynamic source = queryResult[0][0];
                resultModel.IsValid = source.Count > 0;

                if (resultModel.IsValid)
                {
                    resultModel.Status = (int)OrderEnums.GenerateOrderFailType.Success;
                    resultModel.Messages = new List<string>() { CultureHelper.GetAPPLangSgring("SUCCESS", addOrderInfoView.LanguageId) };
                    resultModel.Data = paymentOrderView.PaymentOrderID;
                }

            }
            return resultModel;
        }

        /// <summary>
        /// 重新支付订单
        /// </summary>
        /// <param name="orderView">订单实体 需要提供OrderID,UserID</param>
        /// <returns>操作结果</returns>
        public ResultModel AgainPaymentOrder(OrderView orderView)
        {
            var result = new ResultModel() { IsValid = false };

            var tempData = this._database.Db.Order.Query()
                  .LeftJoin(this._database.Db.PaymentOrder_Orders, OrderID: this._database.Db.Order.OrderID)
                  .LeftJoin(this._database.Db.PaymentOrder, PaymentOrderID: this._database.Db.PaymentOrder_Orders.PaymentOrderID)
                  .Where(this._database.Db.Order.OrderID == orderView.OrderID
                        && this._database.Db.Order.UserID == orderView.UserID)
                  .Select
                  (
                      this._database.Db.Order.OrderID,
                      this._database.Db.Order.OrderStatus,
                      this._database.Db.Order.ExpressMoney,
                      this._database.Db.Order.TotalAmount,
                      this._database.Db.Order.PayChannel,
                      this._database.Db.PaymentOrder.PaymentOrderID

                  )
                  .FirstOrDefault();

            if (tempData == null)
            {
                result.IsValid = false;
                result.Messages.Add("The order was not found");//没有找到该订单
                return result;
            }

            if (tempData.OrderStatus != (int)OrderEnums.OrderStatus.Obligation)
            {
                result.IsValid = false;
                result.Messages.Add("Only unpaid orders can be re paid");//只有未支付的订单才能重新支付
                return result;
            }

            //开启事务
            using (var trans = _database.Db.BeginTransaction())
            {
                try
                {


                    //如果数据库中存在支付单,支付单和订单的关联数据,需要删除
                    if (!string.IsNullOrEmpty(tempData.PaymentOrderID))
                    {
                        trans.PaymentOrder.Delete(PaymentOrderID: tempData.PaymentOrderID);
                        trans.PaymentOrder_Orders.Delete(PaymentOrderID: tempData.PaymentOrderID);
                    }

                    //重新生成支付订单
                    dynamic newPaymentOrder = new SimpleRecord();
                    newPaymentOrder.PaymentOrderID = MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString();
                    newPaymentOrder.CreateDT = DateTime.Now;
                    newPaymentOrder.Flag = (int)OrderEnums.PaymentFlag.NonPaid;
                    newPaymentOrder.RealAmount = 0;
                    newPaymentOrder.ProductAmount = tempData.TotalAmount;
                    newPaymentOrder.PayChannel = tempData.PayChannel;
                    newPaymentOrder.PayType = (int)OrderEnums.PaidType.Mall;
                    newPaymentOrder.UserID = orderView.UserID;
                    newPaymentOrder.OrderNO = string.Empty;
                    newPaymentOrder.RechargeAmount = 0;
                    trans.PaymentOrder.Insert(newPaymentOrder);

                    //重新插入订单、支付单关联数据
                    trans.PaymentOrder_Orders.Insert(new PaymentOrder_OrdersView()
                    {
                        RelateID = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                        OrderID = orderView.OrderID,
                        PaymentOrderID = newPaymentOrder.PaymentOrderID
                    });
                    result.IsValid = true;
                    result.Data = newPaymentOrder.PaymentOrderID;
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;

        }

        /// <summary>
        /// 获取订单集合各项的运费
        /// </summary>
        /// <param name="comInfos">订单列表</param>
        /// <param name="userAddressId">收货地址区域</param>
        /// <returns>操作结果</returns>
        public ResultModel GetOrdersExpressMoney(List<ComInfo> comInfos, long userAddressId)
        {
            decimal total = 0;
            foreach (var item in comInfos)
            {
                total += item.Goods.Sum(P => P.GoodsUnits * P.Count);
            }
            foreach (var comInfo in comInfos)
            {
                comInfo.ExpressMoney = this.GetOrderExpressMoney(comInfo, userAddressId, total).Data;
            }
            return new ResultModel();
        }

        /// <summary>
        /// 获取订单运费
        /// </summary>
        /// <param name="comInfo">订单项</param>
        /// <param name="userAddressId">收货地址区域</param>
        /// <returns></returns>
        public ResultModel GetOrderExpressMoney(ComInfo comInfo, long userAddressId, decimal total = 0)
        {
            decimal expressMoney = 0;
            ResultModel resultModel = new ResultModel();
            resultModel.IsValid = true;
            resultModel.Data = expressMoney;
            int freeShippingCount = comInfo.Goods.Count(x => x.FreeShipping == 0);
            if (freeShippingCount <= 0)
            {
                resultModel.Data = 0;
                return resultModel;
            }
            List<YF_FareTempModel> fareTemp = base._database.Db.YF_FareTemp.All().ToList<YF_FareTempModel>();
            foreach (var item in comInfo.Goods)
            {
                if (item.FreeShipping == 1)
                {
                    continue;
                }
                if (!fareTemp.Exists(P => P.FareTempID == item.FareTemplateID))
                {
                    continue;
                }
                if (!fareTemp.Exists(P => P.IsFreeShip != 2))
                {
                    continue;
                }
                YF_FareTempModel f = fareTemp.Find(P => P.FareTempID == item.FareTemplateID);
                if (f.IsFree && item.GoodsUnits*item.Count >= f.IsFreeValue)
                {
                    continue;
                }
                decimal frequency = 0;
                if (f.InitialAmount <= 0)
                {
                    f.InitialAmount = 1;
                }
                switch (f.Type)
                {
                    case 1:
                        if (item.Count > f.InitialAmount)
                        {
                            frequency = Math.Ceiling((item.Count - f.InitialAmount) / f.AdditiveAmount);
                        }                        
                        break;
                    case 2:
                        if (item.Weight * item.Count > f.InitialAmount)
                        {
                            frequency = Math.Ceiling((item.Weight * item.Count - f.InitialAmount) / f.AdditiveAmount);
                        }
                        break;
                    case 3:
                        if (item.Volume * item.Count > f.InitialAmount)
                        {
                            frequency = Math.Ceiling((item.Volume * item.Count - f.InitialAmount) / f.AdditiveAmount);
                        }                        
                        break;
                    default:
                        break;
                }
                expressMoney += f.InitialValue + frequency * f.AdditiveValue;
            }
            resultModel.Data = expressMoney;
            return resultModel;
        }


        /// <summary>
        /// 获取订单运费
        /// </summary>
        /// <param name="comInfo">订单项</param>
        /// <param name="userAddressId">收货地址区域</param>
        /// <returns></returns>
        public ResultModel GetOrderExpressMoney_bak(ComInfo comInfo, long userAddressId)
        {
            //订单运费
            decimal expressMoney = 0;

            //运费重量区间
            string strWeight = ConfigurationManager.AppSettings["WeightAreas"];
            if (string.IsNullOrEmpty(strWeight))
            {
                strWeight = "1,3,5,10,15,20";
            }


            //运费重量区间集合
            decimal[] weights = Array.ConvertAll(strWeight.Split(','), x => decimal.Parse(x));


            int freeShippingCount = comInfo.Goods.Count(x => x.FreeShipping == 1);
            int goodsCount = comInfo.Goods.Count();

            //如果商品免运费数量等于订单商品数量，此订单免运费（包邮）
            if (freeShippingCount == goodsCount)
            {
                expressMoney = 0;
            }
            else
            {


                //订单总重量
                decimal totalOrderWeight = comInfo.Goods.Where(x => x.FreeShipping == 0).Sum(x => x.Weight * x.Count);
                //区域邮费目前只分  泰国 和香港  
                //                string sql = string.Format(@"SELECT TOP 1 a.FareTemplateID,a.MerchantID,a.NAME,a.ComputeMode,a.IsDefault,
                //		                                    ISNULL(ISNULL(b.Price1, a.Price1), 0) AS Price1,
                //                                            ISNULL(ISNULL(b.Price2, a.Price2), 0) AS Price2,
                //                                            ISNULL(ISNULL(b.Price3, a.Price3), 0) AS Price3,
                //                                            ISNULL(ISNULL(b.Price4, a.Price4), 0) AS Price4,
                //                                            ISNULL(ISNULL(b.Price5, a.Price5), 0) AS Price5,
                //                                            ISNULL(ISNULL(b.Price6, a.Price6), 0) AS Price6,
                //                                            ISNULL(ISNULL(b.Price7, a.Price7), 0) AS Price7
                //                                         FROM    YF_FareTemplate AS a
                //                                         LEFT JOIN (SELECT * FROM ShipmentTemplate  WHERE   ',' + CityIds + ',' LIKE '%,'+CAST((SELECT THAreaID FROM            UserAddress WHERE UserAddressId={0}) AS NVARCHAR(max))+',%') AS b ON a.FareTemplateID = b.FareTemplateID
                //                                         WHERE  MerchantID = {1} AND a.IsDefault={2}",
                //                                             userAddressId, comInfo.ComId, 1);
                //查询父级ID
                UserAddressView thAreaModel = base._database.Db.UserAddress.Find(base._database.Db.UserAddress.UserAddressId == userAddressId);
                long parentid = GetParentID(thAreaModel.THAreaID);

                string sql = string.Format(@"SELECT TOP 1 a.FareTemplateID,a.MerchantID,a.NAME,a.ComputeMode,a.IsDefault,
		                                    ISNULL(ISNULL(b.Price1, a.Price1), 0) AS Price1,
                                            ISNULL(ISNULL(b.Price2, a.Price2), 0) AS Price2,
                                            ISNULL(ISNULL(b.Price3, a.Price3), 0) AS Price3,
                                            ISNULL(ISNULL(b.Price4, a.Price4), 0) AS Price4,
                                            ISNULL(ISNULL(b.Price5, a.Price5), 0) AS Price5,
                                            ISNULL(ISNULL(b.Price6, a.Price6), 0) AS Price6,
                                            ISNULL(ISNULL(b.Price7, a.Price7), 0) AS Price7
                                         FROM    YF_FareTemplate AS a
                                         LEFT JOIN (SELECT * FROM YF_FareTemplateAreaCountry  WHERE   AreaId ={0}) AS b ON a.FareTemplateID = b.FareTemplateID
                                         WHERE  MerchantID = {1} AND a.IsDefault={2}",
                             parentid, comInfo.ComId, 1);
                //执行sql
                var queryResult = _database.RunSqlQuery(x => x.ToResultSets(sql));
                YF_FareTemplateView fareTemplateView = queryResult[0].Count == 0 ? null : queryResult[0][0];
                if (fareTemplateView != null)
                {
                    if (totalOrderWeight <= weights[0])
                    {
                        expressMoney = fareTemplateView.Price1 ?? 0;
                    }
                    else if (totalOrderWeight <= weights[1])
                    {
                        expressMoney = fareTemplateView.Price2 ?? 0;
                    }
                    else if (totalOrderWeight <= weights[2])
                    {
                        expressMoney = fareTemplateView.Price3 ?? 0;
                    }
                    else if (totalOrderWeight <= weights[3])
                    {
                        expressMoney = fareTemplateView.Price4 ?? 0;
                    }
                    else if (totalOrderWeight <= weights[4])
                    {
                        expressMoney = fareTemplateView.Price5 ?? 0;
                    }
                    else if (totalOrderWeight <= weights[5])
                    {
                        expressMoney = fareTemplateView.Price6 ?? 0;
                    }
                    else
                    {
                        //运费重量区间集合 超过区间7 加收的人工费、没多1kg 增加费用
                        decimal[] paramsArray = Array.ConvertAll(ConfigurationManager.AppSettings["FreightParams"].Split(','), x => decimal.Parse(x));
                        expressMoney = fareTemplateView.Price7 ?? 0;
                        expressMoney += paramsArray[0];
                        expressMoney += Math.Ceiling(totalOrderWeight - weights[5]) * paramsArray[1];



                    }
                }
            }
            ResultModel resultModel = new ResultModel
            {
                Data = expressMoney
            };
            return resultModel;
        }

        /// <summary>
        /// 获取父级ID
        /// </summary>
        /// <param name="THAreaID"></param>
        /// <returns></returns>
        private long GetParentID(long THAreaID)
        {
            HKTHMall.Domain.Models.THArea thAreaModel = base._database.Db.THArea.Find(base._database.Db.THArea.THAreaID == THAreaID);
            if (thAreaModel != null && thAreaModel.ParentID != 0)
            {
                return GetParentID(thAreaModel.ParentID);
            }
            return THAreaID;
        }


        #endregion

        #region Sql

        /// <summary>
        /// 生成插入订单的Sql语句
        /// </summary>
        /// <param name="view">订单实体</param>
        /// <returns>Sql语句</returns>
        internal string GenerateInsertSql(OrderView view)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO [Order] ( OrderID ,UserID ,MerchantID ,OrderStatus ,OrderAmount ,TotalAmount ,");
            sqlBuilder.AppendLine("PayChannel ,PayType ,OrderDate ,Vouchers ,ExpressMoney ,OrderSource ,PayDays ,");
            sqlBuilder.AppendLine("DelayDays ,MerchantRemark ,Remark ,IsDisplay ,IsReward ,ComplaintStatus,CostAmount,RefundFlag,IsPurchase,ExpressID,ExpressOrder) ");
            sqlBuilder.AppendFormat("VALUES  ( '{0}' , {1} ,{2} ,{3} ,{4},{5} ,{6} ,{7} ,'{8}' ,'{9}' ,{10} ,{11} ,{12} ,{13} ,'{14}' ,'{15}' ,{16} ,{17} ,{18} ,{19},{20},{21},{22},'{23}')",
               SqlFilterUtil.ReplaceSqlChar(view.OrderID),
               view.UserID,
               view.MerchantID,
               view.OrderStatus,
               view.OrderAmount,
               view.TotalAmount,
               view.PayChannel,
               view.PayType,
               view.OrderDate.DateTimeToString(),
               view.Vouchers,
               view.ExpressMoney,
               view.OrderSource,
               view.PayDays,
               view.DelayDays,
               SqlFilterUtil.ReplaceSqlChar(view.MerchantRemark),
               SqlFilterUtil.ReplaceSqlChar(view.Remark),
               view.IsDisplay,
               view.IsReward,
               view.ComplaintStatus,
               view.CostAmount,
               view.RefundFlag,
               view.IsPurchase,
               view.ExpressID,
               SqlFilterUtil.ReplaceSqlChar(view.ExpressOrder)
               );
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 生成订单状态更新Sql语句(OrderStatus,OrderID,[PaidDate])
        /// </summary>
        /// <param name="view">订单实体</param>
        /// <returns>Sql语句</returns>
        internal string GenerateUpdateOrderStatusSql(OrderView view)
        {
            string sql = " UPDATE [Order] SET OrderStatus= " + view.OrderStatus + (view.PaidDate == null ? "" : ",PaidDate='" + view.PaidDate.Value.DateTimeToString() + "'") + " WHERE OrderID='" + SqlFilterUtil.ReplaceSqlChar(view.OrderID) + "'";
            return sql;
        }

        /// <summary>
        /// 生成订单详情新增Sql语句
        /// </summary>
        /// <param name="view">订单详情</param>
        /// <returns>Sql语句</returns>
        internal string GenerateInsertOrderDetailSql(Domain.WebModel.Models.Orders.OrderDetailsViewT1 view)
        {
            StringBuilder sqlBuilder = new StringBuilder(" INSERT INTO dbo.OrderDetails( OrderDetailsID ,OrderID ,ProductName ,ProductSnapshotID ,ProductId ,CostPrice ,SalesPrice ,DiscountInfo ,Quantity ,Unit ,SKU_ProducId ,SkuName ,SubTotal ,Iscomment ,IsReturn,SupplierId,RetateDays,ReateRedio)");
            sqlBuilder.AppendFormat(" VALUES  ( {0} ,'{1}',N'{2}',{3},{4},{5},{6},'{7}',{8}, '{9}',{10},N'{11}',{12},{13},{14},{15},{16},{17})",
                view.OrderDetailsID,
                SqlFilterUtil.ReplaceSqlChar(view.OrderID),
                SqlFilterUtil.ReplaceSqlChar(view.ProductName),
                view.ProductSnapshotID,
                view.ProductId,
                view.CostPrice,
                view.SalesPrice,
                SqlFilterUtil.ReplaceSqlChar(view.DiscountInfo),
                view.Quantity,
                SqlFilterUtil.ReplaceSqlChar(view.Unit),
                view.SKU_ProducId,
                SqlFilterUtil.ReplaceSqlChar(view.SkuName),
                view.SubTotal,
                view.Iscomment,
                view.IsReturn,
                view.SupplierId,
                view.RebateDays,
                view.RebateRatio
                );
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 生成订单收货地址插入sql语句
        /// </summary>
        /// <param name="view">订单收货地址</param>
        /// <returns>sql语句</returns>
        internal string GenerateInsertOrderAddressSql(Domain.WebModel.Models.Orders.OrderAddressView view)
        {
            StringBuilder sqlBuilder = new StringBuilder(" INSERT INTO OrderAddress( OrderAddressId ,OrderID ,Receiver ,THAreaID ,DetailsAddress ,PostalCode ,Mobile ,Phone ,Email)");
            sqlBuilder.AppendFormat(" VALUES  ({0},'{1}',N'{2}',{3},N'{4}','{5}','{6}','{7}','{8}')",
                                    view.OrderAddressId,
                                    SqlFilterUtil.ReplaceSqlChar(view.OrderID),
                                    SqlFilterUtil.ReplaceSqlChar(view.Receiver),
                                    view.THAreaID,
                                    SqlFilterUtil.ReplaceSqlChar(view.DetailsAddress),
                                    SqlFilterUtil.ReplaceSqlChar(view.PostalCode),
                                    SqlFilterUtil.ReplaceSqlChar(view.Mobile),
                                    SqlFilterUtil.ReplaceSqlChar(view.Phone),
                                    SqlFilterUtil.ReplaceSqlChar(view.Email)
                                    );
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 生成订单详情语言包新增Sql语句
        /// </summary>
        /// <param name="view">订单详情语言包实体</param>
        /// <returns>Sql语句</returns>
        internal string GenerateInsertOrderDetails_langSql(HKTHMall.Domain.WebModel.Models.Orders.OrderDetails_lang view)
        {
            string sql =
                string.Format(
                    " INSERT INTO OrderDetails_lang ( OrderDetails_langId ,OrderDetailsID ,ProductId ,ProductName ,LanguageID) VALUES  ( {0} , {1} , {2} ,N'{3}' ,{4} )",
                    view.OrderDetails_langId,
                    view.OrderDetailsID,
                    view.ProductId,
                    SqlFilterUtil.ReplaceSqlChar(view.ProductName),
                    view.LanguageID
                    );
            return sql;
        }

        #endregion

        #region  订单收益分红算法

        /// <summary>
        /// 订单收益分红算法(无事务封装,如有需要事务,则需要调用者在外部代码中自行封装) 
        /// zhoub 20150727
        /// </summary>
        /// <param name="currUser">用户信息</param>
        /// <param name="orderNumber">订单号</param>
        /// <param name="grossProfit">待分利润金额</param>
        /// <param name="createBy">创建者</param>
        /// <param name="companyAccountUserID">公司虚拟帐户ID</param>
        /// <returns></returns>
        public ResultModel AddOrderEarnings(YH_User currUser, string orderId, decimal grossProfit, long companyAccountUserID, string createBy = "")
        {
            ResultModel result = new ResultModel();
            decimal district = 6;     //区代分成比例6,2,2
            decimal city = 2;         //市代
            decimal province = 2;     //省代

            decimal stair = 30;//之前是3,2,3 刘宏文改
            decimal second = 20;
            decimal three = 30;

            if (!string.IsNullOrWhiteSpace(orderId))
            {
                if (grossProfit > 0)
                {
                    if (currUser != null)
                    {
                        if (currUser.Level > 1 && !string.IsNullOrWhiteSpace(currUser.ParentIDs))
                        {
                            string sqlUser = string.Format(@"select * from YH_User  where UserID in(" + currUser.ParentIDs + ")");
                            List<dynamic> sourcesUser = _database.RunSqlQuery(x => x.ToResultSets(sqlUser))[0];
                            List<SearchYH_UserModel> listParentUser = sourcesUser.ToEntity<SearchYH_UserModel>();

                            if (listParentUser != null && listParentUser.Count > 0)
                            {
                                #region 上级代理商所得收益

                                List<SearchYH_AgentModel> listEarningsAgent = new List<SearchYH_AgentModel>();
                                //获取当前分销商的所有上级代理商
                                string sqlAgent = string.Format(@"select * from YH_Agent where UserID in(" + currUser.ParentIDs + ")");
                                List<dynamic> sourcesAgent = _database.RunSqlQuery(x => x.ToResultSets(sqlAgent))[0];
                                List<SearchYH_AgentModel> listParentAgent = sourcesAgent.ToEntity<SearchYH_AgentModel>();

                                if (listParentAgent != null && listParentAgent.Count > 0)
                                {
                                    listParentAgent.ForEach((agent) =>
                                    {
                                        if (agent.AgentType == 4)
                                        {
                                            agent.AgentType = 3;
                                            agent.IsGlobalAgency = true;
                                        }
                                        agent.YH_User = listParentUser.Find(user => user.UserID == agent.UserID);
                                    });

                                    decimal districtAgentEarningsCost = (district / 100) * grossProfit;
                                    decimal cityAgentEarningsCost = (city / 100) * grossProfit; ;
                                    decimal provinceAgentEarningsCost = (province / 100) * grossProfit;
                                    decimal[] aryAgentEarningsCost = { 0, districtAgentEarningsCost, cityAgentEarningsCost, provinceAgentEarningsCost };

                                    List<SearchYH_AgentModel> listAgent = new List<SearchYH_AgentModel>();
                                    var groupByAgentType = listParentAgent.GroupBy(a => a.AgentType);
                                    foreach (var g in groupByAgentType)
                                    {
                                        var list = g.OrderByDescending(agent => agent.YH_User.Level).ToList();
                                        listAgent.Add(list[0]);
                                    }

                                    listAgent = listAgent.OrderByDescending(agent => agent.YH_User.Level).ToList();
                                    SearchYH_AgentModel firstAgent = listAgent[0];
                                    listEarningsAgent.Add(firstAgent);
                                    if (firstAgent.AgentType < 3)
                                    {
                                        for (int i = 1; i < listAgent.Count; i++)
                                        {
                                            if (listAgent[i].AgentType > 2)
                                            {
                                                listEarningsAgent.Add(listAgent[i]);
                                                break;
                                            }
                                            if (listAgent[i].AgentType > firstAgent.AgentType)
                                            {
                                                listEarningsAgent.Add(listAgent[i]);
                                            }
                                        }
                                    }

                                    foreach (var agent in listEarningsAgent)
                                    {
                                        agent.Earnings += aryAgentEarningsCost[agent.AgentType];
                                        var agentTypeDeficiency = agent.AgentType - 1;
                                        while (agentTypeDeficiency >= 1)
                                        {
                                            var agentDeficiency = listEarningsAgent.Find(a => a.AgentType == agentTypeDeficiency);
                                            if (agentDeficiency == null)//下级代理商缺失
                                            {
                                                agent.Earnings += aryAgentEarningsCost[agentTypeDeficiency];
                                            }
                                            else
                                            {
                                                break;
                                            }
                                            agentTypeDeficiency--;
                                        }
                                    }
                                }

                                #endregion

                                #region 上级分销商所得收益

                                List<SearchYH_UserModel> listEarningsDistributor = new List<SearchYH_UserModel>();
                                decimal stairDistributorEarningsCost = (stair / 100) * grossProfit;
                                decimal secondDistributorEarningsCost = (second / 100) * grossProfit;
                                decimal threeDistributorEarningsCost = (three / 100) * grossProfit;
                                decimal[] aryDistributorEarningsCost = { 0, stairDistributorEarningsCost, secondDistributorEarningsCost, threeDistributorEarningsCost };

                                SearchYH_UserModel stairDistributor = listParentUser.Find(user => user.UserID == currUser.ReferrerID);
                                SearchYH_UserModel secondDistributor = listParentUser.Find(user => user.UserID == currUser.ParentID2);
                                SearchYH_UserModel threeDistributor = listParentUser.Find(user => user.UserID == currUser.ParentID3);

                                if (stairDistributor != null)
                                {
                                    stairDistributor.RelativelyDistributionLevel = 1;
                                    listEarningsDistributor.Add(stairDistributor);
                                }
                                if (secondDistributor != null)
                                {
                                    secondDistributor.RelativelyDistributionLevel = 2;
                                    listEarningsDistributor.Add(secondDistributor);
                                }
                                if (threeDistributor != null)
                                {
                                    threeDistributor.RelativelyDistributionLevel = 3;
                                    listEarningsDistributor.Add(threeDistributor);
                                }

                                foreach (SearchYH_UserModel distributor in listEarningsDistributor)
                                {
                                    distributor.Earnings += aryDistributorEarningsCost[distributor.RelativelyDistributionLevel];
                                }

                                #endregion

                                #region 数据库执行

                                int[] aryEarningsAgentEnum = { 0, 9, 8, 7 };
                                string[] aryEarningsAgentEnumText = { "", "区级代理消费收益", "市级代理消费收益", "省级代理消费收益" };
                                bool flagAgentAddAmount = true;
                                bool flagAgentCutAmount = true;
                                foreach (SearchYH_AgentModel agent in listEarningsAgent)
                                {
                                    flagAgentAddAmount = AddAmountNoTran(orderId, Convert.ToInt64(agent.UserID), agent.Earnings, aryEarningsAgentEnum[agent.AgentType],
                                        agent.IsGlobalAgency ? "全球代理消费收益" : aryEarningsAgentEnumText[agent.AgentType], createBy).IsValid;
                                    if (!flagAgentAddAmount)
                                    {
                                        break;
                                    }

                                    flagAgentCutAmount = CutAmountNoTran(orderId, companyAccountUserID, agent.Earnings, aryEarningsAgentEnum[agent.AgentType],
                                        agent.IsGlobalAgency ? "全球代理消费收益" : aryEarningsAgentEnumText[agent.AgentType], createBy).IsValid;
                                    if (!flagAgentCutAmount)
                                    {
                                        break;
                                    }
                                }

                                int[] aryEarningsDistributorEnum = { 0, 10, 11, 12 };
                                string[] aryEarningsDistributorEnumText = { "", "感恩[一级分销商]粉丝消费收益", "感动[二级分销商]粉丝消费收益", "感谢[三级分销商]粉丝消费收益" };
                                bool flagDistributorAddAmount = true;
                                bool flagDistributorCutAmount = true;
                                foreach (SearchYH_UserModel distributor in listEarningsDistributor)
                                {
                                    flagDistributorAddAmount = AddAmountNoTran(orderId, Convert.ToInt64(distributor.UserID), distributor.Earnings,
                                        aryEarningsDistributorEnum[distributor.RelativelyDistributionLevel], aryEarningsDistributorEnumText[distributor.RelativelyDistributionLevel], createBy).IsValid;
                                    if (!flagDistributorAddAmount)
                                    {
                                        break;
                                    }

                                    flagDistributorCutAmount = CutAmountNoTran(orderId, companyAccountUserID, distributor.Earnings,
                                        aryEarningsDistributorEnum[distributor.RelativelyDistributionLevel], aryEarningsDistributorEnumText[distributor.RelativelyDistributionLevel], createBy).IsValid;
                                    if (!flagDistributorCutAmount)
                                    {
                                        break;
                                    }
                                }

                                if (flagAgentAddAmount && flagAgentCutAmount && flagDistributorAddAmount && flagDistributorCutAmount)
                                {
                                    result.IsValid = true;
                                    result.Messages.Add("Database execution success");//数据库执行成功
                                }
                                else
                                {
                                    result.IsValid = false;
                                    result.Messages.Add("Database execution failure");//数据库执行失败
                                }

                                #endregion
                            }
                            else
                            {
                                result.IsValid = false;
                                result.Messages.Add("The distributor's parent distributor was not found.");//未找到该分销商的父级分销商
                            }
                        }
                        else
                        {
                            result.IsValid = false;
                            result.Messages.Add("To be divided into the profit of a single user can not be the top distributors");//待分成利润的下单用户不能为顶级分销商
                        }
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Messages.Add("The user information for the distributor was not found");//未找到该分销商的用户信息
                    }
                }
                else
                {
                    result.IsValid = false;
                    result.Messages.Add("To be divided into profit amount must be greater than zero");//待分成利润金额必须大于零
                }
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add("The order number cannot be an empty string");//
            }
            return result;
        }

        /// <summary>
        /// 用户余额增加
        /// zhoub 20150727
        /// </summary>
        /// <param name="OrderNumber">关联订单号</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="Amount">金额</param>
        /// <param name="AddOrCutType">异动类型</param>
        /// <param name="Remark">备注</param>
        /// <param name="UpdateBy">变动人(账号名)</param>
        /// <returns></returns>
        public ResultModel AddAmountNoTran(string OrderNumber, long UserID, decimal Amount, int AddOrCutType, string Remark, string UpdateBy)
        {
            ResultModel result = new ResultModel();
            ZJ_UserBalanceChangeLogModel model = new ZJ_UserBalanceChangeLogModel();
            ResultModel resultModel = GetAmount(UserID, Amount, UpdateBy, 1);
            if (resultModel.Data != null)
            {
                decimal OldAmount = resultModel.Data.ConsumeBalance;
                decimal NewAmount = OldAmount + Amount;

                //资金异动记录
                model.UserID = UserID;
                model.AddOrCutAmount = Amount;
                model.IsAddOrCut = 1;
                model.OldAmount = OldAmount;
                model.NewAmount = NewAmount;
                model.AddOrCutType = AddOrCutType;
                model.OrderNo = OrderNumber;
                model.Remark = Remark;
                model.IsDisplay = 1;
                model.CreateBy = UpdateBy;
                model.CreateDT = DateTime.Now;

                result.Data = _database.Db.ZJ_UserBalanceChangeLog.Insert(model);
                if (result.Data == null)
                {
                    result.IsValid = false;
                }
            }
            else
            {
                result.IsValid = false;
            }
            return result;
        }

        /// <summary>
        /// 用户余额减少
        /// zhoub 20150728
        /// </summary>
        /// <param name="OrderNumber">关联订单号</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="Amount">金额</param>
        /// <param name="AddOrCutType">异动类型</param>
        /// <param name="Remark">备注</param>
        /// <param name="UpdateBy">变动人(账号名)</param>
        /// <returns></returns>
        public ResultModel CutAmountNoTran(string OrderNumber, long UserID, decimal Amount, int AddOrCutType, string Remark, string UpdateBy)
        {
            ResultModel result = new ResultModel();
            ZJ_UserBalanceChangeLogModel model = new ZJ_UserBalanceChangeLogModel();
            ResultModel resultModel = GetAmount(UserID, Amount, UpdateBy, 0);
            if (resultModel.Data != null)
            {
                decimal OldAmount = (Decimal)resultModel.Data.ConsumeBalance;
                decimal NewAmount = OldAmount - Amount;

                //资金异动记录
                model.UserID = UserID;
                model.AddOrCutAmount = Amount;
                model.IsAddOrCut = 0;
                model.OldAmount = OldAmount;
                model.NewAmount = NewAmount;
                model.AddOrCutType = AddOrCutType;
                model.OrderNo = OrderNumber;
                model.Remark = Remark;
                model.IsDisplay = 1;
                model.CreateBy = UpdateBy;
                model.CreateDT = DateTime.Now;
                result.Data = _database.Db.ZJ_UserBalanceChangeLog.Insert(model);
                if (result.Data == null)
                {
                    result.IsValid = false;
                }
            }
            else
            {
                result.IsValid = false;
            }
            return result;
        }

        /// <summary>
        /// 用户金额处理
        /// zhoub 20150727
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="amount">异动金额</param>
        /// <param name="UpdateBy">更新人</param>
        /// <param name="addOrcut">添加或减少金额（1-添加 0-减少）</param>
        /// <returns></returns>
        public ResultModel GetAmount(long UserID, decimal amount, string UpdateBy, int addOrcut)
        {
            ResultModel result = new ResultModel();
            decimal changeAmount = Math.Abs(amount);
            StringBuilder str = new StringBuilder();
            decimal consumeBalance = 0;
            ZJ_UserBalance userBalance = _database.Db.ZJ_UserBalance.FindAllByUserID(UserID).ToList<ZJ_UserBalance>()[0];
            if (addOrcut == 1)
            {
                consumeBalance = userBalance.ConsumeBalance + changeAmount;
            }
            else
            {
                consumeBalance = userBalance.ConsumeBalance - changeAmount;
            }
            userBalance.UpdateBy = UpdateBy;
            userBalance.UpdateDT = DateTime.Now;
            result.Data = userBalance;
            _database.Db.ZJ_UserBalance.UpdateByUserID(UserID: UserID, ConsumeBalance: consumeBalance, UpdateBy: UpdateBy, UpdateDT: DateTime.Now);
            return result;
        }


        #endregion

        #region 获取售后列表
        /// <summary>
        /// 获取售后列表
        /// 刘文宁 20150817
        /// </summary>
        /// <param name="OrderID">订单编号</param>
        /// <returns></returns>
        public ResultModel GetAfterSaleList(string userId, int pageNo, int PageSize, int lang)
        {
            ResultModel result = new ResultModel();
            try
            {
                List<AfterSaleListModel> lstModel = new List<AfterSaleListModel>();
                var tb = _database.Db.ReturnProductInfo;
                result.IsValid = true;
                dynamic mi, pp, od, pd, tbChild;
                var qr = tb
                .Query()
                .Join(_database.Db.Product, out pd)
                .On(pd.ProductId == tb.ProductId)
                .Join(_database.Db.ProductPic, out pp)
                .On(pp.ProductId == tb.ProductId && pp.Flag == 1)
                .LeftJoin(_database.Db.OrderDetails, out tbChild)
                .On(tbChild.OrderDetailsID == tb.OrderDetailsID)
                .LeftJoin(_database.Db.Order, out od)
                .On(od.OrderID == tb.OrderID)
                .LeftJoin(_database.Db.YH_MerchantInfo, out mi)
                .On(od.MerchantID == mi.MerchantID)
                .Select(
                    tb.ReturnOrderID.Distinct(),
                    tb.CreateTime,
                    tb.OrderID,
                    tb.ProductId,
                    tb.TradeAmount,
                    tb.RefundAmount,
                    tb.ReturntNumber.As("returnNumber"),
                    tb.ReturnStatus,
                    tb.UpdateTime,

                    pd.StockQuantity,

                    pp.PicUrl,

                    tbChild.OrderDetailsID,
                    tbChild.Quantity,
                    tbChild.ProductName,
                    tbChild.SalesPrice,
                    tbChild.SkuName,
                    tbChild.SKU_ProducId.As("skuId"),
                    tbChild.Iscomment,

                    od.MerchantID,
                    od.OrderStatus,
                    od.OrderDate,

                    mi.ShopName

                )
                .Where(_database.Db.ReturnProductInfo.UserID == userId).OrderByDescending(_database.Db.ReturnProductInfo.CreateTime);
                List<AfterSaleListModel> lstCount = qr.ToList<AfterSaleListModel>();
                int count = lstCount.Count;
                if (pageNo == 1)
                {
                    lstModel = qr.Take(PageSize).ToList<AfterSaleListModel>();
                }
                else
                {
                    lstModel = qr.Skip((pageNo - 1) * PageSize).Take(PageSize).ToList<AfterSaleListModel>();
                }
                List<AfterSaleListModel> lstReduce = new List<AfterSaleListModel>();
                foreach (AfterSaleListModel arr in lstModel)
                {
                    if (arr.returnStatus == 3 && arr.updateTime < DateTime.Now.AddDays(-7))//更新超过7日驳回申请退货状态
                    {
                        lstReduce.Add(arr);
                        count--;
                        continue;
                    }
                    if (arr.orderDate != null)//转换时间戳
                    {
                        arr.orderDate = ConvertsTime.DateTimeToTimeStamp(arr.orderDate).ToString();
                    }
                    if (!string.IsNullOrEmpty(arr.picUrl))//增加图片地址前缀
                    {
                        arr.picUrl = (HKSJ.Common.ConfigHelper.GetConfigString("ImagePath") + arr.picUrl);
                    }
                    if (arr.updateTime != null)//转换时间戳
                    {
                        arr.updateTime = ConvertsTime.DateTimeToTimeStamp(arr.updateTime).ToString();
                    }
                    if (arr.createTime != null)//转换时间戳
                    {
                        arr.createTime = ConvertsTime.DateTimeToTimeStamp(arr.createTime).ToString();
                    }
                }
                foreach (AfterSaleListModel arr in lstReduce)
                {
                    lstModel.Remove(arr);
                }
                result.Messages.Add(count.ToString());
                result.Data = lstModel;
                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = true;
                result.Messages.Add(CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", lang));
                return result;
            }
        }
        #endregion

        /// <summary>
        /// 黄主霞 2016-01-19
        /// </summary>
        /// <param name="DetailsId">明细ID</param>
        /// <param name="Status">0正常，1退款申请中，2已退款，3审核未通过</param>
        /// <returns></returns>
        public ResultModel UpdateRefundStatus(long DetailsId, int Status)
        {
            var result = new ResultModel
            {
                Data = _database.Db.OrderDetails.UpdateByOrderDetailsID(OrderDetailsID: DetailsId, IsReturn: Status)
            };
            return result;
        }
    }
}
