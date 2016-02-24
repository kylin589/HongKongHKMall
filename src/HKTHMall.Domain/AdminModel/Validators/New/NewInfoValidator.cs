using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Validators.New
{
    public class NewInfoValidator : AbstractValidator<NewInfoModel>
    {
        public NewInfoValidator()
        {
            //RuleFor(x => x.NewTitle).NotNull().WithMessage("请输入处理内容");

            //RuleFor(x => x.NewContent).NotNull().WithMessage("请输入处理内容");

            RuleFor(x => x.NewTitle).NotNull();

            RuleFor(x => x.NewContent).NotNull();
        }
    }
}
