using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    public class OrderDetailsViewT1
    {
        public long OrderDetailsID { get; set; }
        public string OrderID { get; set; }
        public string ProductName { get; set; }
        public Nullable<long> ProductSnapshotID { get; set; }
        public long ProductId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalesPrice { get; set; }
        public string DiscountInfo { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public long SKU_ProducId { get; set; }
        public string SkuName { get; set; }
        public decimal SubTotal { get; set; }
        public Nullable<int> Iscomment { get; set; }
        public Nullable<int> IsReturn { get; set; }
        public int? RebateDays { get; set; }
        public decimal? RebateRatio { get; set; }
        /// <summary>
        /// 供应商Id
        /// </summary>
        public long SupplierId { get; set; }

    }
}
