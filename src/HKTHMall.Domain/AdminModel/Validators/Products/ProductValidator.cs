using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using HKTHMall.Domain.AdminModel.Models.Products;

namespace HKTHMall.Domain.AdminModel.Validators.Products
{
    public class ProductValidator : AbstractValidator<ProductModel>
    {
        public ProductValidator()
        {
            this.RuleFor(x => x.Product_LangModels)
                .SetValidator(new ProductLanguageListValidator<ProductModel.Product_LangModel>())
                .SetCollectionValidator(new ProductLanguageValidator())
                ;
            this.RuleFor(x => x.CategoryId)
                .NotEqual(0)
                .WithMessage("Please choose product category")
                .NotNull()
                .WithMessage("Please choose product category");

            this.RuleFor(x => x.ArtNo)
                .NotNull()
                .WithMessage("Please enter item No");

            this.RuleFor(x => x.BrandID)
                .NotEqual(0)
                .WithMessage("Please select brand")
                .NotNull()
                .WithMessage("Please select brand");
        }
    }

    public class ProductLanguageListValidator<T> : PropertyValidator
    {
        public ProductLanguageListValidator()
            : base("Enter at least one language")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var list = context.PropertyValue as IList<T>;
            return list != null && list.Count >= 1;
        }
    }

    public class ProductLanguageValidator : AbstractValidator<ProductModel.Product_LangModel>
    {
        public ProductLanguageValidator()
        {
            this.RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Product name cannot be empty")
                //.Must((x, ProductName) => !string.IsNullOrEmpty(x.ProductName) && !(x.ProductName.Length<1&&x.ProductName.Length > 100))
                .Length(1, 200)
                .WithMessage("Min 1 character and max 100 character");
            this.RuleFor(x => x.Subheading)
                .Length(0, 400)
                .WithMessage("Max length of sub title is 100 characters");
        }
    }

    public class AddProductValidator : AbstractValidator<AddProductModel>
    {
        public AddProductValidator()
        {
            this.RuleFor(x => x.Product_LangModels)
                .SetCollectionValidator(new ProductLanguageValidator())
                .SetValidator(new ProductLanguageListValidator<ProductModel.Product_LangModel>())
                ;
        }
    }
}