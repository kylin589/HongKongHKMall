using System.Collections.Generic;
using System.Web.Mvc;
using Castle.Core.Logging;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;
using HKTHMall.Services.Products;
using System;
using HKTHMall.Admin.Filters;
using HKTHMall.Domain.Models.User;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Users;
using System.Security.Cryptography;
using System.Web.Security;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class HomeController : HKBaseController
    {
        private readonly ICategoryService _categoryService;
        private IAC_RoleService _roleService;
        private readonly AC_UserService _acUserService =new AC_UserService();

        public ILogger _logger;
        public HomeController(ICategoryService categoryService, IAC_RoleService _roleService)
        {
            this._categoryService = categoryService;
            this._roleService = _roleService;
            _logger = NullLogger.Instance;
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //string bb = DateTime.Now.ToString("YYMMDDhhmm");
            _logger.Error("ssssssss");
            //Session["a"] = "oererr";
            //long id = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
            //string ip = ToolUtil.GetRealIP();
           // ViewBag.UserName = "admin";
            
            for(int a=0;a<10;a++)
            {
                long id = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
            }
            if (UserInfo.CurrentUserRoleID==0)
            {
                //Response.Redirect("/AC_User/login");
            }
            ViewBag.UserName = UserInfo.CurrentUserName;
            ViewBag.UserID = UserInfo.CurrentUserID;
            int roleId=UserInfo.CurrentUserRoleID;
            List<AC_ModuleModel> menuList = new List<AC_ModuleModel>();
            if(roleId>0)
            {
                menuList = (_roleService.GetModuleMenuList(roleId)).Data;
            }
            return View(menuList);
        }

        public ActionResult UpdatePass(long? UserID)
        {
            AC_UserModel model = new AC_UserModel();
           
            if (UserID.HasValue)
            {
                
                model.UserID = UserID.Value;
            }
            

            

            return PartialView(model);
            
        }

        [HttpPost]
        public ActionResult UpdatePass(AC_UserModel model)
        {
            ViewBag.ErrorMegess = "";
            var resultModel = new ResultModel();
            AC_UserModel acmodel = _acUserService.GetAC_UserById(model.UserID).Data;
            if (MD5.Equals(acmodel.Password, FormsAuthentication.HashPasswordForStoringInConfigFile(model.PasswordOld, "MD5")) && acmodel.UserMode == 1)
            {
                var password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5");
                var result = _acUserService.ReSetAC_UserPassword(model.UserID, password).Data;
                resultModel.IsValid = true;
                resultModel.Messages = new List<string> { "Upadate password success" };
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.ErrorMegess = "The original password is not correct";
            }
            return PartialView(model);
        }

        public ActionResult Test()
        {
            return View();
        }
	}
}