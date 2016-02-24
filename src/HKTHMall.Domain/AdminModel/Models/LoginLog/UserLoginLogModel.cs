using FluentValidation.Attributes;
using HKTHMall.Domain.Validators.YH_UserLoginLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace HKTHMall.Domain.Models.LoginLog
{
    [Validator(typeof(UserLoginLogValidator))]
    public class UserLoginLogModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Nullable<long> UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录来源
        /// </summary>
        public Nullable<int> LoginSource { get; set; }

        /// <summary>
        /// IP
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
    }
}
