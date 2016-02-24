using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    /// <summary>
    /// 响应登录结果
    /// </summary>
    public class ResponseLoginValidate
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// 性别 1男  2女  0未设置
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long userId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 头像图片地址
        /// </summary>
        public string imageUrl { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string realName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 推荐人真实姓名或昵称
        /// </summary>
        public string referrer { get; set; }
        /// <summary>
        /// 推荐人手机号码
        /// </summary>
        public string referrerPhone { get; set; }
        /// <summary>
        /// 个人二维码地址
        /// </summary>
        public string orcodeUrl { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string version { get; set; }

        public string isPayPassWord { get; set; }

        
    }
}