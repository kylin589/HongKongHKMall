using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.User;
using HKTHMall.Services.AC;
using HKTHMall.Services.Users;
using HKTHMall.Admin.Filters;
using HKTHMall.Core.Utils;
using HKTHMall.Admin.common;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class AC_UserManageController : HKBaseController
    {
        private readonly IAC_DepartmentService _acDepartmentService;
        private readonly IAC_RoleService _acRoleService;
        private readonly IAC_UserService _acUserService;

        public AC_UserManageController(IAC_UserService acUserService, IAC_RoleService acRole,
            IAC_DepartmentService acDepartment)
        {
            _acUserService = acUserService;
            _acRoleService = acRole;
            _acDepartmentService = acDepartment;
        }

        public ActionResult Index()
        {
            var userModeList = new List<SelectListItem>
            {
                new SelectListItem {Text = "Enable", Value = "1", Selected = true},
                new SelectListItem {Text = "Lock", Value = "2"}
            };
            ViewData["UserMode"] = userModeList;
            ViewBag.UserID = UserInfo.CurrentUserID;//wuyf 自己的账号不能重置,只能是修改
            var ac_Department = _acDepartmentService.GetAC_DepartmentsBy().Data;
            var listDepartment = new List<SelectListItem>();
            if (ac_Department != null)
            {
                foreach (var item in ac_Department)
                {
                    var info = new SelectListItem();
                    info.Value = item.ID.ToString();
                    info.Text = item.DeptName;
                    listDepartment.Add(info);
                }
            }
            ViewData["ID"] = listDepartment;

            return View();
        }

        /// <summary>
        ///     创建用户页面
        ///     zhoub 20150707
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            AC_UserModel model = null;
            var result = new ResultModel();
            if (id.HasValue)
            {
                result = _acUserService.GetAC_UserById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            if (model == null)
            {
                model = new AC_UserModel();
            }

            var ac_Department = _acDepartmentService.GetAC_DepartmentsBy().Data;
            ViewData["ID"] = ac_Department;

            var ac_Role = _acRoleService.GetAC_RolesBy().Data;
            ViewData["RoleID"] = ac_Role;

            return PartialView(model);
        }

        /// <summary>
        ///     创建用户保存
        ///     zhoub 20150707
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(AC_UserModel model)
        {
            ResultModel resultModel = resultModel = new ResultModel();
            string opera = "";
            if (model.UserID == 0)
            {
                model.UserID = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                model.LoginTimes = 0;
                model.CreateUser = UserInfo.CurrentUserName;
                model.CreateDT = DateTime.Now;
                model.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5");
                resultModel = _acUserService.AddAC_User(model);
                opera = string.Format("系统用户添加:{0},操作结果:{1}", JsonConverts.ToJson(model), "操作成功");
                LogPackage.InserAC_OperateLog(opera, "System--User-Add");
            }
            else
            {
                model.UpdateUser = UserInfo.CurrentUserName;
                model.UpdateDt = DateTime.Now;
                resultModel = _acUserService.UpdateAC_User(model);
                resultModel.Messages = new List<string> { "Edit user success" };
                opera = string.Format("系统用户修改:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                LogPackage.InserAC_OperateLog(opera, "System--User-Edit");
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     用户更新页面
        ///     zhoub 20150708
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(long? id)
        {
            AC_UserModel model = null;
            var result = new ResultModel();
            if (id.HasValue)
            {
                result = _acUserService.GetAC_UserById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            if (model == null)
            {
                model = new AC_UserModel();
            }

            var ac_Department = _acDepartmentService.GetAC_DepartmentsBy().Data;
            ViewData["ID"] = ac_Department;

            var ac_Role = _acRoleService.GetAC_RolesBy().Data;
            ViewData["RoleID"] = ac_Role;

            return PartialView(model);
        }

        /// <summary>
        ///     用户更新保存
        ///     zhoub 20150707
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(AC_UserModel model)
        {
            ResultModel resultModel = resultModel = new ResultModel();
            model.UpdateUser = "admin";
            model.UpdateDt = DateTime.Now;
            var result = _acUserService.UpdateAC_User(model);
            resultModel.Messages = new List<string> { "Edit user success" };
            string opera = string.Format("系统用户修改:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "System--User-Edit");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     用户列表页
        ///     zhoub 20150707
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public JsonResult List(SearchUsersModel searchModel)
        {
            var result = _acUserService.GetPagingAC_Users(searchModel);

            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     删除用户
        ///     zhoub 20150707
        /// </summary>
        /// <param name="ParamenterID">用户Id</param>
        /// <returns></returns>
        public JsonResult Delete(long? ParamenterID)
        {
            var resultModel = new ResultModel();
            if (ParamenterID.HasValue)
            {
                var result = _acUserService.DeleteAC_UserById(ParamenterID.Value).Data;
                resultModel.IsValid = true;
                resultModel.Messages = new List<string> { "Delete user success" };
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            string opera = string.Format("系统用户删除:{0},操作结果:{1}", ParamenterID, resultModel.IsValid);
            LogPackage.InserAC_OperateLog(opera, "System--User-Delete");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     重置密码
        ///     zhoub 20150707
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public JsonResult RetSetPassword(long? ParamenterID)
        {
            var resultModel = new ResultModel();
            if (ParamenterID.HasValue)
            {
                var password = FormsAuthentication.HashPasswordForStoringInConfigFile("111111", "MD5");
                var result = _acUserService.ReSetAC_UserPassword(ParamenterID.Value, password).Data;
                resultModel.IsValid = true;
                resultModel.Messages = new List<string> { "Reset password success" };

            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            string opera = string.Format("系统用户重置密码:{0},操作结果:{1}", ParamenterID, resultModel.IsValid);
            LogPackage.InserAC_OperateLog(opera, "System--User-RetSetPassword");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
    }
}