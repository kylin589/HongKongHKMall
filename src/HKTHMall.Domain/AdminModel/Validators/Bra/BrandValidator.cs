using FluentValidation;
using HKTHMall.Domain.Models.Bra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Validators.Bra
{
    public class BrandValidator : AbstractValidator<BrandModel>
    {
        public BrandValidator()
        {
            //RuleFor(x => x.ZhBrandName).NotEmpty().WithMessage("请输入中文的商品品牌名称");
            //RuleFor(x => x.EnBrandName).NotEmpty().WithMessage("请输入英文的商品品牌名称");
            //RuleFor(x => x.TaiBrandName).NotEmpty().WithMessage("请输入泰文的商品品牌名称");
            //RuleFor(x => x.BrandUrl).NotEmpty().WithMessage("请选择品牌图片");
            //RuleFor(x => x.Remark).NotEmpty().WithMessage("请输入商品描述");
            //RuleFor(x => x.BrandState).NotEmpty().WithMessage("请选择状态");
            //RuleFor(x => x.FirstPY).NotEmpty().WithMessage("请输入商品品牌首拼音字母");

            RuleFor(x => x.ZhBrandName).NotEmpty();
            RuleFor(x => x.EnBrandName).NotEmpty();
            //RuleFor(x => x.TaiBrandName).NotEmpty();//del by liujc
            RuleFor(x => x.HongkongBrandName).NotEmpty();//add by liujc
            RuleFor(x => x.BrandUrl).NotEmpty();
            RuleFor(x => x.Remark).NotEmpty();
            RuleFor(x => x.BrandState).NotEmpty();
        }
    }
}
