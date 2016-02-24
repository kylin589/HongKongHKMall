using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    /// <summary>
    /// 订单搜索页搜索模型
    /// </summary>
    public class SearchOrderDetailView
    {
        public string OrderID { get; set; }
        public long? UserID { get; set; }
        public long? MerchantID { get; set; }
        public int? OrderStatus { get; set; }
        public int? LanguageID { get; set; }

        public int?  Iscomment { get; set; }
    }
}
