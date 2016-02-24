using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HKTHMall.Web.Common
{
    /// <summary>
    /// cookies 操作类
    /// </summary>
    public class CookieHelper
    {
        

        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="value">值</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        public static void SetCookies(string key, string value, TimeSpan? timeSpan)
        {
            SetCookies(key, value, false, timeSpan);
        }

        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="value">值</param>
        /// <param name="isEncrypt">是否加密</param>
        /// <returns></returns>
        public static void SetCookies(string key, string value, bool isEncrypt)
        {
            SetCookies(key, value, isEncrypt, null);
        }

        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="value">值</param>
        /// <param name="isEncrypt">是否加密</param>
        /// <param name="timeSpan">过期时间</param>
        /// <returns></returns>
        public static void SetCookies(string key, string value, bool isEncrypt, TimeSpan? timeSpan)
        {
            HttpContext.Current.Response.Cookies.Remove(key);
            HttpCookie cookies = new HttpCookie(key)
            {
                Value = isEncrypt
                        ? EncryptUtils.DES.Encrypt(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")))
                        : HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312"))
            };

            if (timeSpan != null)
                cookies.Expires = DateTime.Now.Add(timeSpan.Value);

            HttpContext.Current.Response.AppendCookie(cookies);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">名称</param>
        /// <returns></returns>
        public static string GetCookies(string key)
        {
            return GetCookies(key, false);
        }

        /// <summary>
        /// 获取Cookies
        /// </summary>
        /// <param name="key">名称</param>
        /// <param name="isDecrypt">是否解密</param>
        /// <returns></returns>
        public static string GetCookies(string key, bool isDecrypt)
        {
            HttpCookie cookies = HttpContext.Current.Request.Cookies[key];
            if (cookies != null)
            {
                return isDecrypt
                    ? HttpUtility.UrlDecode(EncryptUtils.DES.Decrypt(cookies.Value))
                    : HttpUtility.UrlDecode(cookies.Value, Encoding.GetEncoding("gb2312"));
            }
            return string.Empty;
        }
        /// <summary>
        /// 删除Cookies
        /// </summary>
        /// <param name="key">名称</param>
        public static void RemoveCookies(string key)
        {
            HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
        }
    }

    public static class Const
    {
        

        /// <summary>
        /// Cookies , 登录Cookies - 用户ID
        /// </summary>
        public const string Cookies_UserId = "UserID";

        /// <summary>
        /// Cookies , 登录Cookies - 用户名称
        /// </summary>
        public const string Cookies_Username = "Account";

        /// <summary>
        /// Cookies , 登录Cookies - 用户手机号
        /// </summary>
        public const string Cookies_Phone = "Phone";

        /// <summary>
        /// Cookies , 登录Cookies - 用户昵称
        /// </summary>
        public const string Cookies_NickName = "NickName";

        /// <summary>
        /// Cookies , 登录Email - Email
        /// </summary>
        public const string Cookies_Email = "Email";

        /// <summary>
        /// Cookies , 登录Cookies - 用户类型
        /// </summary>
        public const string Cookies_UserType = "UserType";

        /// <summary>
        /// Cache,注册时的验证码
        /// </summary>
        public const string Cache_ValidateCode = "ValidateCode";
        /// <summary>
        /// Cache,找回邮箱时的邮箱帐号
        /// </summary>
        public const string Cache_FindEmail = "FindEmail";
        /// <summary>
        /// Cache注册时的验证邮箱
        /// </summary>
        public const string Cache_ValidateEmail = "ValidateEmail";
        /// <summary>
        /// 修改登录密码验证码
        /// </summary>
        public const string CACHE_VALIDATECODE_FOR_Modify_LOGIN_PWD = "ValidateCodeForModifyLoginPwd";
        /// <summary>
        /// 修改交易密码验证码
        /// </summary>
        public const string CACHE_VALIDATECODE_FOR_Modify_PAY_PWD = "ValidateCodeForModifyPayPwd";

    }

}