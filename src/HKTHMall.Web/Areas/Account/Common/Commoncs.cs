using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Mvc;
using System.Text.RegularExpressions;

using HKSJ.Common;
using System.Configuration;

using System.Management;
using HKTHMall.Core;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Domain.Models.Sys;
using HKTHMall.Services.Sys;

namespace HKTHMall.Web.Account
{
    public class Commoncs
    {
    

        /// <summary>
        /// 短信验证码比对
        /// </summary>
        /// <param name="phoneNum">手机号码</param>
        /// <param name="phoneCode">短信验证码</param>
        /// <returns></returns>
        public static bool PhoneVerificationCode(string phoneNum, string phoneCode)
        {
            if (System.Web.HttpContext.Current.Session["GetPhoneMsgDateTime"] != null)
            {
                if ((DateTime.Now - Convert.ToDateTime(System.Web.HttpContext.Current.Session["GetPhoneMsgDateTime"].ToString())).TotalSeconds > 900)//超时,验证码失效
                {
                    //清空Session
                    System.Web.HttpContext.Current.Session["phoneNum"] = "";
                    System.Web.HttpContext.Current.Session["phoneCode"] = "";
                    return false;
                }
            }
            if (System.Web.HttpContext.Current.Session["phoneNum"] != null && System.Web.HttpContext.Current.Session["phoneCode"] != null)
            {
                //判断是否同一手机和正确的验证码
                if (System.Web.HttpContext.Current.Session["phoneCode"].ToString() == phoneCode && System.Web.HttpContext.Current.Session["phoneNum"].ToString() == phoneNum)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
     
    }
}