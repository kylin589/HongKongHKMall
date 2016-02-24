using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Products;

namespace HKTHMall.Services.Products
{
    public interface IProductRuleService : IDependency
    {
        /// <summary>
        /// 通过Id查询商品促销信息对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel GetProductRuleById(long id);

        /// <summary>
        ///商品促销信息分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel Select(SearchProductRuleModel model);

        /// <summary>
        /// 添加商品促销信息
        /// </summary>
        /// <param name="model">商品促销信息对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel Add(ProductRuleModel model);

        /// <summary>
        /// 通过Id删除商品促销信息
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel Delete(long id);

        /// <summary>
        /// 更新商品促销信息
        /// </summary>
        /// <param name="model">商品促销信息对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel Update(ProductRuleModel model);

        /// <summary>
        /// 通过Id查询商品信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel GetProductById(long id);

        /// <summary>
        /// 获取首页显示爆款产品
        /// </summary> 
        /// <param name="languageID">加载的语言</param>
        /// <param name="topCount">取前面多少条数据</param>
        /// <param name="isToday">true:今天 false:预售</param>
        /// <returns></returns>
        ResultModel GetIndexExplosion(int languageID, int topCount, bool isToday);
        /// <summary>
        /// 获取首页显示爆款产品
        /// </summary> 
        /// <param name="languageID">加载的语言</param>       
        /// <returns></returns>
        ResultModel GetIndexExplosionForApi(int languageID);

        /// <summary>
        /// 获取产品爆款信息数据
        /// </summary>
        /// <param name="productid">产品Id</param>
        /// <param name="languageID">语言版本</param>
        /// <returns></returns>
        ResultModel GetPromotionProductForId(long productid, int languageID);

    }
}
