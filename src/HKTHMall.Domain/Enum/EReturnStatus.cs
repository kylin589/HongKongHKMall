using BrCms.Framework.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 退款状态
    /// zhoub 20150728
    /// </summary>
    public enum EReturnStatus
    {
        /// <summary>
        /// 申请中
        /// </summary>
        [EnumDescription("Application", 1)]
        Application = 1,

        /// <summary>
        /// 审核通过
        /// </summary>
        [EnumDescription("Audit through", 2)]
        Auditthrough= 2,

        /// <summary>
        /// 申请已驳回
        /// </summary>
        [EnumDescription("Application has been rejected", 3)]
        Reject = 3,

        /// <summary>
        /// 确认收货
        /// </summary>
        [EnumDescription("Confirm receipt", 4)]
        ConfirmReceipt= 4,

        /// <summary>
        /// 退款完成
        /// </summary>
        [EnumDescription("Confirm receipt", 5)]
        Refund = 5,

        /// <summary>
        /// 申请已撤销
        /// </summary>
        [EnumDescription("Application has been revoked", 6)]
        Revoke = 6
    }
}
