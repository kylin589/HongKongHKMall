using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    /// <summary>
    ///     生成订单模型
    /// </summary>
    [DataContract]
    public class AddOrderInfoView
    {
        /// <summary>
        ///     收货地址
        /// </summary>
        [DataMember]
        public long ReceiverAddressId { get; set; }

        /// <summary>
        ///     支付类型
        /// </summary>
        [DataMember]
        public int PayType { get; set; }

        /// <summary>
        ///     支付单类型
        /// </summary>
        [DataMember]
        public int PaidType { get; set; }

        /// <summary>
        ///     语言代码
        /// </summary>
        [DataMember]
        public int LanguageId { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        [DataMember]
        public long UserId { get; set; }

        /// <summary>
        ///     订单来源
        /// </summary>
        [DataMember]
        public int OrderSource { get; set; }

        /// <summary>
        ///     支付通道
        /// </summary>
        [DataMember]
        public int PayChannel { get; set; }

        /// <summary>
        ///     购买类型
        /// </summary>
        [DataMember]
        public int PurchaseType { get; set; }

        /// <summary>
        /// 支付单号
        /// </summary>
        [DataMember]
        public string PaymentOrderId { get; set; }

       


        [DataMember]
        public List<MerchantView> MerchantViews { get; set; }

        [DataContract]
        public class MerchantView
        {
            [DataMember]
            public string MerchantID { get; set; }

            [DataMember]
            public string Remark { get; set; }

            [DataMember]
            public List<GoodsView> Goods { get; set; }
        }

        [DataContract]
        public class GoodsView
        {
            [DataMember]
            public string ProductID { get; set; }

            [DataMember]
            public string SkuNumber { get; set; }

            [DataMember]
            public string ProductNumber { get; set; }
        }
    }
}