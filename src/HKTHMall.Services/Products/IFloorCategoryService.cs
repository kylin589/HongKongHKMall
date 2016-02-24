using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Categoreis;

namespace HKTHMall.Services.Products
{
    public interface IFloorCategoryService : IDependency
    {
        /// <summary>
        /// 通过Id查询楼层显示分类对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        ResultModel GetFloorCategoryById(long id);

        /// <summary>
        ///楼层显示分类分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        ResultModel Select(SearchFloorCategoryModel model);

        /// <summary>
        /// 添加楼层显示分类
        /// </summary>
        /// <param name="model">楼层显示分类对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        ResultModel Add(FloorCategoryModel model);

        /// <summary>
        /// 通过Id删除楼层显示分类
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        ResultModel Delete(long id);

        /// <summary>
        /// 更新楼层显示分类
        /// </summary>
        /// <param name="model">楼层显示分类对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        ResultModel Update(FloorCategoryModel model);

        /// <summary>
        /// 更新排序位置
        /// </summary>
        /// <param name="floorCategoryId">楼层显示分类Id</param>
        /// <param name="place">位置</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        ResultModel UpdatePlace(long floorCategoryId, int place);

        /// <summary>
        ///首页导航分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-15</remarks>
        ResultModel GetFloorCategoryList(SearchFloorCategoryModel model);
    }
}
