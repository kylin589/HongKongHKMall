using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HKTHMall.Domain.Models.AC;

namespace HKTHMall.Domain.Validators.AC
{
    /// <summary>
    /// 模块视图模型验证
    /// </summary>
    public class AC_ModuleValidator : AbstractValidator<AC_ModuleModel>
    {
        public AC_ModuleValidator()
        {
            //RuleFor(x => x.ModuleName).NotEmpty().WithMessage("请输入模块名称");
            RuleFor(x => x.ModuleName).NotEmpty();
        }
    }
}
