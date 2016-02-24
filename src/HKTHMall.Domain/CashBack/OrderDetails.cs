using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.CashBack
{
    public  class OrderDetails
    {
        /// <summary>
        /// 订单明细ID
        /// </summary>
        public long OrderDetailsID { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品快照ID
        /// </summary>
        public Nullable<long> ProductSnapshotID { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalesPrice { get; set; }
        /// <summary>
        /// 优惠信息
        /// </summary>
        public string DiscountInfo { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// SKUId
        /// </summary>
        public long SKU_ProducId { get; set; }
        /// <summary>
        ///  Sku名称（例如：尺码37）
        /// </summary>
        public string SkuName { get; set; }
        /// <summary>
        /// 商品小计
        /// </summary>
        public decimal SubTotal { get; set; }
        /// <summary>
        /// 评论状态 0未评价，1已评价
        /// </summary>
        public Nullable<int> Iscomment { get; set; }
        /// <summary>
        /// 退货状态 0正常，1退款申请中，2已退款，3审核未通过）
        /// </summary>
        public Nullable<int> IsReturn { get; set; }
        /// <summary>
        /// SupplierId
        /// </summary>
        public Nullable<long> SupplierId { get; set; }
        /// <summary>
        /// 返利天数
        /// </summary>
        public Nullable<int> RetateDays { get; set; }
        /// <summary>
        /// 返利比率（1=100%，0.9=90%）
        /// </summary>
        public Nullable<decimal> ReateRedio { get; set; }
    }
}
