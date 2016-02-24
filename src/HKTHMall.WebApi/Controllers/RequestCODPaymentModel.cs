using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Controllers
{
    /// <summary>
    /// 货到付款 请求模型
    /// </summary>
    [DataContract]
    public class RequestCODPaymentModel
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
    }
}