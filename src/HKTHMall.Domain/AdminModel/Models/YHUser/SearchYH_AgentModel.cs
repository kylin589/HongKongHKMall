using BrCms.Framework.Collections;
using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models
{
    public partial class SearchYH_AgentModel:Paged
    {
        public int AgentID { get; set; }
        public long UserID { get; set; }
        public int AgentType { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
        public int IsLock { get; set; }
        public decimal InitialFee { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }

        public bool IsGlobalAgency { get; set; }

        public decimal Earnings { get; set; }

        public virtual SearchYH_UserModel YH_User { get; set; }

        public Nullable<DateTime> RegisterDateBegin { get; set; }
        public Nullable<DateTime> RegisterDateEnd { get; set; }
    }
}
