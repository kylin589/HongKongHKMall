using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Sys;
using HKTHMall.Services.Sys;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public class ZJ_UserBalanceService : BaseService, IZJ_UserBalanceService
    {
        /// <summary>
        /// 获取用户余额信息表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>退换用户余额信息表</returns>
        public ResultModel GetZJ_UserBalanceList(SearchZJ_UserBalanceModel model)
        {
            var tb = _database.Db.ZJ_UserBalance;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

            if (!string.IsNullOrEmpty(model.RealName) && model.RealName.Trim() != "")
            {
                //用户真实姓名
                where = new SimpleExpression(where,
                    _database.Db.YH_User.RealName.Like("%" + model.RealName.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Phone) && model.Phone.Trim() != "")
            {
                //用户手机
                where = new SimpleExpression(where, _database.Db.YH_User.Phone.Like("%" + model.Phone.Trim() + "%"),
                    SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Email) && model.Email.Trim() != "")
            {
                //Email
                where = new SimpleExpression(where, _database.Db.YH_User.Email.Like("%" + model.Email.Trim() + "%"),
                    SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Account) && model.Account.Trim() != "")
            {
                //用户真实姓名
                where = new SimpleExpression(where, _database.Db.YH_User.Account.Like("%" + model.Account.Trim() + "%"),
                    SimpleExpressionType.And);
            }
            if (model.UserID > 0)
            {
                //用户ID
                where = new SimpleExpression(where, tb.UserID == model.UserID, SimpleExpressionType.And);
            }

            dynamic pc;
            dynamic zjlog;

            var query = tb
                .Query()
                //.LeftJoin(_database.Db.ZJ_UserBalanceChangeLog, out zjlog)
                //.On(_database.Db.ZJ_UserBalanceChangeLog.UserID == tb.UserID)
                .LeftJoin(_database.Db.YH_User, out pc)
                .On(_database.Db.YH_User.UserID == tb.UserID)
                .Select(
                    tb.UserID,
                    tb.ConsumeBalance,
                    tb.Vouchers,
                    tb.AccountStatus,
                    tb.CreateBy,
                    tb.CreateDT,
                    tb.UpdateBy,
                    tb.UpdateDT,

                    //zjlog.Remark,

                    pc.Phone,
                    pc.RealName,
                    pc.Email,
                    pc.NickName,
                    pc.Account
                )
                .Where(where)
                .OrderByUserIDDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<ZJ_UserBalanceModel>(query,
                    model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 添加用户余额信息表
        /// </summary>
        /// <param name="model">用户余额信息表模型</param>
        /// <returns>是否成功</returns>
        public ResultModel AddZJ_UserBalance(ZJ_UserBalanceModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.ZJ_UserBalance.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 更新用户余额信息表（只修改账户状态）
        /// </summary>
        /// <param name="model">用户余额信息表模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateZJ_UserBalance(ZJ_UserBalanceModel model)
        {
            var result = new ResultModel()
            {
                Data =
                    base._database.Db.ZJ_UserBalance.UpdateByUserID(UserID: model.UserID,
                        AccountStatus: model.AccountStatus, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT)
            };

            result.IsValid = result.Data > 0 ? true : false;

            return result;
        }

        /// <summary>
        /// 更新用户余额信息表（后台余额充值）
        /// </summary>
        /// <param name="model">用户余额信息表模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateZJ_UserBalance(ZJ_UserBalanceModel model, ZJ_UserBalanceChangeLogModel ulogModel)
        {
            var result = new ResultModel();
            using (var tx = _database.Db.BeginTransaction())
            {
                try
                {
                    ZJ_UserBalanceServiceWeb zjweb = new ZJ_UserBalanceServiceWeb();
                    zjweb.UpdateZJ_UserBalance(model, ulogModel, tx);
                    ParameterSetService pss = new ParameterSetService();

                    //下面是给公司账户金额操作的
                    model.UserID = Convert.ToInt64(pss.GetParametePValueById(1215894621).Data);
                    ulogModel.UserID = Convert.ToInt64(pss.GetParametePValueById(1215894621).Data);

                    model.AddOrCutAmount = -model.AddOrCutAmount;
                    ulogModel.AddOrCutAmount = -ulogModel.AddOrCutAmount;
                    ulogModel.IsAddOrCut = ulogModel.IsAddOrCut == 0 ? 1 : 0;
                    if (ulogModel.AddOrCutAmount > 0)
                    {
                        ulogModel.AddOrCutType = 15; //后台充值（正数）
                    }
                    else
                    {
                        ulogModel.AddOrCutType = 16; //后台充值扣款(负数）
                    }
                    zjweb.UpdateZJ_UserBalance(model, ulogModel, tx); //调用统一的修改方法
                    //tx.ZJ_UserBalance.UpdateByUserID(UserID: model.UserID, ConsumeBalance: model.ConsumeBalance, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
                    //tx.ZJ_UserBalanceChangeLog.Insert(ulogModel);
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();

                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// 更新用户余额信息表（后台余额充值）
        /// </summary>
        /// <param name="model">用户余额信息表模型</param>
        /// <returns>是否修改成功</returns>
        public void UpdateZJ_UserBalances(ZJ_UserBalanceModel model, ZJ_UserBalanceChangeLogModel ulogModel, dynamic tx)
        {

            ZJ_UserBalanceServiceWeb zjweb = new ZJ_UserBalanceServiceWeb();
            zjweb.UpdateZJ_UserBalance(model, ulogModel, tx); //调用统一的修改方法 个人账户
            ParameterSetService pss = new ParameterSetService();

            //下面是给公司账户金额操作的
            model.UserID = Convert.ToInt64(pss.GetParametePValueById(1215894621).Data);
            ulogModel.UserID = Convert.ToInt64(pss.GetParametePValueById(1215894621).Data);

            model.AddOrCutAmount = -model.AddOrCutAmount;
            ulogModel.AddOrCutAmount = -ulogModel.AddOrCutAmount;
            ulogModel.IsAddOrCut = ulogModel.IsAddOrCut == 0 ? 1 : 0;
            if (ulogModel.AddOrCutType <= 0)
            {
                if (ulogModel.AddOrCutAmount > 0)
                {
                    ulogModel.AddOrCutType = 15; //后台充值（正数）
                }
                else
                {
                    ulogModel.AddOrCutType = 16; //后台充值扣款(负数）
                }
            }

            zjweb.UpdateZJ_UserBalance(model, ulogModel, tx); //调用统一的修改方法 公司账户
        }

        /// <summary>
        /// 获取用户余额信息表
        /// </summary>
        /// <param name="UserID">用户Id</param>
        /// <returns>返回获取用户余额信息表</returns>
        /// <remarks>added by jimmy,2015-7-21</remarks>
        public ResultModel GetZJ_UserBalanceById(long UserID)
        {
            var result = new ResultModel()
            {
                Data = _database.Db.ZJ_UserBalance.Find(_database.Db.ZJ_UserBalance.UserID == UserID)
            };
            return result;
        }

        /// <summary>
        /// 购物消费,更新用户余额
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tx"></param>
        internal void UpdateZJ_UserBalance(ZJ_UserBalanceModel model, dynamic tx)
        {

            if (model.UserID != 0 && model.AddOrCutType != 0)
            {


                //ZJ_UserBalanceChangeLog【用户账户金额异动记录表】(资金流水账)

                #region 给流水账添加信息

                ZJ_UserBalanceChangeLogModel ulogModel = new ZJ_UserBalanceChangeLogModel();
                ulogModel.Account = model.Account;
                ulogModel.AddOrCutAmount = model.AddOrCutAmount;
                ulogModel.AddOrCutType = model.AddOrCutType;
                ulogModel.CreateBy = model.CreateBy;
                ulogModel.CreateDT = DateTime.Now;
                if (model.AddOrCutAmount >= 0)
                {
                    ulogModel.IsAddOrCut = 1;
                }
                else
                {
                    ulogModel.IsAddOrCut = 0;
                }
                ulogModel.IsDisplay = model.IsDisplay == 0 ? 0 : model.IsDisplay;
                //ulogModel.NewAmount = model.ConsumeBalance + model.AddOrCutAmount;
                //ulogModel.OldAmount = model.ConsumeBalance;
                ulogModel.OrderNo = model.OrderNo == null ? "" : model.OrderNo;
                ulogModel.Phone = model.Phone;
                ulogModel.RealName = model.RealName;
                ulogModel.Remark = model.Remark;
                ulogModel.UserID = model.UserID;

                #endregion

                //model.ConsumeBalance = model.ConsumeBalance + model.AddOrCutAmount;
                model.UpdateBy = model.CreateBy;
                model.UpdateDT = DateTime.Now;
                ZJ_UserBalanceService _zjUserBalanceService = new ZJ_UserBalanceService();
                _zjUserBalanceService.UpdateZJ_UserBalances(model, ulogModel, tx);
            }


        }
    }
}
