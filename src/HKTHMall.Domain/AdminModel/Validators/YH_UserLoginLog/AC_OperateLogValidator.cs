using FluentValidation;
using HKTHMall.Domain.Models.LoginLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Validators.YH_UserLoginLog
{
    public class AC_OperateLogValidator : AbstractValidator<AC_OperateLogModel>
    {
        public AC_OperateLogValidator()
        {
            //RuleFor(x => x.UserID).NotEmpty().WithMessage("用户ID不能为空");
            //RuleFor(x => x.OperateName).NotEmpty().WithMessage("操作人不能为空");
            //RuleFor(x => x.OperateTime).NotEmpty().WithMessage("操作时间不能为空");
            //RuleFor(x => x.IP).NotEmpty().WithMessage("用户IP不能为空");

            RuleFor(x => x.UserID).NotEmpty();
            RuleFor(x => x.OperateName).NotEmpty();
            RuleFor(x => x.OperateTime).NotEmpty();
            RuleFor(x => x.IP).NotEmpty();
        }
    }
}
