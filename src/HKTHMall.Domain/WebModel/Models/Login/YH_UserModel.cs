using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Login
{
    public class YH_UserModel
    {
        public long UserID { get; set; }
        public string Account { get; set; }
        public string PassWord { get; set; }
        public string PayPassWord { get; set; }
        public string NickName { get; set; }
        public string HeadImageUrl { get; set; }
        public byte Sex { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public Nullable<byte> RegisterSource { get; set; }
        public Nullable<byte> ActivePhone { get; set; }
        public Nullable<byte> ActiveEmail { get; set; }
        public Nullable<int> RecommendCode { get; set; }
        public long ReferrerID { get; set; }
        public long ParentID2 { get; set; }
        public long ParentID3 { get; set; }
        public string ParentIDs { get; set; }
        public int Level { get; set; }
        public byte UserType { get; set; }
        public Nullable<byte> IsLock { get; set; }
        public Nullable<byte> IsDelete { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
        public int THAreaID { get; set; }
        public string DetailsAddress { get; set; }
        public string OrcodeUrl { get; set; }
        public decimal ConsumeBalance { get; set; }

        public string ThirdID { get; set; }
        public int ThirdType { get; set; }

    }
}
