using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    public class PurchaseOrderModel
    {
        public string PurchaseOrderId { get; set; }
        public string OrderID { get; set; }
        public long SupplierId { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal RealPurchaseAmount { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<int> status { get; set; }

        public string Deliveryer { get; set; }
        public DateTime? CancelDate { get; set; }
        public string CancelUser { get; set; }
        public Nullable<DateTime> DeliveryDate { get; set; }


        #region 供应商采购单明细表
        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        public decimal SalesPrice { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 退货数量
        /// </summary>
        public int returnedQty { get; set; }
        /// <summary>
        /// 实际数量
        /// </summary>
        public int RealQty { get; set; }
        /// <summary>
        /// Sku名称
        /// </summary>
        public string SkuName { get; set; }
        #endregion

        #region 供应商表
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        #endregion
        public virtual ICollection<PurchaseOrderDetails> PurchaseOrderDetails { get; set; }

        /// <summary>
        /// 订单详情
        /// zhoub 20150928
        /// </summary>
        public virtual List<OrderDetailsModel> OrderDetailViews { get; set; } 
    }
}
