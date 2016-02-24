using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.LoginLog
{
    public class SearchAC_OperateLogModel: Paged
    {
        public Nullable<long> UserID { get; set; }
        public string OperateName { get; set; }
        public Nullable<System.DateTime> BeginOperateTime { get; set; }
        public Nullable<System.DateTime> EndOperateTime { get; set; }
    }
}
