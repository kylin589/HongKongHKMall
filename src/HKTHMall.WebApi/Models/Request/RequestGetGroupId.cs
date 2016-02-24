using FluentValidation;
using FluentValidation.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestBase
    {
        /// <summary>
        /// 用户ID(加密)
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
    }

}