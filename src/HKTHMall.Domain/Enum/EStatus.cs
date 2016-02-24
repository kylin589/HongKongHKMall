using BrCms.Framework.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 处理状态 0-待处理 1-已处理
    /// </summary>
    public enum EStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        [EnumDescription("Pending", 0)]
        Processed = 0,

        /// <summary>
        /// 已处理
        /// </summary>
        [EnumDescription("Processed", 1)]
        HaveDeal = 1
    }
}
