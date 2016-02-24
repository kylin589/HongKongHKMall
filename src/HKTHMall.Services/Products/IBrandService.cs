using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models.Bra;

namespace HKTHMall.Services.Products
{
    /// <summary>
    /// 商品品牌接口
    /// <remarks>added by jimmy,2015-7-7</remarks>
    /// </summary>
    public interface IBrandService : IDependency
    {
        /// <summary>
        /// 通过Id查询商品品牌对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        ResultModel GetBrandById(int id);
         /// <summary>
        /// 根据一级分类ID和品牌ID，获取该品牌下的三级分类ID
        /// </summary>
        /// <param name="categoryId">一级分类ID</param>
        /// <param name="brandId">品牌ID</param>
        /// <returns></returns>
        ResultModel GetThirdCategoryId(int categoryId, int brandId);
        /// <summary>
        /// 商品品牌对象分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        ResultModel Select(SearchBrandModel model);

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="languageId">语言</param>
        /// <returns></returns>
        ResultModel GetAll(int languageId);
        /// <summary>
        /// 根据分类Id获取品牌
        /// </summary>
        /// <param name="id">分类Id</param>
        /// <param name="languageId">语言</param>
        /// <returns>品牌列表</returns>
        ResultModel GetBrandByCategoryId(int id, int languageId);
        /// <summary>
        /// 根据分类Id获取品牌
        /// </summary>
        /// <param name="id">分类Id</param>
        /// <param name="languageId">语言</param>
        /// <returns>品牌列表</returns>
        ResultModel GetBrandByCategoryId(int id, int languageId, int level = 3);

        /// <summary>
        /// 添加商品品牌
        /// </summary>
        /// <param name="model">商品品牌对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        ResultModel Add(BrandModel model);

        /// <summary>
        /// 通过Id删除商品品牌
        /// </summary>
        /// <param name="id">商品品牌id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        ResultModel Delete(int id);

        /// <summary>
        /// 更新商品品牌
        /// </summary>
        /// <param name="model">商品品牌对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        ResultModel Update(BrandModel model);

        /// <summary>
        /// 更新商品品牌状态
        /// </summary>
        /// <param name="brandID">商品品牌ID</param>
        /// <param name="brandState">状态</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        ResultModel UpdateState(int brandID, int brandState);

        /// <summary>
        /// 获取首页的楼层品牌（按添加时间倒序）
        /// </summary>
        /// <param name="TopCount">前N个(null表示获取所有)</param>
        /// <param name="CategoryId">分类ID</param>
        /// <param name="Lang">语言：默认繁体</param>
        /// <returns></returns>
        ResultModel GetTopBrandTimeDesc(int? TopCount, int[] CategoryIds, int Lang = 4);

    }
}
