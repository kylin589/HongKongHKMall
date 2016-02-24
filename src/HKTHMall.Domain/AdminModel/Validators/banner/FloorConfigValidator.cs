using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Validators.banner
{
    public class FloorConfigValidator : AbstractValidator<FloorConfigModel>
    {
        public FloorConfigValidator()
        {
            //RuleFor(x => x.FloorConfigId).NotEmpty().WithMessage("请输入FloorID");
            //RuleFor(x => x.FloorConfigName).NotEmpty().WithMessage("请输入配置名称");
            RuleFor(x => x.FloorConfigId).NotEmpty();
            
            
        }
    }
}
