using FluentValidation;
using HKTHMall.Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Validators.Orders
{
    public class ComplaintsValidator : AbstractValidator<ComplaintsModel>
    {
        public ComplaintsValidator()
        {
          //  RuleFor(x => x.Flag).NotEmpty().WithMessage("请选择处理状态");
          //RuleFor(x => x.Comments).NotNull().WithMessage("请输入处理内容");

            RuleFor(x => x.Comments).NotNull();
        }
    }
}
