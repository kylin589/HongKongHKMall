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
    
    public partial class Product
    {
        public Product()
        {
            this.Product_Lang = new HashSet<Product_Lang>();
            this.ProductPic = new HashSet<ProductPic>();
            this.ProductRule = new HashSet<ProductRule>();
            this.SKU_Product = new HashSet<SKU_Product>();
            this.SKU_SKUItems = new HashSet<SKU_SKUItems>();
            this.SP_ProductComment = new HashSet<SP_ProductComment>();
        }
    
        public long ProductId { get; set; }
        public int CategoryId { get; set; }
        public long MerchantID { get; set; }
        public Nullable<long> FareTemplateID { get; set; }
        public string ArtNo { get; set; }
        public string ProductBarcode { get; set; }
        public int Status { get; set; }
        public decimal PostagePrice { get; set; }
        public int StockQuantity { get; set; }
        public bool AllowCustomerReviews { get; set; }
        public bool AllowBackInStockSubscriptions { get; set; }
        public bool IsProvideInvoices { get; set; }
        public decimal Weight { get; set; }
        public string ProductParameter { get; set; }
        public string PackingList { get; set; }
        public decimal HKPrice { get; set; }
        public Nullable<decimal> MarketPrice { get; set; }
        public Nullable<decimal> PurchasePrice { get; set; }
        public Nullable<decimal> ActivityBottomPrice { get; set; }
        public int SaleCount { get; set; }
        public Nullable<int> NotifyAdminForQuantityBelow { get; set; }
        public Nullable<System.DateTime> PutawayDT { get; set; }
        public Nullable<int> IsRecommend { get; set; }
        public Nullable<int> RecommendSort { get; set; }
        public bool IsDelete { get; set; }
        public string ExtensionPropertiesText { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
        public int BrandID { get; set; }
        public Nullable<long> SupplierId { get; set; }
    
        public virtual ICollection<Product_Lang> Product_Lang { get; set; }
        public virtual ICollection<ProductPic> ProductPic { get; set; }
        public virtual ICollection<ProductRule> ProductRule { get; set; }
        public virtual ICollection<SKU_Product> SKU_Product { get; set; }
        public virtual ICollection<SKU_SKUItems> SKU_SKUItems { get; set; }
        public virtual ICollection<SP_ProductComment> SP_ProductComment { get; set; }
    }
}
