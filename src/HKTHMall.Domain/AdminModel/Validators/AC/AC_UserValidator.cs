using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HKTHMall.Domain.Models.User;

namespace HKTHMall.Domain.Validators.AC
{
    public class AC_UserValidator : AbstractValidator<AC_UserModel>
    {

        public AC_UserValidator()
        {
            //RuleFor(x => x.UserName).NotEmpty().WithMessage("请输入用户帐号");
            //RuleFor(x => x.RealName).NotEmpty().WithMessage("请输入真实姓名");
            //RuleFor(x => x.Password).NotEmpty().WithMessage("请输入密码");
            //RuleFor(x => x.PasswordTwo).NotEmpty().WithMessage("请输入确认密码");
            //RuleFor(x => x.PasswordOld).NotEmpty().WithMessage("请输入原密码");

            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.RealName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.PasswordTwo).NotEmpty();
            RuleFor(x => x.PasswordOld).NotEmpty();

        }
    }
}
