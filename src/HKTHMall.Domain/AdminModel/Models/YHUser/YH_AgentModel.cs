using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.YHUser
{
    public class YH_AgentModel
    {
        /// <summary>
        /// 代理商ID
        /// </summary>
        public int AgentID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 代理商类型
        /// </summary>
        public int AgentType { get; set; }

        /// <summary>
        /// 代理商加盟费
        /// </summary>
        public decimal InitialFee { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public Nullable<DateTime> UpdateDT { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Nullable<DateTime> RegisterDate { get; set; }

        public int IsLock { get; set; }

        /// <summary>
        /// 真实姓名
        /// zhoub 20150924
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 用户帐号
        /// zhoub 20150924
        /// </summary>
        public string Account { get; set; }

    }
}
