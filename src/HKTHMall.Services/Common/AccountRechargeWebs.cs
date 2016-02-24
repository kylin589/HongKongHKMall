using System;
using System.Collections.Generic;
using BrCms.Framework.Mvc.Extensions;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.AccountRecharge;
using HKTHMall.Services.Users;

namespace HKTHMall.Services.Common
{
    public class AccountRechargeWebs
    {
        private ZJ_UserBalanceServiceWeb _zjUserBalanceServiceWeb = new ZJ_UserBalanceServiceWeb();

        /// <summary>
        /// web 账户充值(第三方充值成功)
        /// </summary>
        /// <param name="RechargeAmount">充值金额</param>
        /// <param name="Radiochecked">第三方充值方式通道</param>
        /// <param name="UserID">用户ID</param>
        ///  <param name="Account">登录名</param>
        ///  <param name="AddOrCutType">充值类型（账户充值1）</param>
        ///  <param name="OrderNo">订单编号</param>
        ///  <param name="OrderSource">来源0:网站,1移动设备</param>
        /// <returns></returns>
        public ResultModel AccountRechargeWeb(AccountRechargeModel armodel)
        {
            var result = new ResultModel();


            #region 用户余额信息表 和其对应的资金流水账
            //用户余额信息表（前台）
            ZJ_UserBalanceModel zjubModel = new ZJ_UserBalanceModel();
            zjubModel.UserID = armodel.UserID;
            zjubModel.UpdateBy = "前台账户自己充值";
            zjubModel.UpdateDT = DateTime.Now;
            //用户账户金额异动记录表(资金流水账)
            ZJ_UserBalanceChangeLogModel zjublModel = new ZJ_UserBalanceChangeLogModel();
            zjublModel.Account = armodel.Account;
            zjublModel.AddOrCutAmount = armodel.AddOrCutAmount;
            zjublModel.AddOrCutType = armodel.AddOrCutType;
            zjublModel.CreateBy = armodel.Account == null ? "前台登录名为空" : armodel.Account;
            zjublModel.CreateDT = DateTime.Now;
            if (armodel.AddOrCutAmount >= 0)
            {
                zjublModel.IsAddOrCut = 1;
            }
            else
            {
                zjublModel.IsAddOrCut = 0;
            }
            zjublModel.IsDisplay = 1;
            zjublModel.OrderNo = armodel.OrderNo;
            zjublModel.Remark = "前台账户充值";
            zjublModel.UserID = armodel.UserID;
            #endregion

            #region 用户充值订单
            //用户充值订单
            ZJ_RechargeOrderModel zjroModel = new ZJ_RechargeOrderModel();

            zjroModel.OrderNO = armodel.OrderNo;

            zjroModel.RechargeResult = 1;

            #endregion

            #region 订单支付信息表
            //订单支付信息表
            PaymentOrderModel poModel = new PaymentOrderModel();
            poModel.PaymentOrderID = armodel.PaymentOrderID;
            poModel.Flag = 2;
            poModel.outOrderId = armodel.outOrderId;
            //poModel.outOrderId = armodel.OrderNo;
            //poModel.PaymentDate = DateTime.Now;

            //poModel.PayType = 1;
            //poModel.ProductAmount = armodel.AddOrCutAmount;
            //poModel.RealAmount = armodel.AddOrCutAmount;
            //poModel.UserID = armodel.UserID;
            ////订单支付信息与订单关联记录表
            //PaymentOrder_OrdersModel pooModel = new PaymentOrder_OrdersModel();
            //pooModel.OrderID = armodel.OrderNo;
            //pooModel.RelateID = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
            #endregion

            result = _zjUserBalanceServiceWeb.AccountRechargeWeb(zjubModel, zjublModel, poModel, zjroModel, zjublModel);


            return result;
        }

