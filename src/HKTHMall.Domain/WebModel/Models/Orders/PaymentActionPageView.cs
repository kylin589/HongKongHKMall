using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.WebModel.Models.Users;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    /// <summary>
    /// 订单支付页面实体
    /// </summary>
    public class PaymentActionPageView
    {
        /// <summary>
        /// 支付单信息
        /// </summary>
        public PaymentOrderView PaymentOrderView { get; set; }

        /// <summary>
        /// 订单地址
        /// </summary>
        public OrderAddressView OrderAddressView { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoViewForPayment UserInfoViewForPayment { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public List<OrderDetailsForPayResultView> OrderDetailsForPayResultView { get; set; }
    }
}
