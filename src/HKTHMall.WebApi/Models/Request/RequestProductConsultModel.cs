using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestProductConsultModel:RequestModel
    {
        public long productId { get; set; }
        public int pageNo { get; set; }
        public int pageSize { get; set; }
    }
}