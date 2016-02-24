using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    public class SKU_ProductModel
    {
        public long SKU_ProducId { get; set; }
        public long ProductId { get; set; }
        public string SKUStr { get; set; }
        public string ProductCode { get; set; }
        public string SkuName { get; set; }
        public int Stock { get; set; }
        public decimal HKPrice { get; set; }
        public decimal MarketPrice { get; set; } 
        public int IsUse { get; set; }

    }




    public class SKU_DataInfo
    {
        public long SKU_ProducId { get; set; }
        public string SKUStr { get; set; }  
        public int Stock { get; set; }
        public decimal HKPrice { get; set; }
        public decimal MarketPrice { get; set; }

        public bool IsUse { get; set; }

    }
 
}
