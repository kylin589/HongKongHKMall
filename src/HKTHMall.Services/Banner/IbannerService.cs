using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Banner
{
    public interface IbannerService : IDependency
    {
        /// <summary>
        /// 添加banner图片
        /// </summary>
        /// <param name="model">用banner图片模型</param>
        /// <returns>是否成功</returns>
        //ResultModel<int> AddYH_UserLogin(YH_UserLoginLogModel model);
        ResultModel AddBanner(bannerModel model);

        /// <summary>
        /// 根据banner图片id获取banner图片
        /// </summary>
        /// <param name="id">banner图片id</param>
        /// <returns>banner图片模型</returns>
        ResultModel GetBannerById(long bannerId);

        /// <summary>
        /// 获取banner图片列表
        /// </summary>
        /// <returns>banner图片列表</returns>
        ResultModel GetBanner(SearchbannerModel model);

        /// <summary>
        /// 根据Sorts查询的上一条或者下一条
        /// </summary>
        /// <param name="Sorts"></param>
        /// <param name="sx"></param>
        /// <returns></returns>
        ResultModel GetBanner(long Sorts, int sx, int IdentityStatus, long bannerId, int BannerPlaceCode);

        /// <summary>
        /// 更新banner图片
        /// </summary>
        /// <param name="model">banner图片模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateBanner(bannerModel model);

        /// <summary>
        /// 更新banner的Sorts
        /// </summary>
        /// <param name="model">banner图片模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateSorts(bannerModel model, bannerModel model1);

        /// <summary>
        /// 删除banner图片
        /// </summary>
        /// <param name="model">banner图片模型</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteBanner(bannerModel model);



        /// <summary>
        /// 获取首页显示广告条目
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        ResultModel GetTopBanner(int topCount, int identityStatus, int placeCode);
         /// <summary>
        /// 获取首页显示广告条目
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        ResultModel GetTopBannerForApp(int topCount, int identityStatus, int placeCode);

        /// <summary>
        /// 获取banner按时间倒序
        /// 黄主霞 2015-01-14
        /// </summary>
        /// <param name="topCount">获取前N个Banner</param>
        /// <param name="identityStatus">类别：1首页轮播banner，2首页楼层banner,3 分类频道轮播banner,4分类频道楼层banner</param>
        /// <param name="placeCode">分类ID</param>
        /// <returns></returns>
        ResultModel GetBannerByTimeDesc(int? topCount, int identityStatus, int placeCode);

        /// <summary>
        /// 更新banner图片
        /// </summary>
        /// <param name="model">后banner图片模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateThreeBanner(bannerModel model);

        
    }
}
