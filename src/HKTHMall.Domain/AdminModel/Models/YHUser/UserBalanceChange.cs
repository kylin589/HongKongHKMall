using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.YHUser
{
    public class UserBalanceChange
    {
        public int ID { get; set; }

        public long UserID { get; set; }
        public decimal AddOrCutAmount { get; set; }
        public int IsAddOrCut { get; set; }
        public decimal OldAmount { get; set; }
        public decimal NewAmount { get; set; }

        public int AddOrCutType { get; set; }
        public string OrderNo { get; set; }

        public string Remark { get; set; }
        public int IsDisplay { get; set; }
        public string CreateBy { get; set; }
        public Nullable<DateTime> CreateDT { get; set; }

        public string Account { get; set; }
        public string RealName { get; set; }
    }
}
