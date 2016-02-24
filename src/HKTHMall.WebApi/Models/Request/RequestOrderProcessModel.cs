using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestOrderProcessModel
    {
        /// <summary>
        /// 支付单号
        /// </summary>
        public string PaymentOrderId { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public int Lang { get; set; }
    }
}