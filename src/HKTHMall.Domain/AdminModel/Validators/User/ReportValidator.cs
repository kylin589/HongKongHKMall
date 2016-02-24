using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Validators.User
{
    public class ReportValidator : AbstractValidator<ReportModel>
    {
        public ReportValidator() { 
         //RuleFor(x => x.Result).NotNull().WithMessage("请输入处理结果");
            RuleFor(x => x.Result).NotNull();

        }
    }
}
