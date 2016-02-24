using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class CategoryListResult
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        [DataMember]
        public int categoryID { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [DataMember]
        public string categoryName { get; set; }
        /// <summary>
        /// 背景颜色
        /// </summary>
        [DataMember]
        public string backcolor { get; set; }
        /// <summary>
        /// 父类ID
        /// </summary>
        public int parentId { get; set; }
        /// <summary>
        /// 子集合
        /// </summary>
        [DataMember]
        public List<CategoryListResult> childNode { get; set; }
       
    }
}