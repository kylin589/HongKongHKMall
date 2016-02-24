using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;
using HKTHMall.Core;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    ///     系统角色业务处理类
    /// </summary>
      [UserAuthorize]
    public class AC_RoleController : HKBaseController
    {
        private readonly IAC_RoleService _aC_RoleService;
        private readonly IAC_FunctionService _FunctionService;
        private readonly IAC_ModuleService _ModuleService;

        public AC_RoleController(IAC_RoleService aC_RoleService, IAC_ModuleService _ModuleService,
            IAC_FunctionService _FunctionService)
        {
            _aC_RoleService = aC_RoleService;
            this._ModuleService = _ModuleService;
            this._FunctionService = _FunctionService;
        }

        public JsonResult List(SearchAC_RoleModel searchModel)
        {
            var result = _aC_RoleService.GetPagingList(searchModel);

            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            List<AC_ModuleModel> moduleList = (_ModuleService.GetAC_ModuleList()).Data;
            ViewBag.moduleList = moduleList;
            List<AC_FunctionModel> funList = (_FunctionService.GetAC_FunList()).Data;
            ViewBag.funList = funList;
            var model = new AC_RoleModel();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create(AC_RoleModel model)
        {
            if (ModelState.IsValid)
            {
                ResultModel resultModel  = new ResultModel();
                model.CreateDT = DateTime.Now;
                model.CreateUser = UserInfo.CurrentUserName;
                model.UpdateDt = model.CreateDT;
                model.UpdateUser = model.CreateUser;
              
                var result = _aC_RoleService.Add(model);
                resultModel.Messages = new List<string> { result.Data.RoleID > 0 ? "Create a successful" : "Create role failure" };

                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            List<AC_ModuleModel> moduleList = (_ModuleService.GetAC_ModuleList()).Data;
            ViewBag.moduleList = moduleList;
            List<AC_FunctionModel> funList = (_FunctionService.GetAC_FunList()).Data;
            ViewBag.funList = funList;
            return PartialView(model);
        }

        public ActionResult Edit(int id)
        {
            List<AC_ModuleModel> moduleList = (_ModuleService.GetAC_ModuleList()).Data;
            ViewBag.moduleList = moduleList;
            List<AC_FunctionModel> funList = (_FunctionService.GetAC_FunList()).Data;
            ViewBag.funList = funList;
            AC_RoleModel model = (_aC_RoleService.GetAC_RolesById(id)).Data;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(AC_RoleModel model)
        {
            if (ModelState.IsValid)
            {
                ResultModel resultModel = resultModel = new ResultModel();
                model.UpdateDt = DateTime.Now;
                model.UpdateUser = UserInfo.CurrentUserName;
                
                var result = _aC_RoleService.Update(model);
                resultModel.Messages = new List<string> { result.Data > 0 ? "Upadate role success" : "Upadate role failure" };

                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            List<AC_ModuleModel> moduleList = (_ModuleService.GetAC_ModuleList()).Data;
            ViewBag.moduleList = moduleList;
            List<AC_FunctionModel> funList = (_FunctionService.GetAC_FunList()).Data;
            ViewBag.funList = funList;
            return PartialView(model);
        }
    }
}