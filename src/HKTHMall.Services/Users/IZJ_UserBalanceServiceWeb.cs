using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.AccountRecharge;

namespace HKTHMall.Services.Users
{
    public  interface IZJ_UserBalanceServiceWeb: IDependency
    {
        /// <summary>
        /// 更新用户余额信息表（前台）
        /// </summary>
        /// <param name="model">用户余额信息表模型</param>
        /// <returns>是否修改成功</returns>
        void UpdateZJ_UserBalance(ZJ_UserBalanceModel model, ZJ_UserBalanceChangeLogModel ulogModel, dynamic tx);


        /// <summary>
        ///添加【用户账户金额异动记录表】(资金流水账)
        /// </summary>
        /// <param name="ulogModel">【用户账户金额异动记录表】(资金流水账)model</param>
        /// 
        /// <param name="tx"></param>
       void InsertZJ_UserBalanceChangeLog(ZJ_UserBalanceChangeLogModel ulogModel, dynamic tx);
        

        /// <summary>
        ///添加订单支付信息
        /// </summary>
        /// <param name="poomodel">订单支付信息表model</param>
        /// <param name="pomodel">订单支付信息与订单关联记录表model</param>
        /// <param name="tx"></param>
        void InsertPaymentOrder(PaymentOrder_OrdersModel poomodel, PaymentOrderModel pomodel, dynamic tx);

        /// <summary>
        /// 添加用户充值订单表
        /// </summary>
        /// <param name="zjoModel">用户充值订单表Model</param>
        /// <param name="ulogModel">用户账户金额异动记录表model</param>
        /// <param name="tx"></param>
        void InserZJ_RechargeOrder(ZJ_RechargeOrderModel zjoModel, ZJ_UserBalanceChangeLogModel ulogModel, dynamic tx);

        /// <summary>
        /// 添加用户充值订单表
        /// </summary>
        /// <param name="zjoModel">用户充值订单表Model</param>
        /// <param name="tx"></param>
        void InserZJ_RechargeOrder(ZJ_RechargeOrderModel zjoModel, dynamic tx);

        /// <summary>
        /// 添加用户充值订单表
        /// </summary>
        /// <param name="model">用户充值订单表</param>
        /// <returns>是否成功</returns>
        ResultModel AddZJ_RechargeOrder(ZJ_RechargeOrderModel model);

       /// <summary>
        /// 第三方充值成功修改用户充值订单表订单状态
        /// </summary>
        /// <param name="zjoModel"></param>
        /// <param name="tx"></param>
         void UpdateZJ_RechargeOrder(ZJ_RechargeOrderModel zjoModel, dynamic tx);
        

        /// <summary>
        /// web 账户充值
        /// </summary>
        /// <param name="model">用户余额信息表model</param>
        /// <param name="ulogModel">用户账户金额异动记录表model</param>
        /// <param name="poomodel">订单支付信息表model</param>
        /// <param name="pomodel">订单支付信息与订单关联记录表model</param>
        /// <returns></returns>
        ResultModel AccountRechargeWeb(ZJ_UserBalanceModel model, ZJ_UserBalanceChangeLogModel ulogModel,  PaymentOrderModel pomodel, ZJ_RechargeOrderModel zjoModel, ZJ_UserBalanceChangeLogModel zjoulogModel);

        /// <summary>
        /// web 账户充值（没有跳转到第三方前）
        /// </summary>
        /// <param name="zjoModel">用户充值订单表</param>
        /// <param name="poomodel">订单支付信息与订单关联记录表</param>
        /// <param name="pomodel">订单支付信息表</param>
        /// <returns></returns>
        ResultModel AccountRechargeFailure(ZJ_RechargeOrderModel zjoModel, PaymentOrder_OrdersModel poomodel, PaymentOrderModel pomodel);

        /// <summary>
        /// 用户充值订单表查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>wuyf 2015-7-22</remarks>
        ResultModel Select(SearchAccountRechargeModel model);
    }
}
