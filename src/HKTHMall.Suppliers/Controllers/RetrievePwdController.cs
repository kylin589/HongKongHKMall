using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Services.Products;
using HKTHMall.Suppliers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Suppliers.Controllers
{
    /// <summary>
    /// 找回密码
    /// </summary>
    public class RetrievePwdController : Controller
    {
        private readonly ISuppliersService _suppliersService;

        public RetrievePwdController(ISuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }
        // GET: RetrievePwd
        public ActionResult Index()
        {
            return View();
        }

        #region 找回密码发送短信验证码
        /// <summary>
        /// 找回密码发送短信验证码
        /// </summary>
        /// <param name="phone">输入的手机号码</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-29</remarks>
        public JsonResult SendPhoneMsg(string phone)
        {
            if (new Regex(@"^[0][6||8-9][0-9]{8}$").IsMatch(phone))
            {
                try
                {
                    SendPhoneMsg sendMsg = new SendPhoneMsg();
                    PhoneMsg phoneMsg = sendMsg.SendMerchatPhoneCode(phone);
                    if (phoneMsg.IsMessage)
                    {
                        HKSJ.Common.DataCache.SetCache(phone, phoneMsg.PhoneCode, DateTime.Now.AddMinutes(15), TimeSpan.Zero);
                    }
                    return Json(phoneMsg.IsMessage);
                }
                catch (Exception)
                {
                    //return Json(new { IsMessage = false, Msg = CultureHelper.GetLangString("LOGIN_SEND_FAILURE") });//发送失败
                    return Json(new { IsMessage = false, Msg = "发送失败" });//发送失败
                }
            }
            else
            {
                //return Json(new { IsMessage = false, Msg = CultureHelper.GetLangString("LOGIN_PHONE_FORMAT_WRONG") });//手机号码格式不正确
                return Json(new { IsMessage = false, Msg = "手机号码格式不正确" });//手机号码格式不正确
            }
        }
        #endregion

        #region 找回密码验证手机验证码
        /// <summary>
        /// 找回密码验证手机验证码
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="phoneCode">验证码</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-29</remarks>
        [HttpPost]
        public ActionResult GetCode(string phone, string phoneCode)
        {
            var cachePhone = HKSJ.Common.DataCache.GetCache(phone);
            if (cachePhone == null || cachePhone.ToString() != phoneCode)
            {
                return new JsonResult { Data = false };
            }
            else
            {
                return new JsonResult { Data = true };
            }
        }
        #endregion

        #region 根据手机号判断账号是否存在
        /// <summary>
        /// 根据手机号判断账号是否存在
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-29</remarks>
        public JsonResult IsAccount(string phone)
        {
            if (null == _suppliersService.GetSuppliersByPhone(phone).Data)
            {
                return Json(new { rs = 1, isAccount = false });
            }
           // return Json(new { rs = 0, isAccount = true, msg = CultureHelper.GetLangString("LOGIN_PHONE_HAS_REGISTER") });//该手机号码已注册！
             return Json(new { rs = 0, isAccount = true, msg = "该手机号码已注册！" });//该手机号码已注册！
        }
        #endregion

        #region 验证登录密码是否与之前的相同
        /// <summary>
        /// 验证登录密码是否与之前的相同
        /// </summary>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-29</remarks>
        public JsonResult LoginPassExist(string StrPlyPass, string phone)
        {
            SuppliersModel model = _suppliersService.GetSuppliersByPhone(phone).Data;
            if (string.IsNullOrEmpty(StrPlyPass))
            {
                //return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_PWDNOTEMPTY") });
                return Json(new { rs = 0, msg = "密码不能为空" });
            }
            if (model == null)
            {
                //return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_USERNOTEXIST") });
                return Json(new { rs = 0, msg = "用户不存在" });
            }
            string password = CodeHelper.GetMD5(StrPlyPass);

            if (password.Equals(model.PassWord))
            {
                //return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_NOTCONSISTENTPWD") });
                return Json(new { rs = 0, msg = "新密码不能与旧密码一致" });
            }
            //return Json(new { rs = 1, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_VERIFICATION ") });
            return Json(new { rs = 1, msg = "验证通过" });
        }
        #endregion

        #region 找回密码
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="rePwd">原来的密码</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-29</remarks>
        public JsonResult CommitNewPwd(string phone, string newPwd, string rePwd)
        {
            SuppliersModel model = _suppliersService.GetSuppliersByPhone(phone).Data;
            if (model == null)
            {
                //return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_USERNOTEXIST") });
                return Json(new { rs = 0, msg = "用户不存在" });
            }
            if (newPwd == null || newPwd == "")
            {
               // return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_NEWPWD_PWDMESSAGE") });
                return Json(new { rs = 0, msg = "密码不能为空，请重新输入" });
            }
            if (newPwd.Length < 8 || new Regex(@"^[A-Za-z]+$").IsMatch(newPwd) || new Regex(@"^[0-9]*$").IsMatch(newPwd))
            {
                //return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_PASSWORD") });
                return Json(new { rs = 0, msg = "密码由8-16位数字、字母或特殊字符组成，区分大小写" });
            }
            if (newPwd.Length > 16 && new Regex(@"[^\x00-\xff]|\s").IsMatch(newPwd))
            {
                //return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_PASSWORD") });
                return Json(new { rs = 0, msg = "密码由8-16位数字、字母或特殊字符组成，区分大小写" });
            }
            if (string.IsNullOrEmpty(rePwd))
            {
               // return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_PWD_NOTNULL") });
                return Json(new { rs = 0, msg = "请输入确认密码！" });
            }
            if (newPwd != rePwd)
            {
               // return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_PWD_CONFIRM_TWO") });
                return Json(new { rs = 0, msg = "两次输入的密码不一致！" });
            }
            else
            {
                model.PassWord = CodeHelper.GetMD5(newPwd);
                model.UpdateDT = DateTime.Now;
                if (_suppliersService.UpdatePwd(model).Data == 1)
                {
                    //return Json(new { rs = 1, msg = CultureHelper.GetLangString("LOGIN_PWD_UPDATE_SUCCESS") });//修改密码成功！
                    return Json(new { rs = 1, msg = "修改密码成功！" });//修改密码成功！
                }
                else
                {
                    //return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_UPDATE_PWD_FAILURE") });//修改密码失败！
                    return Json(new { rs = 0, msg = "修改密码失败！" });//修改密码失败！
                }
            }
        }
        #endregion
    }
}