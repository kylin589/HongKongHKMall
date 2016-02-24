using BrCms.Framework.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 1 未结算（已下单），2已发货（供应商发货），3已收货，4已结算
    /// </summary>
    public enum EPurchaseOrderStatus
    {
        /// <summary>
        /// 未结算（已下单）
        /// </summary>
        [EnumDescription("Unsettled (Ordered)", 1)]
        Ordered = 1,

        /// <summary>
        /// 已收货
        /// </summary>
        [EnumDescription("Received", 3)]
        Received = 3
    }
}
