using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Products
{
    public class SearchProductImageModel : Paged
    {
        /// <summary>
        /// 产品图名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public int ImageType { get; set; }
    }
}
