using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Framework.Mvc.Extensions;

namespace HKTHMall.Domain.Enum
{

    public static class OrderEnums
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public enum OrderStatus
        {
            /// <summary>
            /// 无效订单
            /// </summary>
            [EnumDescription("Invalid Order", -1)]
            Invalid = -1,

            /// <summary>
            /// 全部
            /// </summary>
            [EnumDescription("ALL", 0)]
            All = 0,

            /// <summary>
            /// 待付款
            /// </summary>
            [EnumDescription("Pending payment", 2)]
            Obligation = 2,

            /// <summary>
            /// 待发货
            /// </summary>
            [EnumDescription("Shipping pengding", 3)]
            WaitDeliver = 3,

            /// <summary>
            /// 待收货
            /// </summary>
            [EnumDescription("Receiving pending", 4)]
            WaitReceiving = 4,

            /// <summary>
            /// 已收货
            /// </summary>
            [EnumDescription("Received", 5)]
            OutTimeReceiving = 5,

            /// <summary>
            /// 已完成
            /// </summary>
            [EnumDescription("Completed", 6)]
            Completed = 6,

            /// <summary>
            /// 已取消
            /// </summary>
            [EnumDescription("Canceled", 7)]
            Canceled = 7,
            /// <summary>
            /// 交易关闭
            /// </summary>
            [EnumDescription("Trading closed", 8)]
            TradingClosed = 8

        }

        /// <summary>
        /// 时间段
        /// </summary>
        public enum TimeSpanType
        {
            /// <summary>
            /// 全部
            /// </summary>
            [EnumDescription("All", 0)]
            All = 0,

            /// <summary>
            /// 近半个月
            /// </summary>
            [EnumDescription("Nearly two weeks", 1)]
            HalfOfMonth = 1,

            /// <summary>
            /// 近三个月
            /// </summary>
            [EnumDescription("Nearly three months", 1)]
            ThreeMonths = 7,

            /// <summary>
            /// 更早
            /// </summary>
            [EnumDescription("Earlier", 1)]
            Earlier = 11
        }

        /// <summary>
        /// 支付通道
        /// </summary>
        public enum PayChannel
        {
            /// <summary>
            /// 余额支付
            /// </summary>
            [EnumDescription("Balance", 1)]
            Balance = 1,

            /// <summary>
            /// PayPal
            /// </summary>
            [EnumDescription("PayPal", 2)]
            PayPal = 2,

            /// <summary>
            /// VISA
            /// </summary>
            [EnumDescription("VISA", 3)]
            VISA = 3,
            /// <summary>
            /// Omise
            /// </summary>
            [EnumDescription("Omise", 4)]
            Omise = 4,
            /// <summary>
            ///货到付款
            /// </summary>
            [EnumDescription("COD", 5)]
            COD = 5,
            /// <summary>
            ///货到付款
            /// </summary>
            [EnumDescription("LinePay", 6)]
            LinePay = 6,

        }

        /// <summary>
        /// 支付类型
        /// </summary>
        public enum PayType
        {
            /// <summary>
            /// 余额支付
            /// </summary>
            [EnumDescription("Balance", 1)]
            BalancePay = 1,
            /// <summary>
            /// 第三方支付
            /// </summary>
            [EnumDescription("3rd Party Payment", 2)]
            ThirdPay = 2
        }

        /// <summary>
        /// 支付单类型
        /// </summary>
        public enum PaidType
        {
            /// <summary>
            /// 商城订单支付
            /// </summary>
            Mall = 1,
            /// <summary>
            /// 充值支付
            /// </summary>
            Recharge = 2

        }

        /// <summary>
        /// 支付订单状态标记
        /// </summary>
        public enum PaymentFlag
        {

            /// <summary>
            /// 未支付
            /// </summary>
            [EnumDescription("Unpaid", 1)]
            NonPaid = 1,

            /// <summary>
            /// 已支付
            /// </summary>
            [EnumDescription("Paid", 2)]
            Paid = 2,

            /// <summary>
            /// 取消
            /// </summary>
            [EnumDescription("Cancel", 3)]
            Cancel = 3
        }

        /// <summary>
        /// 订单来源
        /// </summary>
        public enum OrderSource
        {
            /// <summary>
            ///网站
            /// </summary>
            [EnumDescription("Website", 1)]
            Web = 1,
            /// <summary>
            /// 移动
            /// </summary>
            [EnumDescription("Mobile devices", 2)]
            Mobile = 2
        }

        /// <summary>
        /// 生成订单失败类型
        /// </summary>
        public enum GenerateOrderFailType
        {
            /// <summary>
            /// 处理中
            /// </summary>
            [EnumDescription("Processing", 1)]
            Processing = 1,
            /// <summary>
            /// 成功
            /// </summary>
            [EnumDescription("Success", 0)]
            Success = 0,
            /// <summary>
            /// 失败
            /// </summary>
            [EnumDescription("Failure", -1)]
            Fail = -1,
            /// <summary>
            /// 没有收货地址
            /// </summary>
            [EnumDescription("No delivery address", -2)]
            NotAddress = -2,
            /// <summary>
            /// 库存不足
            /// </summary>
            [EnumDescription("Inventory shortage", -3)]
            NotStock = -3,
            /// <summary>
            /// 商品已下架
            /// </summary>
            [EnumDescription("Product has been off the shelf", -4)]
            UnShelve = -4,
            /// <summary>
            /// 参数错误
            /// </summary>
            [EnumDescription("Parameter error", -5)]
            ParamError = -5
        }

        public enum PurchaseType
        {
            /// <summary>
            /// 正常购买
            /// </summary>
            Normal = 0,

            /// <summary>
            /// 立即购买
            /// </summary>
            Outright = 1
        }
    }


}
