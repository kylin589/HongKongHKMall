using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    public class ProductView
    {
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
    }

    public class ProductPath
    {
        public long ProductId {get;set;}
        public string ProductName { get; set; }
        public int CategoryId1 {get;set;}
        public int CategoryId2 {get;set;}
        public int CategoryId3 {get;set;}
        public string CategoryName1 { get; set; }
        public string CategoryName2 { get; set; }
        public string CategoryName3 { get; set; }
    }
}
