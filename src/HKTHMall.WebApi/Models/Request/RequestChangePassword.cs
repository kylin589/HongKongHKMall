using FluentValidation;
using FluentValidation.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestChangePassword
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
        /// 新密码（加密）
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 旧密码（加密）
        /// </summary>
        public string oldPassword { get; set; }
        /// <summary>
        /// 操作终端的设备类型（1网站 2安卓 3 IOS）
        /// </summary>
        public int registerSource { get; set; }
        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
    }
}