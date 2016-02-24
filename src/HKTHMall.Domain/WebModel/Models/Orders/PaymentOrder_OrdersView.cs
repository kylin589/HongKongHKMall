using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    public class PaymentOrder_OrdersView
    {
        public long RelateID { get; set; }
        public string PaymentOrderID { get; set; }
        public string OrderID { get; set; }
    }
}
