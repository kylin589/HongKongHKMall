using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    [DataContract]
    public class RequestTotalPostagePriceModel
    {
        [DataMember(Name = "comInfos")]
        public List<PostagePriceModel> comInfos { get; set; }

        [DataMember(Name = "userAddressId")]
        public long userAddressId { get; set; }
        [DataMember]
        public int lang { get; set; }
    }
    /// <summary>
    /// 同一个商家的商品
    /// </summary>
     [DataContract]
    public class PostagePriceModel
    {
        /// <summary>商家Id.</summary>          
        [DataMember(Name = "comId")]
        public String ComId { get; set; }
        /// <summary>
        /// 商品集合
        /// </summary>
        [DataMember(Name = "goods")]
        public List<GoodsInfo> GoodsInfo{ get; set; }
    }
    /// <summary>
    /// 单个商品
    /// </summary>
     [DataContract]
    public class GoodsInfo
    {
        /// <summary>
        /// 重量
        /// </summary>
        [DataMember(Name = "weight")]
        public decimal Weight { get; set; }
        /// <summary>
        /// 是否免运费
        /// </summary>
        [DataMember(Name = "freeShipping")]
        public int FreeShipping { get; set; }
        /// <summary>
        /// 商品个数
        /// </summary>
        [DataMember(Name = "count")]
        public Int32 Count { get; set; }
    }
}