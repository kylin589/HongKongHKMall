using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Index
{
    public class IndexExplosion
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }
         
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 惠卡价格
        /// </summary>
        public decimal HKPrice { get; set; }

        /// <summary>
        /// 活动价格
        /// </summary>
        public decimal ActivityPrice { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime StarDate { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime EndDate { get; set; }


        /// <summary>
        /// 开始状态未开始状态
        /// </summary>
        public int StartStatus { get; set; }

        /// <summary>
        /// 时间间隔秒数
        /// </summary>
        public int Intervalsecond { get; set; }

        /// <summary>
        /// 爆款图片地址
        /// </summary>
        public string PicAddress { get; set; }

        public int Sorts { get; set; }

        /// <summary>
        /// 返利天数
        /// </summary>
        public int? RebateDays { get; set; }

    }
}
