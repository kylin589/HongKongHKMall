using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 混合支付 请求模型
    /// </summary>
    [DataContract]
    public class RequestMixturePaymentModel
    {

        /// <summary>
        /// 语言标识
        /// </summary>
        [DataMember(Name = "lang")]
        public int Lang { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// 支付单号
        /// </summary>
        [DataMember(Name = "paymentOrderId")]
        public long PaymentOrderId { get; set; }


        /// <summary>
        /// 支付方式
        /// </summary>
        [DataMember(Name = "payChannel")]
        public int PayChannel { get; set; }
    }
}