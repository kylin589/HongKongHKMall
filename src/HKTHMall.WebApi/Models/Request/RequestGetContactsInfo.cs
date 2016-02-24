using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestGetContactsInfo
    {
        /// <summary>
        /// 联系方式类型（1.邮箱账号,2.手机号码）
        /// </summary>
        [DataMember]
        public int type { get; set; }
        /// <summary>
        /// 联系方式信息列表
        /// </summary>
        [DataMember]
        public string[] key_list { get; set; }
        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
    }
}