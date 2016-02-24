using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using BrCms.Core.Infrastructure;

namespace HKTHMall.Services.YHUser
{
    public interface IReportService : IDependency
    {
        /// <summary>
        /// 通过Id查询惠粉举报
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-18</remarks>
        ResultModel GetReportById(long id);

        /// <summary>
        /// 惠粉举报分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-18</remarks>
        ResultModel Select(SearchReportModel model);

        /// <summary>
        /// 通过Id删除惠粉举报
        /// </summary>
        /// <param name="id">惠粉举报id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-18</remarks>
        ResultModel Delete(long id);

        /// <summary>
        /// 更新惠粉举报
        /// </summary>
        /// <param name="model">惠粉举报</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-8-18</remarks>
        ResultModel Update(ReportModel model);
    }
}
