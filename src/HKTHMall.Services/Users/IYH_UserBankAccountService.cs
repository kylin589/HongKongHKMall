using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.User;
using BrCms.Framework.Collections;

namespace HKTHMall.Services.Users
{
    /// <summary>
    /// 用户银行信息接口
    /// </summary>
    public interface IYH_UserBankAccountService : IDependency
    {
        /// <summary>
        /// 根据用户ID获取银行卡
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultModel GetUserBank(long userId);
                /// <summary>
        /// 更新用户银行卡信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel UpdateModel(UserBankModel model);
        /// <summary>
        /// 添加用户银行卡信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Insert(UserBankModel model);
        /// <summary>
        /// 添加用户银行卡信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Insert(UserBankModel model, UserBankModel model2);
        /// <summary>
        /// 通过Id查询用户银行帐户信息表对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        ResultModel GetYH_UserBankAccountById(int id);

        /// <summary>
        /// 用户银行帐户信息表对象分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        ResultModel Select(SearchYH_UserBankAccountModel model);

        /// <summary>
        /// 通过Id删除用户银行帐户信息表
        /// </summary>
        /// <param name="id">用户银行帐户信息表id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        ResultModel Delete(int id);

        /// <summary>
        /// 更新用户银行帐户信息表
        /// </summary>
        /// <param name="model">用户银行帐户信息表对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        ResultModel Update(YH_UserBankAccountModel model);
    }
}
