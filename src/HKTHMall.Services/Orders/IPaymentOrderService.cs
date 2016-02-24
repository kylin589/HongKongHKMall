using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Orders;
using BrCms.Framework.Collections;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Domain.Enum;

namespace HKTHMall.Services.Orders
{
    /// <summary>
    /// 订单支付信息服务接口
    /// </summary>
    public interface IPaymentOrderService : IDependency
    {
        /// <summary>
        /// 订单支付信息分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        ResultModel Select(SearchPaymentOrderModel model);


        /// <summary>
        /// 根据支付单号查找支付单
        /// </summary>
        /// <param name="paymentOrderView">支付单实体（需提供PaymengOrderId,UserID）</param>
        /// <returns>支付单实体</returns>
        ResultModel GetPaymentOrderBy(PaymentOrderView paymentOrderView);


        /// <summary>
        /// 根据订单ID，用户ID获取支付单
        /// </summary>
        /// <param name="paymentOrderView">查询支付单对象(需提供 PaymentOrderID,UserID)</param>
        /// <returns></returns>
        ResultModel GetPaymentOrderByOrderNO(PaymentOrderView paymentOrderView);

        /// <summary>
        /// 支付成功 支付订单(充值订单、商城订单)更新操作
        /// </summary>
        /// <param name="view">支付单信息</param>
        /// <returns>操作结果</returns>
        ResultModel PaymentOrder(PaymentOrderView view);

        /// <summary>
        /// 根据支付通道、第三方订单号获取支付单
        /// </summary>
        /// <param name="payChannel">支付通道</param>
        /// <param name="outerOrderId">第三方订单号</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        ResultModel GetPaymentOrderBy(OrderEnums.PayChannel payChannel, string outerOrderId, long userId);

        /// <summary>
        /// 获取支付单数据
        /// </summary>
        /// <param name="view">支付单查找条件(需要提供 PaymentOrderID,UserID)</param>
        /// <param name="languageID">Id</param>
        /// <returns></returns>
        ResultModel GetPaymentActionData(PaymentOrderView view, int languageID);

        /// <summary>
        /// 更新支付单
        /// </summary>
        /// <param name="view">支付单信息（PaymentOrderID,UserID）</param>
        /// <returns></returns>
        ResultModel Update(PaymentOrderView view);


        /// <summary>
        /// 货到付款 支付处理
        /// </summary>
        /// <param name="view">支付单信息（PaymentOrderID,UserID）</param>
        /// <returns></returns>
        ResultModel PaymentCODOrder(PaymentOrderView view);

        /// <summary>
        /// 余额支付
        /// </summary>
        /// <param name="view">支付单信息（需提供 PaymentOrderID,UserID）</param>
        /// <param name="userInfo">>用户信息（用户支付 需提供 UserId,PayPassword,LanguageID）</param>
        /// <param name="isCheckPayPassword">是否检查交易密码（如果是混合支付，充值回来，无需检查交易密码）</param>
        /// <returns></returns>
        ResultModel PaymentBalanceOrder(PaymentOrderView view, UserInfoViewForPayment userInfo, bool isCheckPayPassword);

        /// <summary>
        /// 更新支付单、订单支付通道
        /// </summary>
        /// <param name="view">订单支付信息(PaymentOrderID,PayChannel)</param>
        /// <returns>Sql语句</returns>
        ResultModel UpdatePayChannel(Domain.WebModel.Models.Orders.PaymentOrderView view);

        /// <summary>
        /// 判断充值单类型
        /// </summary>
        /// <param name="paymentOrderId">支付单号</param>
        /// <param name="prefix">充值单前缀</param>
        /// <returns>判断充值单类型</returns>
        ResultModel IsCurrentRechargeOrder(string paymentOrderId, ERechargeOrderPrefix prefix);


        /// <summary>
        /// 根据支付ID获取实付金额和产品名称（用于余额支付，支付成功后的邮件内容）
        /// </summary>
        /// <param name="PaymentOrderID"></param>
        /// <param name="Lang"></param>
        /// <returns></returns>
        ResultModel GetProductNameForEmail(string PaymentOrderID, int Lang);

    }
}
