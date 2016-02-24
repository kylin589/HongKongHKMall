using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace HKTHMall.WebApi.Models.Result
{
    public class ResponseConsumeDetails
    {
        [DataMember(Name = "consumeDT")]
        public long consumeDT { get; set; }
        [DataMember(Name = "consumeAccount")]
        public decimal consumeAccount { get; set; }
    }
}