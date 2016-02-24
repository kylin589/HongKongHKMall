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
    public interface IZJ_RechargeOrderService : IDependency
    {
        /// <summary>
        /// 获取用户余额信息表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>退换用户余额信息表</returns>
        ResultModel GetZJ_RechargeOrderList(SearchZJ_RechargeOrderModel model);

        /// <summary>
        /// 添加用户余额信息表
        /// </summary>
        /// <param name="model">退换用户余额信息表</param>
        /// <returns>是否成功</returns>
        ResultModel AddZJ_RechargeOrder(ZJ_RechargeOrderModel model);
    }
}
