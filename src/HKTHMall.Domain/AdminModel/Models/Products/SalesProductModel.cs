using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Products
{
    /// <summary>
    /// 广告促销商品模型
    /// </summary>
    [Validator(typeof(SalesProductValidator))]
    public class SalesProductModel
    {
        public SalesProductModel() {
            this.SKU_ProductModels = new List<HKTHMall.Domain.AdminModel.Models.Products.ProductModel.AddSKU_ProductModel>();
        }
        public long SalesProductId { get; set; }
        public long productId { get; set; }
        public int PlaceCode { get; set; }
        public int IdentityStatus { get; set; }
        public long Sorts { get; set; }
        public string PicAddress { get; set; }
        public string CreateBy { get; set; }

        public string ProductName { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }

        public decimal SalePrice { get; set; }
        public decimal HKPrice { get; set; }

        public decimal Discount { get; set; }

        public int Status { get; set; }

        public int SalesRuleId { get; set; }

        public Nullable<System.DateTime> StarDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }

        public virtual IList<HKTHMall.Domain.AdminModel.Models.Products.ProductModel.AddSKU_ProductModel> SKU_ProductModels { get; set; }
    }
}
