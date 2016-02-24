using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Services.Products;

namespace HKTHMall.Suppliers.Controllers
{
    public class UserInfoController : Controller
    {
        private readonly ISuppliersService _suppliersService;

        /// <summary>
        /// 构造函数
        /// ywd 20150925
        /// </summary>
        /// <param name="suppliersService"></param>
        public UserInfoController(ISuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }

        #region 修改登陆密码操作

        /// <summary>
        /// 验证原始登录密码是否正确
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult LoginPassVerify(string pwd)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                return Json(new { rs = 0, msg = "LOGIN_GETPASSWORD_PWDNOTEMPTY" });//密码不能为空
            }
            SuppliersModel model = _suppliersService.GetSuppliersByPhone("098888888").Data;
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
            if (!password.Equals(model.PassWord))
            {
                return Json(new { rs = 0, msg = "USERINFO_ORIGINAL_PASSWORD_INCORRECT" });//原始密码不正确
            }
            return Json(new { rs = 1, msg = "USERINFO_ORIGINAL_PASSWORD_CORRECT" });//原始密码正确
        }


        /// <summary>
        /// 验证新的登录密码是否与之前的相同
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult LoginPassExist(string StrPlyPass)
        {
            if (string.IsNullOrEmpty(StrPlyPass))
            {
                return Json(new { rs = 0, msg = "LOGIN_GETPASSWORD_PWDNOTEMPTY" });//密码不能为空
            }
            SuppliersModel model = _suppliersService.GetSuppliersByPhone("098888888").Data;
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(StrPlyPass, "MD5");
            if (password.Equals(model.PassWord))
            {
                return Json(new { rs = 0, msg = "LOGIN_GETPASSWORD_NOTCONSISTENTPWD" });//新密码不能与旧密码一致
            }
            return Json(new { rs = 1, msg = "USERINFO_VALIDATION_SUCCESS" });//验证通过
        }

        /// <summary>
        /// 功能:修改登录密码页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UpdatePass()
        {
            string phone = "098888888";//base.Phone.Substring(0, 3) + "***" + base.Phone.Substring(base.Phone.Length - 4);
            ViewBag.Phone = phone;
            return View();
        }

        /// <summary>
        /// 功能:修改登录密码方法
        /// </summary>
        /// <param name="code">短信验证码</param>
        /// <param name="PassWord">原始密码</param>
        /// <param name="NewPassWord">新密码</param>
        /// <param name="okPassWord">确认新密码</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult UpdatePass(string PassWord, string NewPassWord, string okPassWord)
        {
            if (string.IsNullOrEmpty(NewPassWord))
            {
                return Json(new { rs = 0, msg = "LOGIN_GETPASSWORD_PWDNOTEMPTY" });//密码不能为空
            }
            if (!NewPassWord.Equals(okPassWord))
            {
                return Json(new { rs = 0, msg = "USERINFO_TWO_PASSWORD_NOT_SAME" });//新密码与确认密码不一致！
            }
            SuppliersModel model = _suppliersService.GetSuppliersByPhone("098888888").Data;
            if (model == null)
            {
                return Json(new { rs = 0, msg = "USERINFO_USER_NOT_EXIST" });//用户不存在！
            }
            if (!model.PassWord.Equals(FormsAuthentication.HashPasswordForStoringInConfigFile(PassWord, "MD5")))
            {
                return Json(new { rs = 0, msg = "USERINFO_ORIGINAL_PASSWORD_INCORRECT" });//原始密码不正确！
            }
            if (model.PassWord.Equals(FormsAuthentication.HashPasswordForStoringInConfigFile(NewPassWord, "MD5")))
            {
                return Json(new { rs = 0, msg = "LOGIN_GETPASSWORD_NOTCONSISTENTPWD" });//新密码不能与旧密码一致！
            }

            model.PassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(NewPassWord, "MD5");
            model.UpdateDT = DateTime.Now;


            if (_suppliersService.UpdatePwd(model).Data == 1)
            {
                FormsAuthentication.SignOut();
                return Json(new { rs = 1, msg = "USERINFO_UPDATE_SUCCESS" });//修改成功！
            }
            
            return Json(new { rs = 0, msg = "USERINFO_UPDATE_FAILURE" });//修改失败！

        }

        #endregion
    }
}