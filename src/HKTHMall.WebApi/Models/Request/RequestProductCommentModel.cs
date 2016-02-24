using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestProductCommentModel
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public Nullable<long> productId { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PagedIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PagedSize { get; set; }

        /// <summary>
        /// 语言1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }

        /// <summary>
        /// 1：好评；2：中评；3：差评  
        /// <remarks>added by jimmy,2015-9-11</remarks>
        /// </summary>
        public int ? typeLevel { get; set; }
    }
}