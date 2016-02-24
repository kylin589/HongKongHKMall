using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKSJ.Common;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.WebLogin;

using HKTHMall.Web.Common;

using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Services.LoginLog;
using HKTHMall.Services.Sys;
using HKTHMall.Domain.Models.Sys;
using HKTHMall.Services;
using System.Text.RegularExpressions;
using HKTHMall.Web.Models;
using System.Web.Security;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Core.Security;
using HKTHMall.Web.Account;


namespace HKTHMall.Web.Controllers
{
    public class LoginController : BaseController
    {
        /// <summary>
        ///     登录服务类
        /// </summary>
        private readonly ILoginService _LoginService;
        private readonly IYH_UserLoginLogService _YH_userLoginLogService;
        private readonly IParameterSetService _parameterSetService;
        private readonly IYH_PasswordErrorService _passwordErrorService;
        BackMessage msg = new BackMessage();
        private readonly IEncryptionService _enctyptionService;
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="acDepartmentService"></param>
        public LoginController(ILoginService loginService, IYH_UserLoginLogService loginlogService, IParameterSetService parameterService, IYH_PasswordErrorService passwordService, IEncryptionService enctyptionService)
        {
            _LoginService = loginService;
            _YH_userLoginLogService = loginlogService;
            _parameterSetService = parameterService;
            _passwordErrorService = passwordService;
            _enctyptionService = enctyptionService;
        }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

       

        /// <summary>
        /// 找回密码页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPassword()
        {
            return View();
        }

        /// <summary>
        /// 功能:验证登录密码是否与之前的相同
        /// </summary>
        /// <returns></returns>
        public JsonResult LoginPassExist(string StrPlyPass, string phone)
        {
            YH_UserModel model = _LoginService.GetUserInfoByPhone(phone).Data;
            if (string.IsNullOrEmpty(StrPlyPass))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_PWDNOTEMPTY") });
            }
            if (model == null)
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_USERNOTEXIST") });
            }
            string password = CodeHelper.GetMD5(StrPlyPass);

