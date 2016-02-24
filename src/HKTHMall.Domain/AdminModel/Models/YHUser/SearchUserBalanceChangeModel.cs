using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.YHUser
{
    /// <summary>
    /// 余额变动搜索类
    /// zhoub 20150714
    /// </summary>
    public class SearchUserBalanceChangeModel : Paged
    {
        public Nullable<long> UserID { get; set; }
        public Nullable<int> ID { get; set; }
        public string Account { get; set; }

        public string RealName { get; set; }

        public int IsAddOrCut { get; set; }

        public int ? AddOrCutType { get; set; }

        public string CreateBy { get; set; }
        public string OrderNo { get; set; }
        public Nullable<DateTime> CreateDTBegin { get; set; }
        public Nullable<DateTime> CreateDTEnd { get; set; }
    }
}
