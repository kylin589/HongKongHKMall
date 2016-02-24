using FluentValidation;
using HKTHMall.Domain.Models.banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Validators.banner
{
    public class bannerProductValidator: AbstractValidator<bannerProductModel>
    {
        public bannerProductValidator()
        {
            //RuleFor(x => x.productId).NotEmpty().WithMessage("请输入商品ID或者商品ID不能为0");
            //RuleFor(x => x.PlaceCode).NotEmpty().WithMessage("请选择位置名称");
            //RuleFor(x => x.PicAddress).NotEmpty().WithMessage("请上传图片");

            //RuleFor(x => x.PlaceCode)
            //    .NotEmpty()
            //    .WithMessage("请选择位置")
            //    .GreaterThanOrEqualTo(0)
            //    .WithMessage("请输入大于或等于0的数字");

            RuleFor(x => x.productId).NotEmpty();
            RuleFor(x => x.PicAddress).NotEmpty();
        }
    }
}
