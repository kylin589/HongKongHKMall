using FluentValidation;
using HKTHMall.Domain.Models.Bra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Validators.Bra
{
    public class Brand_LangValidator : AbstractValidator<Brand_LangModel>
    {
        public Brand_LangValidator()
        {
            //RuleFor(x => x.BrandName).NotEmpty().WithMessage("请输入品牌名称");
            RuleFor(x => x.BrandName).NotEmpty();
        }
    }
}
