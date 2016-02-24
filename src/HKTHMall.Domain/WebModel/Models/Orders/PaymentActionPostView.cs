using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    /// <summary>
    /// 支付提交数据
    /// </summary>
    public class PaymentActionPostView
    {
        public int PayChannel { get; set; }

        public int PayChannel2 { get; set; }

        public string PaymentOrderId { get; set; }

        public string PayPassword { get; set; }
    }
}
