using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.WebModel.Models.Login;
using FluentValidation;

namespace HKTHMall.Domain.WebModel.Validators.Login
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.account).NotEmpty().WithMessage("请输入账号");
            RuleFor(x => x.passWord).NotEmpty().WithMessage("请输入密码");
        }
    }
}
