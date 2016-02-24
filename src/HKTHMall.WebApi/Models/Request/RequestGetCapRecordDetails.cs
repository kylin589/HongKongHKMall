using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestGetCapRecordDetails
    {
        /// <summary>
        /// 记录ID
        /// </summary>

        [DataMember(Name = "recordId")]
        public int RecordId { get; set; }
        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        [DataMember(Name = "lang")]
        public int Lang { get; set; }
    }
}