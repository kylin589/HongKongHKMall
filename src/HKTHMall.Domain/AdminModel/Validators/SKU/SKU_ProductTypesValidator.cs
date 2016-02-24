using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.SKU;

namespace HKTHMall.Domain.AdminModel.Validators.SKU
{
    /// <summary>
    /// 商品类型模型验证
    /// </summary>
    public class SKU_ProductTypesValidator : AbstractValidator<SKU_ProductTypesModel>
    {
        public SKU_ProductTypesValidator()
        {
            //RuleFor(x => x.Name).NotEmpty().WithMessage("请输入商品类型名");
            //RuleFor(x => x.SKU_AttributeValuesModels).SetValidator(new ListItemsValidator<SKU_AttributeValuesModel>(1, "请输入规格值"));
            //RuleFor(x => x.AttributeName).Matches("^(X|Y)?$").WithMessage()

            RuleFor(x => x.Name).NotEmpty();

        }

    }
}
