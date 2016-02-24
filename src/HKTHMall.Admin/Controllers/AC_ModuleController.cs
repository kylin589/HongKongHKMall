using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;
using HKTHMall.Core;
using HKTHMall.Admin.Filters;
using HKTHMall.Admin.common;
using HKTHMall.Core.Utils;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    /// <summary>
    ///     系统模块控制器
    /// </summary>
    public class AC_ModuleController : HKBaseController
    {
        private readonly IAC_ModuleService _aC_ModuleService;

        public AC_ModuleController(IAC_ModuleService aC_ModuleService)
        {
            _aC_ModuleService = aC_ModuleService;
        }

        // GET: AC_Module
        public ActionResult Index()
        {
            return View();
        }

        #region 查询系统菜单列表

        /// <summary>
        ///     查询系统菜单列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchAC_ModuleModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var paramList =
                _aC_ModuleService.Select(new SearchAC_ModuleModel
                {
                    PagedIndex = model.PagedIndex,
                    PagedSize = model.PagedSize,
                    ParentID = model.ParentID
                });
            var data = new { rows = paramList.Data, total = paramList.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除系统菜单

        /// <summary>
        ///     删除系统菜单
        /// </summary>
        /// <param name="moduleID">系统菜单Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(int? moduleID)
        {
            var resultModel = new ResultModel();
            if (moduleID.HasValue)
            {
                var result = _aC_ModuleService.Delete(moduleID.Value);
                if (result.Data > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete system menu success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = result.Messages;
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID of System menu error" };
            }
            string opera = string.Format("系统菜单删除:{0},操作结果:{1}", moduleID, resultModel.IsValid);
            LogPackage.InserAC_OperateLog(opera, "System--Menu-Delete");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建系统信息

        /// <summary>
        ///     创建系统信息
        /// </summary>
        /// <param name="model">系统菜单对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(AC_ModuleModel model)
        {
            string opera = "";
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (model.ModuleID != 0)
                {
                    var result = _aC_ModuleService.Update(model);
                    if (result.Data > 0)
                    {
                        resultModel.Messages = new List<string> { "Update system menu success." };
                    }
                    else
                    {
                        resultModel.Messages = result.Messages;
                    }
                    opera = string.Format("系统菜单更新:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "System--Menu-Delete");
                }
                else
                {
                    model.Place = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    var result = _aC_ModuleService.Add(model);
                    if (result.Data != null)
                    {
                        resultModel.Messages = new List<string> { "Add system menu success." };
                    }
                    else
                    {
                        resultModel.Messages = result.Messages;
                    }
                    opera = string.Format("系统菜单添加:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "System--Menu-Add");
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
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
            AC_ModuleModel model = null;
            if (id.HasValue)
            {
                var result = _aC_ModuleService.GetAC_ModuleById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new AC_ModuleModel();
            }
            return PartialView(model);
        }

        #endregion

        public ActionResult TestTree()
        {
            ViewBag.parentList = _aC_ModuleService.GetAC_ModuleList(0).Data;
            return View();
        }

        /// <summary>
        /// 菜单树形数据获取
        /// zhoub 20150709
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAC_ModuleTree()
        {
            var result = _aC_ModuleService.GetAC_ModuleToTree();

            return this.Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 菜单排序
        /// zhoub 20150713
        /// </summary>
        /// <param name="bannerId"></param>
        /// <param name="sx"></param>
        /// <param name="Sorts"></param>
        /// <param name="IdentityStatus"></param>
        /// <returns></returns>
        public JsonResult UpdatePlace(int? moduleId, int rowId, int parentID, int sortType)
        {
            var resultModel = new ResultModel();
            resultModel.Messages = new List<string> { "Success." };
            if (moduleId.HasValue)
            {
                List<AC_ModuleModel> moduleList = _aC_ModuleService.GetAC_ModuleList(parentID).Data;
                if (moduleList != null && moduleList.Count > 0)
                {
                    if (sortType == 1)
                    {
                        if (rowId > 0)
                        {
                            _aC_ModuleService.UpdatePlace(moduleList[rowId].ModuleID, moduleList[rowId - 1].Place);
                            _aC_ModuleService.UpdatePlace(moduleList[rowId - 1].ModuleID, moduleList[rowId].Place);
                        }
                        else
                        {
                            resultModel.IsValid = false;
                            resultModel.Messages = new List<string> { "This is top line." };
                        }
                    }
                    else
                    {
                        if (rowId < (moduleList.Count - 1))
                        {
                            _aC_ModuleService.UpdatePlace(moduleList[rowId].ModuleID, moduleList[rowId + 1].Place);
                            _aC_ModuleService.UpdatePlace(moduleList[rowId + 1].ModuleID, moduleList[rowId].Place);
                        }
                        else
                        {
                            resultModel.IsValid = false;
                            resultModel.Messages = new List<string> { "This is last line!." };
                        }
                    }
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Failed, ID error." };
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
    }
}