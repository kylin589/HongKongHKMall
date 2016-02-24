using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Users
{
    public class UserBalanceView
    {
        public long UserID { get; set; }
        public decimal ConsumeBalance { get; set; }
        public decimal Vouchers { get; set; }
        public byte AccountStatus { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
    }
}
