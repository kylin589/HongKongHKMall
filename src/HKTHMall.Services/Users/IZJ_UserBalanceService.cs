using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public interface IZJ_UserBalanceService : IDependency
    {
        /// <summary>
        /// 获取用户余额信息表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>退换用户余额信息表</returns>
        ResultModel GetZJ_UserBalanceList(SearchZJ_UserBalanceModel model);

        /// <summary>
        /// 添加用户余额信息表
        /// </summary>
        /// <param name="model">退换用户余额信息表</param>
        /// <returns>是否成功</returns>
        ResultModel AddZJ_UserBalance(ZJ_UserBalanceModel model);

        /// <summary>
        /// 更新用户余额信息表（只修改账户状态）
        /// </summary>
        /// <param name="model">用户余额信息表模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateZJ_UserBalance(ZJ_UserBalanceModel model);

        /// <summary>
        /// 更新用户余额信息表（后台余额充值）
        /// </summary>
        /// <param name="model">用户余额信息表模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateZJ_UserBalance(ZJ_UserBalanceModel model,ZJ_UserBalanceChangeLogModel ulogModel);

        /// <summary>
        /// 更新用户余额信息表（后台余额充值）
        /// </summary>
        /// <param name="model">用户余额信息表模型</param>
        /// <returns>是否修改成功</returns>
        void UpdateZJ_UserBalances(ZJ_UserBalanceModel model, ZJ_UserBalanceChangeLogModel ulogModel, dynamic tx);

        /// <summary>
        /// 获取用户余额信息表
        /// </summary>
        /// <param name="UserID">用户Id</param>
        /// <returns>返回获取用户余额信息表</returns>
        /// <remarks>added by jimmy,2015-7-21</remarks>
        ResultModel GetZJ_UserBalanceById(long UserID);
    }
}
