using FluentValidation.Attributes;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Validators.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    /// <summary>
    ///  订单类
    ///  zhoub 20150713
    /// </summary>
    public class OrderModel
    {
        public string OrderID { get; set; }

        public long UserID { get; set; }

        public long MerchantID { get; set; }

        public int OrderStatus { get; set; }

        public decimal OrderAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int PayChannel { get; set; }
        public int PayType { get; set; }
        public DateTime PaidDate { get; set; }
        public decimal Vouchers { get; set; }
        public decimal ExpressMoney { get; set; }
        public int OrderSource { get; set; }
        public int PayDays { get; set; }
        public int DelayDays { get; set; }
        public string MerchantRemark { get; set; }
        public string Remark { get; set; }
        public int IsDisplay { get; set; }
        public int IsReward { get; set; }

        public string ShopName { get; set; }
        public string Tel { get; set; }
        public string NickName { get; set; }

        public DateTime OrderDate { get; set; }

        public string DetailsAddress { get; set; }
        public string Receiver { get; set; }
        public string Mobile { get; set; }

        public string Email { get; set; }

        public int THAreaID { get; set; }

        public decimal CostAmount { get; set; }

        public int RefundFlag { get; set; }

        public long SupplierId { get; set; }

        public string Content { get; set; }

        public int ComplaintStatus { get; set; }
        /// <summary>
        /// 电话 OrderAddress
        /// </summary>
        public string Phone { get; set; }
    }
}
