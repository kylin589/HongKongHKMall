using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestShoppingCartModel
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public long productId { get; set; }

        /// <summary>
        /// 商品SKU码
        /// </summary>
        public long product_SKU { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// 语言1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }

        /// <summary>
        /// 是否立刻购买
        /// </summary>
        //public bool nowShopping { get; set; }
        
    }
}