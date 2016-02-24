using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models.Orders;
using BrCms.Framework.Collections;

namespace HKTHMall.Services.Orders
{
    /// <summary>
    /// 投诉接口
    /// <remarks>added by jimmy,2015-7-8</remarks>
    /// </summary>
    public interface IComplaintsService : IDependency
    {
        /// <summary>
        /// 通过Id查询投诉对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel GetComplaintsById(string id);

        /// <summary>
        /// 投诉对象分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Select(SearchComplaintsModel model);

        /// <summary>
        /// 通过Id删除投诉
        /// </summary>
        /// <param name="id">投诉id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Delete(string id);

        /// <summary>
        /// 更新投诉
        /// </summary>
        /// <param name="model">投诉对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        ResultModel Update(ComplaintsModel model);

        /// <summary>
        /// 新增投诉
        /// zhoub 20150716
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddComplaints(ComplaintsModel model);

        /// <summary>
        /// 根据用户ID查询投诉数据
        /// zhoub 20150716
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultModel GetPagingComplaints(SearchComplaintsModel model);
    }
}
