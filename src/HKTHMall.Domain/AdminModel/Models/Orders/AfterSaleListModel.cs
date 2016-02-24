using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Orders
{

    public class AfterSaleListModel
    {
        /// <summary>
        /// 退换货服务单号
        /// </summary>
        public long returnOrderID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public long orderID { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal tradeAmount { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal refundAmount { get; set; }
        /// <summary>
        /// 退换货商品数量
        /// </summary>
        public int returnNumber { get; set; }
        /// <summary>
        /// 退换货状态
        /// </summary>
        public int returnStatus { get; set; }
        /// <summary>
        /// 订单状态(-1:无效订单；2:待付款,3:待发货,4:待收货,5:超时收货,6:已完成,7:已取消)
        /// </summary>
        public int orderStatus { get; set; }
        /// <summary>
        /// 商家ID
        /// </summary>
        public long merchantID { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        public string shopName { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int productNumber { get; set; }
        /// <summary>
        /// 下单时间戳
        /// </summary>
        public dynamic orderDate { get; set; }

        /// <summary>
        /// 订单详情ID
        /// </summary>
        public long orderDetailsID { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public long productId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// 商品图片地址
        /// </summary>
        public string picUrl { get; set; }
        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal salesPrice { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// sku名称
        /// </summary>
        public string skuName { get; set; }
        /// <summary>
        /// 是否已评价,0未评价,1已评价
        /// </summary>
        public int iscomment { get; set; }
        /// <summary>
        /// skuId
        /// </summary>
        public long skuId { get; set; }

        /// <summary>
        /// 更新时间戳
        /// </summary>
        public dynamic updateTime { get; set; }

        /// <summary>
        /// 创建时间戳
        /// </summary>
        public dynamic createTime { get; set; }
 
    }  
}
