using BrCms.Framework.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 是否已经审核(1:审核通过,2:待审核,3.拒审)
    /// <remarks>added by jimmy,2015-8-11</remarks>
    /// </summary>
   public enum EIsCheck
    {
        /// <summary>
        /// 审核通过
        /// </summary>
        [EnumDescription("Audit through", 1)]
       Auditthrough = 1,

        /// <summary>
        /// 待审核
        /// </summary>
        [EnumDescription("Pending audit", 2)]
        ToAudit = 2,

        /// <summary>
        /// 拒审
        /// </summary>
        [EnumDescription("Refused to trial", 3)]
        Refuse = 3,
    }
}
