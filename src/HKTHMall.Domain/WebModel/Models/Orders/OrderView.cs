using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.WebModel.Models.YH;

namespace HKTHMall.Domain.WebModel.Models.Orders
{
    /// <summary>
    /// 订单模型
    /// </summary>
    public class OrderView
    {
        public OrderView()
        {

            this.OrderAddressView = new OrderAddressView();
            this.OrderDetailViews = new List<OrderDetailsView>();
            this.YH_MerchantInfoView = new YH_MerchantInfoView();

        }

        public string OrderID { get; set; }
        public long UserID { get; set; }
        public long MerchantID { get; set; }
        public int OrderStatus { get; set; }
        public Nullable<decimal> OrderAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> CostAmount { get; set; }
        public Nullable<int> PayChannel { get; set; }
        public Nullable<int> PayType { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public Nullable<decimal> Vouchers { get; set; }
        public decimal ExpressMoney { get; set; }
        public int OrderSource { get; set; }
        public Nullable<int> PayDays { get; set; }
        public Nullable<int> DelayDays { get; set; }
        public string MerchantRemark { get; set; }
        public string Remark { get; set; }
        public Nullable<int> IsDisplay { get; set; }
        public Nullable<int> IsReward { get; set; }

        public int ComplaintStatus { get; set; }
        public int RefundFlag { get; set; }

        /// <summary>
        /// 是否生成采购单
        /// </summary>
        public int IsPurchase { get; set; }

        /// <summary>
        /// 快递公司Id
        /// </summary>
        public int ExpressID { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressOrder { get; set; }


        public virtual OrderAddressView OrderAddressView { get; set; }
        public virtual List<OrderDetailsView> OrderDetailViews { get; set; }

        public virtual YH_MerchantInfoView YH_MerchantInfoView { get; set; }






    }
}
