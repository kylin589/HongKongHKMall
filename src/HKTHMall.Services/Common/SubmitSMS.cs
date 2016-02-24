using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Common
{
     [Serializable]
    public class SubmitSMS
    {
        /// <summary>
        /// 发送账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 发送密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 发送类型（1:发送验证码；2:发送任意内容）
        /// </summary>
        public int SendType { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 短信内容,可以是验证码和任意内容（如果是验证码短信,短信内容就只有验证码）
        /// </summary>
        public string SMSContent { get; set; }

        /// <summary>
        /// 业务系统ID
        /// </summary>
        public int InvokerID { get; set; }

        /// <summary>
        /// 业务类型（由各业务系统自定义后提交到短信平台,例如0:其他、1:注册、2:活动、3:支付、4:提现、5:转账、6:告警、7:找回密码）
        /// </summary>
        public int BusinessType { get; set; }
    }
}
