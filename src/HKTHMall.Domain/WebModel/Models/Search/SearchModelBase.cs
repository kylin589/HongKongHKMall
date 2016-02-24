using System.Runtime.Serialization;
using HKTHMall.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Search
{

    public class SearchModel
    {
        public long UserID { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 惠卡价格
        /// </summary>
        public decimal HKPrice { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        public decimal MarketPrice { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 显示图片
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 主图标识
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDT { get; set; }

        /// <summary>
        /// 促销类型 1:无促销,2:打折
        /// </summary>
        public int SalesRuleId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StarDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 折扣率
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 销售量
        /// </summary>
        public int SaleCount { get; set; }

    }

    public class SearchModelBase
    {
        public SearchModelBase()
        {
            this.sf = SearchField.ZongHe;
            this.st = SearchType.ZongHe;
            AscOrDesc = AscOrDescType.DESC;
            this.PageSize = 20; //默认每页20
        }
        private int pageIndex = 1;
        public int Page
        {
            get { return pageIndex <= 0 ? 1 : pageIndex; }
            set { pageIndex = value <= 0 ? 1 : value; }
        }
        private int pageSize = 1;
        public int PageSize
        {
            get
            {
                return pageSize <= 0 ? 1 : pageSize;
            }
            set { pageSize = value <= 0 ? 1 : value; }
        }
        public int AllCount { get; set; }
        /// <summary>
        /// 查询类型。
        /// </summary>
        public SearchType st { get; set; }

        public SearchField sf { get; set; }

        public AscOrDescType AscOrDesc { get; set; }

    }

    /// <summary>
    /// 关键字搜索
    /// </summary>
    public class KeyWordsSearch : SearchModelBase
    {
        /// <summary>
        /// 查询的关键字
        /// </summary>
        public string k { get; set; }
        /// <summary>
        /// 语言Id
        /// </summary>
        public int languageId { get; set; }
        /// <summary>
        /// 类别Id
        /// </summary>
        public int cateId { get; set; }
        /// <summary>
        /// type 1:一级分类  2:二级分类  3:三级分类
        /// </summary>
        public string type { get; set; }

    
    }
}
