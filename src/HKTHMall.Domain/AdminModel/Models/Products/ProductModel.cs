using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.AdminModel.Validators.Products;
using HKTHMall.Domain.Models.Bra;

namespace HKTHMall.Domain.AdminModel.Models.Products
{
    public class ProductPagedListModel : ProductModel
    {
        public string CreateBy { get; set; }
        public DateTime? CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDT { get; set; }
    }

    [Validator(typeof (ProductValidator))]
    public class ProductModel
    {
        public ProductModel()
        {
            this.PrdoctRuleModels = new List<ProdctRuleModel>();
            this.Product_LangModels = new List<Product_LangModel>();
            this.ProductPicModels = new List<ProductPicModel>();
            this.ProductRuleModels = new List<ProductRuleModel>();
            this.SKU_ProductModels = new List<SKU_ProductModel>();
            this.SKU_SKUItemsModels = new List<SKU_SKUItemsModel>();
            this.SP_ProductCommentModels = new List<SP_ProductCommentModel>();
            this.CategoryLanguageModel = new CategoryLanguageModel();
            this.Brand_LangModel = new Brand_LangModel();
            this.SKU_ProductAttributesModels = new List<SKU_ProductAttributesModel>();
            this.ProductParametersModels = new List<ProductParameters>();
        }

        /// <summary>
        ///     产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///     分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        ///     品牌名称
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        ///     是否推荐
        /// </summary>
        public bool RewriteIsRecommend
        {
            get { return this.IsRecommend == 1; }
            set { this.IsRecommend = value ? 1 : 0; }
        }

        public long? ProductId { get; set; }
        public int CategoryId { get; set; }
        public long MerchantID { get; set; }
        public long? FareTemplateID { get; set; }
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
        public int? RebateDays { get; set; }
        public decimal? RebateRatio { get; set; }
        public decimal? MarketPrice { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? ActivityBottomPrice { get; set; }
        public int SaleCount { get; set; }
        public int? NotifyAdminForQuantityBelow { get; set; }
        public DateTime? PutawayDT { get; set; }
        public virtual int IsRecommend { get; set; }
        public int? RecommendSort { get; set; }
        //public byte? IsDelete { get; set; }
        public string ExtensionPropertiesText { get; set; }
        public decimal? Volume { get; set; }
        public int BrandID { get; set; }
        public Nullable<long> SupplierId { get; set; }
        public string SupplierName { get; set; }
        public bool FreeShipping { get; set; }
        public virtual IList<ProdctRuleModel> PrdoctRuleModels { get; set; }
        public virtual IList<Product_LangModel> Product_LangModels { get; set; }
        public Product_LangModel GetProduct_LangModel(int lang)
        {
            if(Product_LangModels==null)
            {
                return new Product_LangModel();
            }
            foreach(Product_LangModel m in Product_LangModels)
            {
                if (m.LanguageID == lang)
                    return m;
            }
            return new Product_LangModel();
        }
        public virtual IList<ProductPicModel> ProductPicModels { get; set; }
        public virtual IList<ProductRuleModel> ProductRuleModels { get; set; }
        public virtual IList<SKU_ProductModel> SKU_ProductModels { get; set; }
        public virtual IList<SKU_SKUItemsModel> SKU_SKUItemsModels { get; set; }
        public virtual IList<SP_ProductCommentModel> SP_ProductCommentModels { get; set; }
        public virtual IList<SKU_ProductAttributesModel> SKU_ProductAttributesModels { get; set; }
        public IList<ProductParameters> ProductParametersModels { get; set; }
        public ProdctRuleModel ProdctRuleModel { get; set; }
        //public virtual SelectList CategorySelectList1 { get; set; }
        //public virtual SelectList CategorySelectList2 { get; set; }
        //public virtual SelectList CategorySelectList3 { get; set; }
        //public virtual SelectList BrandSelectList { get; set; }

        public virtual CategoryLanguageModel CategoryLanguageModel { get; set; }
        public virtual Brand_LangModel Brand_LangModel { get; set; }

        [Validator(typeof (ProductLanguageValidator))]
        public class Product_LangModel
        {
            public int Id { get; set; }
            public long ProductId { get; set; }
            public string ProductName { get; set; }
            public string Subheading { get; set; }
            public string Introduction { get; set; }
            public string SalesService { get; set; }
            public int LanguageID { get; set; }
        }

        public class ProductPicModel
        {
            public long ProductPicId { get; set; }
            public long ProductID { get; set; }
            public string PicUrl { get; set; }
            public int Flag { get; set; }
            public long sort { get; set; }
        }

        public class ProductRuleModel
        {
        }

        public class AddSKU_ProductModel : SKU_ProductModel
        {
            public string CreateBy { get; set; }
            public DateTime? CreateDT { get; set; }
            public decimal Discount { get; set; }

        }
      
        public class UpdateSKU_ProductModel : SKU_ProductModel
        {
            public string UpdateBy { get; set; }
            public DateTime? UpdateDT { get; set; }
        }

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
            public decimal PurchasePrice { get; set; }

            public bool IsUseBool
            {
                get { return this.IsUse == 1; }
                set { this.IsUse = value ? 1 : 0; }
            }

            public int IsUse { get; set; }
        }

        public class SKU_SKUItemsModel
        {
            public long SKU_SKUItemsId { get; set; }
            public long ProductId { get; set; }
            public long AttributeId { get; set; }
            public long ValueId { get; set; }
            public string ValueStr { get; set; }
            public string AttributeGroup { get; set; }
        }

        public class SP_ProductCommentModel
        {
        }

