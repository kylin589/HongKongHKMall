using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class FirstFloorResult
    {
        /// <summary>
        /// 楼层分类id
        /// </summary>
        [DataMember]
        public int categoryId { get; set; }

        /// <summary>
        /// 楼层名称
        /// </summary>
        [DataMember]
        public string categoryName { get; set; }

        /// <summary>
        /// 楼层要显示的广告
        /// </summary>
        [DataMember]
        public List<FirstFloorBanner> bannerList = new List<FirstFloorBanner>();
        /// <summary>
        /// 楼层要显示的产品
        /// </summary>
        [DataMember]
        public List<FirstFloorBannerProduct> bannerProductList = new List<FirstFloorBannerProduct>();

        /// <summary>
        /// 排序id
        /// </summary>
        [DataMember]
        public int place { get; set; }
    }
    [DataContract]
    public class FirstFloorBanner
    {
        /// <summary>
        /// banner名称
        /// </summary>        
        [DataMember]
        public string bannerName { get; set; }

        /// <summary>
        /// banner链接
        /// </summary>       
        [DataMember]
        public string bannerUrl { get; set; }

        /// <summary>
        /// banner图片地址
        /// </summary>        
        [DataMember]
        public string bannerPic { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>        
        [DataMember]
        public long productId { get; set; }
    }
    [DataContract]
    public class FirstFloorBannerProduct
    {
        /// <summary>
        /// 商品ID 
        /// </summary>        
        [DataMember]
        public long productId { get; set; }
        /// <summary>
        /// 商品广告图片地址
        /// </summary>         
        [DataMember]
        public string picAddress { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember]
        public string productName { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        [DataMember]
        public decimal hKPrice { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        [DataMember]
        public long categoryId { get; set; }
    }
}