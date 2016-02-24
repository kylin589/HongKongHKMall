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
    public interface IbannerProductService : IDependency
    {
        /// <summary>
        /// 添加bannerProduct广告商品
        /// </summary>
        /// <param name="model">用bannerProduct广告商品模型</param>
        /// <returns>是否成功</returns>

        ResultModel AddBannerProduct(bannerProductModel model);

        /// <summary>
        /// 根据bannerProduct广告商品id获取bannerProduct广告商品
        /// </summary>
        /// <param name="id">banner图片id</param>
        /// <returns>banner图片模型</returns>
        ResultModel GetBannerProductById(int bannerProductId);

        /// <summary>
        /// 获取bannerProduct广告商品列表
        /// </summary>
        /// <returns>bannerProduct广告商品列表</returns>
        ResultModel GetBannerProduct(SearchbannerProductModel model);
        
        /// <summary>
        /// 根据Sorts查询的上一条或者下一条
        /// </summary>
        /// <param name="Sorts"></param>
        /// <param name="sx"></param>
        /// <returns></returns>
        ResultModel GetBannerProduct(long Sorts, int sx, int IdentityStatus, long bannerId, int LanguageID, int BannerPlaceCode);

        ResultModel GetBannerProduct(SearchbannerProductModel model, out int total);

        /// <summary>
        /// 更新bannerProduct广告商品
        /// </summary>
        /// <param name="model">bannerProduct广告商品模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateBannerProduct(bannerProductModel model);

        /// <summary>
        /// 更新banner的Sorts
        /// </summary>
        /// <param name="model">需要上下移动的model</param>
        /// <param name="model">需要上下移动的model对应的上下行model</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateSorts(bannerProductModel model, bannerProductModel model1);

        /// <summary>
        /// 删除bannerProduct广告商品
        /// </summary>
        /// <param name="model">bannerProduct广告商品模型</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteBannerProduct(bannerProductModel model);

        /// <summary>
        ///  获取首页显示广告产品
        /// </summary>
        /// <param name="topCount">显示行数</param>
        /// <param name="identityStatus">1为首页使用,2为楼层使用</param>
        /// <param name="placeCode">标示类型id</param>
        /// <returns></returns>
        ResultModel GetTopBanner(int topCount, int identityStatus, int placeCode,int Lang=4);
              /// <summary>
        ///  获取首页显示广告产品
        /// </summary>
        /// <param name="topCount">显示行数</param>
        /// <param name="identityStatus">1为首页使用,2为楼层使用</param>
        /// <param name="placeCode">标示类型id</param>
        /// <returns></returns>
        ResultModel GetTopBannerForApi(int topCount, int identityStatus, int placeCode, int languageID);
    }
}
