using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 订单评论请求参数
    /// </summary>
    public class RequestOrderComment
    {
        /// <summary>
        /// 用户ID（加密）
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public long orderId { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public int lang { get; set; }     
        /// <summary>
        /// 是否匿名
        /// </summary>
        public bool isanonymous { get; set; }
        /// <summary>
        ///  评论参数集合
        /// </summary>
        public dynamic rq { get; set; }   
    }
    public class Rs {
        /// <summary>
        /// 商品ID
        /// </summary>
        public long productId { get; set; }
       
        /// <summary>
        /// SKUID
        /// </summary>
        public int skuId { get; set; }
        /// <summary>
        /// 评论星级
        /// </summary>
        public int commentlevel { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string commentcontent { get; set; }
    
    }
}