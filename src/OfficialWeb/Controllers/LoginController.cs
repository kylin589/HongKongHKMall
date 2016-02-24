using HKTHMall.Domain.OfficialWeb.Models.Suppliers;
using HKTHMall.Services.OfficialWebSuppliers.Impl;
using OfficialWeb.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OfficialWeb.Controllers
{
    public class LoginController : Controller
    {
        private ISuppliersService _suppliersService;

        public LoginController(ISuppliersService suppliersService)
        {
            this._suppliersService = suppliersService;
        }

        //吴育富 20150928
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult IsLogin()
        {
            var UserName = Request["UserName"];//手机号码
            var PassWord = Request["PassWord"];//密码
            string type = "1";//用户不存在
            string Messages = "Users do not exist or password is not correct";

            try
            {
                //根据手机号码和密码查询
                SuppliersModel model = this._suppliersService.GetSuppliersMobile(UserName, FormsAuthentication.HashPasswordForStoringInConfigFile(PassWord, "MD5")).Data;

                if (model != null)
                {
                    type = "3";//登陆成功
                    Messages = "Suppliers Login success";
                    GetAC_UserByUserName(model);
                }
                var data = new
                {

                    logintype = type,
                    Messages = Messages,
                };
                return Json(data);
            }
            catch (Exception ex)
            {
                var data = new
                {

                    logintype = type,
                    Messages = Messages,
                };

                var opera = string.Format("登录错误-用户名:{0},错误结果:{1}", UserName, ex.Message);
                //LogPackage.InserAC_OperateLog(opera, "Suppliers Login error");

                return Json(data);

            }


        }

        /// <summary>
        /// 保存登陆信息和日记
        /// </summary>
        /// <param name="model"></param>
        public void GetAC_UserByUserName(SuppliersModel model)
        {
            model.PassWord = "";
            Session["SuppliersModel"] = model;//登陆成功保存用户信息

            //插入登陆时间日记
            //var result = LogPackage.InserUserLoginLog("Suppliers Login", 1);
        }
    }
}