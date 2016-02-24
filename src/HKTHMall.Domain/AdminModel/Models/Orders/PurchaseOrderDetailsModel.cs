using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    public class PurchaseOrderDetailsModel
    {
        public long PurchaseOrderDetailsId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalesPrice { get; set; }
        public string DiscountInfo { get; set; }
        public int Quantity { get; set; }
        public int returnedQty { get; set; }
        public int RealQty { get; set; }
        public string Unit { get; set; }
        public long SKU_ProducId { get; set; }
        public string SkuName { get; set; }
        public long OrderDetailsID { get; set; }
        /// <summary>
        /// 采购金额
        /// </summary>
        public decimal PurchaseAmount { get; set; }

        /// <summary>
        /// 小计
        /// </summary>
        public decimal Subtotal { get; set; }

        public string PurchaseOrderId { get; set; }
    }
}
