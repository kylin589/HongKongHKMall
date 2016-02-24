using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.CashBack
{
    public class OderModel
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 买家ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 卖家ID
        /// </summary>
        public long MerchantID { get; set; }
        /// <summary>
        ///订单状态 2：待付款，3：待发货，4：待收货，5：已收货，6：已完成，7：已取消,8交易关闭
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 订单商品金额
        /// </summary>
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 订单成本
        /// </summary>
        public decimal CostAmount { get; set; }
        /// <summary>
        /// 支付渠道（1：余额支付；2：PayPal.3:VISA(暂时未用),4: omise，5货到付款)
        /// </summary>
        public int PayChannel { get; set; }
        /// <summary>
        /// 支付方式（1.余额支付,2第三方支付
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public System.DateTime OrderDate { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public Nullable<System.DateTime> PaidDate { get; set; }
        /// <summary>
        /// 抵用金
        /// </summary>
        public decimal Vouchers { get; set; }
        /// <summary>
        /// 快递费
        /// </summary>
        public decimal ExpressMoney { get; set; }
        /// <summary>
        /// 订单来源（1： 网站；2：移动设备）
        /// </summary>
        public int OrderSource { get; set; }
        /// <summary>
        /// 支付时限天数
        /// </summary>
        public int PayDays { get; set; }
        /// <summary>
        /// 延迟收货天数
        /// </summary>
        public int DelayDays { get; set; }
        /// <summary>
        /// 商家备注
        /// </summary>
        public string MerchantRemark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否显示（0：不显示，1：显示）
        /// </summary>
        public int IsDisplay { get; set; }
        /// <summary>
        /// 是否执行返利
        /// </summary>
        public int IsReward { get; set; }
        /// <summary>
        /// 投诉状态 0未投诉，1已投诉
        /// </summary>
        public int ComplaintStatus { get; set; }
        /// <summary>
        ///退款标识 0正常，1退款中，2已处理（包括成功，失败）
        /// </summary>
        public int RefundFlag { get; set; }
        /// <summary>
        /// 是否生成采购单 0没有生成，1已生成
        /// </summary>
        public Nullable<int> IsPurchase { get; set; }
        /// <summary>
        /// 快递公司ID 
        /// </summary>
        public Nullable<int> ExpressID { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressOrder { get; set; }
    }
}
