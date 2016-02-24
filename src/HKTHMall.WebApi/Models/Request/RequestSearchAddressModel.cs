using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models
{
    public class RequestSearchByAddressModel : RequestSearchByUserModel
    {
        public long userAddressId { get; set; }
    }
}