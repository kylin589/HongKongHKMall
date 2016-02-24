using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.WebLogin
{
    public interface IYH_PasswordErrorService : IDependency
    {
        /// <summary>
        /// 根据用户ID 和密码类型获取密码错误信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="passwordType">密码类型</param>
        /// <returns></returns>
        ResultModel GetPasswordErrorInfo(long userId, int passwordType);

        /// <summary>
        /// 根据用户ID 和密码类型获取密码错误信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="passwordType">密码类型</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        ResultModel GetPasswordErrorInfo(long userId, int passwordType, dynamic trans);

        /// <summary>
        /// 密码错误表插入
        /// </summary>
        /// <param name="model">密码错误对象</param>
        /// <returns></returns>
        ResultModel AddError(YH_PasswordErrorModel model);

        /// <summary>
        /// 密码错误表插入
        /// </summary>
        /// <param name="model">密码错误对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        ResultModel AddError(YH_PasswordErrorModel model, dynamic trans);

        /// <summary>
        /// 密码错误表更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Update(YH_PasswordErrorModel model);

        /// <summary>
        /// 密码错误表更新
        /// </summary>
        /// <param name="model">密码错误对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        ResultModel Update(YH_PasswordErrorModel model, dynamic trans);
    }
}
