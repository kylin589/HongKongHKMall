using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.CashBack
{
   public class SearchRebate
    {
       public long ID { get; set; }
       /// <summary>
       /// 返还用户ID
       /// </summary>
       public Nullable<long> RebateUserID { get; set; }
       /// <summary>
       /// 订单ID
       /// </summary>
       public Nullable<long> OrderID { get; set; }
       /// <summary>
       /// 产品ID
       /// </summary>
       public long ProductId { get; set; }
       /// <summary>
       /// 返还状态0待返还，1 返还中，2返还完成，3暂停返回
       /// </summary>
       public Nullable<int> RebateStatus { get; set; }
       /// <summary>
       /// 生成时间
       /// </summary>
       public Nullable<System.DateTime> CreateTime { get; set; }
       /// <summary>
       /// 最后修改时间
       /// </summary>
       public Nullable<System.DateTime> UpdateTime { get; set; }
       /// <summary>
       /// 返回开始时间
       /// </summary>
       public Nullable<System.DateTime> StarteTime { get; set; }
       /// <summary>
       /// 返还结束时间爱你
       /// </summary>
       public Nullable<System.DateTime> EndTime { get; set; }
    }
}
