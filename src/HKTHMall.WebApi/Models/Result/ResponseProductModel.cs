using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    /// <summary>
    /// 产品列表返回参数 zzr
    /// </summary>
    public class ResponseProductModel
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public long productId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// 惠卡价
        /// </summary>
        public float hKPrice { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        public float marketPrice { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string picAddress { get; set; }
        
        public decimal activityPrice { get; set; }
       
        public bool isActivity { get; set; }
    }
}