        public class ProductParameters
        {
            public long? ParametersId { get; set; }
            public long ProductId { get; set; }
            public string PName { get; set; }
            public string PValue { get; set; }
            public string GroupName { get; set; }
            public long Sort { get; set; }
        }
    }

    public class ProdctRuleModel
    {
        public long ProductRuleId { get; set; }
        public long ProductId { get; set; }
        public int SalesRuleId { get; set; }
        public string PrdoctRuleName { get; set; }
        public int BuyQty { get; set; }
        public int SendQty { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class SearchResultModel : ProductModel
    {
        public string CreateBy { get; set; }
        public DateTime? CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDT { get; set; }
        public IList<string> AttrJsonStrs { get; set; }
    }

    //[Validator(typeof(AddProductValidator))]
    public class AddProductModel : ProductModel
    {
        public AddProductModel()
        {
            this.CategoryModels = new List<ResultCategoryModel>();
            this.BrandModels = new List<BrandModel>();
            this.Product_LangModels = new List<Product_LangModel>();
            this.AttrJsonStrs = new List<string>();
            this.SKU_SKUItemsModels = new List<AddSKU_SKUItemsModel>();
            this.SKU_ProductModels = new List<AddSKU_ProductModel>();
            this.SKU_ProductAttributesModels = new List<AddSKU_ProductAttributesModel>();
            this.ProdctRuleModel = new ProdctRuleModel
            {
                StarDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };
        }

        public string CreateBy { get; set; }
        public DateTime? CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDT { get; set; }
        public decimal Discount { get; set; }
        

        /// <summary>
        ///     商品分类
        /// </summary>
        public List<ResultCategoryModel> CategoryModels { get; set; }

        /// <summary>
        ///     品牌
        /// </summary>
        public List<BrandModel> BrandModels { get; set; }

        /// <summary>
        ///     商品Sku
        /// </summary>
        public new IList<AddSKU_ProductModel> SKU_ProductModels { get; set; }

        public new IList<AddSKU_SKUItemsModel> SKU_SKUItemsModels { get; set; }

        public new IList<AddSKU_ProductAttributesModel> SKU_ProductAttributesModels { get; set; }
        public IList<string> AttrJsonStrs { get; set; }
    }

    public class NewProductModel
    {
        public long ProductId { get; set; }
        public int CategoryId { get; set; }
        public long MerchantID { get; set; }
        public long? FareTemplateID { get; set; }
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
        public decimal? MarketPrice { get; set; }
        public decimal? PurchasePrice { get; set; }
        public int? RebateDays { get; set; }
        public decimal? RebateRatio { get; set; }
        public decimal? ActivityBottomPrice { get; set; }
        public int SaleCount { get; set; }
        public int? NotifyAdminForQuantityBelow { get; set; }
        public DateTime? PutawayDT { get; set; }
        public int? IsRecommend { get; set; }
        public int? RecommendSort { get; set; }
        public bool IsDelete { get; set; }
        public string ExtensionPropertiesText { get; set; }
        public decimal? Volume { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDT { get; set; }
        public int BrandID { get; set; }
        public long SupplierId { get; set; }
        public bool FreeShipping { get; set; }
    }

    public class AddSKU_SKUItemsModel : SKU_SKUItemsModel
    {
        public string CreateBy { get; set; }
        public DateTime? CreateDT { get; set; }
    }

    public class SKU_SKUItemsModel
    {
        public long? SKU_SKUItemsId { get; set; }
        public long ProductId { get; set; }
        public long AttributeId { get; set; }
        public long ValueId { get; set; }
        public string ValueStr { get; set; }
        public string AttributeGroup { get; set; }
    }

    public class AddSKU_ProductAttributesModel : SKU_ProductAttributesModel
    {
        public string CreateBy { get; set; }
        public DateTime? CreateDT { get; set; }
    }

    public class SKU_ProductAttributesModel
    {
        public long SKU_ProductAttributesId { get; set; }
        public long ProductId { get; set; }
        public long AttributeId { get; set; }
        public long ValueId { get; set; }
        public string ValueStr { get; set; }
        public string ImageUrl { get; set; }
        public int AttributeType { get; set; }
        public string AttributeGroup { get; set; }
    }

    public class UpdateProductModel : ProductModel
    {
        public UpdateProductModel()
        {
            this.Product_LangModels = new List<Product_LangModel>();
            this.AttrJsonStrs = new List<string>();
            this.SKU_SKUItemsModels = new List<UpdateSKU_SKUItemsModel>();
            this.SKU_ProductModels = new List<UpdateSKU_ProductModel>();
            this.SKU_ProductAttributesModels = new List<UpdateSKU_ProductAttributesModel>();
        }

        public string UpdateBy { get; set; }
        public DateTime? UpdateDT { get; set; }
        public Nullable<long> SupplierId { get; set; }
        /// <summary>
        ///     商品Sku
        /// </summary>
        public new IList<UpdateSKU_ProductModel> SKU_ProductModels { get; set; }
        public new IList<UpdateSKU_SKUItemsModel> SKU_SKUItemsModels { get; set; }
        public new IList<UpdateSKU_ProductAttributesModel> SKU_ProductAttributesModels { get; set; }
        public IList<string> AttrJsonStrs { get; set; }
    }

    public class UpdateSKU_SKUItemsModel : SKU_SKUItemsModel
    {
        public string UpdateBy { get; set; }
        public DateTime? UpdateDT { get; set; }
    }

    public class UpdateSKU_ProductAttributesModel : SKU_ProductAttributesModel
    {
        public string UpdateBy { get; set; }
        public DateTime? UpdateDT { get; set; }
    }
}