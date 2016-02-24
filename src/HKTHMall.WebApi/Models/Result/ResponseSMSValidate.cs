using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models
{
    public class ResponseSMSValidate
    {
        public string key { get; set; }

        [DataContract]
        public class FindPasswordCheckCodeResult
        {
            [DataMember(Name = "key")]
            public string Key { get; set; }

            [DataMember(Name = "validateCode")]
            public string ValidateCode { get; set; }
        }
    }
}