using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    public class CategoryToListModel
    {
        /// <summary>
        /// 分类ID
        /// </summary>
       
        public int categoryID { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
      
        public string categoryName { get; set; }
        /// <summary>
        /// 背景颜色
        /// </summary>
      
        public string backcolor { get; set; }
        /// <summary>
        /// 父类ID
        /// </summary>

        public int parentId { get; set; }
    }
}