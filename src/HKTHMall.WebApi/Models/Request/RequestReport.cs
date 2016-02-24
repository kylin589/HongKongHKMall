using FluentValidation;
using FluentValidation.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestReport
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 被举报人ID(加密)
        /// </summary>
        public string reportId { get; set; }
        /// <summary>
        /// 举报原因（1.色情,2.骚扰广告,3.侮辱诋毁,4.诈骗钱财,5.其他）
        /// </summary>
        public int reasonType { get; set; }
        /// <summary>
        /// 补充说明（不允诉为空,如未写默认为未说明。）
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
    }
}