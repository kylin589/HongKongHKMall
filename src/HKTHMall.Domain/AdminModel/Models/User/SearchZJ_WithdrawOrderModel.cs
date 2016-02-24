using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    public class SearchZJ_WithdrawOrderModel : Paged
    {
        public string OrderNO { get; set; }

        public string Account { get; set; }

        public string RealName { get; set; }

        public string Verifier { get; set; }

        public string Remitter { get; set; }

        public string BankAccount { get; set; }

        public string BankSubbranch { get; set; }

        public string BankUserName { get; set; }

        public Nullable<DateTime> BeginPaymentDate { get; set; }
        public Nullable<DateTime> EndPaymentDate { get; set; }

        public int ? OrderSource { get; set; }

        public int? WithdrawResult { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
