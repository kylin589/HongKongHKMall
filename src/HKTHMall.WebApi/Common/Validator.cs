using HKTHMall.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HKTHMall.Services;
using HKSJ.MidMessage.Services;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.WebApi.Common
{
    public class Validator
    {
        /// <summary>
        /// 验证邮件合法性
        /// </summary>
        /// <param name="email"></param>
        /// <returns>0：失败 1 成功</returns>
        public static BackMessage CheckMail(string email)
        {
            BackMessage dataMsg = new BackMessage();
            dataMsg.status = 0;
            if (string.IsNullOrEmpty(email))
            {
                dataMsg.message = CultureHelper.GetLangString("REGISTER_EMAIL_MUST");
                return dataMsg;
            }
            if (!new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").IsMatch(email))
            {
                dataMsg.message = CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_EMAILERROR");//邮箱格式错误
                return dataMsg;
            }
            dataMsg.status = 1;
            if (Settings.IsEnableEM)
            {
                var useMsg = User_Account_EmailExist(email);
                if (useMsg.status==0)
                {
                    dataMsg.status = 0;
                    dataMsg.message = useMsg.message;
                }
                else
                {
                    dataMsg.message = CultureHelper.GetLangString("REGISTER_EMAIL_VALIDATION_SUCCESS");//邮箱通过
                }
            }
            else
            {
                YH_UserModel model = new Services.WebLogin.Impl.LoginService().GetUserInfoByEmail(email).Data;
                if (model != null)
                {
                    dataMsg.status = 0;
                    dataMsg.message = CultureHelper.GetLangString("REGISTER_EMAIL_HAS_BEEN_BOUND_TO_CHANGE");//邮箱已经被绑定,请更改
                }
                else
                {
                    dataMsg.message = CultureHelper.GetLangString("REGISTER_EMAIL_VALIDATION_SUCCESS");//邮箱通过
                }
            }
            return dataMsg;
        }



        /// <summary>
        /// 功能:邮箱是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="old_pwd"></param>
        /// <param name="new_pwd"></param>
        /// <returns>0:存在 1 成功</returns>
        public static BackMessage User_Account_EmailExist(string Email)
        {
            BackMessage user_msg = new BackMessage();
            var result = EmMethodManage.EmEmailInstance.MsgQueryEmailIsBindReq(Email);
            if (!result.isOK)
            {
                user_msg.status = 0;
                user_msg.message = CultureHelper.GetIMErrorLangString(result.Status);
                if (string.IsNullOrWhiteSpace(user_msg.message))
                {
                    user_msg.message = CultureHelper.GetLangString("SYSTEM_ERROR");//系统异常,请稍后再试
                }
                return user_msg;
            }
            user_msg.status = 1;
            user_msg.message = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_EMAILCANUSED");
            return user_msg;

        }
    }
}