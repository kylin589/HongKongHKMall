using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 订单售后列表实体类
    /// </summary>
    public class RequestAfterSaleListModel
    {
        /// <summary>
        /// 用户ID(RSA加密)
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public int lang { get; set; }

        private int _pageNo = 1;

        /// <summary>
        /// 当前页
        /// </summary>
        public int pageNo
        {
            get { return _pageNo; }
            set { _pageNo = value < 1 ? 1 : value; }
        }


        /// <summary>
        /// 每页显示量
        /// </summary>
        public int pageSize { get; set; }
    }
}