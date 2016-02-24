using FluentValidation.Attributes;
using HKTHMall.Domain.Validators.YH_UserLoginLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.LoginLog
{
    [Validator(typeof(YH_UserLoginLogValidator))]
    public class YH_UserLoginLogModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Nullable<long> UserID { get; set; }
        /// <summary>
        /// 登录来源
        /// </summary>
        public int LoginSource { get; set; }
        /// <summary>
        /// 登录IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 登录地址
        /// </summary>
        public string LoginAddress { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public Nullable<System.DateTime> LoginTime { get; set; }

        /// <summary>
        /// 用户名（会员表）
        /// </summary>
        public string Account { get; set; }
        
        
    }
}
