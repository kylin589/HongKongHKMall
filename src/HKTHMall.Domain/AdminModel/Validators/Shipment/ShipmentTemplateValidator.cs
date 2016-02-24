using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.Shipment;

namespace HKTHMall.Domain.AdminModel.Validators.Shipment
{
    public class ShipmentTemplateValidator : AbstractValidator<ShipmentTemplateModel>
    {
        public ShipmentTemplateValidator()
        {
            this.RuleFor(x => x.CityIds)
                .NotEmpty()
                .WithMessage(@"'City' must not be empty.");

            this.RuleFor(x => x.Price1)
                .NotNull()
                //.Must(p => p != 0)
                .WithMessage(@"'0-1KG' must not be empty.");

            this.RuleFor(x => x.Price2)
                .NotNull()
                //.Must(p => p != 0)
                .WithMessage(@"'1-3KG' must not be empty.");

            this.RuleFor(x => x.Price3)
                .NotNull()
                //.Must(p => p != 0)
                .WithMessage(@"'3-5KG' must not be empty.");
            this.RuleFor(x => x.Price4)
                .NotNull()
                //.Must(p => p != 0)
                .WithMessage(@"'5-10KG' must not be empty.");
            this.RuleFor(x => x.Price5)
                .NotNull()
                //.Must(p => p != 0)
                .WithMessage(@"'10-15KG' must not be empty.");
            this.RuleFor(x => x.Price6)
                .NotNull()
                //.Must(p => p != 0)
                .WithMessage(@"'15-20KG' must not be empty.");

            this.RuleFor(x => x.Price7)
                .NotNull()
                //.Must(p => p != 0)
                .WithMessage(@"'Over 20KG' must not be empty.");
        }
    }
}