using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// OmisePayment 支付参数
    /// </summary>
    [DataContract]
    public class RequestOmisePaymentModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// omiseToken
        /// </summary>
        [DataMember(Name = "omiseToken")]
        public string OmiseToken { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 支付单号
        /// </summary>
        [DataMember(Name = "paymentOrderId")]
        public string PaymentOrderId { get; set; }

        /// <summary>
        /// 语言标识
        /// </summary>
        [DataMember(Name = "lang")]
        public int Lang { get; set; }
    }
}