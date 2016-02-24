using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Validators
{
    public class YH_MerchantInfoValidator : AbstractValidator<YH_MerchantInfoModel>
    {
        public YH_MerchantInfoValidator()
        {
            RuleFor(x => x.ShopName).NotEmpty();
            RuleFor(x => x.ShopAddress).NotEmpty();
        }
    }
}
