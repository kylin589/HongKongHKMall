using BrCms.Framework.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 订单来源（0: 网站；1:移动设备）
    /// </summary>
    public enum IOrderSource
    {
        /// <summary>
        /// 网站
        /// </summary>
        [EnumDescription("Website", 0)]
        WebSite = 0,

        /// <summary>
        /// 移动设备
        /// </summary>
        [EnumDescription("Mobile devices", 1)]
        Mobile = 1
    }
}
