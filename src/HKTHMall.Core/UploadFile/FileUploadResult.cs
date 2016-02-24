using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.Core.UploadFile
{
    public class FileUploadResult
    {
        /// <summary>
        /// 上传结果 成功失败
        /// </summary>
        public bool result { get; set; }
        /// <summary>
        /// 上传结果说明
        /// </summary>
        public string ResultExplain { get; set; }
        /// <summary>
        /// 上传的地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 上传的文件的名字
        /// </summary>
        public string name { get; set; }
    }
}