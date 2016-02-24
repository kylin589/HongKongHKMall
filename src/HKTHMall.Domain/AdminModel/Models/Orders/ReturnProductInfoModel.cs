using System;
using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.Orders;
using HKTHMall.Domain.Entities;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    [Validator(typeof (ReturnProductInfoValidator))]
    public class ReturnProductInfoModel
    {
        /// <summary>
        /// 退换货服务单号
        /// </summary>
        public string ReturnOrderID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 商品快照ID
        /// </summary>
        public Nullable<int> ProductSnapshotID { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TradeAmount { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退换货商品数量
        /// </summary>
        public int ReturntNumber { get; set; }

        /// <summary>
        /// 退货原因类型 1质量不好，2不想要了
        /// </summary>
        public int ReasonType { get; set; }

        /// <summary>
        /// 退换货说明
        /// </summary>
        public string Discription { get; set; }

        /// <summary>
        /// 退换货类型  退换货类型（1：换货，2：退货  3:维修）
        /// </summary>
        public int ReturnType { get; set; }

        /// <summary>
        /// 换货收货地址
        /// </summary>
        public string ReturnAddress { get; set; }

        /// <summary>
        /// 退换货人姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 退换货人手机
        /// </summary>
        public string ReceiverMobile { get; set; }

        /// <summary>
        /// 退货收货人座机
        /// </summary>
        public string ReceiverTel { get; set; }

        /// <summary>
        /// 退换货状态（1：申请中，2：审核通过，3申请已驳回，4确认收货，5退款完成，6申请已撤销
        /// </summary>
        public Nullable<int> ReturnStatus { get; set; }

        /// <summary>
        /// 商家退换货地址
        /// </summary>
        public string MerchantReturnAddress { get; set; }

        /// <summary>
        /// 客服回执
        /// </summary>
        public string ReturnText { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateTime { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public Nullable<System.DateTime> UpdateTime { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUser { get; set; }

        /// <summary>
        /// 确认收货人
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 确认收货时间
        /// </summary>
        public Nullable<System.DateTime> DeliveryDate { get; set; }

        /// <summary>
        /// 退款操作人
        /// </summary>
        public string RefundPerson { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        public Nullable<System.DateTime> RefundDate { get; set; }

        public virtual Order Order { get; set; }

        /// <summary>
        /// 用户名（用户表）
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 用户手机 （YH_User表）
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户Email （YH_User表）
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 用户昵称 （YH_User表）
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 商品名称（订单明细）
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 成本价（订单明细）
        /// </summary>
        public string CostPrice { get; set; }

        /// <summary>
        ///  销售价（订单明细）
        /// </summary>
        public string SalesPrice { get; set; }

        public string SkuName { get; set; }

        public string ShopName { get; set; }

        /// <summary>
        /// 订单明细主键
        /// </summary>
        public long OrderDetailsID { get; set; }

        /// <summary>
        /// 订单明细表 退货状态 （0正常，1退款申请中，2已退款，3审核未通过）
        /// </summary>
        public int IsReturn { get; set; }

        /// <summary>
        /// 订单明细表 商品数量
        /// </summary>
        public int Quantity { get; set; }
    }
}