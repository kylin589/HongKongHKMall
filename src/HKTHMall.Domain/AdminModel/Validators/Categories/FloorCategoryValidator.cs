using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.Categoreis;

namespace HKTHMall.Domain.AdminModel.Validators.Categories
{
    public class FloorCategoryValidator : AbstractValidator<FloorCategoryModel>
    {
        public FloorCategoryValidator()
        {
            //this.RuleFor(x => x.DCategoryId).NotEmpty().WithMessage("请选择一级分类");
            //this.RuleFor(x => x.CategoryIdSecond).NotEmpty().WithMessage("请选择二级分类");
            //this.RuleFor(x => x.CategoryId).NotEmpty().WithMessage("请选择三级分类");
            //RuleFor(x => x.Place).GreaterThanOrEqualTo(0).WithMessage("请输入大于或等于0的排序位");

            this.RuleFor(x => x.DCategoryId).NotEmpty();
            this.RuleFor(x => x.CategoryIdSecond).NotEmpty();
            this.RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}