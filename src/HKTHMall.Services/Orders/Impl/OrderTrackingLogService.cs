using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Core.Extensions;
using HKTHMall.Core.Sql;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.Orders;

namespace HKTHMall.Services.Orders.Impl
{
    /// <summary>
    /// 订单日志跟踪服务类
    /// </summary>
    public class OrderTrackingLogService :BaseService,IOrderTrackingLogService
    {

        /// <summary>
        /// 生成订单跟踪日志插入sql语句
        /// </summary>
        /// <param name="view">日志实体</param>
        /// <returns>sql语句</returns>
        internal string GenerateInsertSql(Domain.WebModel.Models.Orders.OrderTrackingLogView view)
        {
            string sql =
                string.Format(" INSERT INTO dbo.OrderTrackingLog (OrderID,OrderStatus,TrackingContent,CreateTime ,CreateBy) VALUES  ('{0}',{1},'{2}','{3}','{4}')",
                    SqlFilterUtil.ReplaceSqlChar(view.OrderID),
                    view.OrderStatus,
                    SqlFilterUtil.ReplaceSqlChar(view.TrackingContent),
                    view.CreateTime.DateTimeToString(),
                    SqlFilterUtil.ReplaceSqlChar(view.CreateBy)
                    );
            return sql;
        }

        /// <summary>
        /// 增加订单跟踪数据
        /// zhoub 20150902
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddOrderTrackingLog(OrderTrackingLogModel model)
        {
            ResultModel result = new ResultModel();
            result.Data = base._database.Db.OrderTrackingLog.Insert(model);
            return result;
        }

        /// <summary>
        /// 黄主霞 2015-01-19
        /// 获取订单跟踪记录
        /// </summary>
        /// <param name="DetailsId">订单明细ID</param>
        /// <returns></returns>
        public ResultModel GetLogByDetaisId(long DetailsId)
        {
            var tbLog=base._database.Db.OrderTrackingLog;
            var tbDetails= base._database.Db.OrderDetails;
            var result = new ResultModel
            {
                Data = tbLog.Query()
                .Join(tbDetails)
                .On(tbLog.OrderID == tbDetails.OrderID)
                .Where(tbDetails.OrderDetailsID=DetailsId)
                .Select(tbLog.OrderStatus, tbLog.OrderID)
            };
            return result;
        }
    }
}
