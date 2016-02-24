using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    public class SearchReturnProductInfoModel : Paged
    {
        /// <summary>
        /// Return_GoodsId
        /// </summary>
        public string ReturnOrderID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 订单明细主键
        /// </summary>
        public long OrderDetailsID { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 退换货类型 1退货,2换货,3返修
        /// </summary>
        public int ReturnType { get; set; }

        /// <summary>
        /// 状态 1申请,2处理中,3已完成
        /// </summary>
        public int ReturnStatus { get; set; }

        /// <summary>
        /// 语言ID
        /// zhoub 20150815
        /// </summary>
        public int LanguageID { get; set; }

        /// <summary>
        /// 用户手机 （YH_User表）
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户Email （YH_User表）
        /// </summary>
        public string Email { get; set; }
    }
}
