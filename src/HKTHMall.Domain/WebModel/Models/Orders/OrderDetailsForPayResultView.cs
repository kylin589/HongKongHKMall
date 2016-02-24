using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    public class OrderDetailsForPayResultView
    {
        public string OrderId { get; set; }
        public string ProductName { get; set; }

        public string ProductId { get; set; }
        
        public string TotalAmount { get; set; }
        /// <summary>
        /// 成交价
        /// </summary>
        public decimal SalesPrice { get; set; }

        /// <summary>
        /// 订单明细ID
        /// </summary>
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
            get { return (SubTotal / (RetateDays > 0 ? RetateDays : 1500)) * (ReateRedio > 0 ? ReateRedio : 1); }//商品总金额/天数*反利率
        }
        /// <summary>
        /// 某个商品总金额 
        /// </summary>
        public decimal SubTotal
        {
            get;
            set;
        }
    }
}
