using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HKTHMall.Domain.Models.Sys;

namespace HKTHMall.Domain.Validators.Sys
{
    /// <summary>
    /// 系统参数设置验证
    /// <remarks>added by jimmy,2015-7-2</remarks>
    /// </summary>
    public class ParameterSetValidator : AbstractValidator<ParameterSetModel>
    {

        public ParameterSetValidator() {
            //RuleFor(p => p.keys).NotEmpty().WithMessage("请输入键名称");
            //RuleFor(p => p.PValue).NotEmpty().WithMessage("请输入键值");
            //RuleFor(p => p.Remark).NotEmpty().WithMessage("请输入备注");

            RuleFor(p => p.keys).NotEmpty();
            RuleFor(p => p.PValue).NotEmpty();
            RuleFor(p => p.Remark).NotEmpty();
        }
    }
}
