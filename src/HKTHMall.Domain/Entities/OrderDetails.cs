//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HKTHMall.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetails
    {
        public OrderDetails()
        {
            this.OrderDetails_lang = new HashSet<OrderDetails_lang>();
        }
    
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
        public Nullable<long> SupplierId { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual ICollection<OrderDetails_lang> OrderDetails_lang { get; set; }
    }
}
