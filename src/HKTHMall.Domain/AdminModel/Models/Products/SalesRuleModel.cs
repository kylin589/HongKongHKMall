using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.Products;
using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Products
{
    /// <summary>
    /// 促销规则模型
    /// </summary>
    [Validator(typeof(SalesRuleValidator))]
    public class SalesRuleModel
    {
        public SalesRuleModel()
        {
            this.ProductRule = new List<ProductRule>();
        }
        public long ProductId { get; set; }
        public int SalesRuleId { get; set; }
        public string RuleName { get; set; }

        public string ProductName { get; set; }

        public virtual ICollection<ProductRule> ProductRule { get; set; }
    }
}
