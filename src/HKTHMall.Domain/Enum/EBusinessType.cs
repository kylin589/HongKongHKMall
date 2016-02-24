using BrCms.Framework.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 业务类型(1、注册 2、找回密码 3、设置交易密码 4、修改交易密码,5、修改登陆密码)
    /// </summary>
    public enum EBusinessType
    {
        /// <summary>
        /// 注册 
        /// </summary>
        [EnumDescription("Registered", 1)]
        Registered = 1,
        /// <summary>
        /// 找回密码 
        /// </summary>
        [EnumDescription("RetrievePwd", 2)]
        RetrievePwd = 2,
        /// <summary>
        /// 设置交易密码 
        /// </summary>
        [EnumDescription("SetTradingPwd", 3)]
        SetTradingPwd = 3,
        /// <summary>
        /// 修改交易密码 
        /// </summary>
        [EnumDescription("ModifyTradingPwd", 4)]
        ModifyTradingPwd = 4,
        /// <summary>
        /// 修改登陆密码 
        /// </summary>
        [EnumDescription("Registered", 5)]
        ModifyLoginPwd = 5
    }
}
