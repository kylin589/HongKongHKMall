using FluentValidation;
using FluentValidation.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestGetGroupList
    {
        /// <summary>
        /// 用户ID(加密)
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉）
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 分页码
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
    }
}