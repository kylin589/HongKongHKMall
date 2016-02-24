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
    public interface IUserLoginLogService : IDependency
    {
        /// <summary>
        /// 添加后台用户登录日记
        /// </summary>
        /// <param name="model">用户登录日记模型</param>
        /// <returns>是否成功</returns>
        //ResultModel<int> AddYH_UserLogin(YH_UserLoginLogModel model);
        ResultModel AddUserLoginLog(UserLoginLogModel model);

        /// <summary>
        /// 根据后台用户登录日记id获取用户登录日记
        /// </summary>
        /// <param name="id">类别id</param>
        /// <returns>后台用户登录日记模型</returns>
        ResultModel GetUserLoginLogById(int ID);

        /// <summary>
        /// 获取后台用户登录日记列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>后台用户登录日记列表</returns>
        ResultModel GetUserLoginLog(SearchUserLoginLogModel model);

        /// <summary>
        /// 更新后台用户登录日记
        /// </summary>
        /// <param name="model">后台用户登录日记模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateUserLoginLog(UserLoginLogModel model);

        /// <summary>
        /// 删除后台用户登录日记
        /// </summary>
        /// <param name="model">后台用户登录日记模型</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteUserLoginLog(UserLoginLogModel model);
    }
}
