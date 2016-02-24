using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ResponseGetGroupNumber
    {
        /// <summary>
        /// 感恩惠粉人数
        /// </summary>
        [DataMember(Name = "gnNumber")]
        public string GnNumber { get; set; }

        /// <summary>
        /// 感动惠粉人数
        /// </summary>
        [DataMember(Name = "gdNumber")]
        public string GdNumber { get; set; }

        /// <summary>
        /// 感谢惠粉人数
        /// </summary>
        [DataMember(Name = "gxNumber")]
        public string GxNumber { get; set; }
    }
}