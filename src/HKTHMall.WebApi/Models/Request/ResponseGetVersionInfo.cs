using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 返回版本信息 zzr
    /// </summary>
    public class ResponseGetVersionInfo
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string appName { get; set; }
        /// <summary>
        /// 包名
        /// </summary>
        public string packageName { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string versionNo { get; set; }
        /// <summary>
        /// 版本名称
        /// </summary>
        public string versionName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string fileSize { get; set; }
        /// <summary>
        /// 下载地址
        /// </summary>
        public string downloadURL { get; set; }
        /// <summary>
        /// 更新内容
        /// </summary>
        public string updateInfo { get; set; }
        /// <summary>
        /// 是否强制升级（0:否,1:是）
        /// </summary>
        public string isForceUpdate { get; set; }

    }
}