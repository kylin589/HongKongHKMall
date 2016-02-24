
namespace HKTHMall.Domain.WebModel.Models.Orders
{
    public class OrderTrackingLogView
    {
        public long OrderTrackingId { get; set; }
        public string OrderID { get; set; }
        public int OrderStatus { get; set; }
        public string TrackingContent { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string CreateBy { get; set; }

    }
}
