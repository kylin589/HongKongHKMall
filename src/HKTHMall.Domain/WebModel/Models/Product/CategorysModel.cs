using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    public class CategorysModel
    {
        public int CategoryId { get; set; }

        public int LanguageID { get; set; }

        public string CategoryName { get; set; }

        public int Place { get; set; }

        public int parentId { get; set; }
    }


    /// <summary>
    /// 显示二三级分类的信息实体
    /// </summary>
    public class CateList
    {
        /// <summary>
        /// 分类id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 楼层要显示的广告
        /// </summary>
        public List<CategorysModel> cateLlist = new List<CategorysModel>();
        /// <summary>
        /// 排序id
        /// </summary>
        public int Place { get; set; }

        public int parentId { get; set; }
    }
}
