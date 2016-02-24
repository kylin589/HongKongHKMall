using FluentValidation.Attributes;
using HKTHMall.Domain.Validators.YH_UserLoginLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.LoginLog
{
    [Validator(typeof(AC_OperateLogValidator))]
    public class AC_OperateLogModel
    {
        /// <summary>
        /// 操作日志ID
        /// </summary>
        public long OperateID { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperateName { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public System.DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperateContent { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }
    }
}
