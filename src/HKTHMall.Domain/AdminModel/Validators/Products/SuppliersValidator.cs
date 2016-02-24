using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.Products;

namespace HKTHMall.Domain.AdminModel.Validators.Products
{
    public class SuppliersValidator : AbstractValidator<SuppliersModel>
    {
        public SuppliersValidator()
        {
            this.RuleFor(x => x.Mobile)
                .NotNull()
                .WithMessage("Please enter item Mobile")//.Matches("^[0][6||8-9][0-9]{8}$") del by liujc
                //.Must((x, Mobile) => Regex.IsMatch(Mobile, "/^[0][6||8-9][0-9]{8}$/", RegexOptions.IgnoreCase))
                //.WithMessage("Please enter the correct Thailand Mobile number")
                ;

            this.RuleFor(x => x.SupplierName)
                .NotNull()
                .WithMessage("Please enter item SupplierName");

            //this.RuleFor(x => x.PassWord)
            //    .NotNull()
            //    .WithMessage("Please enter item PassWord")
            //    .Matches("^((?=.*[a-z])(?=.*\\d)|(?=[a-z])(?=.*[#@!~%^&*])|(?=.*\\d)(?=.*[#@!~%^&*]))[a-z\\d#@!~%^&*]{8,16}$")
            //    .WithMessage("Password format is incorrect")
            this.RuleFor(x => x.PassWord)
               .NotNull().WithMessage("'Password' must not be empty").Length(8, 16).WithMessage("'Password' must be between 8 and 16 characters.");
              // .WithMessage("Please enter item PassWord")
               //.Length(8, 16).WithMessage("Length in 8-16 characters or characters");
                ;

            this.RuleFor(x => x.THAreaID)
                .GreaterThan(1).WithMessage("Select area");
        }
    }
}