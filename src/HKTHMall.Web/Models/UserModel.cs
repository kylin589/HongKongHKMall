using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.Web.Models
{
    /// <summary>
    /// cookie保存类
    /// </summary>
    [Serializable]
    public class UserModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户类型（0:会员 1:商家（待定））
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 注册激活验证码
        /// </summary>
        public string ValidateEmail { get; set; }
    }
}