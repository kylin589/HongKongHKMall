using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ExplosiveResult
    {
        [DataMember]
        public int ipTotal { get; set; }
        /// <summary>
        /// 进行中
        /// </summary>
        [DataMember]
        public List<ExplosiveGoodsResult> inProgressList { get; set; }
        [DataMember]
        public int atsTotal { get; set; }
        /// <summary>
        /// 即将开始
        /// </summary>
        [DataMember]
        public List<ExplosiveGoodsResult> aboutToStart { get; set; }

    }
    [DataContract]
    public class ExplosiveGoodsResult
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
        /// 活动价
        /// </summary>
         [DataMember]
        public decimal activityPrice { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
         [DataMember]
        public decimal discount { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
         [DataMember]
        public string picAddress { get; set; }
        /// <summary>
        /// 开始时间戳
        /// </summary>
         [DataMember]
        public long starDate { get; set; }
        /// <summary>
        /// 结束时间戳
        /// </summary>       
         [DataMember]
         public long endDate { get; set; }
        /// <summary>
        /// 当前服务器时间
        /// </summary>
         [DataMember]
         public long serverDt { get; set; }
         public int sorts { get; set; }
    }
}