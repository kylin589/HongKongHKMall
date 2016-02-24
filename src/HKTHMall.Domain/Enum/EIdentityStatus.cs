using BrCms.Framework.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 1首页轮播banner，2首页楼层banner,3 分类频道轮播banner,4分类频道楼层banner,5 APP 轮播banner,6 APP 首页楼层Banner
    /// </summary>
    public enum EIdentityStatus
    {
        /// <summary>
        /// 首页轮播banner
        /// </summary>
        [EnumDescription("HomePage", 1)]
        HomePage = 1,

        /// <summary>
        /// 首页楼层banner
        /// </summary>
        [EnumDescription("HomeFloor", 2)]
        HomeFloor = 2,

           /// <summary>
        /// 分类频道轮播banner
        /// </summary>
        [EnumDescription("Classified", 3)]
        Classified = 3,

           /// <summary>
        /// 分类频道楼层banner
        /// </summary>
        [EnumDescription("ClassifiChannel", 4)]
        ClassifiChannel = 4,

           /// <summary>
        ///  APP 轮播banner
        /// </summary>
        [EnumDescription("APPShuffling", 5)]
        AppShuffling = 5,

           /// <summary>
        ///  APP 首页楼层banner
        /// </summary>
        [EnumDescription("AppFloor", 6)]
        AppFloor = 6
    }
}
