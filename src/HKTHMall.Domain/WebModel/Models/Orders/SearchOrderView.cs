using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Enum;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    /// <summary>
    /// 订单列表搜索模型
    /// </summary>
    public class SearchOrderView
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public SearchOrderView()
        {
            this.pageSize = 5;
            this.s = OrderEnums.OrderStatus.All;
            this.d = OrderEnums.TimeSpanType.All;

        }

        private int _page = 1;

        /// <summary>
        /// 当前页
        /// </summary>
        public int page
        {
            get { return _page; }
            set { _page = value < 1 ? 1 : value; }
        }


        /// <summary>
        /// 每页显示量
        /// </summary>
        public int pageSize { get; set; }


        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderEnums.OrderStatus s { get; set; }

        /// <summary>
        /// 时间段
        /// </summary>
        public OrderEnums.TimeSpanType d { get; set; }

        /// <summary>
        /// 语言标识
        /// </summary>
        public int LanguageID { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long? UserID { get; set; }

        public int? Iscomment { get; set; }

    }
}
