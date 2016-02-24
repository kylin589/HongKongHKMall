using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    /// <summary>
    /// 订单地址
    /// </summary>
    public class OrderAddressView
    {
        public long OrderAddressId { get; set; }
        public string OrderID { get; set; }
        public string Receiver { get; set; }
        public long THAreaID { get; set; }
        public string DetailsAddress { get; set; }
        public string PostalCode { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
