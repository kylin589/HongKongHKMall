using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.Products;

namespace HKTHMall.Domain.AdminModel.Validators.Products
{
    public class SalesRuleValidator : AbstractValidator<SalesRuleModel>
    {
        public SalesRuleValidator()
        {
            //RuleFor(x => x.RuleName).NotEmpty().WithMessage("请输入促销名称");

            RuleFor(x => x.RuleName).NotEmpty();

        }
    }
}
