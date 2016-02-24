using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.Products;

namespace HKTHMall.Domain.AdminModel.Validators.Products
{
    /// <summary>
    /// 广告促销验证类
    /// </summary>
    public class SalesProductValidator:AbstractValidator<SalesProductModel>
    {
        public SalesProductValidator() {
            //RuleFor(x => x.PlaceCode).NotEmpty().WithMessage("请选择位置（分类）");
            //RuleFor(x => x.IdentityStatus).NotEmpty().WithMessage("请选择标识");
            //RuleFor(x => x.Sorts).NotNull().WithMessage("请输入排序").GreaterThanOrEqualTo(0).WithMessage("请输入大于或等于0的数字排序");
            RuleFor(x => x.productId).NotNull().GreaterThan(0).WithMessage("Please enter more than 0 of the number of goods");
            RuleFor(x => x.PicAddress).NotEmpty().WithMessage("Select a picture");
        }
    }
}
