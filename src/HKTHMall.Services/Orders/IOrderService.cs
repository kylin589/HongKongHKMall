using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models.Orders;
using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.Entities;
using Autofac.Extras.DynamicProxy2;
using HKTHMall.Domain.AdminModel.Models;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;

namespace HKTHMall.Services.Orders
{
    [Intercept(typeof(ServiceIInterceptor))]
    public interface IOrderService : IDependency
    {
        /// <summary>
        /// 分页获取订单列表
        /// </summary>
        /// <param name="model">订单搜索模型</param>
        /// <returns>订单列表数据</returns>
        ResultModel GetPagingOrder(SearchOrderModel model);

        /// <summary>
        /// 更新订单状态
        /// zhoub 20150713
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        ResultModel UpdateOrderStatus(string orderId, int status);

        /// <summary>
        /// 更新订单快递单号
        /// zhoub 20150909
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="expressOrder"></param>
        /// <returns></returns>
        ResultModel UpdateExpressOrder(string orderId, string expressOrder);


        /// <summary>
        /// 根据订单号,用户ID获取订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultModel GetOrderStatus(long orderId, long userId);

        /// <summary>
        /// 订单分页详情
        /// zhoub 20150713
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel GetPagingOrderDetails(long orderId, int languageID);

        /// <summary>
        /// 订单详情
        /// zhoub 20150714
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        ResultModel GetOrderDetails(long orderId);
        /// <summary>
        /// 获取订单明细
        /// 刘文宁20160116
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        ResultModel GetOrderDetailsByPaymentOrderId(string PaymentOrderId);

        /// <summary>
        /// 根据支付单Id获取为付款的订单id集合
        /// </summary>
        /// <author>樊利民</author>
        /// <param name="paymentOrderId">支付单Id</param>
        /// <returns>Data:List`string`</returns>
        ResultModel GetOrderIdByPaymentOrderId(string paymentOrderId);

        /// <summary>
        /// 根据订单ID获取订单信息
        /// zhoub 20150716
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        ResultModel GetOrderByOrderID(string orderId);

        /// <summary>
        /// 根据商家ID、状态，获取商家信息  wuyf  2015-9-9
        /// </summary>
        /// <returns></returns>
        ResultModel GetYH_MerchantInfoByMerchantID(long MerchantID, int AuditStatus);

        /// <summary>
        /// 订单投诉状态更改
        /// zhoub 20150716
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        ResultModel UpdateOrderComplaintStatus(string orderId, int status);

        /// <summary>
        /// 更改快递费
        /// 刘文宁 20160114
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        ResultModel UpdateOrderExpressMoney(OrderModel model, decimal oldExpressFee);

        /// <summary>
        /// 根据用户ID 订单状态 获取待付款/待收货订单数目
        /// </summary>
        /// <returns></returns>
        ResultModel GetOrderByUserIDStatus(long userId, int status);
        /// <summary>
        /// 根据用户ID获取待评价订单数目
        /// </summary>
        /// <returns></returns>
        ResultModel GetOrderUnComment(long userId);


        /// <summary>
        /// 根据订单详情ID查询数据
        /// zhoub 20150720
        /// </summary>
        /// <param name="orderDetailsID"></param>
        /// <returns></returns>
        ResultModel GetOrderDetailsById(long orderDetailsID, long userId, int languageID);

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="searchModel">条件模型（需提供OrderID,[UserId]可选）</param>
        /// <returns>操作结果</returns>
        ResultModel OutTimeReceivingOrder(SearchOrderDetailView searchModel);

        #region 前台


        /// <summary>
        /// 获取订单详情数据
        /// </summary>
        /// <param name="model">订单搜索模型</param>
        /// <returns>订单详情数据</returns>
        ResultModel GetOrderDetailIntoWebBy(HKTHMall.Domain.WebModel.Models.Orders.SearchOrderDetailView model);

