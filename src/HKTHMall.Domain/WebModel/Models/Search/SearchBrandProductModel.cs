using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Search
{
    /// <summary>
    /// 带有品牌分类的产品搜索
    /// </summary>
    public class SearchBrandProductModel
    {
        public SearchBrandProductModel() {
            pageIndex = 1;
            pageSize = 20;
            status = 4;
        }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int categoryId { get; set; }
        /// <summary>
        /// 分类级别
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int pageIndex { get; set; }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 开始价格
        /// </summary>
        public decimal startPrice { get; set; }
        /// <summary>
        /// 结束价格
        /// </summary>
        public decimal endPrice { get; set; }
        /// <summary>
        /// 语言ID
        /// </summary>
        public int langId { get; set; }
        /// <summary>
        /// 未提交=1，待审核=2，审核不通过=3，已上架=4，已下架=5
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 品牌ID
        /// </summary>
        public int brandId { get; set; }
        /// <summary>
        /// 创建时间开始时间
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// 创建时间结束时间
        /// </summary>
        public string endDate { get; set; }
        /// <summary>
        /// 销量排序(0:不排序 1：升序 2：降序)
        /// </summary>
        public int sortSell { get; set; }
        /// <summary>
        /// 评价排序(0:不排序 1：升序 2：降序)
        /// </summary>
        public int sortComment { get; set; }
        /// <summary>
        /// 价格排序(0:不排序 1：升序 2：降序)
        /// </summary>
        public int sortPrice { get; set; }
        /// <summary>
        /// 返现天数年排序(0:不排序 1：升序 2：降序)
        /// </summary>
        public int sortRebateDays { get; set; }
        /// <summary>
        /// 默认排序
        /// </summary>
        public bool sort { get; set; }
    }
}
