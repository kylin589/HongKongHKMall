using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestExplosiveGoods 
    {
        /// <summary>
        /// 语言:1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
        /// <summary>
        /// 是否首页(首页0,列表1)
        /// </summary>
        public int isFirstPage { get; set; }
        /// <summary>
        /// 爆款数量
        /// </summary>
        public int bannerSize { get; set; }
        /// <summary>
        /// 分页码
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int pageSize { get; set; }
    }
}