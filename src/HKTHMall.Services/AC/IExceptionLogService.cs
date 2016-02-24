using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.AC
{
    public interface IExceptionLogService : IDependency
    {
        /// <summary>
        /// 添加信息
        /// zhoub 20150902
        /// </summary>
        ResultModel Add(ExceptionLogModel model);

        /// <summary>
        /// 更新信息
        /// zhoub 20150902
        /// </summary>
        /// <param name="model">角色模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel Update(ExceptionLogModel model);

        /// <summary>
        /// 分页查询
        /// zhoub 20150902
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel GetExceptionLogList(SearchExceptionLogModel model);

        /// <summary>
        /// 服务概况查询
        /// zhoub 20150902
        /// </summary>
        /// <returns></returns>
        ResultModel GetExceptionLogSurvey();
    }
}
