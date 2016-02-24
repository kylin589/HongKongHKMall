using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Orders;

namespace HKTHMall.Services.Orders
{
    /// <summary>
    /// 订单跟踪服务接口
    /// </summary>
    public interface IOrderTrackingLogService : IDependency
    {
        /// <summary>
        /// 增加订单跟踪数据
        /// zhoub 20150902
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddOrderTrackingLog(OrderTrackingLogModel model);
        /// <summary>
        /// 黄主霞 2015-01-19
        /// 获取订单跟踪记录
        /// </summary>
        /// <param name="DetailsId">订单明细ID</param>
        /// <returns></returns>
        ResultModel GetLogByDetaisId(long DetailsId);
    }
}
