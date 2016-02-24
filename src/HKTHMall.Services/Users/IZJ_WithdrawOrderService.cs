using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    /// <summary>
    /// 提现订单接口
    /// <remarks>added by jimmy,2015-7-20</remarks>
    /// </summary>
    public interface IZJ_WithdrawOrderService : IDependency
    {
        /// <summary>
        /// 通过Id查询提现订单
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-20</remarks>
        ResultModel GetZJ_WithdrawOrderById(string  id);

        /// <summary>
        /// 提现订单分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-20</remarks>
        ResultModel Select(SearchZJ_WithdrawOrderModel model);

        /// <summary>
        /// 更新提现订单
        /// </summary>
        /// <param name="model">提现订单对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-20</remarks>
        ResultModel Update(ZJ_WithdrawOrderModel model);

        /// <summary>
        /// 新增提现订单
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="withdrawAmount">提现金额</param>
        /// <param name="OrderSource">提现来源</param>
        /// <returns>返回true时,表示新增成功；反之,表示新增失败</returns>
        /// <remarks>added by jimmy,2015-7-21</remarks>
        ResultModel AddZJ_WithdrawOrder(long userId, decimal withdrawAmount, IOrderSource OrderSource);
    }
}
