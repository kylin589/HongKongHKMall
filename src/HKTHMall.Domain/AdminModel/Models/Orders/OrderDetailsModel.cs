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
    /// 订单详情
    /// zhoub 20150713
    /// </summary>
    public class OrderDetailsModel
    {
        public long OrderDetailsID { get; set; }
        public string OrderID { get; set; }
        public string ProductName { get; set; }
        public int ProductSnapshotID { get; set; }
        public long ProductId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalesPrice { get; set; }
        public string DiscountInfo { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public long SKU_ProducId { get; set; }
        public string SkuName { get; set; }
        public decimal SubTotal { get; set; }
        public int Iscomment { get; set; }
        public string PicUrl { get; set; }

        public decimal ExpressMoney { get; set; }

        public decimal TotalAmount { get; set; }

        public Nullable<DateTime> OrderDate { get; set; }
        public string ShopName { get; set; }

        public long SupplierId { get; set; }
        public int RetateDays { get; set; }
        public decimal ReateRedio { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime
        {
            get
            {
                return StartTime.AddDays(RetateDays);
            }        
        }


    }
}
