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
    /// 用户银行模型
    /// </summary>
    [Validator(typeof(YH_UserBankAccountValidator))]
    public class YH_UserBankAccountModel
    {
        public int ID { get; set; }
        public int BankID { get; set; }
        public Nullable<long> UserID { get; set; }
        public int IsDefault { get; set; }
        public string BankAccount { get; set; }
        public string BankSubbranch { get; set; }
        public string BankUserName { get; set; }
        public string BankAddress { get; set; }
        public int IsUse { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }

        public virtual BD_Bank BD_Bank { get; set; }
        public virtual YH_User YH_User { get; set; }

        public string Account { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string BankName { get; set; }

        public string RealName { get; set; }

    }

    public class UserBankModel
    {
        public int ID { get; set; }
        public int BankID { get; set; }
        public long UserID { get; set; }
        public int IsDefault { get; set; }
        public string BankAccount { get; set; }
        public string BankSubbranch { get; set; }
        public string BankUserName { get; set; }
        public string BankAddress { get; set; }
        public int IsUse { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDT { get; set; }
    }

}
