using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 请求获取支付单详情
    /// </summary>
    public class RequestPaymentOrderDatilModel
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 支付单号
        /// </summary>
        public string PaymentOrderId { get; set; }
        /// <summary>
        /// 语言Id
        /// </summary>
        public int Lang { get; set; }

    }
}