using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.SKU
{
    public class SKU_ProductView
    {
        public long SKU_ProducId { get; set; }
        public long ProductId { get; set; }
        public string SKUStr { get; set; }
        public string ProductCode { get; set; }
        public string SkuName { get; set; }
        public int Stock { get; set; }
        public decimal HKPrice { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public int IsUse { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
    }
}
