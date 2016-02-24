using BrCms.Core.Infrastructure;

using HKTHMall.Domain.CashBack;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    /// <summary>
    /// 返现服务类
    /// </summary>
    public interface IZJ_RebateService : IDependency
    {
        /// <summary>
        /// 获取返现记录
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        ResultModel GetList(SearchRebate model);
        /// <summary>
        /// 生成返现订单
        /// </summary>
        /// <returns></returns>
        BackMessage GenerateList(OderModel model);
           /// <summary>
        /// 返现操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void CashBackOrder();

        /// <summary>
        /// 获取剩余返现资金
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        ResultModel GetSurplusRebeatAmount(long UserId);

        /// <summary>
        /// 获取返现列表
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="Lang">语言</param>
        /// <returns></returns>
        ResultModel GetRebeatAmountList(long UserID, int PageIndex, int PageSize, int Lang = 4);
        /// <summary>
        /// 根据用户ID获取总数
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        ResultModel GetCountByUserID(long UserID);

        /// <summary>
        /// 获取已返金额
        /// </summary>
        /// <param name="UserID">获取已返金额</param>
        /// <returns></returns>
        ResultModel GetPaidAmount(long UserID);

        /// <summary>
        /// 获取总金额
        /// </summary>
        /// <param name="UserID">用户</param>
        /// <returns></returns>
        ResultModel GetTotalAmount(long UserID);

        /// <summary>
        /// 获取每日应返还金额
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        ResultModel GetRebateEveryDay(long UserID);
    }
}
