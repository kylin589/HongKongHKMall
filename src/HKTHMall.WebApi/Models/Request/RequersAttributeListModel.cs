using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequersAttributeListModel
    {
        /// <summary>
        /// 语言:1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public long productId { get; set; }
    }
}