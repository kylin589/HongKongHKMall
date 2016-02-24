
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.WebApi.Models.Result.Cart
{

    public class FinalOrderResult : ExResult
    {
        [DataMember]
        public List<ProductShop> rs { get; set; }
        [DataContract]
        public class ProductShop
        {
            [DataMember]
            public string merchantID { get; set; }
            [DataMember]
            public string shopName { get; set; }
            /// <summary>
            /// 是否提供发票(0 不提供, 1 提供)
            /// </summary>
            [DataMember]
            public string isProvideInvoices { get; set; }
            /// <summary>
            /// 邮费
            /// </summary>
            [DataMember]
            public decimal postagePrice { get; set; }

            [DataMember(Name = "products")]
            public List<Productobj> Products { get; set; }

            /// <summary>
            /// 是否爆款或秒杀
            /// </summary>
            [DataMember(Name = "isHot")]
            public string IsHot { get; set; }

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
            [DataMember]
            public string tradePrice { get; set; }
            [DataMember]
            //购物车清单
            public string stock { get; set; }
            [DataMember]
            public string failType { get; set; }
            [DataMember]
            public string failReason { get; set; }
        }
    }


}
