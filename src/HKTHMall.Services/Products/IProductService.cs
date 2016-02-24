using System.Collections.Generic;
using Autofac.Extras.DynamicProxy2;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Domain.WebModel.Models.Search;

namespace HKTHMall.Services.Products
{
    /// <summary>
    ///     产品
    /// </summary>
    [Intercept(typeof(ServiceIInterceptor))]
    public interface IProductService : IDependency
    {
        /// <summary>
        ///     查询产品
        /// </summary>
        /// <param name="model">查询条件</param>
        /// <returns>结果</returns>
        ResultModel Search(SearchProductModel model);
        /// <summary>
        /// 查询分类品牌产品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel SearchCategoryBrandProduct(SearchBrandProductModel model, ref int totalCount);

        /// <summary>
        /// 根据产品id获取产品详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        ResultModel GetProductById(long id, int languageId);

        /// <summary>
        ///     添加产品
        /// </summary>
        /// <param name="model">AddProductModel</param>
        /// <returns>结果</returns>
        ResultModel Add(AddProductModel model);

        /// <summary>
        ///     更新产品
        /// </summary>
        /// <param name="model">需要更新的内容</param>
        /// <returns>结果</returns>
        ResultModel Update(UpdateProductModel model);
          /// <summary>
        /// 复制商品
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        ResultModel CopyProductById(long id);
        /// <summary>
        /// 获取惠卡推荐数据
        /// </summary>
        /// <param name="languageid">显示的语言</param>
        /// <param name="userId">用户ID</param>
        /// <param name="tJCount">推荐条数</param>
        /// <returns></returns>
        ResultModel GetTopRecommend(int languageid = 3, long userId = 0,int tJCount=100);


        /// <summary>
        ///     获取惠卡推荐数据
        /// </summary>
        /// <param name="languageid">显示的语言</param>
        /// <returns></returns>
        ResultModel GetTopRecommend(long productId, int top = 6, int languageid = 4, long userId = 0);

        /// <summary>
        ///     获取惠卡推荐数据(其他类型的，不排序 随意推荐)
        /// </summary>
        /// <param name="languageid">显示的语言</param>
        /// <returns></returns>
        ResultModel GetTopRecommend(long productId, int top = 6, int languageid = 4);

        /// <summary>
        /// 获取惠卡推荐数据
        /// </summary>
        /// <param name="languageid">显示的语言</param>
        /// <returns></returns>
        ResultModel GetTopRecommendForApi(int languageid = 3);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        ResultModel GetBannerProductByProductId(long ProductId);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">要删除的产品id集合</param>
        /// <returns></returns>
        ResultModel DeleteList(IList<long> ids);

        /// <summary>
        /// 取得商品信息
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <param name="LanguageID">语言Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        ResultModel GetProduct(long id, int LanguageID);


        /// <summary>
        /// 获取产品类型规格属性数据集
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ResultModel GetProductCategoryTypeForSKU_Attributes(long productId);


        /// <summary>
        /// 获取产品规格参数
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ResultModel GetProductSpecifications(long productId);


        /// <summary>
        /// 根据产品id获取商品库存信息
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        ResultModel GetSKU_ProductById(long id, int languageId);

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        ResultModel Check(long productId, ProductStatus status);
        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="productIds">产品Id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        ResultModel Check(long[] productIds, ProductStatus status);

        /// <summary>
        /// 根据产品id获取商品信息（前台方法）
        /// </summary>
        /// <param name="model">商品Id</param>
        /// <returns></returns>
        ResultModel SearchProduct(SearchProductModel model);

        /// <summary>
        /// 根据产品id获取商品信息（前台方法,申请不通过时也可以预览）
        /// </summary>
        /// <param name="model">商品Id</param>
        /// <returns></returns>
        ResultModel SearchProductShow(SearchProductModel model);

        /// <summary>
        /// 获取热销列表
        /// </summary>
        /// <param name="TopCount"></param>
        /// <param name="Lang"></param>
        /// <returns></returns>
        ResultModel GetSellingList(int TopCount, int Lang = 4);

        /// <summary>
        /// 获取产品详情页的面包屑导航
        /// </summary>
        /// <param name="ProductId">产品ID</param>
        /// <param name="Lang">语言ID</param>
        /// <returns></returns>
        ResultModel GetProductPath(long ProductId, int Lang = 4);

                /// <summary>
        /// 根据产品id获取购物车商品列表
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="languageId">语言id</param>
        /// <returns>购物车商品模型</returns>
        ResultModel GetProductListByPrdouctId(string productIds, string productSkuIds, int languageId = 4);
    }
}