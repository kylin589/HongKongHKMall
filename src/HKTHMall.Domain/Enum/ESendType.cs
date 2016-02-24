using BrCms.Framework.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 发送类型（1:验证码,2:任意内容）
    /// </summary>
    public enum ESendType
    {
        /// <summary>
        /// 验证码
        /// </summary>
        [EnumDescription("VerifiCode", 1)]
        VerifiCode = 1,
        /// <summary>
        /// 任意内容
        /// </summary>
        [EnumDescription("AnyContent", 2)]
        AnyContent = 2
    }
}
