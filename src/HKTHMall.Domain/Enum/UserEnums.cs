using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 有关用户表的枚举值
    /// </summary>
    public static class UserEnums
    {
        /// <summary>
        /// 密码类型
        /// </summary>
        public enum PasswordType
        {
            /// <summary>
            /// 登陆密码
            /// </summary>
            LoginPassWord = 1,

            /// <summary>
            /// 交易密码
            /// </summary>
            PayPassword = 2

        }
    }
}
