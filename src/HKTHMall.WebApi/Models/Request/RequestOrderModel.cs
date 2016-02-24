using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestOrderModel
    {
        /// <summary>
        /// 订单获取请求对象
        /// zhoub 20150807
        /// </summary>
        public string userId { get; set; }
        public int orderStatus { get; set; }
        public int lang { get; set; }
        public int pageNo { get; set; }
        public int pageSize { get; set; }
    }
}