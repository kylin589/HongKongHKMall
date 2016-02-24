using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;
using HKTHMall.Admin.common;
using HKTHMall.Core.Utils;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class AC_FunctionController : HKBaseController
    {
        private readonly IAC_FunctionService _aC_FunctionService;
        private readonly IAC_ModuleService _aC_ModuleService;
        
        public AC_FunctionController(IAC_FunctionService aC_FunctionService, IAC_ModuleService aC_ModuleService)
        {
            _aC_FunctionService = aC_FunctionService;
            _aC_ModuleService = aC_ModuleService;
        }

        // GET: ParameterSet
        public ActionResult Index()
        {
            return View();
        }

        #region 查询系统功能

        /// <summary>
        ///     查询系统功能
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchAC_FunctionModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var paramList =
                _aC_FunctionService.Select(new SearchAC_FunctionModel
                {
                    PagedIndex = model.PagedIndex,
                    PagedSize = model.PagedSize,
                    FunctionName = model.FunctionName,
                     ParentID = model.ParentID
                });
            var data = new { rows = paramList.Data, total = paramList.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除权限功能

        /// <summary>
        ///  删除权限功能
        /// </summary>
        /// <param name="ParamenterID">系统参数Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(int? functionID)
        {
            var resultModel = new ResultModel();
            if (functionID.HasValue)
            {
                var result = _aC_FunctionService.Delete(functionID.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            var opera = string.Format("删除权限功能 functionID:{0},结果:{1}", functionID, resultModel.IsValid?"成功":"失败");
            LogPackage.InserAC_OperateLog(opera, "系统管理--权限管理");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建系统功能

        /// <summary>
        ///     创建系统功能
        /// </summary>
        /// <param name="model">系统参数对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(AC_FunctionModel model)
        {
            var opera = string.Empty;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (model.FunctionID != 0)
                {
                    var result = _aC_FunctionService.Update(model);
                    if (result.Data > 0)
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { "Change system function success" };
                    }
                    else
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { "Change system function failed" };
                    }
                    opera = string.Format("修改系统功能参数:{0},结果:{1}", JsonConverts.ToJson(model), resultModel.IsValid ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "系统管理--权限管理");
                }
                else
                {
                    var resut = _aC_FunctionService.Add(model);
                    resultModel.Messages = new List<string> { resut.Data != null ? "Add system function success" : "Add system function failed" };
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DrowList(null);
            }
            return PartialView(model);
        }

        #endregion

        #region 加载数据

        /// <summary>
        ///     加载数据
        /// </summary>
        /// <param name="id">对象系统Id</param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            AC_FunctionModel model = null;
            var result = new ResultModel();
            if (id.HasValue)
            {
                result = _aC_FunctionService.GetAC_FunctionById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new AC_FunctionModel();
            }
            DrowList(model);
            return PartialView(model);
        }

        #endregion

        private void DrowList(AC_FunctionModel model)
        {
            int moduleId = 0;
            #region 一级菜单
            if (model != null)
            {
                var aC_Module = _aC_ModuleService.GetAC_ModuleById(model.ModuleID).Data;
                moduleId = aC_Module != null ? aC_Module.ParentID : 0;
            }

            var FirstAc_Module = _aC_ModuleService.GetAC_ModuleList(0).Data;
            var listFirst = new List<SelectListItem>() { new SelectListItem() { Text = "--Select--", Value = "" } };
            if (FirstAc_Module != null)
            {
                foreach (var item in FirstAc_Module)
                {
                    var infoFirst = new SelectListItem();
                    if (model != null)
                    {
                        if (moduleId == item.ModuleID)
                        {
                            infoFirst.Selected = true;
                        }
                    }
                    infoFirst.Value = item.ModuleID.ToString();
                    infoFirst.Text = item.ModuleName;
                    listFirst.Add(infoFirst);
                }
            }
            ViewData["FirstACModules"] = listFirst;
            #endregion

            #region 二级菜单
            var ac_Module = _aC_ModuleService.GetAC_ModuleList(moduleId).Data;
            var list = new List<SelectListItem>() { new SelectListItem() { Text = "--Select--", Value = "" } };
            if (ac_Module != null)
            {
                foreach (var item in ac_Module)
                {
                    var info = new SelectListItem();
                    if (model != null)
                    {
                        if (model.ModuleID == item.ModuleID)
                        {
                            info.Selected = true;
                        }
                    }
                    info.Value = item.ModuleID.ToString();
                    info.Text = item.ModuleName;
                    list.Add(info);
                }
            }
            ViewData["ACModules"] = list;
            #endregion


        }

        #region 请求二级或三级分类查询
        /// <summary>
        /// 请求二级或三级分类查询
        /// </summary>
        /// <param name="parenId"></param>
        /// <returns></returns>
        public JsonResult GetAC_FunctionModuleId(int? parenId)
        {
            if (parenId.HasValue)
            {
                var moduleList = this._aC_ModuleService.GetAC_ModuleList(parenId.Value).Data;
                return Json(moduleList, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}