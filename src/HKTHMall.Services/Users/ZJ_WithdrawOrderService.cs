using BrCms.Framework.Collections;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public class ZJ_WithdrawOrderService : BaseService, IZJ_WithdrawOrderService
    {
        /// <summary>
        /// 通过Id查询提现订单
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-20</remarks>
        public ResultModel GetZJ_WithdrawOrderById(string id)
        {
            var result = new ResultModel() { Data = base._database.Db.ZJ_WithdrawOrder.FindByOrderNO(id) };
            return result;
        }

        /// <summary>
        /// 提现订单分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-20</remarks>

        public ResultModel Select(SearchZJ_WithdrawOrderModel model)
        {

            var zJ_WithdrawOrder = _database.Db.ZJ_WithdrawOrder;
            var user = _database.Db.YH_User;
            dynamic u;

            #region 查询参数条件
            //查询参数条件
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //订单号
            if (!string.IsNullOrEmpty(model.OrderNO))
            {
                whereParam = new SimpleExpression(whereParam, zJ_WithdrawOrder.OrderNO.Like("%" + model.OrderNO + "%"), SimpleExpressionType.And);
            }
            //手机号码
            if (!string.IsNullOrEmpty(model.Phone))
            {
                whereParam = new SimpleExpression(whereParam, user.Phone.Like("%" + model.Phone + "%"), SimpleExpressionType.And);
            }
            //手机号码
            if (!string.IsNullOrEmpty(model.Email))
            {
                whereParam = new SimpleExpression(whereParam, user.Email.Like("%" + model.Email + "%"), SimpleExpressionType.And);
            }
            //用户真实姓名
            if (!string.IsNullOrEmpty(model.RealName))
            {
                whereParam = new SimpleExpression(whereParam, user.RealName.Like("%" + model.RealName + "%"), SimpleExpressionType.And);
            }
            //审核人
            if (!string.IsNullOrEmpty(model.Verifier))
            {
                whereParam = new SimpleExpression(whereParam, zJ_WithdrawOrder.Verifier.Like("%" + model.Verifier + "%"), SimpleExpressionType.And);
            }
            //打款人
            if (!string.IsNullOrEmpty(model.Remitter))
            {
                whereParam = new SimpleExpression(whereParam, zJ_WithdrawOrder.Remitter.Like("%" + model.Remitter + "%"), SimpleExpressionType.And);
            }
            //银行账号
            if (!string.IsNullOrEmpty(model.BankAccount))
            {
                whereParam = new SimpleExpression(whereParam, zJ_WithdrawOrder.BankAccount.Like("%" + model.BankAccount + "%"), SimpleExpressionType.And);
            }
            //开户支行
            if (!string.IsNullOrEmpty(model.BankSubbranch))
            {
                whereParam = new SimpleExpression(whereParam, zJ_WithdrawOrder.BankSubbranch.Like("%" + model.BankSubbranch + "%"), SimpleExpressionType.And);
            }
            //开户名
            if (!string.IsNullOrEmpty(model.BankUserName))
            {
                whereParam = new SimpleExpression(whereParam, zJ_WithdrawOrder.BankUserName.Like("%" + model.BankUserName + "%"), SimpleExpressionType.And);
            }
            //来源
            if (model.OrderSource != null)
            {
                whereParam = new SimpleExpression(whereParam, zJ_WithdrawOrder.OrderSource == model.OrderSource.Value, SimpleExpressionType.And);
            }
            //提现结果
            if (model.WithdrawResult != null)
            {
                whereParam = new SimpleExpression(whereParam, zJ_WithdrawOrder.WithdrawResult == model.WithdrawResult.Value, SimpleExpressionType.And);
            }
            //申请提现时间
            if (model.BeginPaymentDate != null)
            {
                whereParam = new SimpleExpression(whereParam, zJ_WithdrawOrder.WithdrawDT >= model.BeginPaymentDate, SimpleExpressionType.And);
            }
            if (model.EndPaymentDate != null)
            {
                whereParam = new SimpleExpression(whereParam, zJ_WithdrawOrder.WithdrawDT < Convert.ToDateTime(model.EndPaymentDate).AddDays(1), SimpleExpressionType.And);
            }
            #endregion

            var query = zJ_WithdrawOrder.All().
                 LeftJoin(user, out u).On(u.UserID == zJ_WithdrawOrder.UserID).
                 Select(
                 zJ_WithdrawOrder.OrderNO,
                 zJ_WithdrawOrder.UserID,
                zJ_WithdrawOrder.WithdrawAmount,
                zJ_WithdrawOrder.WithdrawCommission,
                zJ_WithdrawOrder.WithdrawDT,
                zJ_WithdrawOrder.Verifier,
                zJ_WithdrawOrder.VerifyDT,
                zJ_WithdrawOrder.Remitter,
                zJ_WithdrawOrder.RemittanceDT,
                zJ_WithdrawOrder.Remark,
                zJ_WithdrawOrder.WithdrawResult,
                zJ_WithdrawOrder.CreateDT,
                zJ_WithdrawOrder.BankAccount,
                zJ_WithdrawOrder.BankName,
                zJ_WithdrawOrder.BankSubbranch,
                zJ_WithdrawOrder.BankUserName,
                zJ_WithdrawOrder.IsDisplay,
                zJ_WithdrawOrder.OrderSource,
                 u.Account,
                 u.RealName,
                 u.Phone,
                 u.Email,
                 u.NickName
                 ).Where(whereParam).OrderBy(zJ_WithdrawOrder.WithdrawResult);


            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<ZJ_WithdrawOrderModel>(query,
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 更新提现订单
        /// </summary>
        /// <param name="model">提现订单对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-20</remarks>
        public ResultModel Update(ZJ_WithdrawOrderModel model)
        {
            var result = new ResultModel();

            using (var bt = this._database.Db.BeginTransaction())
            {
                try
                {
                    //更新审核信息
                    dynamic record = new SimpleRecord();
                    record.OrderNO = model.OrderNO;
                    record.Remark = model.Remark;
                    record.WithdrawResult = model.WithdrawResult;
                    if (!string.IsNullOrEmpty(model.Verifier))
                    {
                        record.Verifier = model.Verifier;
                    }
                    if (model.VerifyDT != null)
                    {
                        record.VerifyDT = model.VerifyDT;
                    }
                    if (!string.IsNullOrEmpty(model.Remitter))
                    {
                        record.Remitter = model.Remitter;
                    }
                    if (model.RemittanceDT != null)
                    {
                        record.RemittanceDT = model.RemittanceDT;
                    }
                    if (model.WithdrawCommission != null)
                    {
                        record.WithdrawCommission = model.WithdrawCommission;
                    }
                    var upWithdrawOrder = bt.ZJ_WithdrawOrder.UpdateByOrderNO(record);

                    if (model.WithdrawResult == (int)IWithdrawResult.ApprovedMoney)
                    {
                        var wOrder = bt.ZJ_WithdrawOrder.FindByOrderNO(model.OrderNO);
                        if (wOrder != null)
                        {
                            model.UserID = wOrder.UserID;
                            model.WithdrawAmount = wOrder.WithdrawAmount;
                            model.WithdrawCommission = wOrder.WithdrawCommission;
                        }
                        //判断用户余额是否存在数据
                        ZJ_UserBalanceModel zJUserBalanceModel = bt.ZJ_UserBalance.FindByUserID(model.UserID);
                        if (zJUserBalanceModel == null)
                        {
                            bt.Rollback();
                            result.IsValid = false;
                            result.Messages = new List<string>() { "系统中不存在用户余额信息" };
                            return result;
                        }
                        if ((zJUserBalanceModel.ConsumeBalance - model.WithdrawAmount - model.WithdrawCommission) < 0)
                        {
                            bt.Rollback();
                            result.IsValid = false;
                            result.Messages = new List<string>() { "手续费用不能大于用户提现后的余额" };
                            return result;
                        }
                        //新增用户账户金额异动
                        ZJ_UserBalanceChangeLogModel ulogModel = new ZJ_UserBalanceChangeLogModel();
                        ulogModel.UserID = model.UserID;
                        ulogModel.AddOrCutAmount = -(model.WithdrawAmount.Value+model.WithdrawCommission.Value);//变动金额 = 提现金额 + 手续费
                        ulogModel.IsAddOrCut = 0;
                        ulogModel.OldAmount = zJUserBalanceModel.ConsumeBalance;
                        ulogModel.NewAmount = zJUserBalanceModel.ConsumeBalance - model.WithdrawAmount.Value - model.WithdrawCommission.Value;
                        ulogModel.AddOrCutType = 3;
                        ulogModel.OrderNo = model.OrderNO;
                        ulogModel.Remark = model.Remark;
                        ulogModel.IsDisplay = 1;
                        ulogModel.CreateBy = model.Verifier;
                        ulogModel.CreateDT = DateTime.Now;
                        new ZJ_UserBalanceServiceWeb().UpdateZJ_UserBalance(zJUserBalanceModel, ulogModel,bt);
                    }
                    bt.Commit();
                    result.Data = upWithdrawOrder;
                }
                catch (Exception ex)
                {
                    //todo错误日志记录
                    bt.Rollback();
                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// 新增提现订单
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="withdrawAmount">提现金额</param>
        /// <param name="OrderSource">提现来源</param>
        /// <returns>返回true时,表示新增成功；反之,表示新增失败</returns>
        /// <remarks>added by jimmy,2015-7-21</remarks>
        public ResultModel AddZJ_WithdrawOrder(long userId, decimal withdrawAmount, IOrderSource OrderSource)
        {
            var result = new ResultModel();
            //事务开始进行
            using (var bt = this._database.Db.BeginTransaction())
            {
                try
                {
                    //判断用户余额是否存在数据
                    ZJ_UserBalanceModel zJUserBalanceModel = bt.ZJ_UserBalance.FindByUserID(userId);
                    if (zJUserBalanceModel == null)
                    {
                        bt.Rollback();
                        result.IsValid = false;
                        result.Messages = new List<string>() { "M01" };
                        return result;
                    }
                    //判断提现余额是否大于用户余额,如果大于,则回滚
                    if (zJUserBalanceModel.ConsumeBalance < withdrawAmount)
                    {
                        bt.Rollback();
                        result.IsValid = false;
                        result.Messages = new List<string>() { "M02" };
                        return result;
                    }
                    //获取用户银行账户信息
                    dynamic bank;
                    List<YH_UserBankAccountModel> userbankList = bt.YH_UserBankAccount.FindAll(bt.YH_UserBankAccount.UserID == userId && bt.YH_UserBankAccount.IsUse == 1).
                        LeftJoin(bt.BD_Bank, out bank).On(bt.YH_UserBankAccount.BankID == bank.BankID).
                        Select(
                        bt.YH_UserBankAccount.BankID,
                        bt.YH_UserBankAccount.BankAccount,
                        bt.YH_UserBankAccount.BankSubbranch,
                        bt.YH_UserBankAccount.BankUserName,
                        bank.BankName
                        );
                    if (userbankList == null || userbankList.Count == 0)
                    {
                        bt.Rollback();
                        result.IsValid = false;
                        result.Messages = new List<string>() { "M03" };//M03表示用户不存在银行账户信息
                        return result;
                    }


                    //新增提现订单
                    dynamic recordWithdraw = new SimpleRecord();
                    long OrderNo = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    recordWithdraw.OrderNO = OrderNo;
                    recordWithdraw.UserID = userId;
                    recordWithdraw.WithdrawAmount = withdrawAmount;
                    recordWithdraw.WithdrawCommission = 0;
                    recordWithdraw.WithdrawDT = DateTime.Now;
                    recordWithdraw.BankAccount = userbankList[0].BankAccount;
                    recordWithdraw.BankName = userbankList[0].BankName;
                    recordWithdraw.BankSubbranch = userbankList[0].BankSubbranch;
                    recordWithdraw.BankUserName = userbankList[0].BankUserName;
                    recordWithdraw.IsDisplay = 1;
                    recordWithdraw.WithdrawResult = (int)IWithdrawResult.ToAudit;
                    recordWithdraw.OrderSource = (int)OrderSource;

                    bt.ZJ_WithdrawOrder.Insert(recordWithdraw);

                    bt.Commit();
                    result.Data = OrderNo;
                }
                catch (Exception ex)
                {
                    //todo错误日志记录

                    bt.Rollback();
                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                    throw;
                }
            }
            return result;
        }
    }
}
