using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.banner;
using HKTHMall.Domain.Models.Bra;

namespace HKTHMall.Domain.WebModel.Models.Index
{

    /// <summary>
    /// 显示首页楼层产品的信息实体
    /// </summary>
    public class IndexFloorInfo
    {
        /// <summary>
        /// 楼层分类id
        /// </summary>
        public int categoryId { get; set; }

        /// <summary>
        /// 楼层名称
        /// </summary>
        public string categoryName { get; set; }

        /// <summary>
        /// 楼层要显示的广告
        /// </summary>
        public List<bannerModel> bannerList = new List<bannerModel>();
        /// <summary>
        /// 楼层要显示的产品
        /// </summary>
        public List<bannerProductModel> bannerProductList = new List<bannerProductModel>();
        /// <summary>
        /// 楼层分类推广类型
        /// </summary>
        public List<CategoryModel> categoryList = new List<CategoryModel>();

        public List<BrandAdvertiseModel> brandList = new List<BrandAdvertiseModel>();

        public bannerModel floorAd { get; set; }
        /// <summary>
        /// 排序id
        /// </summary>
        public int Place { get; set; }
    }
}
