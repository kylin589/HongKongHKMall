using FluentValidation;
using HKTHMall.Domain.Models.LoginLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Validators.YH_UserLoginLog
{
    public class YH_UserLoginLogValidator : AbstractValidator<YH_UserLoginLogModel>
    {
        public YH_UserLoginLogValidator()
        {
            //RuleFor(x => x.UserID).NotEmpty().WithMessage("用户ID不能为空");

            RuleFor(x => x.UserID).NotEmpty();
        }
    }
}
