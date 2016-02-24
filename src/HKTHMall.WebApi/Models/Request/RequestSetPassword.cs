using FluentValidation;
using FluentValidation.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models
{
    public class RequestSetPassword
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 密码类型 1登陆密码 2交易密码
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string validateCode { get; set; }
        /// <summary>
        /// 验证码唯一识别码
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
    }
}