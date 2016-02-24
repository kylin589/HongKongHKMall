using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 请求更新支付订单
    /// </summary>
    [DataContract]
    public class RequestPaymentOrdersModel
    {

        /// <summary>
        /// 支付订单号
        /// </summary>
        [DataMember(Name = "paymentOrderId")]
        public string PaymentOrderId { get; set; }

        /// <summary>
        /// 第三方单号
        /// </summary>
        [DataMember(Name = "outOrderId")]

        public string outOrderId { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        [DataMember(Name = "realAmount")]
        public decimal RealAmount { get; set; }

        /// <summary>
        ///用户Id
        /// </summary>
        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// 语言标识
        /// </summary>
        [DataMember(Name = "lang")]
        public int Lang { get; set; }

    }
}