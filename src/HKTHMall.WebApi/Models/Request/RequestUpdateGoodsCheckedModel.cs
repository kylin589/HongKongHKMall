using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestUpdateGoodsCheckedModel
    {
        /// <summary>
        /// 商品ID集合
        /// </summary>
        public List<string> GoodsIDs { get; set; }
        /// <summary>
        /// 语言Id
        /// </summary>
        public int Lang { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
    }
}