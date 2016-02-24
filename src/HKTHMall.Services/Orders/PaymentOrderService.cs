using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BrCms.Framework.Collections;
using BrCms.Framework.Mvc.Extensions;
using HKTHMall.Core.Sql;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Domain.WebModel.Models.SKU;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Services.AC;
using HKTHMall.Services.Common;
using HKTHMall.Services.Orders.Impl;
using HKTHMall.Services.Sys;
using HKTHMall.Services.YHUser;
using Simple.Data;
using System;
using HKTHMall.Services.Products;
using HKTHMall.Core;
using Simple.Data.RawSql;
using HKTHMall.Services.Users;
using HKTHMall.Core.Extensions;
using ZJ_UserBalanceModel = HKTHMall.Domain.AdminModel.Models.User.ZJ_UserBalanceModel;
using HKTHMall.Services.Common.MultiLangKeys;


namespace HKTHMall.Services.Orders
{
    /// <summary>
    /// 订单支付信息服务类
    /// </summary>
    public class PaymentOrderService : BaseService, IPaymentOrderService
    {
        /// <summary>
        /// 订单服务对象
        /// </summary>
        private OrderService orderService;

        /// <summary>
        /// 商品服务对象
        /// </summary>
        private ProductService productService;

        /// <summary>
        /// 订单日志服务对象
        /// </summary>
        private OrderTrackingLogService orderTrackingLogService;


        /// <summary>
        /// 用户余额服务对象
        /// </summary>
        private ZJ_UserBalanceService userBalanceService;

        /// <summary>
        /// 区域服务对象
        /// </summary>
        private ITHAreaService thAreaService;

        /// <summary>
        /// 用户服务
        /// </summary>
        private IYH_UserService userService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="orderTrackingLogService"></param>
        /// <param name="userBalanceService"></param>
        /// <param name="thAreaService"></param>
        /// <param name="userService"></param>
        public PaymentOrderService(ProductService productService, OrderTrackingLogService orderTrackingLogService, ZJ_UserBalanceService userBalanceService, ITHAreaService thAreaService, IYH_UserService userService)
        {

            this.productService = productService;
            this.orderTrackingLogService = orderTrackingLogService;
            this.userBalanceService = userBalanceService;
            this.thAreaService = thAreaService;
            this.userService = userService;
            orderService = new OrderService(null, null,null, null, null, null, null,null);
        }



