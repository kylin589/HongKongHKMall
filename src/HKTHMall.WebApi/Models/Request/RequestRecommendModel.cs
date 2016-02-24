using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 惠卡推荐输入参数 zzr
    /// </summary>
    public class RequestRecommendModel
    {
        /// <summary>
        /// 语言:1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
        /// <summary>
        /// 推荐商品数量
        /// </summary>
        public int referrerSize { get; set; }
    }
}