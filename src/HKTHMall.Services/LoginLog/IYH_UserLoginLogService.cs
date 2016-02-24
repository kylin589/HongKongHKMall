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
    public interface IYH_UserLoginLogService : IDependency
    {
        /// <summary>
        /// 添加用户登录日记
        /// </summary>
        /// <param name="model">用户登录日记模型</param>
        /// <returns>是否成功</returns>
        //ResultModel<int> AddYH_UserLogin(YH_UserLoginLogModel model);
        ResultModel AddYH_UserLogin(YH_UserLoginLogModel model);

        /// <summary>
        /// 根据用户登录日记id获取用户登录日记
        /// </summary>
        /// <param name="id">类别id</param>
        /// <returns>用户登录日记模型</returns>
        ResultModel GetYH_UserLoginLogById(int ID);

        

        /// <summary>
        /// 获取用户登录日记列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>用户登录日记列表</returns>
        ResultModel GetYH_UserLogin(SearchYH_UserLoginLogModel model);

        /// <summary>
        /// 更新用户登录日记
        /// </summary>
        /// <param name="model">用户登录日记模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateYH_UserLogin(YH_UserLoginLogModel model);

        

        /// <summary>
        /// 删除用户登录日记
        /// </summary>
        /// <param name="model">用户登录日记模型</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteYH_UserLogin(YH_UserLoginLogModel model);
        /// <summary>
        /// 根据用户ID 获取上次登录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultModel GetUserLoginInfo(long userId);
    }
}
