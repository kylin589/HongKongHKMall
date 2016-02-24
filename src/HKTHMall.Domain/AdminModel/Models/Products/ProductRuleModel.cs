using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.Products;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.WebModel.Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Products
{
    /// <summary>
    /// 商品促销信息模型
    /// </summary>
    [Validator(typeof(ProductRuleValidator))]
    public class ProductRuleModel
    {
        public ProductRuleModel()
        {
            this.Product = new Product();
            this.SalesRule = new SalesRule();
            this.SKU_ProductModels = new List<HKTHMall.Domain.AdminModel.Models.Products.ProductModel.AddSKU_ProductModel>();
        }
        public long ProductRuleId { get; set; }
        public long ProductId { get; set; }
        public int SalesRuleId { get; set; }
        public string PrdoctRuleName { get; set; }
        public string ProductName { get; set; }

        public decimal HKPrice { get; set; }
        public int BuyQty { get; set; }
        public int SendQty { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public System.DateTime StarDate { get; set; }
        public System.DateTime EndDate { get; set; }

        public string TempStarDate { get; set; }
        public string TempEndDate { get; set; }

        /// <summary>
        /// 时
        /// </summary>
        public string Hour { get; set; }

        /// <summary>
        /// 分
        /// </summary>
        public int Minute { get; set; }

        public string RuleName { get; set; }

        public virtual Product Product { get; set; }
        public virtual SalesRule SalesRule { get; set; }

        public decimal SalePrice { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>
        public int Status { get; set; }
        public virtual IList<HKTHMall.Domain.AdminModel.Models.Products.ProductModel.AddSKU_ProductModel> SKU_ProductModels { get; set; }


    }
}
