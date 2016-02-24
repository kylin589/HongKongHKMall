using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Validators.New
{
    public class BD_NewsInfoValidator : AbstractValidator<BD_NewsInfoModel>
    {
        public BD_NewsInfoValidator(){
            //RuleFor(x => x.Title).NotNull().WithMessage("请输入标题");
            //RuleFor(x => x.NewsContent).NotNull().WithMessage("请输入新闻内容");

            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.NewsContent).NotNull();
        }
    }
}
