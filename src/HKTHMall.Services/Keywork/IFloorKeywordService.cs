using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.Keywork;

namespace HKTHMall.Services.Keywork
{
    /// <summary>
    /// 关键字接口
    /// <remarks>added by jimmy,2015-7-8</remarks>
    /// </summary>
    public interface IFloorKeywordService : IDependency
    {
        /// <summary>
        /// 通过Id查询关键字对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel GetFloorKeywordById(long id);

        /// <summary>
        ///关键字分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Select(SearchFloorKeywordModel model);

        /// <summary>
        /// 添加关键字
        /// </summary>
        /// <param name="model">关键字对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Add(FloorKeywordModel model);

        /// <summary>
        /// 通过Id删除关键字
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Delete(long id);

        /// <summary>
        /// 更新关键字
        /// </summary>
        /// <param name="model">关键字对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Update(FloorKeywordModel model);

        /// <summary>
        /// 更新排序位置
        /// </summary>
        /// <param name="floorKeywordId">关键字管理Id</param>
        /// <param name="place">位置</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        ResultModel UpdatePlace(long floorKeywordId, int place);


        /// <summary>
        /// 首页查询
        /// </summary>
        /// <param name="language"></param>
        /// <param name="counts"></param>
        /// <returns></returns>
        ResultModel GetTopList(int language, int counts);
    }
}
