using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 获取产品列表请求参数 zzr
    /// </summary>
    public class RequestProductModel
    {
        public RequestProductModel()
        {
            level = 3;
        }
        /// <summary>
        /// 语言:1、中文 2、英文 3、泰文
        /// </summary>
        public int lang { get; set; }
        /// <summary>
        /// 商口分类（三级）
        /// </summary>
        public int categoryId { get; set; }
        /// <summary>
        /// 排序字段（0综合,1销量优先,3价格降序,4价格升序）
        /// </summary>
        public int orderBy { get; set; }
        /// <summary>
        /// 分页码
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int pageSize { get; set; }  

        /// <summary>
        /// 分类级别（1，2，3）默认是3
        /// </summary>
        public int level { get; set; }
    }
}