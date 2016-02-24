using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Enum;

namespace HKTHMall.Services.Common.MultiLangKeys
{
    /// <summary>
    /// 多语言订单状态
    /// </summary>
    public static class ML_OrderStatus
    {
        /// <summary>
        /// 中文状态集合
        /// </summary>
        public static readonly Dictionary<int, string> OrderStatuss_zh_CN = new Dictionary<int, string>
        {
            {-1,"无效订单"},
            {0,"全部"},
            {2,"待付款"},
            {3,"待发货"},
            {4,"待收货"},
            {5,"已收货"},
            {6,"已完成"},
            {7,"已取消"},
            {8,"交易关闭"}
        };

        /// <summary>
        /// 泰语状态集合
        /// </summary>
        public static readonly Dictionary<int, string> OrderStatuss_th_TH = new Dictionary<int, string>
        {
            {-1,"无效订单"},
            {0,"ทั้งหมด"},
            {2,"ยังไม่ชำระ"},
            {3,"ยังไม่จัดส่ง"},
            {4,"ยังไม่ได้รับสินค้า"},
            {5,"ได้รับสินค้าแล้ว"},
            {6,"เรียบร้อย"},
            {7,"ยกเลิกแล้ว"},
            {8,"ปิดการซื้อขาย"}
        };

        /// <summary>
        /// 英文状态集合
        /// </summary>
        public static readonly Dictionary<int, string> OrderStatuss_en_US = new Dictionary<int, string>
        {
            {-1,"无效订单"},
            {0,"All"},
            {2,"Unpaid"},
            {3,"Unshipped"},
            {4,"Unreceived"},
            {5,"Received"},
            {6,"Fulfilled"},
            {7,"Cancelled"},
            {8," Deal closed"}
        };

        /// <summary>
        /// 中文繁體状态集合
        /// </summary>
        public static readonly Dictionary<int, string> OrderStatuss_zh_HK = new Dictionary<int, string>
        {
            {-1,"無效訂單"},
            {0,"全部"},
            {2,"待付款"},
            {3,"待發貨"},
            {4,"待收貨"},
            {5,"已收貨"},
            {6,"已完成"},
            {7,"已取消"},
            {8,"交易關閉"}
        };


        /// <summary>
        /// 获取订单状态下拉选项
        /// </summary>
        /// <param name="languageType">语言类型</param>
        /// <param name="excludeStatus">需要排除的状态</param>
        /// <param name="defaultStatus">默认状态</param>
        /// <returns>订单状态集合</returns>
        public static List<SelectListItem> GetLocalOrderStatusInto(LanguageType languageType = LanguageType.zh_CN, int[] excludeStatus = null, OrderEnums.OrderStatus defaultStatus = OrderEnums.OrderStatus.All)
        {
            Dictionary<int, string> orderStatuss = ML_OrderStatus.GetLocalOrderStatus(languageType);

            if (excludeStatus != null)
            {
                foreach (var status in excludeStatus)
                {
                    if (orderStatuss.ContainsKey(status))
                    {
                        orderStatuss.Remove(status);
                    }
                }
            }
            return orderStatuss.Select(x => new SelectListItem() { Text = x.Value, Value = x.Key.ToString(), Selected = x.Key == (int)defaultStatus }).ToList();
        }


        /// <summary>
        /// 获取订单状态下拉选项
        /// </summary>
        /// <param name="languageId">语言Id</param>
        /// <param name="excludeStatus">需要排除的状态</param>
        /// <param name="defaultStatus">默认状态</param>
        /// <returns>订单状态集合</returns>
        public static List<SelectListItem> GetLocalOrderStatusInto(int languageId = (int)LanguageType.zh_CN, int[] excludeStatus = null, OrderEnums.OrderStatus defaultStatus = OrderEnums.OrderStatus.All)
        {

            LanguageType languageType = (LanguageType)languageId;
            return ML_OrderStatus.GetLocalOrderStatusInto(languageType, excludeStatus, defaultStatus);
        }

        /// <summary>
        /// 获取本地语言订单状态集合
        /// </summary>
        /// <param name="languageId">语言Id</param>
        /// <returns>订单状态集合</returns>
        public static Dictionary<int, string> GetLocalOrderStatus(int languageId = (int) LanguageType.zh_CN)
        {
            LanguageType languageType = (LanguageType)languageId;
            return ML_OrderStatus.GetLocalOrderStatus(languageType);
        }

        /// <summary>
        /// 获取本地语言订单状态集合
        /// </summary>
        /// <param name="languageType">语言类型</param>
        /// <returns>订单状态集合</returns>
        public static Dictionary<int, string> GetLocalOrderStatus(LanguageType languageType = LanguageType.zh_CN)
        {
            Dictionary<int, string> orderStatuss = new Dictionary<int, string>();
            switch (languageType)
            {
                case LanguageType.zh_CN:
                    orderStatuss = OrderStatuss_zh_CN;
                    break;
                case LanguageType.en_US:
                    orderStatuss = OrderStatuss_en_US;
                    break;
                case LanguageType.th_TH:
                    orderStatuss = OrderStatuss_th_TH;
                    break;

                case LanguageType.zh_HK:
                    orderStatuss = OrderStatuss_zh_HK;
                    break;
            }
            return orderStatuss;
        }

        /// <summary>
        /// 获取本地订单状态描述
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public static string GetLocalOrderStatusDescription(int languageId, OrderEnums.OrderStatus orderStatus)
        {
            Dictionary<int, string> orderStatusDict = GetLocalOrderStatus(languageId);
            return orderStatusDict[(int)orderStatus];
        }
    }
}
