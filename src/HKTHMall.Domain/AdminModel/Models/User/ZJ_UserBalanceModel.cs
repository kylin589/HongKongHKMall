using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    /// <summary>
    /// 
    /// </summary>
    [Validator(typeof(ZJ_UserBalanceValidator))]
    public class ZJ_UserBalanceModel
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 用户余额
        /// </summary>
        public decimal ConsumeBalance { get; set; }

        /// <summary>
        /// 抵用金
        /// </summary>
        public decimal Vouchers { get; set; }

        /// <summary>
        /// 账户状态 （1 -正常 2-异常）
        /// </summary>
        public byte AccountStatus { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public Nullable<System.DateTime> CreateDT { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public Nullable<System.DateTime> UpdateDT { get; set; }

        /// <summary>
        /// 用户名（YH_User表,登陆账号）
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户手机 （YH_User表）
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户Email （YH_User表）
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 用户Email （YH_User表）
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 备注ZJ_UserBalanceChangeLog【用户账户金额异动记录表】(资金流水账)
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 异动金额(充值金额)ZJ_UserBalanceChangeLog【用户账户金额异动记录表】(资金流水账)
        /// </summary>
        public decimal AddOrCutAmount { get; set; }

        /// <summary>
        /// 异动类型 ZJ_UserBalanceChangeLog
        /// </summary>
        public int AddOrCutType { get; set; }

        /// <summary>
        /// 是否显示（0:否,1:是） ZJ_UserBalanceChangeLog
        /// </summary>
        public int IsDisplay { get; set; }

        /// <summary>
        /// 订单编号 ZJ_UserBalanceChangeLog
        /// </summary>
        public string OrderNo { get; set; }
    }
}