            if (password.Equals(model.PassWord))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_NOTCONSISTENTPWD") });
            }
            return Json(new { rs = 1, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_VERIFICATION ") });
        }
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="newPwd"></param>
        /// <param name="rePwd"></param>
        /// <returns></returns>
        public JsonResult CommitNewPwd(string newPwd, string rePwd, string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_UPDATE_PWD_FAILURE") });//修改密码失败！
            }
            YH_UserModel model = _LoginService.GetUserInfoByEmail(email).Data;
            if (model == null)
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_USERNOTEXIST") });
            }
            if (newPwd == null || newPwd == "")
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_NEWPWD_PWDMESSAGE") });
            }
            if (newPwd.Length < 8 || new Regex(@"^[A-Za-z]+$").IsMatch(newPwd) || new Regex(@"^[0-9]*$").IsMatch(newPwd))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_PASSWORD") });
            }
            if (newPwd.Length > 16 && new Regex(@"[^\x00-\xff]|\s").IsMatch(newPwd))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_PASSWORD") });
            }
            if (string.IsNullOrEmpty(rePwd))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_PWD_NOTNULL") });
            }
            if (newPwd != rePwd)
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_PWD_CONFIRM_TWO") });
            }
            else
            {
                //if (Settings.IsEnableEM)
                //{

                //    Int64 uid = model.UserID; //Convert.ToInt64(HKSJ.Common.DataCache.GetCache(model.UserID.ToString()));
                //    string NewPwd = CodeHelper.GetMD5(newPwd);
                //    var result = EmMethodManage.EmEmailInstance.ForgetPwdMailResetPwdReq(key, NewPwd);

                //    if (result.isOK && result.Status == 0)
                //    {
                //        model.PassWord = NewPwd;
                //        if (_LoginService.Update(model).Data == 1)
                //        {
                //            return Json(new { rs = 1, msg = CultureHelper.GetLangString("LOGIN_PWD_UPDATE_SUCCESS") });//修改密码成功！
                //        }
                //        return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_UPDATE_PWD_FAILURE") });//修改密码失败！
                //    }
                //    else
                //    {
                //        return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_UPDATE_PWD_FAILURE") });//修改密码失败！
                //    }
                //}
                //else
                //{
                model.PassWord = CodeHelper.GetMD5(newPwd);
                model.UpdateDT = DateTime.Now;
                if (_LoginService.Update(model).Data == 1)
                {
                    return Json(new { rs = 1, msg = CultureHelper.GetLangString("LOGIN_PWD_UPDATE_SUCCESS") });//修改密码成功！
                }
                else
                {
                    return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_UPDATE_PWD_FAILURE") });//修改密码失败！
                }
                //}
            }
        }



        /// <summary>
        /// 找回密码成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSuccess(string cekey = "")
        {
            string flag = "false";
            var aa = MemCacheFactory.GetCurrentMemCache().GetCache<string>(Const.Cache_FindEmail);
            if (!string.IsNullOrEmpty(aa))
            {
                if (cekey == aa.Split('|')[1].Replace("+", " "))
                {
                    ViewBag.email = aa.Split('|')[0];
                    ViewBag.isFirst = "true";
                }
            }
            else
            {
                ViewBag.isFirst = "false";
            }
            return View();
        }

        [HttpPost]
        public JsonResult Index(LoginModel m)
        {
            YH_UserModel model = null;
            var modelResult = _LoginService.GetUserInfoByEmail(m.account);
            model = modelResult.Data;
            if (model != null)//添加
            {
                if (model.IsLock == 1)
                {
                    return Json(new { status = 0, type = 1, message = CultureHelper.GetLangString("LOGIN_ACCOUNT_LOCKED") });//您输入的账号已经被锁定!
                }
                if (model.IsDelete == 1)
                {
                    return Json(new { status = 0, type = 1, message = CultureHelper.GetLangString("LOGIN_ACCOUNT_NOT_EXIST") });//您输入的账号不存在,请核对后重新输入!
                }
                if (model.ActiveEmail != 1)
                {
                    return Json(new { status = 0, type = 1, message = CultureHelper.GetLangString("HOME_LOGIN_NOACTIVENOTLOGIN") });//您输入的账号不存在,请核对后重新输入!
                }
                if(!string.IsNullOrEmpty(model.ThirdID)&&model.ThirdType>0)
                {
                    return Json(new { status = 0, type = 1, message = CultureHelper.GetLangString("LOGIN_ACCOUNT_NOT_EXIST") });//您输入的账号不存在,请核对后重新输入!
                }
                model.ActivePhone = 0;
                model.ActiveEmail = 1;
                return judgePwdError(model, m);
            }
            else
            {
                return Json(new { status = 0, type = 1, message = CultureHelper.GetLangString("LOGIN_ACCOUNT_NOT_EXIST") });//您输入的账号不存在,请核对后重新输入!
            }

        }

        /// <summary>
        /// 发送邮件 zzr
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult SendEmail(string email)
        {
            var result = _LoginService.GetUserInfoByEmail(email);
            if (result.Data != null)
            {

                #region 发送验证邮箱
                try
                {
                    _enctyptionService.RSADecrypt("");
                    _enctyptionService.RSAEncrypt("");
                    System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                    string cod = CodeHelper.GetRandomNumber(6);
                    objCache.Insert("RegGetCodeTime" + email, cod, null, DateTime.Now.AddDays(1), TimeSpan.Zero);
                    string key = _enctyptionService.RSAEncrypt(email);
                    string link = Request.Url.Authority + "/Login/GetSuccess?cekey=" + key + "&Verification=" + _enctyptionService.RSAEncrypt(cod);
                    string emailContent = CultureHelper.GetLangString("HK_FAILDPWDEMAIL");
                    link = "http://" + link;
                    emailContent = string.Format(emailContent, email, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "<a href='" + link + "'>" + link + "</a>");
                    bool flag = Mail.sendMail(email, CultureHelper.GetLangString("HOME_LOGIN_APPLYRESETPWDEMAIL"), emailContent);
                    if (flag)
                    {
                        MemCacheFactory.GetCurrentMemCache().AddCache<string>(Const.Cache_FindEmail, email + "|" + key, 120);
                        return Json(new { status = 1, message = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_EMAILSENDSUCCESS") }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 0, message = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_EMAILSENDFAILURE") }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json("邮件发送异常", JsonRequestBehavior.AllowGet);
                }
                #endregion
            }
            else
            {
                return Json(new { status = 0, message = CultureHelper.GetLangString("ACCOUNT_EMAIL_NOTREGISTER") }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 本地登录
        /// </summary>
        [NonAction]
        public JsonResult LoginThisAddress(YH_UserModel model, LoginModel m)
        {
            char[] c = m.account.ToCharArray();
            if (m.account.Contains("@"))//邮箱登陆
            {
                model = _LoginService.GetUserInfoByEmail(m.account).Data;
            }
            else if (c[0] >= 'A' && c[0] <= 'z')  //账号登录
            {
                model = _LoginService.GetUserInfoByAccount(m.account).Data;
            }
            else//手机号登陆
            {
                model = _LoginService.GetUserInfoByPhone(m.account).Data;
            }

            if (null != model)
            {
                if (model.IsLock == 1)
                {
                    return Json(new { status = 0, type = 1, message = CultureHelper.GetLangString("LOGIN_ACCOUNT_LOCKED") });//您输入的账号已经被锁定!
                }
                else if (model.IsDelete == 1)
                {
                    return Json(new { status = 0, type = 1, message = CultureHelper.GetLangString("LOGIN_ACCOUNT_NOT_EXIST") });//您输入的账号不存在,请核对后重新输入!
                }
                else
                {
                    return judgePwdError(model, m);
                }
            }
            else
            {
                return Json(new { status = 0, type = 1, message = CultureHelper.GetLangString("LOGIN_ACCOUNT_NOT_EXIST") });//您输入的账号不存在,请核对后重新输入!
            }
        }

        public JsonResult commonLogin(LoginModel m, YH_UserModel model)
        {
            UserModel cookieModel = new UserModel();
            cookieModel.UserID = model.UserID;
            cookieModel.Account = model.Account;
            cookieModel.Phone = model.Phone == null ? "" : model.Phone;
            cookieModel.NickName = model.NickName;
            cookieModel.UserType = int.Parse(model.UserType.ToString());
            cookieModel.Email = model.Email;
            SetAuthCookie(cookieModel);

            if (m.IsJz) //记住用户名
            {
                //存储7天帐号
                HttpCookie cookie = Request.Cookies["loginUserPhone"];
                if (cookie == null || Request.Cookies["loginUserPhone"]["loginUserPhone"] != m.account)
                {
                    cookie = new HttpCookie("loginUserPhone");
                    cookie.Values.Set("loginUserPhone", m.account);
                    cookie.Expires = DateTime.Now.AddDays(7);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                }
            }
            else
            {
                //清除登录用户名
                HttpCookie cookie = new HttpCookie("loginUserPhone");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            //新增用户登录日志 
            YH_UserLoginLogModel user = new YH_UserLoginLogModel
            {
                UserID = model.UserID,
                LoginSource = 1,
                IP = Util.KeFu_GetIP(),
                LoginTime = DateTime.Now
            };
            int res = _YH_userLoginLogService.AddYH_UserLogin(user).Data.ID;
            if (res == 0)
            {
                return Json(new { status = 0, type = 2, message = CultureHelper.GetLangString("LOGIN_LOGIN_FAILED") });//登录失败
            }
            msg.message = CultureHelper.GetLangString("LOGIN_LOGIN_SUCCESS");//登录成功
            msg.status = 1;
            return Json(msg);
        }

        /// <summary>
        /// 判断密码错误
        /// </summary>
        /// <param name="model"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public JsonResult judgePwdError(YH_UserModel model, LoginModel m)
        {

            int loginNums = int.Parse(_parameterSetService.GetParametePValueById(1215896046).Data);//登陆错误次数  5次
            int loginTimes = int.Parse(_parameterSetService.GetParametePValueById(1215896045).Data);//锁定时间   120分钟

            List<YH_PasswordErrorModel> pwdlist = _passwordErrorService.GetPasswordErrorInfo(model.UserID, 1).Data;
            YH_PasswordErrorModel pwd = null;
            if (pwdlist.Count > 0)
            {
                pwd = (YH_PasswordErrorModel)pwdlist[0];
            }
            int verNums = pwd == null ? 0 : pwd.FailVerifyTimes;
            if (null != pwd)
            {
                var min = (DateTime.Now - pwd.VerifyTime).TotalMinutes;
                if (min < loginTimes)
                {
                    if (verNums >= loginNums) //当天两小时内次数大于等于5
                    {
                        string errorMsg = string.Empty;
                        if (Convert.ToInt32(loginTimes - min) <= 0)
                        {
                            errorMsg = CultureHelper.GetLangString("LOGIN_ACCOUNT_LOCKED_PLS_ASK_UNLOCK");//账号已被锁定,请于1分钟解锁后再试!
                        }
                        else
                        {
                            //账号已被锁定,请于{0}分钟解锁后再试!
                            //errorMsg = CultureHelper.GetLangString("API_ACCOUNTPASSWORDERROR") + "," + CultureHelper.GetLangString("HOME_CEBIANLAN_HAIKEYISHURU") + Convert.ToInt32(loginTimes - min) + CultureHelper.GetLangString("LOGIN_TIMES");
                            errorMsg = string.Format(CultureHelper.GetLangString("LOGIN_ACCOUNTLOCKED"), Convert.ToInt32(loginTimes - min));
                        }
                        return Json(new { status = 0, type = 1, message = errorMsg });
                    }
                }
            }
            if (CodeHelper.GetMD5(m.passWord) != model.PassWord) //密码输入错误后 判断输入密码错误次数大于等于0并且小于5 就新增或修改一条密码输入错误表数据
            {
                if (pwd == null)
                {
                    pwd = new YH_PasswordErrorModel()
                    {
                        FailVerifyTimes = 1,
                        VerifyTime = DateTime.Now,
                        Account = model.Account,
                        UserID = model.UserID,
                        PassWordType = 1
                    };
                    _passwordErrorService.AddError(pwd);
                }
                else
                {
                    double min = (DateTime.Now - pwd.VerifyTime).TotalMinutes;
                    pwd.Account = model.Account;
                    pwd.FailVerifyTimes = min > loginTimes ? 1 : pwd.FailVerifyTimes + 1;
                    pwd.VerifyTime = DateTime.Now;
                    _passwordErrorService.Update(pwd);
                }


                int[] f = { 0, 1, 2, 3, 4, 5 };
                if (f[pwd.FailVerifyTimes] == 5)
                {
                    //账号已被锁定,请于{0}分钟解锁后再试!
                    return Json(new { status = 0, type = 1, message = string.Format(CultureHelper.GetLangString("LOGIN_ACCOUNTLOCKED"), 120) });
                }
                return Json(new { status = 0, type = 2, times = f[pwd.FailVerifyTimes], message = string.Format(CultureHelper.GetLangString("ACCOUNT_LOGIN_ERRORTIMES"), (loginNums - f[pwd.FailVerifyTimes])) });//密码错误,还可以再输入  //次!

                //替换掉
                //msg.status = 0;
                ////密码输入错误 //次,请重新输入!
                //msg.message = CultureHelper.GetLangString("LOGIN_LOGIN_PWD_WRONG") + f[pwd.FailVerifyTimes].ToString() + CultureHelper.GetLangString("LOGIN_TIME_INPUT_AGAIN");
                //return Json(msg);
            }
            else
            {
                if (pwd != null)
                {
                    pwd.VerifyTime = DateTime.Now;
                    pwd.FailVerifyTimes = 0;
                    _passwordErrorService.Update(pwd);
                }
                return commonLogin(m, model);
            }
        }

        /// <summary>获取当前登陆的用户</summary>
        /// <remarks></remarks>
        /// <author>Yun 2015-04-19 09:32:26</author>
        /// <returns>ActionResult.</returns>
        public ActionResult getCur_YH_User()
        {
            var user = GetUserResultModel();
            if (user != null && user.UserID > 0)
            {
                return new JsonResult { Data = new { ID = base.UserID, Account = base.Account } };
            }
            else
            {
                return new JsonResult { Data = null };
            }
        }

        //退出
        [HttpPost]
        public void LoginOut()
        {
            FormsAuthentication.SignOut();

            RemoveCookie();

            //if (Request.IsAjaxRequest())
            //{
            //    loginuser = null;
            //    System.Web.Security.FormsAuthentication.SignOut();
            //    CookieHelper.OpCookieItem(Const.TMall_Cookie_Info_Key, -1, Const.TMall_Domain);             
            //}
        }

        [HttpPost]
        public JsonResult ThirdLogin(string id, string name, int sourceType)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name) && sourceType > 0)
            {
                ResultModel rm = _LoginService.LoginThree(id, name, sourceType);
                YH_UserModel model = rm.Data;
                if (rm.IsValid && model != null)
                {
                    var user = new UserModel { UserID = model.UserID, Account = model.Account, NickName = model.NickName, Email = model.Email, Phone = model.Phone, UserType = (int)model.UserType };
                    //SetAuthCookie(new UserModel { UserID = model.UserID, Account = model.Account, NickName = model.NickName, Eamil = model.Email, Phone = model.Phone, UserType = (int)model.UserType });
                    model.Phone = model.Phone ?? "";
                    return commonLogin(new LoginModel { IsJz = false }, model);
                    //return Json(new { status = 1 });
                }
            }
            return Json(new { status = 0, msg = "ERROR" });
        }
        
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDialog"></param>
        /// <returns></returns>
        public ActionResult _Login(bool? isDialog)
        {
            HttpCookie cookie = Request.Cookies["loginUserPhone"];
            if (cookie != null && Request.Cookies["loginUserPhone"] != null && !string.IsNullOrEmpty(Request.Cookies["loginUserPhone"]["loginUserPhone"]))
            {
                ViewBag.email = Request.Cookies["loginUserPhone"]["loginUserPhone"].ToString();
            }
            return PartialView(isDialog);
        }

    }
}