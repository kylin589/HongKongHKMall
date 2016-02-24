using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.Web.Account
{
    //信息提示语
    public class PromptString
    {
    }

    public class DataMSg
    {
        /// <summary>
        /// 是否验证成功
        /// </summary>
        private bool _isTrue;

        /// <summary>
        /// 消息
        /// </summary>
        private string _msg;


        public bool IsTrue
        {
            set { _isTrue = value; }
            get { return _isTrue; }
        }


        public string Msg
        {
            set { _msg = value; }
            get { return _msg; }
        }
    }


    public class UserMsg
    {
        /// <summary>
        /// 是否验证成功
        /// </summary>
        private bool _flag=false;

        /// <summary>
        /// 消息
        /// </summary>
        private string _strMsg;


        public bool flag
        {
            set { _flag = value; }
            get { return _flag; }
        }


        public string strMsg
        {
            set { _strMsg = value; }
            get { return _strMsg; }
        }
    }


}