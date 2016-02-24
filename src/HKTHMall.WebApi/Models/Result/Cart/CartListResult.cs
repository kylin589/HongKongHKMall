
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.WebApi.Models.Result.Cart
{

    public class CartListResult : ExResult
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
            public List<Productobj>  Products { get; set; }
        }
        [DataContract]
        public class Productobj
        {
            /// <summary>
            /// 商家ID
            /// </summary>
            [DataMember]
            public string merchantID { get; set; }
            /// <summary>
            /// 购物车ID
            /// </summary>
            [DataMember]
            public string shoppingCartId { get; set; }
            /// <summary>
            /// 产品ID
            /// </summary>
            [DataMember]
            public string productId { get; set; }
            /// <summary>
            /// 产品名称
            /// </summary>
            [DataMember]
            public string productName { get; set; }
            /// <summary>
            /// SKU 
            /// </summary>
            [DataMember]
            public string sku { get; set; }

            /// <summary>
            /// SKU 描述
            /// </summary>
            [DataMember]
            public string skuText { get; set; }

            [DataMember]
            public string imageUrl { get; set; }
            /// <summary>
            /// 购买数量
            /// </summary>
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
            public string isFavorites { get; set; }
            

        }
    }


}
