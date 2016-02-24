using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    public class SearchOrderModel : Paged
    {
        public string OrderID { get; set; }

        public Nullable<System.DateTime> StartPaidDate { get; set; }

        public Nullable<System.DateTime> EndPaidDate { get; set; }

        public int OrderStatus { get; set; }

        public string ShopName { get; set; }
        public string NickName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public long UserID { get; set; }
    }
}
