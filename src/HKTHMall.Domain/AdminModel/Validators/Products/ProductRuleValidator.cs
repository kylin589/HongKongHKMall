using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.Products;
using FluentValidation.Validators;

namespace HKTHMall.Domain.AdminModel.Validators.Products
{
    public class ProductRuleValidator : AbstractValidator<ProductRuleModel>
    {
        public ProductRuleValidator()
        {
            RuleFor(x => x.ProductId).NotNull().GreaterThan(0).WithMessage("Please enter product number greater than 0");
            //RuleFor(x => x.SalesRuleId).NotEmpty().WithMessage("请输入促销类型");
            //RuleFor(x => x.PrdoctRuleName).NotEmpty().WithMessage("请输入促销信息");
            //RuleFor(x => x.BuyQty).NotEmpty().WithMessage("请输入买多少件").GreaterThan(0).WithMessage("请输入大于0的购买件数");
            //RuleFor(x => x.SendQty).NotEmpty().WithMessage("请输入送多少件").GreaterThan(0).WithMessage("请输入大于0的送件数");
           // RuleFor(x => x.Discount).NotEmpty().WithMessage("请输入折扣").GreaterThan(0).WithMessage("请输入大于0的折扣");
            //RuleFor(x => x.Price).NotEmpty().WithMessage("请输入折扣价").GreaterThan(0).WithMessage("请输入大于0的折扣价");
            //RuleFor(x => x.StarDate).NotEmpty().WithMessage("请输入开始时间");
            //RuleFor(x => x.EndDate).NotEmpty().WithMessage("请输入结束时间");
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.EndDate).WithName("End time must later than start time"); 
        }
    }
}