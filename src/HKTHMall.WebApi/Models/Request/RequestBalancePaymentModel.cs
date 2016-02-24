﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 余额支付请求模型
    /// </summary>
    [DataContract]
    public class RequestBalancePaymentModel
    {

        /// <summary>
        /// 交易密码
        /// </summary>
        [DataMember(Name = "payPassword")]
        public string PayPassword { get; set; }

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