        /// <summary>
        /// web 账户充值(第三方充值成功)
        /// </summary>
        /// <param name="PaymentOrderID"></param>
        /// <returns></returns>
        public ResultModel AccountRechargeWeb(string PaymentOrderID, string outOrderId)
        {
            var result = new ResultModel();
            SearchAccountRechargeModel sarmodle = new SearchAccountRechargeModel();
            sarmodle.PagedIndex = 0;
            sarmodle.PagedSize = 10;
            sarmodle.PaymentOrderID = PaymentOrderID;
            result = _zjUserBalanceServiceWeb.Select(sarmodle);
            List<ZJ_RechargeOrderModel> zjmodel = new List<ZJ_RechargeOrderModel>();
            zjmodel = result.Data;

            AccountRechargeModel arModel = new AccountRechargeModel();
            if (zjmodel != null && zjmodel.Count > 0)
            {
                arModel.Account = zjmodel[0].Account;
                arModel.AddOrCutAmount = zjmodel[0].RechargeAmount;
                arModel.AddOrCutType = 1;//充值类型 1充值
                arModel.OrderNo = zjmodel[0].OrderNO;
                arModel.OrderSource = zjmodel[0].OrderSource;
                arModel.PaymentOrderID = zjmodel[0].PaymentOrderID;
                arModel.RechargeChannel = zjmodel[0].RechargeChannel;
                arModel.UserID = zjmodel[0].UserID;
                arModel.outOrderId = outOrderId;
                result = AccountRechargeWeb(arModel);
            }


            return result;
        }

        /// <summary>
        /// 添加 用户充值订单表（第三方用户没有返汇前添加）
        /// </summary>
        /// <param name="RechargeAmount">充值金额</param>
        /// <param name="Radiochecked">第三方充值方式通道</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="OrderSource">来源0:网站,1移动设备</param>
        ///  <param name="OrderNO">用户充值订单表ID</param>
        ///   <param name="PaymentOrderID">订单支付信息表ID</param>
        /// <returns></returns>
        public ResultModel InsertAddZJ_RechargeOrder(AccountRechargeModel armodel, ERechargeOrderPrefix prefix, out string OrderNO, out string PaymentOrderID)
        {
            var result = new ResultModel();
            #region 用户充值订单表
            ZJ_RechargeOrderModel zjroModel = new ZJ_RechargeOrderModel();
            zjroModel.CreateDT = DateTime.Now;
            zjroModel.IsDisplay = 1;
            zjroModel.OrderNO = EnumDescription.GetFieldText(prefix) + MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString();//这里是为了区分商品订单还是充值订单
            zjroModel.OrderSource = armodel.OrderSource;
            zjroModel.RechargeAmount = armodel.AddOrCutAmount;
            zjroModel.RechargeChannel = armodel.RechargeChannel;
            zjroModel.RechargeDT = DateTime.Now;
            zjroModel.RechargeResult = 0;//默认是失败的
            zjroModel.UserID = armodel.UserID;

            OrderNO = zjroModel.OrderNO;
            #endregion

            #region 订单支付信息表
            //订单支付信息表
            PaymentOrderModel poModel = new PaymentOrderModel();
            poModel.Flag = 1;
            poModel.outOrderId = "";
            poModel.PaymentDate = DateTime.Now;
            poModel.PaymentOrderID = MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString();
            poModel.PayType = 2;//支付类型（1:商城订单支付,2充值支付）
            poModel.ProductAmount = armodel.AddOrCutAmount;
            poModel.RealAmount = armodel.AddOrCutAmount;
            poModel.UserID = armodel.UserID;
            poModel.CreateDT = DateTime.Now;
            poModel.PayChannel = armodel.RechargeChannel;
            //订单支付信息与订单关联记录表
            PaymentOrder_OrdersModel pooModel = new PaymentOrder_OrdersModel();
            pooModel.OrderID = zjroModel.OrderNO;
            pooModel.RelateID = MemCacheFactory.GetCurrentMemCache().Increment("commonId");

            pooModel.PaymentOrderID = poModel.PaymentOrderID;

            #endregion
            result = _zjUserBalanceServiceWeb.AccountRechargeFailure(zjroModel, pooModel, poModel);

            PaymentOrderID = poModel.PaymentOrderID;
            return result;
        }
    }
}