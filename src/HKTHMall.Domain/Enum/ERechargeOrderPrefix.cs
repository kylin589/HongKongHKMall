using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Framework.Mvc.Extensions;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 充值订单前缀
    /// </summary>
    public enum ERechargeOrderPrefix
    {
        [EnumDescription("C", 1)]
        Normal = 1,       //正常的充值单
        [EnumDescription("M", 1)]
        Mixture = 2       //混合支付的充值单
    }
}
