using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    public class OrderExpressFeeLogModel
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 更改前的快递费
        /// </summary>
        public decimal OldExpressFee { get; set; }
        /// <summary>
        /// 更改后的快递费
        /// </summary>
        public decimal NewExpressFee { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string ExpressFeeContent { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
    }
}
