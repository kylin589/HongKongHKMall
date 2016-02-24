using FluentValidation;
using HKTHMall.Domain.OfficialWeb.Models.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.OfficialWeb.Validators
{
    public class SuppliersValidator : AbstractValidator<SuppliersModel>
    {
        public SuppliersValidator()
        {
            this.RuleFor(x => x.Mobile)
                .NotNull()
                .WithMessage("Please enter item Mobile").Matches("^[0][6||8-9][0-9]{8}$")
                //.Must((x, Mobile) => Regex.IsMatch(Mobile, "/^[0][6||8-9][0-9]{8}$/", RegexOptions.IgnoreCase))
                .WithMessage("Please enter the correct Thailand Mobile number")
                ;

            this.RuleFor(x => x.SupplierName)
                .NotNull()
                .WithMessage("Please enter item SupplierName");

            this.RuleFor(x => x.PassWord)
                .NotNull()
                .WithMessage("Please enter item PassWord")
                .Matches("^((?=.*[a-z])(?=.*\\d)|(?=[a-z])(?=.*[#@!~%^&*])|(?=.*\\d)(?=.*[#@!~%^&*]))[a-z\\d#@!~%^&*]{8,16}$")
                .WithMessage("Password format is incorrect")
                ;

            this.RuleFor(x => x.THAreaID)
                .GreaterThan(1).WithMessage("Select area");
        }
    }
}
