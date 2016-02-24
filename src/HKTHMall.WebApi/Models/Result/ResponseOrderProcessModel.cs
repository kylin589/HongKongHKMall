using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ResponseOrderProcessModel
    {
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}