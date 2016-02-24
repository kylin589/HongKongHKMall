using FluentValidation;
using System;
using System.Collections.Generic;
using HKTHMall.Domain.AdminModel.Models.Products;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Validators.Products
{
    /// <summary>
    /// 验证产品图模型
    /// <remarks>added by jimmy,2015-7-27</remarks>
    /// </summary>
    public class ProductImageValidator : AbstractValidator<ProductImageModel>
    {
          public ProductImageValidator(){
            //RuleFor(x => x.ProductName).NotEmpty().WithMessage("请输入产品图名称");
            //RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("请选择产品图图片");

              RuleFor(x => x.ProductName).NotEmpty();
              RuleFor(x => x.ImageUrl).NotEmpty();
          }
    }
}
