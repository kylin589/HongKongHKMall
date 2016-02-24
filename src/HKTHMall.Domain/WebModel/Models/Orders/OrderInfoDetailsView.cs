using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    public class OrderInfoDetailsView
    {
        public long OrderDetailsID { get; set; }
        public string OrderNumber { get; set; }
        public long ProductID { get; set; }
        public int ProductNumber { get; set; }
        public decimal TransactionPrice { get; set; }
        public decimal ExpressMoney { get; set; }
        public string Remark { get; set; }
        public int IsDisplay { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDT { get; set; }
        public string SkuNumber { get; set; }
        public string ImageURL { get; set; }
    }
}
