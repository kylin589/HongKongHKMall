using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models
{
    public class RequestSearchPagingByUserModel : RequestSearchByUserModel
    {
        public int pageNo { get; set; }
        public int pageSize { get; set; }
    }
}