using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using HKTHMall.Domain.WebModel.Models.AccountRecharge;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.AdminModel.Models.Orders;
using Simple.Data;
using HKTHMall.Domain.WebModel.Models.AccountRecharge;
using BrCms.Framework.Collections;

namespace HKTHMall.Services.Users
{
    /// <summary>
    /// 以下方法都是wuyf写
    /// </summary>
    public class ZJ_UserBalanceServiceWeb : BaseService, IZJ_UserBalanceServiceWeb
    {
        /// <summary>
        /// 更新用户余额信息表（前台and后台）
        /// </summary>
        /// <param name="model">用户余额信息表模型</param>
        /// <returns>是否修改成功</returns>
        public void UpdateZJ_UserBalance(ZJ_UserBalanceModel model, ZJ_UserBalanceChangeLogModel ulogModel, dynamic tx)
        {
            model.IsDisplay = 1;//默认显示
            ulogModel.IsDisplay = 1;
            ZJ_UserBalanceService zjubs = new ZJ_UserBalanceService();
            
            HKTHMall.Domain.AdminModel.Models.User.SearchZJ_UserBalanceModel szjub = new Domain.AdminModel.Models.User.SearchZJ_UserBalanceModel();
            szjub.UserID = model.UserID;
            szjub.PagedIndex = 0;
            szjub.PagedSize = 100;
            //List<HKTHMall.Domain.AdminModel.Models.User.ZJ_UserBalanceModel> list = zjubs.GetZJ_UserBalanceList(szjub).Data;
            HKTHMall.Domain.AdminModel.Models.User.ZJ_UserBalanceModel newmodel = zjubs.GetZJ_UserBalanceById(model.UserID).Data;
            //if (list!=null&&list.Count>0)
            //{
            //    model.ConsumeBalance = list[0].ConsumeBalance + ulogModel.AddOrCutAmount;
            //    ulogModel.NewAmount = model.ConsumeBalance;
            //    ulogModel.OldAmount = list[0].ConsumeBalance;
            //    tx.ZJ_UserBalance.UpdateByUserID(UserID: model.UserID, ConsumeBalance: model.ConsumeBalance, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
            //}
            if (newmodel != null )
            {
                model.ConsumeBalance = newmodel.ConsumeBalance + ulogModel.AddOrCutAmount;//用户余额
                ulogModel.NewAmount = model.ConsumeBalance;//用户新增的金额
                ulogModel.OldAmount = newmodel.ConsumeBalance;//用户原来金额
                tx.ZJ_UserBalance.UpdateByUserID(UserID: model.UserID, ConsumeBalance: model.ConsumeBalance, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
            }
            else
            {
                model.ConsumeBalance =ulogModel.AddOrCutAmount;
                ulogModel.NewAmount = model.ConsumeBalance;
                ulogModel.OldAmount = 0;
                model.UpdateBy = "";
                model.UpdateDT = DateTime.Now;
                tx.ZJ_UserBalance.Insert(model);
            }

            InsertZJ_UserBalanceChangeLog(ulogModel,tx);
        }

        /// <summary>
        ///添加【用户账户金额异动记录表】(资金流水账)
        /// </summary>
        /// <param name="ulogModel">【用户账户金额异动记录表】(资金流水账)model</param>
        /// 
        /// <param name="tx"></param>
        public void InsertZJ_UserBalanceChangeLog(ZJ_UserBalanceChangeLogModel ulogModel, dynamic tx)
        {
            tx.ZJ_UserBalanceChangeLog.Insert(ulogModel);
        }

        /// <summary>
        ///添加订单支付信息
        /// </summary>
        /// <param name="poomodel">订单支付信息与订单关联记录表model</param>
        /// <param name="pomodel">订单支付信息表model</param>
        /// <param name="tx"></param>
        public void InsertPaymentOrder(PaymentOrder_OrdersModel poomodel, PaymentOrderModel pomodel, dynamic tx)
        {
            tx.PaymentOrder.Insert(pomodel);
            poomodel.PaymentOrderID = pomodel.PaymentOrderID;
            tx.PaymentOrder_Orders.Insert(poomodel);
        }

        /// <summary>
        /// 添加用户充值订单表
        /// </summary>
        /// <param name="zjoModel">用户充值订单表Model</param>
        /// <param name="ulogModel">用户账户金额异动记录表model</param>
        /// <param name="tx"></param>
        public void InserZJ_RechargeOrder(ZJ_RechargeOrderModel zjoModel, ZJ_UserBalanceChangeLogModel ulogModel, dynamic tx)
        {
            tx.ZJ_RechargeOrder.Insert(zjoModel);
            tx.ZJ_UserBalanceChangeLog.Insert(ulogModel);
        }

        /// <summary>
        /// 添加用户充值订单表
        /// </summary>
        /// <param name="zjoModel">用户充值订单表Model</param>
        /// <param name="tx"></param>
        public void InserZJ_RechargeOrder(ZJ_RechargeOrderModel zjoModel, dynamic tx)
        {
            tx.ZJ_RechargeOrder.Insert(zjoModel);
            
        }

        /// <summary>
        /// 添加用户充值订单表
        /// </summary>
        /// <param name="model">用户充值订单表</param>
        /// <returns>是否成功</returns>
        public ResultModel AddZJ_RechargeOrder(ZJ_RechargeOrderModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.ZJ_RechargeOrder.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 第三方充值成功修改用户充值订单表订单状态
        /// </summary>
        /// <param name="zjoModel"></param>
        /// <param name="tx"></param>
        public void UpdateZJ_RechargeOrder(ZJ_RechargeOrderModel zjoModel, dynamic tx)
        {
            tx.ZJ_RechargeOrder.UpdateByOrderNO(OrderNO: zjoModel.OrderNO, RechargeResult: zjoModel.RechargeResult);

        }

        /// <summary>
        /// PaymentOrder订单支付信息表 修改支付状态和给第三方编号赋值
        /// </summary>
        /// <param name="zjoModel"></param>
        /// <param name="tx"></param>
        public void UpdatePaymentOrder(PaymentOrderModel poModel, dynamic tx)
        {
            tx.PaymentOrder.UpdateByPaymentOrderID(PaymentOrderID: poModel.PaymentOrderID, Flag: poModel.Flag, outOrderId: poModel.outOrderId);

        }

        /// <summary>
        /// web 账户充值（成功）
        /// </summary>
        /// <param name="model">用户余额信息表model</param>
        /// <param name="ulogModel">用户账户金额异动记录表model</param>
        /// <param name="poomodel">订单支付信息表model</param>
        /// <param name="pomodel">订单支付信息与订单关联记录表model</param>
        /// <returns></returns>
        public ResultModel AccountRechargeWeb(ZJ_UserBalanceModel model, ZJ_UserBalanceChangeLogModel ulogModel,  PaymentOrderModel pomodel, ZJ_RechargeOrderModel zjoModel, ZJ_UserBalanceChangeLogModel zjoulogModel)
        {
            var result = new ResultModel();
            using (var tx1 = _database.Db.BeginTransaction())
            {
                try
                {
                    UpdateZJ_RechargeOrder(zjoModel, tx1);
                    
                    UpdatePaymentOrder(pomodel, tx1);

                    UpdateZJ_UserBalance(model, ulogModel, tx1);
                    
                    tx1.Commit();
                }
                catch (Exception ex)
                {
                    tx1.Rollback();

                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// web 账户充值（没有跳转到第三方前）
        /// </summary>
        /// <param name="zjoModel">用户充值订单表</param>
        /// <param name="poomodel">订单支付信息与订单关联记录表</param>
        /// <param name="pomodel">订单支付信息表</param>
        /// <returns></returns>
        public ResultModel AccountRechargeFailure(ZJ_RechargeOrderModel zjoModel, PaymentOrder_OrdersModel poomodel, PaymentOrderModel pomodel)
        {
            var result = new ResultModel();
            using (var tx1 = _database.Db.BeginTransaction())
            {
                try
                {
                    InserZJ_RechargeOrder(zjoModel, tx1);
                    
                    InsertPaymentOrder(poomodel, pomodel, tx1);
                    tx1.Commit();
                }
                catch (Exception ex)
                {
                    tx1.Rollback();

                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                }
            }
            return result;
            
        }

        /// <summary>
        /// 用户充值订单表查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>wuyf 2015-7-22</remarks>
        public ResultModel Select(SearchAccountRechargeModel model)
        {

            var paymentOrder = _database.Db.PaymentOrder;
            var paymentOrder_Orders = _database.Db.PaymentOrder_Orders;
            var _zjRechargeOrder = _database.Db.ZJ_RechargeOrder;
            var user = _database.Db.YH_User;
            dynamic po;
            dynamic u;

            //查询参数条件
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //订单号
            if (model.OrderNo!=null&&!string.IsNullOrEmpty(model.OrderNo.Trim()))
            {
                whereParam = new SimpleExpression(whereParam, paymentOrder_Orders.OrderID== model.OrderNo , SimpleExpressionType.And);
            }
            //支付编号
            if (model.PaymentOrderID!=null&& !string.IsNullOrEmpty(model.PaymentOrderID.Trim()))
            {
                whereParam = new SimpleExpression(whereParam, paymentOrder_Orders.PaymentOrderID == model.PaymentOrderID, SimpleExpressionType.And);
            }

            var query = _zjRechargeOrder
                .Query()
                .LeftJoin(paymentOrder_Orders, out po).On(po.OrderID == _zjRechargeOrder.OrderNo).
                 LeftJoin(user, out u).On(u.UserID == _zjRechargeOrder.UserID).
                 Select(
                 _zjRechargeOrder.OrderNO,
                 _zjRechargeOrder.UserID,
                 _zjRechargeOrder.RechargeChannel,
                 _zjRechargeOrder.RechargeAmount,
                 _zjRechargeOrder.RechargeDT,
                 _zjRechargeOrder.RechargeResult,
                 _zjRechargeOrder.CreateDT,
                 _zjRechargeOrder.IsDisplay,
                  _zjRechargeOrder.OrderSource,

                 po.PaymentOrderID,
                 u.Account

                 ).Where(whereParam)
                 .OrderByCreateDT();


            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<ZJ_RechargeOrderModel>(query,
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

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
        public ResultModel AccountRechargeWebs(AccountRechargeModel armodel)
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
            
            #endregion

            result = AccountRechargeWeb(zjubModel, zjublModel, poModel, zjroModel, zjublModel);


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
            result = Select(sarmodle);
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
                result = AccountRechargeWebs(arModel);
            }


            return result;
        }
    }
}
