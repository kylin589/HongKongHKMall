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
    public interface IZJ_UserBalanceChangeLogService : IDependency
    {
        /// <summary>
        /// 获取用户账户金额异动记录表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        ResultModel GetZJ_UserBalanceChangeLogList(SearchZJ_UserBalanceChangeLogModel model);

        /// <summary>
        /// 添加用户账户金额异动记录表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>是否成功</returns>
        ResultModel AddZJ_UserBalanceChangeLog(ZJ_UserBalanceChangeLogModel model);

        /// <summary>
        /// 更新用户账户金额异动记录表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateZJ_UserBalanceChangeLog(ZJ_UserBalanceChangeLogModel model);

        #region 获取资金异动记录Web

        /// <summary>
        /// 获取用户资金异动记录ywd
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        ResultModel GetUserBalanceChangeLogList(SearchZJ_UserBalanceChangeLogModel model);

        #endregion 获取资金异动记录Web

        #region 惠粉相关接口
        #region 获取惠粉消费总收益
        /// <summary>
        /// 获取惠粉消费总收益
        /// </summary>
        /// <param name="model">用户Id</param>
        /// <returns></returns>
        ResultModel GetSellIncome(long userId);
        #endregion

        #region 获取账户账单记录
        /// <summary>
        /// 获取账户账单记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageNo">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="type">0:收支明细 1.消费记录 2.惠粉收益记录 3.退款</param>
        ///  <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        ResultModel GetCapitalRecordList(long userId, int lang, int type = 0);
        /// <summary>
        /// 获取商品名称
        /// </summary>
        /// <param name="orderNo">订单ID</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        ResultModel GetProductName(string orderNo, int lang);
        /// <summary>
        /// 获取商品图片
        /// </summary>
        /// <param name="orderNo">订单ID</param>
        /// <returns></returns>
        ResultModel GetProductURL(string orderNo);
        #endregion

        #region 获取账户账单记录详情 李霞
        /// <summary>
        /// 获取账户账单记录详情 
        /// </summary>
        /// <param name="id">记录Id</param>
        ///  <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        ResultModel GetCapitalRecordDetails(int id, int lang);



        #region 获取消费收益列表
        /// <summary>
        /// 获取消费收益列表
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="GType">类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉 4.外围惠粉）</param>
        /// <param name="PageNo">分页码</param>
        /// <param name="PageSize">分页大小</param>
        /// <returns></returns>
        ResultModel GetJoinAndConsumeList(long userId, int GType, int PageNo, int PageSize);
        #endregion

        #region 消费收益明细
        /// <summary>
        /// 惠粉消费收益明细
        /// </summary>
        /// <param name="loginId">当前登录人ID</param>
        /// <param name="userId">下级用户id</param>
        /// <param name="gtype">类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉 4.外围惠粉）</param>
        /// <param name="pageNo">分页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        ResultModel GetConsumeDetails(long loginId, long userId, int gtype, int pageNo, int pageSize);
        #endregion
        #endregion
        #endregion

        ResultModel GetConsumeList(long userid, int pageNo, int pageSize);

        /// <summary>
        /// 获取某一天的返现资金
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        ResultModel GetRebeatAmountByDate(long UserId, DateTime dt);

    }
}