        /// <summary>
        /// 分页获取订单列表
        /// </summary>
        /// <param name="model">搜索模型</param>
        /// <returns>订单列表</returns>
        ResultModel GetPagingOrdersIntoWeb(HKTHMall.Domain.WebModel.Models.Orders.SearchOrderView model);

        /// <summary>
        /// 分页获取待评价订单列表
        /// zhoub 20150817
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel GetPagingEvaluationOrdersIntoWeb(HKTHMall.Domain.WebModel.Models.Orders.SearchOrderView model);

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="searchModel">条件模型</param>
        /// <returns>结果</returns>
        ResultModel CancelOrderBy(HKTHMall.Domain.WebModel.Models.Orders.SearchOrderDetailView searchModel);


        /// <summary>
        /// 生成普通订单
        /// </summary>
        /// <param name="addOrderInfoView">新增订单信息</param>
        /// <returns>结果</returns>
        ResultModel GenerateNormalOrder(AddOrderInfoView addOrderInfoView);

        /// <summary>
        /// 生成立即购买订单
        /// </summary>
        /// <param name="addOrderInfoView">新增订单信息</param>
        /// <returns>结果</returns>
        ResultModel GenerateOutrightOrder(AddOrderInfoView addOrderInfoView);



        /// <summary>
        /// 重新支付订单
        /// </summary>
        /// <param name="orderView">订单实体 需要提供OrderID,UserID</param>
        /// <returns>操作结果</returns>
        ResultModel AgainPaymentOrder(OrderView orderView);

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
        ResultModel AddOrderEarnings(YH_User currUser, string orderId, decimal grossProfit, long companyAccountUserID, string createBy = "");

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
        ResultModel AddAmountNoTran(string OrderNumber, long UserID, decimal Amount, int AddOrCutType, string Remark, string UpdateBy);

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
        ResultModel CutAmountNoTran(string OrderNumber, long UserID, decimal Amount, int AddOrCutType, string Remark, string UpdateBy);

        /// <summary>
        /// 用户金额处理
        /// zhoub 20150727
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="amount">异动金额</param>
        /// <param name="UpdateBy">更新人</param>
        /// <param name="addOrcut">添加或减少金额（1-添加 0-减少）</param>
        /// <returns></returns>
        ResultModel GetAmount(long UserID, decimal amount, string UpdateBy, int addOrcut);

        #endregion

        #region 获取售后列表
        /// <summary>
        /// 获取售后列表
        /// 刘文宁 20150817
        /// </summary>
        /// <param name="OrderID">订单编号</param>
        /// <returns></returns>
        ResultModel GetAfterSaleList(string OrderID, int pageNo, int PageSize, int lang);
        #endregion

        /// <summary>
        /// 获取订单集合各项的运费
        /// </summary>
        /// <param name="comInfos">订单列表</param>
        /// <param name="userAddressId">收货地址区域</param>
        /// <returns>操作结果</returns>
        ResultModel GetOrdersExpressMoney(List<ComInfo> comInfos, long userAddressId);

        /// <summary>
        /// 获取订单运费
        /// </summary>
        /// <param name="comInfo">订单项</param>
        /// <param name="areaId">收货地址区域</param>
        /// <returns></returns>
        ResultModel GetOrderExpressMoney(ComInfo comInfo, long userAddressId, decimal total);

        /// <summary>
        /// 更新订单状态
        /// 黄主霞 2016-01-19
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="RefundFlag">0正常，1退款中，2已处理（包括成功，失败）</param>
        /// <returns></returns>
        ResultModel UpdateRefundFlag(string orderId, int RefundFlag);

        /// <summary>
        /// 黄主霞 2016-01-19
        /// </summary>
        /// <param name="DetailsId">明细ID</param>
        /// <param name="Status">0正常，1退款申请中，2已退款，3审核未通过</param>
        /// <returns></returns>
        ResultModel UpdateRefundStatus(long DetailsId, int Status);
    }
}
