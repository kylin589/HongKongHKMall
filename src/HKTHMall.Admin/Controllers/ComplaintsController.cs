using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Orders;
using HKTHMall.Services.Orders;
using HKTHMall.Core;
using HKTHMall.Admin.common;
using HKTHMall.Core.Utils;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    ///     投诉控制器处理类
    /// </summary>
      [UserAuthorize]
    public class ComplaintsController : HKBaseController
    {
        private readonly IComplaintsService _complaintsService;

        public ComplaintsController(IComplaintsService complaintsService)
        {
            _complaintsService = complaintsService;
        }

        // GET: ParameterSet
        public ActionResult Index()
        {
            return View();
        }

        #region 查询投诉列表

        /// <summary>
        ///     查询投诉列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchComplaintsModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var paramList =
                _complaintsService.Select(model);
            var data = new { rows = paramList.Data, total = paramList.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除投诉

        /// <summary>
        ///     删除投诉
        /// </summary>
        /// <param name="ParamenterID">投诉Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(string  complaintsID)
        {
            var resultModel = new ResultModel();
            if (!string.IsNullOrEmpty(complaintsID))
            {
                var result = _complaintsService.Delete(complaintsID).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Complaint delete success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Complaint delete failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            var opera = string.Format("删除投诉信息成功 complaintsID:{0},操作结果:{1}", complaintsID, resultModel.IsValid ? "成功" : "失败");
            LogPackage.InserAC_OperateLog(opera, "订单业务--投诉管理");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 编辑投诉

        /// <summary>
        /// 编辑投诉
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ComplaintsModel model)
        {
            var admin = UserInfo.CurrentUserName;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (!string.IsNullOrEmpty(model.ComplaintsID))
                {
                    model.DealPeople = admin;
                    model.DealDate = DateTime.Now;
                    model.Flag = 2;
                    var result = _complaintsService.Update(model);
                    resultModel.Messages = new List<string>
                    {
                        result.Data > 0 ? "Handle complaint success" : "Handle complaint failed"
                    };
                    var opera = string.Format("处理投诉状态:{0},操作结果:{1}",JsonConverts.ToJson(model), result.Data > 0 ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "订单业务--投诉管理");
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
        public ActionResult Create(string id)
        {
            ComplaintsModel model = null;
            if (!string.IsNullOrEmpty(id))
            {
                var result = _complaintsService.GetComplaintsById(id);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new ComplaintsModel();
            }
            //投诉状态
            var states = new Dictionary<int, string>();
            states.Add(2, "Handled");
            var list = new List<SelectListItem>();
            if (states != null)
            {
                foreach (var item in states)
                {
                    var info = new SelectListItem();
                    info.Selected = true;
                    info.Value = item.Key.ToString();
                    info.Text = item.Value;
                    list.Add(info);
                }
            }
            ViewData["flagList"] = list;
            return PartialView(model);
        }

        #endregion
    }
}