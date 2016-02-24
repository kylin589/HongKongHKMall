using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 验证码
    /// </summary>
    public enum EImageVerifyCodeType
    {
        ModifyLoginPwd = 1,      //重置验证码

        UpdatePayPwd = 2//修改交易密码
    }
}
