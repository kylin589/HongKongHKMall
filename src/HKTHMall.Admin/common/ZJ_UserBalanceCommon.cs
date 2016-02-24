using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HKTHMall.Services.Users;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Core;

namespace HKTHMall.Admin.common
{
    public static class ZJ_UserBalanceCommon
    {
        private static ZJ_UserBalanceService _zjUserBalanceService = new ZJ_UserBalanceService();

        /// <summary>
        /// 后台余额充值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool UpdateZJ_UserBalance(ZJ_UserBalanceModel model)
        {
            bool bl = false;
            if ( model.UserID != 0 && model.AddOrCutType !=0)
            {
                #region 这里重新查询用户余额是为防止数据库已更新余额
                //ZJ_UserBalanceModel Newmodel = new ZJ_UserBalanceModel();
                //SearchZJ_UserBalanceModel smodel = new SearchZJ_UserBalanceModel();
                //smodel.UserID = model.UserID;
                //smodel.PagedIndex = 0;
                //smodel.PagedSize = 100;
                ////查询列表 

                //List<ZJ_UserBalanceModel> List = _zjUserBalanceService.GetZJ_UserBalanceList(smodel).Data;

                //if (List != null && List.Count > 0)
                //{
                //    Newmodel = List[0];
                //}
                //else
                //{
                //    return bl=false;
                //}
                #endregion

                //ZJ_UserBalanceChangeLog【用户账户金额异动记录表】(资金流水账)
                #region 给流水账添加信息
                ZJ_UserBalanceChangeLogModel ulogModel = new ZJ_UserBalanceChangeLogModel();
                ulogModel.Account = model.Account;
                ulogModel.AddOrCutAmount = model.AddOrCutAmount;
                ulogModel.AddOrCutType = model.AddOrCutType;
                ulogModel.CreateBy = UserInfo.CurrentUserName;
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
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;

                bl = _zjUserBalanceService.UpdateZJ_UserBalance(model,ulogModel).IsValid;
            }

            return bl;
        }
    }
}