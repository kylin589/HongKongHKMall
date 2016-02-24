using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    public class ResponseCartListResultModel : ApiResultModel
    {

        [DataContract]
        public class ProductShop
        {
            [DataMember]
            public string merchantID { get; set; }
            [DataMember]
            public string shopName { get; set; }
            /// <summary>
            /// 是否含爆款
            /// </summary>
            [DataMember(Name = "isHot")]
            public string IsHot { get; set; }

            [DataMember(Name = "products")]
            public List<Productobj> Products { get; set; }
        }
        [DataContract]
        public class Productobj
        {
            [DataMember]
            public string merchantID { get; set; }
            [DataMember]
            public string productId { get; set; }
            [DataMember]
            public string productName { get; set; }
            [DataMember]
            public string sku { get; set; }
            [DataMember]
            public string skuText { get; set; }
            [DataMember]
            public string imageUrl { get; set; }
            [DataMember]
            public int buyNum { get; set; }
            /// <summary>
            /// 惠卡价
            /// </summary>
            [DataMember]
            public string tradePrice { get; set; }
            /// <summary>
            /// 市场价
            /// </summary>
            [DataMember]
            public string marketPrice { get; set; }
            [DataMember]
            //购物车清单
            public string stock { get; set; }
            /// <summary>
            /// 是否已收藏
            /// </summary>
            [DataMember]
            public string IsFavorites { get; set; }
        }
    }
}