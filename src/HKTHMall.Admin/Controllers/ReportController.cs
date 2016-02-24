using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Services.YHUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class ReportController : HKBaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }


        #region 查询惠粉举报列表

        /// <summary>
        ///     查询惠粉举报列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchReportModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var list =
                _reportService.Select(model);
            var data = new { rows = list.Data, total = list.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除惠粉举报

        /// <summary>
        ///   删除惠粉举报
        /// </summary>
        /// <param name="ParamenterID">惠粉举报Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(long? ReportId)
        {
            var resultModel = new ResultModel();
            if (ReportId.HasValue)
            {
                var result = _reportService.Delete(ReportId.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete Huifen report information success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete Huifen report information failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            var opera = string.Format("删除惠粉举报信息成功 ReportId:{0},操作结果:{1}", ReportId, resultModel.IsValid ? "成功" : "失败");
            LogPackage.InserAC_OperateLog(opera, "其他管理--惠粉举报管理");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 编辑惠粉举报

        /// <summary>
        /// 编辑惠粉举报
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ReportModel model)
        {
            var userName = UserInfo.CurrentUserName;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (model.ReportId != 0)
                {
                    model.Status = (int)EStatus.HaveDeal;
                    model.DealDate = DateTime.Now;
                    model.DealUser = userName;
                    var result = _reportService.Update(model);
                    resultModel.Messages = new List<string>
                    {
                        result.Data > 0 ? "Handle Huifen report information success" : "Handle Huifen report information failed"
                    };
                    var opera = string.Format("处理惠粉举报状态:{0},操作结果:{1}", JsonConverts.ToJson(model), result.Data > 0 ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "其他管理--惠粉举报管理");
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
            ReportModel model = null;
            if (id.HasValue)
            {
                var result = _reportService.GetReportById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new ReportModel();
            }
            return PartialView(model);
        }

        #endregion
    }
}