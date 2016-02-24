using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ResponseGetContactsInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public string uid { get; set; }

        /// <summary>
        /// 惠信号
        /// </summary>
        [DataMember]
        public string acc { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [DataMember]
        public string nick { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [DataMember]
        public string phone { get; set; }

        /// <summary>
        /// 头像url
        /// </summary>
        [DataMember]
        public string img_url { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [DataMember]
        public string sign { get; set; }

        /// <summary>
        /// 性别(1=男,2=女,0=未设)
        /// </summary>
        [DataMember]
        public int sex { get; set; }
    }
}