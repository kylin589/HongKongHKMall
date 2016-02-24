using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.YHUser
{
    /// <summary>
    /// 商城用户搜索类
    /// zhoub 20150714
    /// </summary>
    public class SearchYHUserModel : Paged
    {
        public string Account { get; set; }

        public string RealName { get; set; }
        public string Phone { get; set; }
        public int IsLock { get; set; }

        public Nullable<DateTime> RegisterDateBegin { get; set; }
        public Nullable<DateTime> RegisterDateEnd { get; set; }
    }
}
