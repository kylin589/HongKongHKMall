using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    ///     部门控制器
    /// </summary>
    [UserAuthorize]
    public class AC_DepartmentController : HKBaseController
    {
        /// <summary>
        ///     部门服务类
        /// </summary>
        private readonly IAC_DepartmentService _ACDepartmentService;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="acDepartmentService"></param>
        public AC_DepartmentController(IAC_DepartmentService acDepartmentService)
        {
            _ACDepartmentService = acDepartmentService;
        }

        /// <summary>
        ///     列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     列表页数据
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public JsonResult List(SearchAC_DepartmentModel searchModel)
        {
            searchModel.DeptName = string.IsNullOrEmpty(searchModel.DeptName) ? null : searchModel.DeptName.Trim();
            var result = _ACDepartmentService.GetPagingAC_Departments(searchModel);

            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     添加、编辑部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            AC_DepartmentModel model = null;
            if (id.HasValue)
            {
                var result = _ACDepartmentService.GetAC_DepartmentById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            if (model == null)
            {
                model = new AC_DepartmentModel();
                model.IsActive = 1;
            }
            return PartialView(model);
        }

        /// <summary>
        ///     保存部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(AC_DepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();
                if (model.ID == 0)
                {
                    model.CreateBy = UserInfo.CurrentUserName;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = model.CreateBy;
                    model.UpdateDT = model.CreateDT;
                    model.ParentID = 0;
                    var result = _ACDepartmentService.AddAC_Department(model);
                    resultModel.Messages = new List<string> { result.Data.ID > 0 ? "Add dept success" : "Add dept failed" };
                }
                else
                {
                    model.UpdateBy = UserInfo.CurrentUserName;
                    model.UpdateDT = DateTime.Now;
                    var result = _ACDepartmentService.UpdateAC_Department(model);
                    resultModel.Messages = new List<string> { "Edit dept success" };
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }
    }
}