using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models
{
    public class RequestSearchByUserModel : RequestModel
    {
        public string userId { get; set; }
    }
}