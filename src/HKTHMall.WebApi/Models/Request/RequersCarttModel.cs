using FluentValidation;
using HKTHMall.Services.Common.MultiLangKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequersCarttModel
    {
        /// <summary>
        /// 账号id 戴勇军 修改
        /// </summary>
        [DataMember(Name = "userId")]
        public string userId { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        [DataMember(Name = "productId")]
        public string productId { get; set; }
        /// <summary>
        /// 1:中文；2、英文；3、泰文
        /// </summary>
        [DataMember(Name = "lang")]
        public int lang { get; set; }
        /// <summary>
        /// 商品ID 戴勇军 修改
        /// </summary>
        [DataMember(Name = "productId")]
        public string ProductId { get; set; }

        /// <summary>
        /// 活动ID
        /// </summary>
        [DataMember(Name = "activityId")]
        public string ActivityId { get; set; }

        /// <summary>
        /// 编辑购物车的商品ID列表
        /// </summary>
        [DataMember(Name = "product")]
        public string Product { get; set; }

        /// <summary>
        /// 购物车ID
        /// </summary>
        [DataMember(Name = "shoppingCartId")]
        public string shoppingCartId { get; set; }

        /// <summary>
        /// sku
        /// </summary>
        [DataMember(Name = "sku")]
        public string sku { get; set; }
        /// <summary>
        /// 购买数量  戴勇军 修改
        /// </summary>
        [DataMember(Name = "buyNum")]
        public string buyNum { get; set; }
        /// <summary>
        /// 操作:1 修改数量,2 删除
        /// </summary>
        [DataMember(Name = "action")]
        public int Action { get; set; }

          

    }
    public class CartModelValidator : AbstractValidator<RequersCarttModel>
    {
        public CartModelValidator(int lang)
        {
            When(x => x == null, () => { });
            RuleFor(x => x.userId).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("USER_ADDFAVORITES_USERVALID", lang));
            RuleFor(x => x.productId).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("USER_ADDFAVORITES_COMMODILLEGAL", lang));
        }
    }
    public class CartIdArr
    {
        [DataMember(Name = "productId")]
        public string ProductId { get; set; }

        [DataMember(Name = "sku")]
        public string SKU { get; set; }

        [DataMember(Name = "buyNum")]
        public string BuyNum { get; set; }
    }
}