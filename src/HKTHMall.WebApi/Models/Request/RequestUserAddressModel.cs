using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 用户地址请求
    /// </summary>
    public class RequestUserAddressModel
    {
        public RequestUserAddressModel()
        {
            this.lang = 1;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 语言Id
        /// </summary>
        public int lang { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }
    }
}