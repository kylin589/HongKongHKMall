using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 重新生成支付单号
    /// </summary>
    [DataContract]
    public class RequestGeneratePaymentOrderModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [DataMember(Name = "orderId")]
        public string OrderId { get; set; }

        /// <summary>
        /// 语言标识
        /// </summary>
        [DataMember(Name = "lang")]
        public int Lang { get; set; }
    }
}