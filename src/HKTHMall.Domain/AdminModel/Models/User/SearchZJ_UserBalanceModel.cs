using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    public class SearchZJ_UserBalanceModel:Paged
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 用户名（YH_User表,登陆账号）
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户Email （YH_User表）
        /// </summary>
        public string Email { get; set; }

         /// <summary>
        /// 用户手机 （YH_User表）
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }
    }
}
