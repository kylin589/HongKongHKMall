using BrCms.Core.Infrastructure;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.LoginLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.LoginLog
{
    public interface IAC_OperateLogService : IDependency
    {
        /// <summary>
        /// 添加系统操作日志
        /// </summary>
        /// <param name="model">系统操作日志模型</param>
        /// <returns>是否成功</returns>
        ResultModel AddAC_OperateLog(AC_OperateLogModel model);

        /// <summary>
        /// 根据系统操作日志id获取系统操作日志
        /// </summary>
        /// <param name="id">类别id</param>
        /// <returns>系统操作日志模型</returns>
        ResultModel GetAC_OperateLogById(int? OperateID);

        /// <summary>
        /// 获取系统操作日志列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>系统操作日志列表</returns>
        ResultModel GetAC_OperateLogList(SearchAC_OperateLogModel model);

        /// <summary>
        /// 更新系统操作日志
        /// </summary>
        /// <param name="model">系统操作日志模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateAC_OperateLog(AC_OperateLogModel model);

        /// <summary>
        /// 删除系统操作日志
        /// </summary>
        /// <param name="model">系统操作日志模型</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteAC_OperateLog(AC_OperateLogModel model);
    }
}
