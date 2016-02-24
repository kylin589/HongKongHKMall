using BrCms.Framework.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{

    /// <summary>
    /// 提现结果,1:待审核,2:已审核,待打款,3:审核不通过,4:已打款,5:打款失败,
    /// <remarks>added by jimmy,2015-7-21</remarks>
    /// </summary>
    public enum IWithdrawResult
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [EnumDescription("Pending", 1)]
        ToAudit = 1,
        /// <summary>
        /// 已审核待打款
        /// </summary>
        [EnumDescription("Has been audited to fight", 2)]
        ApprovedMoney = 2,
        /// <summary>
        /// 审核不通过
        /// </summary>
        [EnumDescription("Audit not passed", 3)]
        AuditNotThrough = 3,
        /// <summary>
        /// 已打款
        /// </summary>
        [EnumDescription("He has to fight money", 4)]
        HaveMoney = 4,
        /// <summary>
        /// 打款失败
        /// </summary>
        [EnumDescription("Payment failed", 5)]
        Playfail = 5,
    }
}
