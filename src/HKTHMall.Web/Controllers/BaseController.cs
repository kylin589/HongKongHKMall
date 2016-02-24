using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using HKTHMall.Core;
using HKTHMall.Domain.Entities;
using HKTHMall.Web.Common;
using HKTHMall.Domain.WebModel.Models.Login;
using System.Threading;
using System.Globalization;
using HKTHMall.Web.Models;
using System.Web.Security;
using Newtonsoft.Json;
using HKTHMall.Services.Common.MultiLangKeys;


namespace HKTHMall.Web.Controllers
{
    public class BaseController : Controller
    {

        public BaseController()
        {
            ViewBag.RootImage = ToolUtil.getFilePath("ImagePath");
        }

        /// <summary>
        /// 设置用户登录信息
        /// </summary>
        /// <param name="resultModel"></param>
        protected void SetAuthCookie(UserModel resultModel)
        {
            CookieHelper.SetCookies(Const.Cookies_UserId,resultModel.UserID.ToString(),true);
            CookieHelper.SetCookies(Const.Cookies_Username, resultModel.Account, true);
            //CookieHelper.SetCookies(Const.Cookies_Phone, resultModel.Phone, true);
            CookieHelper.SetCookies(Const.Cookies_NickName, resultModel.NickName, true);
            CookieHelper.SetCookies(Const.Cookies_UserType, resultModel.UserType.ToString(), true);
            CookieHelper.SetCookies(Const.Cookies_Email, resultModel.Email.ToString(), true);
            //创建票证
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                resultModel.Phone??string.Empty,
                DateTime.Now,
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                false,
                JsonConvert.SerializeObject(resultModel, Formatting.None));

            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                Domain = FormsAuthentication.CookieDomain
                //Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };

            Response.Cookies.Add(authCookie);
        }

        /// <summary>
        ///     删除cookie
        /// </summary>
        /// <param name="key"></param>
        protected void RemoveCookie()
        {
            CookieHelper.RemoveCookies(Const.Cookies_UserId);
            CookieHelper.RemoveCookies(Const.Cookies_Username);
            CookieHelper.RemoveCookies(Const.Cookies_UserType);
            CookieHelper.RemoveCookies(Const.Cookies_NickName);
            CookieHelper.RemoveCookies(Const.Cookies_Email);
        }

        #region 登录用户基本信息

        private long _UserID;
        private string _Account;
        private string _Email;
        private string _Phone;
        private string _NickName;
        private int _UserType;

        /// <summary>
        /// 用户ID
        /// </summary>
        protected long UserID
        {
            get
            {
                if (_UserID <= 0)
                {
                    var user = GetUserResultModel();
                    _UserID = user != null ? user.UserID : 0;
                }
                return _UserID;
            }
        }

        protected string Email
        {
            get
            {
                if (Util.StrIsNullOrEmpty(_Email))
                {
                    var user = GetUserResultModel();
                    _Email = user != null ? user.Email : "";
                }
                return _Email;
            }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        protected string Account
        {
            get
            {
                if (Util.StrIsNullOrEmpty(_Account))
                {
                    var user = GetUserResultModel();
                    _Account = user != null ? user.Account : "";
                }
                return _Account;
            }
        }

        /// <summary>
        /// 手机号
        /// </summary>
        protected string Phone
        {
            get
            {
                if (Util.StrIsNullOrEmpty(_Phone))
                {
                    var user = GetUserResultModel();
                    _Phone = user != null ? user.Phone : "";
                }
                return _Phone;
            }
        }

        /// <summary>
        /// 昵称
        /// </summary>
        protected string NickName
        {
            get
            {
                if (Util.StrIsNullOrEmpty(_NickName))
                {
                    var user = GetUserResultModel();
                    _NickName = user != null ? user.NickName : "";
                }
                return _NickName;
            }
        }


        /// <summary>
        /// 用户类型
        /// </summary>
        protected int UserType
        {
            get
            {
                if (Util.StrIsNullOrEmpty(_UserType.ToString()))
                {
                    var user = GetUserResultModel();
                    _UserType = user != null ? user.UserType : 0;
                }
                return _UserType;
            }
        }


        /// <summary>
        /// 获取用户登录的序列化对象
        /// </summary>
        /// <returns></returns>
        protected UserModel GetUserResultModel()
        {
            try
            {
                string cookieValue = CookieHelper.GetCookies(FormsAuthentication.FormsCookieName, false);

                if (!Util.StrIsNullOrEmpty(cookieValue))
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookieValue);

                    if (ticket != null)
                        return JsonConvert.DeserializeObject<UserModel>(ticket.UserData);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = string.Empty;
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                //update by liuji.默认语言为繁体
                //cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                //    Request.UserLanguages[0] : null;
                cultureName = "zh-HK";
            }
            cultureName = CultureHelper.GetImplementedCulture(cultureName);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            return base.BeginExecuteCore(callback, state);
        }
    }
}