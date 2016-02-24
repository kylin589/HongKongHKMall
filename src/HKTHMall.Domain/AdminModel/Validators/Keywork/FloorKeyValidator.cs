using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Keywork;

namespace HKTHMall.Domain.AdminModel.Validators.Keywork
{
    public class FloorKeyValidator : AbstractValidator<FloorKeywordModel>
    {
        public FloorKeyValidator()
        {
          //  RuleFor(x => x.PlaceCode).NotEmpty().WithMessage("请输入地方编码");
            //RuleFor(x => x.KeyWordName).NotEmpty().WithMessage("请输入关键字名称");
            //RuleFor(x => x.Sorts).NotNull().WithMessage("请输入排序").GreaterThanOrEqualTo(0).WithMessage("请输入大于或等于0的数字排序");
            //RuleFor(x => x.LanguageID).NotEmpty().WithMessage("请选择语言类型");

            RuleFor(x => x.KeyWordName).NotEmpty();
        }
    }
}
