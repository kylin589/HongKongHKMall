using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using HKTHMall.Domain.WebModel.Validators.ShoppingCart;

namespace HKTHMall.Domain.WebModel.Models.ShoppingCart
{
    /// <summary>
    /// 购物车视图模型
    /// </summary>
    [Validator(typeof(ShoppingCartValidator))]
    public class ShoppingCartModel
    {
        public long ShoppingCartId { get; set; }
        public long ProductID { get; set; }
        public long SKU_ProducId { get; set; }
        public long UserID { get; set; }
        public int Quantity { get; set; }
        public Nullable<System.DateTime> CartDate { get; set; }

        public int IsCheck { get; set; }

    }
}
