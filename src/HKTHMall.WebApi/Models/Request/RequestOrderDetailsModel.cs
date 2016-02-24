using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 订单详情请求对象
    /// zhoub 20150810
    /// </summary>
    public class RequestOrderDetailsModel
    {
        public string orderNumber { get; set; }
        public string userId { get; set; }
        public int lang { get; set; }
        public int orderStatus { get; set; }

        public int? operType { get; set; }

        public string returnOrderID { get; set; }

        public string orderDetailsID { get; set; }
    }
}