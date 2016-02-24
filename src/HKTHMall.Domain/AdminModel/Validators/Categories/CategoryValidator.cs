using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Validators;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.Entities;

namespace HKTHMall.Domain.AdminModel.Validators.Categories
{
    public class AddCategoryModelValidator : AbstractValidator<AddCategoryModel>
    {
        public AddCategoryModelValidator()
        {
            this.RuleFor(x => x.Category_Lang)
                .SetCollectionValidator(new CategoryLanguageValidator())
                //.SetValidator(new CategoryLanguageListValidator<CategoryLanguageModel>())
                ;
        }
    }

    public class UpdateCategoryModelValidator : AbstractValidator<UpdateCategoryModel>
    {
        public UpdateCategoryModelValidator()
        {
            this.RuleFor(x => x.Category_Lang)
                .SetCollectionValidator(new CategoryLanguageValidator())
                //.SetValidator(new CategoryLanguageListValidator<CategoryLanguageModel>())
                ;
        }
    }

    public class CategoryLanguageListValidator<T> : PropertyValidator
    {
        public CategoryLanguageListValidator()
            : base("Enter at least one language")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var list = context.PropertyValue as IList<T>;
            return list != null && list.Count >= 1;
        }
    }

    public class CategoryLanguageValidator : AbstractValidator<CategoryLanguageModel>
    {
        public CategoryLanguageValidator()
        {
            this.RuleFor(x => x.CategoryName)
                //.Must((x, CategoryName) => !(x.LanguageID == 3 && string.IsNullOrEmpty(x.CategoryName)))
                //.WithMessage("Thai category name cannot be empty")
                .Must((x, CategoryName) => !(x.LanguageID == 1 && string.IsNullOrEmpty(x.CategoryName)))
                .WithMessage("Chinese category name cannot be empty")
                .Must((x, CategoryName) => !(x.LanguageID == 2 && string.IsNullOrEmpty(x.CategoryName)))
                .WithMessage("English category name cannot be empty")
                .Must((x, CategoryName) => !(x.LanguageID == 4 && string.IsNullOrEmpty(x.CategoryName)))//add by liujc
                .WithMessage("Chinese Hongkong category name cannot be empty")
                .Length(1,50)
                .WithMessage("Max length of title is 50 characters")
                ;
        }
    }
}