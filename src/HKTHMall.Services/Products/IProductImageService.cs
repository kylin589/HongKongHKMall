using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Products;
using BrCms.Framework.Collections;

namespace HKTHMall.Services.Products
{
    /// <summary>
    /// 产品图接口
    /// <remarks>added by jimmy,2015-7-27</remarks>
    /// </summary>
    public interface IProductImageService:IDependency
    {
        /// <summary>
        /// 通过Id查询产品图对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        ResultModel GetProductImageById(long id);

        /// <summary>
        /// 产品图对象分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        ResultModel Select(SearchProductImageModel model);

        /// <summary>
        /// 添加产品图
        /// </summary>
        /// <param name="model">产品图对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        ResultModel Add(ProductImageModel model);

        /// <summary>
        /// 通过Id删除产品图
        /// </summary>
        /// <param name="id">产品图id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        ResultModel Delete(long id);

        /// <summary>
        /// 更新产品图
        /// </summary>
        /// <param name="model">产品图对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        ResultModel Update(ProductImageModel model);

        /// <summary>
        /// 更新排序位置
        /// </summary>
        /// <param name="floorKeywordId">关键字管理Id</param>
        /// <param name="place">位置</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        ResultModel UpdatePlace(long productImageId, int place);
    }
}
