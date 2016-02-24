using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Sys;
using HKTHMall.Services.Sys;
using HKTHMall.Admin.common;
using HKTHMall.Core.Utils;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    ///     系统参数设置业务类
    /// </summary>
      [UserAuthorize]
    public class ParameterSetController : HKBaseController
    {
        private readonly IParameterSetService _parameterSetService;

        public ParameterSetController(IParameterSetService parameterSetService)
        {
            _parameterSetService = parameterSetService;
        }

        // GET: ParameterSet
        public ActionResult Index()
        {
            return View();
        }

        #region 查询系统参数列表

        /// <summary>
        ///     查询系统参数列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchParaSetModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var paramList =
                _parameterSetService.Select(new SearchParaSetModel
                {
                    PagedIndex = model.PagedIndex,
                    PagedSize = model.PagedSize,
                    KeysName = model.KeysName
                });
            var data = new { rows = paramList.Data, total = paramList.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除系统参数

        /// <summary>
        ///     删除系统参数
        /// </summary>
        /// <param name="ParamenterID">系统参数Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(long? ParamenterID)
        {
            var resultModel = new ResultModel();
            if (ParamenterID.HasValue)
            {
                var result = _parameterSetService.Delete(ParamenterID.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete system parameter success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete system parameter failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            var opera = string.Format("删除系统参数 ParamenterID:{0},操作结果:{1}", ParamenterID, resultModel.IsValid ? "成功" : "失败");
            LogPackage.InserAC_OperateLog(opera, "系统管理--系统参数");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建系统信息

        /// <summary>
        ///     创建系统信息
        /// </summary>
        /// <param name="model">系统参数对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ParameterSetModel model)
        {
            var admin = UserInfo.CurrentUserName;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (model.ParamenterID != 0)
                {
                    model.UpdateBy = admin;
                    model.UpdateDT = DateTime.Now;
                    var result = _parameterSetService.Update(model);
                    resultModel.Messages = new List<string>
                    {
                       result.Data > 0 ? "Modify system parameter success" : "Modify system parameter failed"
                    };
                    var opera = string.Format("修改系统参数:{0},操作结果:{1}",JsonConverts.ToJson(model), result.Data > 0 ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "系统管理--系统参数");

                }
                else
                {
                    model.ParamenterID = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.CreateBy = admin;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = admin;
                    model.UpdateDT = DateTime.Now;
                    resultModel.Messages = new List<string>
                    {
                        _parameterSetService.Add(model).Messages.Count == 0 ? "Add system parameter success" : "Add system parameter failed"
                    };
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
        public ActionResult Create(long? id)
        {
            ParameterSetModel model = null;
            if (id.HasValue)
            {
                var result = _parameterSetService.GetParameterSetById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new ParameterSetModel();
            }
            return PartialView(model);
        }

        #endregion
    }
}