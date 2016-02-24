using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;

namespace HKTHMall.Services.WebLogin
{
    public interface ILoginService:IDependency
    {
        /// <summary>
        /// 根据手机号获取用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        ResultModel GetUserInfoByPhone(string phone);
          /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultModel GetUserInfoById(long id);
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        ResultModel Update(YH_UserModel model);
          /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        ResultModel Add(YH_UserModel model);
          /// <summary>
        /// 根据账号获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        ResultModel GetUserInfoByAccount(string account);
       /// <summary>
        /// 根据邮箱获取用户信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        ResultModel GetUserInfoByEmail(string email);
        
       
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="welcomeCode"></param>
        /// <param name="phone"></param>
        /// <param name="pwd"></param>
        /// <param name="pwd_md5"></param>
        /// <param name="code"></param>
        /// <param name="_user"></param>
        /// <returns></returns>
        BackMessage Register( string pwd, string pwd_md5, string email, out YH_UserModel _user);

         /// <summary>
        /// 获取邮箱验证
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultModel GetModelByUserID(long userId);

           /// <summary>
        /// 更改邮箱验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel UpdateValidEmail(YH_ValidEmailModel model);

           /// <summary>
        /// 插入邮箱验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddValidEmail(YH_ValidEmailModel model);

        
      

        /// <summary>
        /// 第三方账号登录 
        /// </summary>
        /// <param name="id_3rd"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ResultModel LoginThree(string id_3rd, string name, int type);

    }
}
