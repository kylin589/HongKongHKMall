using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    public class SearchPurchaseOrderModel:Paged
    {
        /// <summary>
        /// 供应商主键Id
        /// </summary>
        public string PurchaseOrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        public int ? status { get; set; }

        /// <summary>
        /// 供应商ID
        /// zhoub 20150928
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 商品ID
        /// zhoub 20150928
        /// </summary>
        public long ProductId { get; set; }

        public Nullable<DateTime> StartTime { get; set; }

        public Nullable<DateTime> EndTime { get; set; }

        public int LanguageID { get; set; }
    }
}
