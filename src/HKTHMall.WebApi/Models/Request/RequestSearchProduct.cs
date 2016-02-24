using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 产品搜索（关键字） zzr
    /// </summary>
    public class RequestSearchProduct
    {
        /// <summary>
        /// 语言:1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string keyword { get; set; }
        /// <summary>
        /// 起始页
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 分页数
        /// </summary>
        public int pageSize { get; set; }
    }
}