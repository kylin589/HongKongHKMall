using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ResponseGetGroupId
    {
        /// <summary>
        /// 群ID
        /// </summary>
        [DataMember(Name = "groupId")]
        public long GroupId { get; set; }
        /// <summary>
        /// 类型 1.我的感恩群,2.我的感动群,3.我的感谢群,4,我加入的感恩群,5.我加入的感动群,6我加入的感谢群
        /// </summary>
        [DataMember(Name = "type")]
        public int Type { get; set; }
    }
}