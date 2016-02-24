using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.User;
using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    /// <summary>
    /// 提现订单模型
    /// </summary>
    [Validator(typeof(ZJ_WithdrawOrderValidator))]
    public class ZJ_WithdrawOrderModel
    {
        public string OrderNO { get; set; }
        public long UserID { get; set; }
        public Nullable<decimal> WithdrawAmount { get; set; }
        public Nullable<decimal> WithdrawCommission { get; set; }
        public Nullable<System.DateTime> WithdrawDT { get; set; }
        public string Verifier { get; set; }
        public Nullable<System.DateTime> VerifyDT { get; set; }
        public string Remitter { get; set; }
        public Nullable<System.DateTime> RemittanceDT { get; set; }
        public string Remark { get; set; }
        public Nullable<int> WithdrawResult { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string BankSubbranch { get; set; }
        public string BankUserName { get; set; }
        public int IsDisplay { get; set; }
        public int OrderSource { get; set; }

        public virtual YH_User YH_User { get; set; }

        public string Account { get; set; }

        public string RealName { get; set; }

        public string Phone { get;set; }

        public string Email { get; set; }

        public string NickName{get;set;}
    }
}
