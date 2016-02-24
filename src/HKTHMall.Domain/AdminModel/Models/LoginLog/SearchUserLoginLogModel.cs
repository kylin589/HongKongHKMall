using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.LoginLog
{
    public class SearchUserLoginLogModel : Paged
    {
        public Nullable<long> UserID { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> BeginLoginTime { get; set; }
        public Nullable<System.DateTime> EndLoginTime { get; set; }

    }
}
