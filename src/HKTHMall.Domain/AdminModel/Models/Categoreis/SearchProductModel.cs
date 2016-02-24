using BrCms.Framework.Collections;
using HKTHMall.Domain.Enum;
using System.Runtime.Serialization;

namespace HKTHMall.Domain.Models.Categoreis
{
    [DataContract]
    public class SearchProductModel : Paged
    {
        /// <summary>
        /// 升降序名称
        /// </summary>
        [DataMember]
        public string SortOrder { get; set; }
        /// <summary>
        /// 排序名称
        /// </summary>
        [DataMember]
        public string SortName { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        [DataMember]
        public long? ProductId { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        [DataMember]
        public int? CategoryId { get; set; }
        /// <summary>
        /// 分类Id
        /// </summary>
        [DataMember]
        public int? CategoryId1 { get; set; }
        /// <summary>
        /// 分类1
        /// </summary>
        [DataMember]
        public int? CategoryId2 { get; set; }
        /// <summary>
        /// 分类2
        /// </summary>
        [DataMember]
        public int? CategoryId3 { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        [DataMember]
        public int? LanguageId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [DataMember]
        public string CategoryName { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        [DataMember]
        public string BrandName { get; set; }
        /// <summary>
        /// 品牌ID
        /// </summary>
        [DataMember]
        public int? BrandId { get; set; }
        /// <summary>
        /// 获取产品状态
        /// </summary>
        public ProductStatus? Status { get; set; }
    }
}