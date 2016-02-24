using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;


namespace HKTHMall.Domain.WebModel.Validators.ShoppingCart
{
    /// <summary>
    /// 购物车视图模型验证
    /// </summary>
    public class ShoppingCartValidator : AbstractValidator<ShoppingCartModel>
    {
        public ShoppingCartValidator()
        {
            RuleFor(x => x.UserID).NotEmpty().WithMessage("用户ID不能为空");
        }
        //RuleFor(x => x.UserName).NotEmpty().WithMessage("请输入用户帐号");
        //RuleFor(x => x.RealName).NotEmpty().WithMessage("请输入真实姓名");
        //RuleFor(x => x.Password).NotEmpty().WithMessage("请输入密码");
        //RuleFor(x => x.PasswordTwo).NotEmpty().WithMessage("请输入确认密码");
        
        //public long ShoppingCartId { get; set; }
        //public long ProductID { get; set; }
        //public long SKU_ProducId { get; set; }
        //public long UserID { get; set; }
        //public int Quantity { get; set; }
        //public System.DateTime CartDate { get; set; }
    }
}
