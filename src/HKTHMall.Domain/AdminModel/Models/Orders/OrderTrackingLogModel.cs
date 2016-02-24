using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    /// <summary>
    /// 订单跟踪信息
    /// zhoub 20150721
    /// </summary>
    public class OrderTrackingLogModel
    {
        public long OrderTrackingId { get; set; }
        public string OrderID { get; set; }
        public int OrderStatus { get; set; }
        public string TrackingContent { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateBy { get; set; }
    }
}
