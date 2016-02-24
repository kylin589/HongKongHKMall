using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.CashBack
{
    public class ZJ_RebateInfo
    {
        public long ID { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public Nullable<long> ProductID { get; set; }
        /// <summary>
        /// 返利用户ID
        /// </summary>
        public Nullable<long> UserID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<long> SKUID { get; set; }
        /// <summary>
        /// 订单明细ID
        /// </summary>
        public Nullable<long> OrderDetailsID { get; set; }
        /// <summary>
        /// 返款总金额
        /// </summary>
        public decimal TotalMoney { get; set; }
        /// <summary>
        /// 返利总天数
        /// </summary>
        public int TotalDay { get; set; }
        /// <summary>
        /// 已返金额
        /// </summary>
        public decimal PaidMoney { get; set; }
        /// <summary>
        /// 已返天数
        /// </summary>
        public int PaidDays { get; set; }
        /// <summary>
        /// 状态(0=待返利,1=返利中,2=返利完成,3=返利终止)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 返利开始时间
        /// </summary>
        public Nullable<System.DateTime> StartTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBY { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CreateTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string LastUpdateBY { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