        /// <summary>
        /// 订单支付信息分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        public ResultModel Select(SearchPaymentOrderModel model)
        {

            var paymentOrder = _database.Db.PaymentOrder;
            var paymentOrder_Orders = _database.Db.PaymentOrder_Orders;
            var user = _database.Db.YH_User;
            dynamic po;
            dynamic u;

            //查询参数条件
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //订单号
            if (!string.IsNullOrEmpty(model.OrderID))
            {
                whereParam = new SimpleExpression(whereParam, paymentOrder_Orders.OrderID.Like("%" + model.OrderID + "%"), SimpleExpressionType.And);
            }
            //手机号码
            if (!string.IsNullOrEmpty(model.Phone))
            {
                whereParam = new SimpleExpression(whereParam, user.Phone.Like("%" + model.Phone + "%"), SimpleExpressionType.And);
            }
            //Email
            if (!string.IsNullOrEmpty(model.Email))
            {
                whereParam = new SimpleExpression(whereParam, user.Email.Like("%" + model.Email + "%"), SimpleExpressionType.And);
            }
            //支付编号
            if (!string.IsNullOrEmpty(model.outOrderId))
            {
                whereParam = new SimpleExpression(whereParam, paymentOrder.PaymentOrderID.Like("%" + model.outOrderId + "%"), SimpleExpressionType.And);
            }
            //支付状态
            if (model.Flag != null)
            {
                whereParam = new SimpleExpression(whereParam, paymentOrder.Flag == model.Flag.Value, SimpleExpressionType.And);
            }
            //支付方式
            if (model.PayChannel != null)
            {
                whereParam = new SimpleExpression(whereParam, paymentOrder.PayChannel == model.PayChannel.Value, SimpleExpressionType.And);
            }
            //支付时间
            if (model.BeginPaymentDate != null)
            {
                whereParam = new SimpleExpression(whereParam, paymentOrder.PaymentDate >= model.BeginPaymentDate, SimpleExpressionType.And);
            }
            if (model.EndPaymentDate != null)
            {
                whereParam = new SimpleExpression(whereParam, paymentOrder.PaymentDate < Convert.ToDateTime(model.EndPaymentDate).AddDays(1), SimpleExpressionType.And);
            }
            var query = paymentOrder.All().
                 LeftJoin(paymentOrder_Orders, out po).On(po.PaymentOrderID == paymentOrder.PaymentOrderID).
                 LeftJoin(user, out u).On(u.UserID == paymentOrder.UserID).
                 Select(
                 paymentOrder.PaymentOrderID,
                 paymentOrder.UserID,
                 paymentOrder.ProductAmount,
                 paymentOrder.RealAmount,
                 paymentOrder.Flag,
                 paymentOrder.PaymentDate,
                 paymentOrder.PayType,
                 paymentOrder.PayChannel,
                 paymentOrder.outOrderId,
                 paymentOrder.CreateDT,
                 po.OrderID,
                 u.Phone,
                 u.Email,
                 u.NickName
                 ).Where(whereParam).OrderByCreateDTDescending();


            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<PaymentOrderModel>(query,
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 更新支付单
        /// </summary>
        /// <param name="view">支付单</param>
        /// <returns></returns>
        public ResultModel Update(PaymentOrderView view)
        {
            return this.UpdatePrivate(view, this._database.Db);
        }


        /// <summary>
        /// 更新支付单
        /// </summary>
        /// <param name="view">支付单</param>
        /// <param name="db">数据Db</param>
        /// <returns></returns>
        private ResultModel UpdatePrivate(PaymentOrderView view, dynamic db)
        {
            ResultModel resultModel = new ResultModel()
            {
                IsValid = db.PaymentOrder.Update(view) > 0
            };
            return resultModel;
        }


        #region internal Methods

        /// <summary>
        /// 生成订单支付信息新增Sql语句
        /// </summary>
        /// <param name="view">订单支付信息</param>
        /// <returns>Sql语句</returns>
        internal string GenerateInsertSql(Domain.WebModel.Models.Orders.PaymentOrderView view)
        {
            StringBuilder sqlBuilder = new StringBuilder(" INSERT INTO [PaymentOrder]([PaymentOrderID],[UserID],[ProductAmount],[RealAmount],[Flag],[CreateDT],[PayType],[PayChannel],[outOrderId],[OrderNO],[RechargeAmount])");
            sqlBuilder.AppendFormat(" VALUES  ( '{0}',{1},{2},{3},{4},'{5}',{6},{7} ,'{8}','{9}',{10}) ",
                                                SqlFilterUtil.ReplaceSqlChar(view.PaymentOrderID),
                                                view.UserID,
                                                view.ProductAmount,
                                                view.RealAmount,
                                                view.Flag,
                                                view.CreateDT.DateTimeToString(),
                                                view.PayType,
                                                view.PayChannel,
                                                SqlFilterUtil.ReplaceSqlChar(view.outOrderId),
                                                view.OrderNO,
                                                view.RechargeAmount
                                                );
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 生成支付单状态更新Sql语句
        /// </summary>
        /// <param name="view">订单支付信息(PaymentOrderID,Flag)</param>
        /// <returns>Sql语句</returns>
        internal string GenerateUpdateStatusSql(Domain.WebModel.Models.Orders.PaymentOrderView view)
        {
            string sql = string.Format(" UPDATE PaymentOrder SET Flag={0} WHERE PaymentOrderID='{1}'", view.Flag, SqlFilterUtil.ReplaceSqlChar(view.PaymentOrderID));
            return sql;
        }

        /// <summary>
        /// 生成支付记录、支付信息关联表新增Sql语句
        /// </summary>
        /// <param name="view">支付记录、支付信息关联实体</param>
        /// <returns>sql语句</returns>
        internal string GenerateInsertPaymentOrder_OrdersSql(Domain.WebModel.Models.Orders.PaymentOrder_OrdersView view)
        {
            StringBuilder sqlBuilder = new StringBuilder(" INSERT INTO PaymentOrder_Orders ( RelateID, PaymentOrderID, OrderID )");
            sqlBuilder.AppendFormat("  VALUES  ( {0},'{1}','{2}')", view.RelateID, SqlFilterUtil.ReplaceSqlChar(view.PaymentOrderID), SqlFilterUtil.ReplaceSqlChar(view.OrderID));
            return sqlBuilder.ToString();
        }


        /// <summary>
        /// 生成支付单 支付成功Sql
        /// </summary>
        /// <param name="view">订单支付信息</param>
        /// <returns>语句</returns>
        internal string GenerateUpdatePaymentSuccessSql(PaymentOrderView view)
        {
            string sql = string.Format(" UPDATE [PaymentOrder] SET [RealAmount] = {0},[Flag] = {1},[PaymentDate] ='{2}',[outOrderId] = '{3}' WHERE [PaymentOrderID] ='{4}'", view.RealAmount, view.Flag, view.PaymentDate.Value.DateTimeToString(), SqlFilterUtil.ReplaceSqlChar(view.outOrderId), SqlFilterUtil.ReplaceSqlChar(view.PaymentOrderID));
            return sql;
        }

        #endregion

        /// <summary>
        /// 根据支付单号查找支付单
        /// </summary>
        /// <param name="paymentOrderView">支付单实体（需提供PaymengOrderId,UserID）</param>
        /// <returns>支付单实体</returns>
        public ResultModel GetPaymentOrderBy(PaymentOrderView paymentOrderView)
        {
            return this.GetPaymentOrderPrivate(paymentOrderView, this._database.Db);
        }


        /// <summary>
        /// 获取支付单信息
        /// </summary>
        /// <param name="paymentOrderView">查询支付对象（需提供PaymengOrderId,UserID）</param>
        /// <param name="db">数据Db</param>
        /// <returns></returns>
        private ResultModel GetPaymentOrderPrivate(PaymentOrderView paymentOrderView, dynamic db)
        {
            ResultModel result = new ResultModel()
            {
                IsValid = false
            };
            PaymentOrderView view =
                db.PaymentOrder.Find(db.PaymentOrder.PaymentOrderID == paymentOrderView.PaymentOrderID
                                        && db.PaymentOrder.UserID == paymentOrderView.UserID);
            result.IsValid = view != null;
            result.Data = view;
            return result;

        }

        /// <summary>
        /// 根据订单ID，用户ID获取支付单
        /// </summary>
        /// <param name="paymentOrderView">查询支付单对象(需提供 PaymentOrderID,UserID)</param>
        /// <returns>Data:PaymentOrderView</returns>
        public ResultModel GetPaymentOrderByOrderNO(PaymentOrderView paymentOrderView)
        {
            ResultModel result = new ResultModel()
            {
                IsValid = false
            };
            string sql = string.Format(
                "SELECT TOP 1 * FROM dbo.PaymentOrder WHERE OrderNO  IN (SELECT b.OrderID FROM PaymentOrder AS a INNER JOIN PaymentOrder_Orders AS b ON a.PaymentOrderID=b.PaymentOrderID WHERE a.PaymentOrderID={0} AND a.UserID={1}) AND UserID={1}", paymentOrderView.PaymentOrderID, paymentOrderView.UserID);

            //执行sql
            var queryResult = _database.RunSqlQuery(x => x.ToResultSets(sql));
            PaymentOrderView paymentOrder = queryResult[0].Count > 0 ? queryResult[0][0] : null;
            result.IsValid = paymentOrder != null;
            result.Data = paymentOrder;
            return result;
        }


        /// <summary>
        /// 根据支付通道、第三方订单号获取支付单
        /// </summary>
        /// <param name="payChannel">支付通道</param>
        /// <param name="outerOrderId">第三方订单号</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Data:PaymentOrderView</returns>
        public ResultModel GetPaymentOrderBy(OrderEnums.PayChannel payChannel, string outerOrderId,long userId)
        {
            ResultModel result = new ResultModel()
            {
                IsValid = false
            };
            PaymentOrderView view =
                _database.Db.PaymentOrder.Find(_database.Db.PaymentOrder.PayChannel == (int)payChannel
                                        && _database.Db.PaymentOrder.outOrderId == outerOrderId
                                        && _database.Db.PaymentOrder.UserID == userId
                                        );
            result.IsValid = view != null;
            result.Data = view;
            return result;
        }


        /// <summary>
        /// 支付成功 支付订单(充值订单、商城订单)更新操作
        /// </summary>
        /// <param name="view">支付单信息</param>
        /// <returns>操作结果</returns>
        public ResultModel PaymentOrder(PaymentOrderView view)
        {

            var result = new ResultModel() { IsValid = false };

            PaymentOrderView tempPaymentOrder =
                base._database.Db.PaymentOrder.Find(base._database.Db.PaymentOrder.paymentOrderId == view.PaymentOrderID);
            if (tempPaymentOrder != null && (OrderEnums.PaymentFlag)tempPaymentOrder.Flag == OrderEnums.PaymentFlag.NonPaid)
            {
                view.PayType = tempPaymentOrder.PayType;
                switch ((OrderEnums.PaidType)tempPaymentOrder.PayType)
                {
                    //商城订单处理
                    case OrderEnums.PaidType.Mall:
                        result = this.PaymentMallOrder(view);
                        break;
                    //充值订单处理    
                    case OrderEnums.PaidType.Recharge:
                    default:
                        ZJ_UserBalanceServiceWeb zjbsw = new ZJ_UserBalanceServiceWeb();
                        result = zjbsw.AccountRechargeWeb(view.PaymentOrderID, view.outOrderId);
                        break;
                }


            }
            return result;
        }

        /// <summary>
        /// 支付商城订单
        /// </summary>
        /// <param name="view">支付单实体</param>
        /// <returns>支付结果</returns>
        private ResultModel PaymentMallOrder(PaymentOrderView view)
        {
            var result = new ResultModel();
            DateTime now = DateTime.Now;        //操作时间
            List<string> sqls = new List<string>();

            //查询需要更新的商品集合
            List<OrderDetailsViewT1> orderDetails = base._database.Db.PaymentOrder.Query()
               .LeftJoin(_database.Db.PaymentOrder_Orders,
                   PaymentOrderID: base._database.Db.PaymentOrder.PaymentOrderID)
               .LeftJoin(_database.Db.OrderDetails, OrderID: _database.Db.PaymentOrder_Orders.OrderID)
               .Where(base._database.Db.PaymentOrder.PaymentOrderID == view.PaymentOrderID)
               .Select(
               _database.Db.OrderDetails.OrderID,
               _database.Db.OrderDetails.ProductId,
               _database.Db.OrderDetails.Quantity).ToList<OrderDetailsViewT1>();


            if (orderDetails != null && orderDetails.Count > 0)
            {
                List<string> orderIds = orderDetails.Select(x => x.OrderID).Distinct().ToList();

                //更新订单状态
                foreach (var orderId in orderIds)
                {
                    sqls.Add(orderService.GenerateUpdateOrderStatusSql(new OrderView() { OrderID = orderId, OrderStatus = (int)OrderEnums.OrderStatus.WaitDeliver, PaidDate = now }));
                    sqls.Add(orderTrackingLogService.GenerateInsertSql(new OrderTrackingLogView()
                    {
                        CreateBy = "用户",
                        CreateTime = now,
                        OrderID = orderId,
                        OrderTrackingId = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                        TrackingContent = "用户付款",
                        OrderStatus = (int)OrderEnums.OrderStatus.WaitDeliver
                    }));
                }

                //更新产品销售量
                foreach (var orderDetail in orderDetails)
                {
                    sqls.Add(productService.GenerateUpdateSaleCountSql(new ProductView() { SaleCount = orderDetail.Quantity, ProductId = orderDetail.ProductId }));
                }

            }

            view.PaymentDate = now;
            view.Flag = (int)OrderEnums.PaymentFlag.Paid;

            //更新支付单状态
            sqls.Add(this.GenerateUpdatePaymentSuccessSql(view));
            string sql = SqlTransactionUtil.GenerateTransSql(sqls);

            //执行sql
            var queryResult = _database.RunSqlQuery(x => x.ToResultSets(sql));
            dynamic source = queryResult[0][0];
            result.IsValid = source.Count > 0;
            return result;
        }

        /// <summary>
        /// 余额支付
        /// </summary>
        /// <param name="view">支付单信息（需提供 PaymentOrderID,UserID）</param>
        /// <param name="userInfo">>用户信息（用户支付 需提供 UserId,PayPassword,LanguageID）</param>
        /// <param name="isCheckPayPassword">是否检查交易密码（如果是混合支付，充值回来，无需检查交易密码）</param>
        /// <returns></returns>
        public ResultModel PaymentBalanceOrder(PaymentOrderView view, UserInfoViewForPayment userInfo, bool isCheckPayPassword)
        {

            var resultModel = new ResultModel()
            {
                IsValid = false,
                //"服务器繁忙，请稍候再试"
                Messages = new List<string>() { CultureHelper.GetAPPLangSgring("PAYMENT_NETWORK_BUSY", userInfo.LanguageId) }
            };

            using (var trans = _database.Db.BeginTransaction())
            {
                try
                {
                    //获取用户数据
                    UserInfoViewForPayment userInfoView = userService.GetYH_UserForPayment(userInfo.UserID, trans).Data;

                    //检查用户是否删除、锁定等
                    ResultModel userInfoResultModel = userService.GetYH_UserForPaymentMessage(userInfoView, isCheckPayPassword ? userInfo : null, trans);


                    //用户、交易密码错误等信息
                    if (!userInfoResultModel.IsValid)
                    {
                        trans.Rollback();
                        resultModel.Messages = new List<string>() { userInfoResultModel.Messages[0] };
                        return resultModel;
                    }

                    //从数据库中获取支付单
                    PaymentOrderView paymentOrder = this.GetPaymentOrderPrivate(view, trans).Data;

                    //余额不足
                    if (paymentOrder.ProductAmount > userInfoView.ConsumeBalance)
                    {
                        trans.Rollback();
                        resultModel.Messages = new List<string>() { CultureHelper.GetAPPLangSgring("MONEY_ORDER_NSUFFICIENT_BALANCE", userInfo.LanguageId) };
                        return resultModel;
                    }

                    //查询该支付单下,所有订单
                    List<OrderView> orderViews = base._database.Db.PaymentOrder.Query()
                        .LeftJoin(_database.Db.PaymentOrder_Orders,
                            PaymentOrderID: base._database.Db.PaymentOrder.PaymentOrderID)
                        .LeftJoin(_database.Db.Order, OrderID: _database.Db.PaymentOrder_Orders.OrderID)
                        .Where(base._database.Db.PaymentOrder.PaymentOrderID == view.PaymentOrderID &&
                               base._database.Db.PaymentOrder.UserID == view.UserID)
                        .Select(
                            _database.Db.Order.UserID,
                            _database.Db.Order.OrderID,
                            _database.Db.Order.TotalAmount).ToList<OrderView>();

                    //循环订单 扣除余额
                    foreach (OrderView orderView in orderViews)
                    {
                        ZJ_UserBalanceModel zjmodel = new ZJ_UserBalanceModel();
                        zjmodel.Account = userInfoView.Account;
                        zjmodel.AddOrCutAmount = -orderView.TotalAmount.Value;
                        zjmodel.AddOrCutType = 2;
                        zjmodel.CreateBy = userInfoView.Account;
                        zjmodel.IsDisplay = 1;
                        zjmodel.Remark = "购物消费";
                        zjmodel.UserID = view.UserID;
                        zjmodel.OrderNo = orderView.OrderID;
                        userBalanceService.UpdateZJ_UserBalance(zjmodel, trans);
                    }

                    trans.Commit();
                    resultModel.IsValid = true;

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }

            return resultModel;
        }


        /// <summary>
        /// 货到付款 支付处理
        /// </summary>
        /// <param name="view">支付单信息（PaymentOrderID,UserID）</param>
        /// <returns></returns>
        public ResultModel PaymentCODOrder(PaymentOrderView view)
        {

            ResultModel result = new ResultModel();

            dynamic orderDetails = base._database.Db.PaymentOrder.All()
                .Join(base._database.Db.PaymentOrder_Orders,
                    PaymentOrderID: base._database.Db.PaymentOrder.PaymentOrderID)
                .Join(base._database.Db.Order, OrderID: base._database.Db.PaymentOrder_Orders.OrderID)
                .Join(base._database.Db.OrderDetails, OrderID: base._database.Db.PaymentOrder_Orders.OrderID)
                .Where(base._database.Db.PaymentOrder.PaymentOrderID == view.PaymentOrderID
                        && base._database.Db.PaymentOrder.UserID == view.UserID
                        && base._database.Db.Order.OrderStatus == (int)OrderEnums.OrderStatus.Obligation
                        )
                .Select(base._database.Db.OrderDetails.ProductId, base._database.Db.OrderDetails.Quantity, base._database.Db.OrderDetails.OrderID).ToList();

            List<string> sqls = new List<string>();
            List<string> orderIds = new List<string>();
            foreach (var orderDetail in orderDetails)
            {
                sqls.Add(
                           productService.GenerateUpdateSaleCountSql(new ProductView()
                           {
                               ProductId = orderDetail.ProductId,
                               SaleCount = orderDetail.Quantity
                           }));
                if (!orderIds.Any(x => x == orderDetail.OrderID))
                {
                    orderIds.Add(orderDetail.OrderID);
                }

            }
            foreach (string orderId in orderIds)
            {
                sqls.Add(orderService.GenerateUpdateOrderStatusSql(new OrderView() { OrderID = orderId, OrderStatus = (int)OrderEnums.OrderStatus.WaitDeliver }));
            }

            string sql = SqlTransactionUtil.GenerateTransSql(sqls);

            //执行sql
            var queryResult = _database.RunSqlQuery(x => x.ToResultSets(sql));
            dynamic source = queryResult[0][0];
            result.IsValid = source.Count > 0;
            return result;

        }

        /// <summary>
        /// 获取支付单数据
        /// </summary>
        /// <param name="view">支付单查找条件(需要提供 PaymentOrderID,UserID)</param>
        /// <param name="languageID">Id</param>
        /// <returns></returns>
        public ResultModel GetPaymentActionData(PaymentOrderView view, int languageID)
        {
            ResultModel resultModel = new ResultModel()
            {
                IsValid = false
            };

            PaymentActionPageView paymentAction = null;

            view.Flag = (int)OrderEnums.PaymentFlag.NonPaid;
            string sql = string.Format(@"SELECT TOP 1 a.PaymentOrderID, a.PayChannel, a.Flag,a.UserID, a.ProductAmount,
                                         c.OrderID,c.OrderAddressId,c.PostalCode,c.Mobile,c.THAreaID,c.Email,c.DetailsAddress,c.Receiver  
                                         FROM PaymentOrder AS a 
                                         INNER JOIN PaymentOrder_Orders AS b 
                                         ON a.PaymentOrderID=b.PaymentOrderID 
                                         INNER JOIN OrderAddress AS c 
                                         ON b.OrderID=c.OrderID
                                         WHERE a.PaymentOrderID='{0}' AND a.Flag={1} AND a.UserID={2}
                                        ", SqlFilterUtil.ReplaceSqlChar(view.PaymentOrderID), view.Flag, view.UserID);
            //执行sql
            var queryResult = _database.RunSqlQuery(x => x.ToResultSets(sql));
            if (queryResult[0].Count > 0)
            {
                resultModel.IsValid = true;
                dynamic paymentData = queryResult[0][0];

                paymentAction = new PaymentActionPageView();
                paymentAction.PaymentOrderView =
                    new PaymentOrderView()
                {
                    PaymentOrderID = paymentData.PaymentOrderID,
                    ProductAmount = paymentData.ProductAmount,
                    PayChannel = paymentData.PayChannel,
                    Flag = paymentData.Flag,
                    UserID = paymentData.UserID
                };
               

                paymentAction.OrderAddressView = new OrderAddressView()
                {
                    OrderID = paymentData.OrderID,
                    OrderAddressId = paymentData.OrderAddressId,
                    PostalCode = paymentData.PostalCode,
                    Mobile = paymentData.Mobile,
                    THAreaID = paymentData.THAreaID,
                    Email = paymentData.Email,
                    DetailsAddress = paymentData.DetailsAddress,
                    Receiver = paymentData.Receiver
                };

                //获取 省、市、区
                Dictionary<string, string> userAreas = thAreaService.GetSingleTierAreaNames(new SearchUserAddressModel()
                    {
                        THAreaID = paymentAction.OrderAddressView.THAreaID
                    }, languageID).Data;

                paymentAction.OrderAddressView.DetailsAddress = AddressHelper.ShowUserAddress(userAreas["Country"],userAreas["Sheng"], userAreas["Shi"], userAreas["Qu"],
                                                                paymentAction.OrderAddressView.DetailsAddress, languageID);

                paymentAction.OrderDetailsForPayResultView = orderService.GetOrderDetailsByPaymentOrderId(paymentAction.PaymentOrderView.PaymentOrderID).Data;

            }

            resultModel.Data = paymentAction;
            return resultModel;
        }

        /// <summary>
        /// 更新支付单、订单支付通道
        /// </summary>
        /// <param name="view">订单支付信息(PaymentOrderID,PayChannel)</param>
        /// <returns>Sql语句</returns>
        public ResultModel UpdatePayChannel(Domain.WebModel.Models.Orders.PaymentOrderView view)
        {
            ResultModel result = new ResultModel();

            List<string> sqls = new List<string>();

            sqls.Add(string.Format(" UPDATE PaymentOrder SET PayChannel={0},OrderNO='{1}',RechargeAmount={2} WHERE PaymentOrderID='{3}'",
                view.PayChannel,
                view.OrderNO,
                view.RechargeAmount,
                SqlFilterUtil.ReplaceSqlChar(view.PaymentOrderID)));
            int payType = view.PayChannel == (int)OrderEnums.PayChannel.Balance
                                                ? (int)OrderEnums.PayType.BalancePay
                                                : (int)OrderEnums.PayType.ThirdPay;
            string orderSql = string.Format(@" UPDATE dbo.[Order] SET PayChannel='{0}', PayType='{1}' WHERE OrderID IN (SELECT b.OrderID FROM PaymentOrder  
                                            AS a INNER JOIN dbo.PaymentOrder_Orders AS b ON a.PaymentOrderID=b.PaymentOrderID AND a.PaymentOrderID='{2}')"
                                            , view.PayChannel, payType, view.PaymentOrderID);
            sqls.Add(orderSql);

            string sql = SqlTransactionUtil.GenerateTransSql(sqls);

            //执行sql
            var queryResult = _database.RunSqlQuery(x => x.ToResultSets(sql));
            dynamic source = queryResult[0][0];
            result.IsValid = source.Count > 0;
            return result;
        }

        /// <summary>
        /// 判断充值单类型
        /// </summary>
        /// <param name="paymentOrderId">支付单号</param>
        /// <param name="prefix">充值单前缀</param>
        /// <returns>判断充值单类型</returns>
        public ResultModel IsCurrentRechargeOrder(string paymentOrderId, ERechargeOrderPrefix prefix)
        {
            ResultModel resultModel = new ResultModel()
            {
                IsValid = false
            };

            string sql = string.Format("SELECT COUNT(*) AS [Count] FROM dbo.PaymentOrder_Orders WHERE PaymentOrderID='{0}' AND OrderID LIKE '{1}%'"
                                        , SqlFilterUtil.ReplaceSqlChar(paymentOrderId), EnumDescription.GetFieldText(prefix));
            //执行sql
            var queryResult = _database.RunSqlQuery(x => x.ToResultSets(sql));
            dynamic source = queryResult[0][0];
            resultModel.IsValid = source.Count > 0;
            return resultModel;
        }
        /// <summary>
        /// 根据支付ID获取产品名称（用于余额支付，支付成功后的邮件内容）
        /// </summary>
        /// <param name="PaymentOrderID"></param>
        /// <param name="Lang"></param>
        /// <returns></returns>
        public ResultModel GetProductNameForEmail(string PaymentOrderID, int Lang)
        {
            string sql=string.Format("SELECT  reverse(stuff(reverse((SELECT CONVERT(varchar,C.Quantity)+'*'+D.ProductName+',' "+
            " FROM [PaymentOrder] AA  JOIN [PaymentOrder_Orders] B ON AA.PaymentOrderID=B.PaymentOrderID JOIN [OrderDetails] C ON B.OrderID=C.OrderID "+
            " JOIN [Product_Lang] D ON C.ProductId=D.ProductId WHERE AA.Flag=2 AND AA.PaymentOrderID='{0}' AND D.LanguageID={1} AND AA.PayChannel=1 FOR XML PATH(''))),1,1,'')) AS 'ProductName'"
            ,PaymentOrderID, Lang);
            ResultModel result = new ResultModel();
            result.Data = _database.RunSqlQuery(x => x.ToResultSets(sql))[0][0].ProductName;
            return result;
        }

        

    }
}
