using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{

    /// <summary>
    /// 获取产品详细信息
    /// </summary>
    public class RequestProductInfoModel
    {
        /// <summary>
        /// 语言:1、中文 2、英文 3、泰文
        /// </summary>
        public int lang { get; set; }
         
        /// <summary>
        /// 产品编号
        /// </summary>
        public long productId { get; set; }


        /// <summary>
        /// 用户ID
        /// </summary>
        public string userId { get; set; }

    }


     
}