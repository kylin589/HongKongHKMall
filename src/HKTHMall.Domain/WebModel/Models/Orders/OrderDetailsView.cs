using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    /// <summary>
    /// 订单详情页模型
    /// </summary>
    public class OrderDetailsView
    {

        public long SKU_ProducId { get; set; }

        public string OrderId { get; set; }

        public long ProductId { get; set; }

        public int Quantity { get; set; }

        public string ProductName { get; set; }

        public string PicUrl { get; set; }

        /// <summary>
        /// 成交价
        /// </summary>
        public decimal SalesPrice { get; set; }

        /// <summary>
        /// 小计
        /// </summary>
        public decimal TotalPrice
        {
            get { return SalesPrice * Quantity; }
        }

        public int Iscomment { get; set; }

        public string SkuName { get; set; }

        public int IsReturn { get; set; }

        public long OrderDetailsID { get; set; }
        /// <summary>
        /// 返现天数
        /// </summary>
        public long RetateDays { get; set; }
        /// <summary>
        /// 返现比例
        /// </summary>
        public decimal ReateRedio { get; set; }
        /// <summary>
        /// 每天返现金额
        /// </summary>
        public decimal ReatePrice 
        {
            get { return SalesPrice / RetateDays * ReateRedio; }
        }


    }
}

