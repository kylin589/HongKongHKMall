using HKTHMall.Core;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Services.LoginLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.Admin.common
{
    public static class LogPackage
    {
        private static AC_OperateLogService _ac_OperateLogService = new AC_OperateLogService();
        private static UserLoginLogService _userLoginLogService = new UserLoginLogService();
        private static YH_UserLoginLogService _yh_UserLoginLogService = new YH_UserLoginLogService();

        /// <summary>
        /// 添加系统操作日记
        /// </summary>
        /// <param name="OperateContent">操作内容</param>
        /// <param name="Remark">操作说明</param>
        /// <returns></returns>
        public static ResultModel InserAC_OperateLog(string OperateContent, string Remark)
        {

            var resultModel = _ac_OperateLogService.AddAC_OperateLog(new AC_OperateLogModel()
            {
                OperateID = MemCacheFactory.GetCurrentMemCache().Increment("logId"),
                UserID = UserInfo.CurrentUserID,
                OperateName = UserInfo.CurrentUserName,
                IP = ToolUtil.GetRealIP(),
                OperateContent = OperateContent,
                Remark = Remark,
                OperateTime = DateTime.Now
            });
            return resultModel;

        }

        /// <summary>
        /// 用户登录日志表
        /// </summary>
        /// <param name="LoginSource">登录来源(1:网站,2:Android,3-IOS)</param>
        /// <param name="LoginAddress">登录地址</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static ResultModel InserYH_UserLoginLog(byte LoginSource, string LoginAddress, long userID)
        {
            var resultModel = _yh_UserLoginLogService.AddYH_UserLogin(new YH_UserLoginLogModel()
            {
                UserID = userID<0?0:userID,
                LoginSource = LoginSource,
                IP = ToolUtil.GetRealIP(),
                LoginAddress = LoginAddress,
                LoginTime = DateTime.Now
            });
            return resultModel;
        }

        /// <summary>
        /// 后台用户登录日志表
        /// </summary>
        /// <param name="LoginAddress">登录地址</param>
        /// <param name="LoginSource">登录来源(1:网站,2:Android,3-IOS)</param>
        /// <returns></returns>
        public static ResultModel InserUserLoginLog(string LoginAddress, int LoginSource)
        {

            var resultModel = _userLoginLogService.AddUserLoginLog(new UserLoginLogModel()
                {
                    UserID = UserInfo.CurrentUserID,
                    UserName = UserInfo.CurrentUserName,
                    LoginSource = LoginSource,
                    IP = ToolUtil.GetRealIP(),
                    LoginAddress = LoginAddress,
                    LoginTime = DateTime.Now
                });

            return resultModel;

        }

        /// <summary>
        /// 返回josn
        /// </summary>
        /// <param name="isValid">状态</param>
        /// <param name="Messagest">提示内容</param>
        /// <returns></returns>
        public static ResultModel GetResulMessagest(bool isValid, List<string> Messagest)
        {
            var result = new ResultModel();
            result.IsValid = isValid;
            result.Messages = Messagest;
            return result;
        }
    }
}