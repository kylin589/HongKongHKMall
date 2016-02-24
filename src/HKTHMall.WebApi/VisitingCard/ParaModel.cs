using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.VisitingCard
{
    public class ParaModel
    {
        /// <summary>
        /// 用户ID 
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string HeadImageUrl { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
      
    }
}