using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{

    /// <summary>
    /// 请求验证交易密码
    /// </summary>
    [DataContract]
    public class RequestValidPayPasswordModel 
    {

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

        /// <summary>
        /// 交易密码
        /// </summary>
        [DataMember(Name = "PayPassword")]
        public string PayPassword { get; set; }
    }
}