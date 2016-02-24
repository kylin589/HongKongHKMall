using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    public class PaymentOrderModel
    {
        public string PaymentOrderID { get; set; }
        public long UserID { get; set; }
        public decimal ProductAmount { get; set; }
        public decimal RealAmount { get; set; }
        public int Flag { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }

        /// <summary>
        /// 支付类型（1：商城订单支付，2充值支付）
        /// </summary>
        public int PayType { get; set; }
        public string outOrderId { get; set; }
        public virtual ICollection<PaymentOrder_Orders> PaymentOrder_Orders { get; set; }

        public string OrderID { get; set; }

        public string Account { get; set; }

        public string RealName { get; set; }

        public string NickName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Nullable<System.DateTime> CreateDT { get; set; }

        /// <summary>
        /// 支付通道 2：贝宝支付，3国际信用卡支付
        /// </summary>
        public int? PayChannel { get; set; }
    }
}
