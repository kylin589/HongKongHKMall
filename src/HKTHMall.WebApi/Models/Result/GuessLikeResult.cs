using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class GuessLikeResult
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        [DataMember]
        public long productId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember]
        public string productName { get; set; }
        /// <summary>
        /// 惠卡价
        /// </summary>
        [DataMember]
        public decimal hKPrice { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        [DataMember]
        public decimal marketPrice { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        [DataMember]
        public string picAddress { get; set; }
        /// <summary>
        /// 活动价
        /// </summary>
        [DataMember]
        public decimal activityPrice { get; set; }
        /// <summary>
        /// 是否活动
        /// </summary>
        [DataMember]
        public bool isActivity { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>      
        public DateTime? starDate { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime? endDate { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal discount { get; set; }
       
    }
}