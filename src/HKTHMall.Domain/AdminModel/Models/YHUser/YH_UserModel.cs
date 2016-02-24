using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.YHUser
{
    public class YH_UserModel
    {
        public long UserID { get; set; }
        public string Account { get; set; }
        public string PassWord { get; set; }
        public string PayPassWord { get; set; }
        public string NickName { get; set; }
        public string HeadImageUrl { get; set; }
        public int Sex { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<DateTime> Birthday { get; set; }
        public Nullable<DateTime> RegisterDate { get; set; }
        public int RegisterSource { get; set; }
        public int ActivePhone { get; set; }
        public int RecommendCode { get; set; }
        public int ReferrerID { get; set; }
        public int ParentID2 { get; set; }
        public int ParentID3 { get; set; }
        public string ParentIDs { get; set; }
        public int Level { get; set; }
        public int UserType { get; set; }
        public int IsLock { get; set; }
        public int IsDelete { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDT { get; set; }
        public string THAreaID { get; set; }
        public string DetailsAddress { get; set; }
        public string signature { get; set; }
        public string OrcodeUrl { get; set; }
        public int GanEnCount { get; set; }

        /// <summary>
        /// 代理商类型
        /// </summary>
        public int AgentType { get; set; }


    }
}
