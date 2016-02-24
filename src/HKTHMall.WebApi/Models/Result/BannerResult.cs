using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{  
        /// <summary>
        /// 返回结果
        /// </summary>
        [DataContract]
        public class BannerResult
        {
            /// <summary>
            /// 广告ID
            /// </summary>
            [DataMember(Name = "bannerId")]
            public long BannerId { get; set; }

            /// <summary>
            /// 广告名称
            /// </summary>
            [DataMember(Name = "bannerName")]
            public string BannerName { get; set; }
            /// <summary>
            /// banner链接
            /// </summary>
            [DataMember(Name = "bannerUrl")]
            public string BannerUrl { get; set; }
            /// <summary>
            /// banner图片地址
            /// </summary>
            [DataMember(Name = "bannerPic")]
            public string BannerPic { get; set; }
            /// <summary>
            /// 有分类的填分类ID,没有的填写为0
            /// </summary>
            [DataMember(Name = "placeCode")]
            public int PlaceCode { get; set; }
            /// <summary>
            /// 标识ID （1首页轮播banner,2首页楼层banner,3 分类频道轮播banner,4分类频道楼层banner）
            /// </summary>
            [DataMember(Name = "identityStatus")]
            public int IdentityStatus { get; set; }
            /// <summary>
            /// 排序
            /// </summary>
            [DataMember(Name = "sorts")]
            public long Sorts { get; set; }
            /// <summary>
            /// 商品Id
            /// </summary>
            [DataMember]
            public long productId { get; set; }
        }    
}