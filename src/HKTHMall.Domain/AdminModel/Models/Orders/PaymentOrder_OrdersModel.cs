using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    public class PaymentOrder_OrdersModel
    {
        /// <summary>
        /// RelateID
        /// </summary>
        public long RelateID { get; set; }

        /// <summary>
        /// 支付编号
        /// </summary>
        public string PaymentOrderID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderID { get; set; }
    }
}
