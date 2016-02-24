using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Runtime.Serialization;
using HKTHMall.Services.Common.MultiLangKeys;


namespace HKTHMall.WebApi.Models.Request
{
    [Validator(typeof(FavoritesModelValidator))]
    public class RequestFavoritesModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember(Name = "userId")]
        public string userId { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        [DataMember(Name = "productIds")]
        public string productIds { get; set; }

        /// <summary>
        /// 1:中文；2、英文；3、泰文
        /// </summary>
       [DataMember(Name = "lang")]
        public int lang { get; set; }
    }

    public class FavoritesModelValidator : AbstractValidator<RequestFavoritesModel>
    {
        public FavoritesModelValidator(int lang)
        {
            When(x => x == null, () => { });
            RuleFor(x => x.userId).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("USER_ADDFAVORITES_USERVALID", lang));
            RuleFor(x => x.productIds).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("USER_ADDFAVORITES_COMMODILLEGAL", lang));        
        }
    }
    /// <summary>
    /// 返回字段
    /// </summary>
    public class IsNotFavorites {
        /// <summary>
        /// 判断是否收藏成功
        /// </summary>
        public int isFavorites { get; set; }

        /// <summary>
        /// 收藏ID
        /// </summary>
        public long favoritesID { get; set; }
    }
}