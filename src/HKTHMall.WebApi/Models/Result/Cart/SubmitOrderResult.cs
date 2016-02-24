using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace HKTHMall.WebApi.Models.Result.Cart
{
    public class SubmitOrderResult : ExResult
    {
        [DataMember(Name = "rs")]

        public ResultM Rs { get; set; }

        [DataContract]
        public class ResultM
        {
            /// <summary>
            /// 邮费
            /// </summary>
            [DataMember]
            public decimal postagePrice { get; set; }
            /// <summary>
            /// 订单号数组,用|隔开,如:10001|10002
            /// </summary>
            [DataMember]
            public string orderNumberArray { get; set; }
            /// <summary>
            /// 失败商品
            /// </summary>
            [DataMember]
            public List<orderReason> fail { get; set; }
            /// <summary>
            /// 商家信息
            /// </summary>
            [DataMember]
            public List<Merchant> merchants { get; set; }
        }

        [DataContract]
        public class Merchant
        {
            /// <summary>
            /// 商家ID
            /// </summary>
            [DataMember]
            public string merchantID { get; set; }
            /// <summary>
            /// 是否提供发票(0 不提供, 1 提供)
            /// </summary>
            [DataMember]
            public string IsProvideInvoices { get; set; }
            /// <summary>
            /// 邮费
            /// </summary>
            [DataMember]
            public decimal postagePrice { get; set; }
        }

        [DataContract]
        public class orderReason
        {
            /// <summary>
            /// 商品ID
            /// </summary>
            [DataMember]
            public string productId { get; set; }
            /// <summary>
            /// 商品名称
            /// </summary>
            [DataMember]
            public string productName { get; set; }
            /// <summary>
            /// SKU编码
            /// </summary>
            [DataMember]
            public string sku { get; set; }
            /// <summary>
            /// 失败原因
            /// </summary>
            [DataMember]
            public string failReason { get; set; }
            /// <summary>
            /// 库存
            /// </summary>
            [DataMember]
            public string stock { get; set; }

            /// <summary>
            /// 失败类型1已卖光,2库存不足,3商品失效,4不存在
            /// </summary>
            [DataMember]
            public string failType { get; set; }

            /// <summary>
            /// 商家ID
            /// </summary>
            [DataMember]
            public string merchantID { get; set; }
        }
    }
}
