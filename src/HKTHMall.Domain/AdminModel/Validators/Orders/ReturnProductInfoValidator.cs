using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Validators.Orders
{
    public class ReturnProductInfoValidator : AbstractValidator<ReturnProductInfoModel>
    {
        public ReturnProductInfoValidator()
        {
            //RuleFor(x => x.RefundAmount).NotEmpty().WithMessage("请输入正确金额");
            //RuleFor(x => x.Comments).NotEmpty().WithMessage("请输入处理内容");

            RuleFor(x => x.RefundAmount).NotEmpty();
        }
    }
}
