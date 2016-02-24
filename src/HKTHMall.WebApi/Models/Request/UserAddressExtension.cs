using HKTHMall.Domain.WebModel.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    [DataContract]
    public class UserAddressExtension : UserAddress
    {

        [DataMember(Name = "userIdEnc")]
        public string userIdEnc { get; set; }
        [DataMember(Name = "lang")]
        public int lang { get; set; }
        [DataMember(Name = "defaultAddress")]
        public int defaultAddress { get; set; }
        [DataMember(Name = "phoneEnc")]
        public string phoneEnc { get; set; }
        [DataMember(Name = "mobileEnc")]
        public string mobileEnc { get; set; }
    }
}