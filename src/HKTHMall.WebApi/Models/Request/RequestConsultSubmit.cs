using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 提交商品咨询 朱志容
    /// </summary>
    public class RequestConsultSubmit
    {
        /// <summary>
        /// 商品咨询
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public long productId { get; set; }
        /// <summary>
        /// 是否匿名1:是0：否
        /// </summary>
        public bool isanonymous { get; set; }
        /// <summary>
        /// 语语1：中文2、英文3、泰文
        /// </summary>
        public int lang { get; set; }
        /// <summary>
        /// 咨询内容
        /// </summary>
        public string content { get; set; }
    }
}