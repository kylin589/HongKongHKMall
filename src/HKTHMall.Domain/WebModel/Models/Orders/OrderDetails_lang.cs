using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    /// <summary>
    /// 订单详情语言包
    /// </summary>
    public class OrderDetails_lang
    {
        public long OrderDetails_langId { get; set; }
        public long OrderDetailsID { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int LanguageID { get; set; }
    }
}
