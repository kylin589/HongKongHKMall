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
    /// <summary>
    /// 广告促销商品接口
    /// <remarks>added by jimmy,2015-7-8</remarks>
    /// </summary>
    public interface ISalesProductService : IDependency
    {
        /// <summary>
        /// 通过Id查询广告促销商品对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel GetSalesProductById(long id);

        /// <summary>
        ///广告促销商品分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Select(SearchSalesProductModel model);

        /// <summary>
        /// 添加广告促销商品
        /// </summary>
        /// <param name="model">广告促销商品对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Add(SalesProductModel model);

        /// <summary>
        /// 通过Id删除广告促销商品
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Delete(long id);

        /// <summary>
        /// 更新广告促销商品
        /// </summary>
        /// <param name="model">广告促销商品对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Update(SalesProductModel model);

        /// <summary>
        /// 更新排序位置
        /// </summary>
        /// <param name="SalesProductId">广告促销商品Id</param>
        /// <param name="place">位置</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        ResultModel UpdatePlace(long SalesProductId, int place);
    }
}
