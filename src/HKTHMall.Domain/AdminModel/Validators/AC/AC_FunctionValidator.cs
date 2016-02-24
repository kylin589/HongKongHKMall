using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models.AC;

namespace HKTHMall.Domain.Validators.AC
{
    public class AC_FunctionValidator : AbstractValidator<AC_FunctionModel>
    {
        public AC_FunctionValidator()
        {
            //RuleFor(p => p.FunctionName).NotEmpty().WithMessage("请输入功能名称");
            //RuleFor(p => p.FirstModuleID).NotEmpty().WithMessage("请选择一级菜单");
            //RuleFor(p => p.ModuleID).NotEmpty().WithMessage("请选择二级菜单");
            //RuleFor(p => p.Controller).NotEmpty().WithMessage("请输入控制器名称");
            //RuleFor(p => p.Action).NotEmpty().WithMessage("请输入控制器方法");

            RuleFor(p => p.FunctionName).NotEmpty();
            RuleFor(p => p.FirstModuleID).NotEmpty();
            RuleFor(p => p.ModuleID).NotEmpty();
            RuleFor(p => p.Controller).NotEmpty();
            RuleFor(p => p.Action).NotEmpty();
        }
    }
}
