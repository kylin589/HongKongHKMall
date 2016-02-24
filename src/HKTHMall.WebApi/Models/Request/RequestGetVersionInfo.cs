using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestGetVersionInfo
    {
        /// <summary>
        /// 语言:1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }

        /// <summary>
        /// 包名
        /// </summary>
        public string packageName{get;set;}

        /// <summary>
        /// 版本编号
        /// </summary>
        public string versionNo{get;set;}

    }
}