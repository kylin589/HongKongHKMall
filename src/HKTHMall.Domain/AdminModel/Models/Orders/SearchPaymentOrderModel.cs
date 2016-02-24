using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    public class SearchPaymentOrderModel : Paged
    {
        public string OrderID { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string outOrderId { get; set; }

        public int ? Flag { get; set; }

        public int? PayChannel { get; set; }

        public Nullable<DateTime> BeginPaymentDate { get; set; }
        public Nullable<DateTime> EndPaymentDate { get; set; }
     }
}
