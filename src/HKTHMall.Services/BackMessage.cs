using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.Services
{
    /// <summary>
    /// 回调消息 实体类
    /// </summary>
    public class BackMessage
    {
        public BackMessage(int status)
        {
            this.status = status;
        }
        public BackMessage()
        {

        } 
        /// <summary>
        /// 返回状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 返回消息 
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 返回自定义数据 
        /// </summary>
        public string data { get; set; }

    }
}