using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Areas.Register.Models
{
    public class PhoneMsg:Object
    {
        /// <summary>
        /// 是否发送成功
        /// </summary>
        private bool _isMessage;

        /// <summary>
        /// 手机验证码
        /// </summary>
        private string _phoneCode;

        /// <summary>
        /// 消息
        /// </summary>
        private string _msg;


        public bool IsMessage
        {
            set { _isMessage = value; }
            get { return _isMessage; }
        }

        public string PhoneCode
        {
            set { _phoneCode = value; }
            get { return _phoneCode; }
        }

        public string Msg
        {
            set { _msg = value; }
            get { return _msg; }
        }
    }
}