using HKTHMall.Admin.Filters;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.Models;
using HKTHMall.Services.AC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class ExceptionLogController : HKBaseController
    {       
         private IExceptionLogService _exceptionLogService;

         public ExceptionLogController(IExceptionLogService exceptionLogService)
        {
            this._exceptionLogService = exceptionLogService;
        }

        #region 服务日志详情

         public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 系统服务日志数据查询
        /// zhoub 20150902
        /// </summary>
        /// <param name="logmodel"></param>
        /// <returns></returns>
        public JsonResult List(SearchExceptionLogModel model)
        {
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;

            var result = this._exceptionLogService.GetExceptionLogList(model);
            List<ExceptionLogModel> ds = result.Data;
            var data = new
            {
                rows = ds,
                total = result.Data.TotalCount
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 数据异常处理查询
        /// zhoub 20150902
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            ExceptionLogModel model = new ExceptionLogModel();
            if (id.HasValue)
            {
                SearchExceptionLogModel smodel = new SearchExceptionLogModel();
                smodel.ElId = id.Value;
                smodel.PagedIndex = 0;
                smodel.PagedSize = 100;
                //查询列表 
                List<ExceptionLogModel> List = this._exceptionLogService.GetExceptionLogList(smodel).Data;
                if (List != null && List.Count > 0)
                {
                    model = List[0];
                }
            }
            return PartialView(model);
        }

        /// <summary>
        /// 数据异常处理
        /// zhoub 20150902
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ExceptionLogModel model)
        {
            ResultModel resultModel = new ResultModel();
            model.UpdateBy = UserInfo.CurrentUserName;
            model.UpdateDT = DateTime.Now;
            model.Status = 2;
            resultModel = this._exceptionLogService.Update(model);
            resultModel.Messages = new List<string> { resultModel.IsValid == true ? "Success！" : "Failed！" };
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  服务概况

        public ActionResult SurveyIndex()
        {
            return View();
        }

        /// <summary>
        /// 系统服务根概数据查询
        /// zhoub 20150902
        /// </summary>
        /// <param name="logmodel"></param>
        /// <returns></returns>
        public JsonResult SurveyList(SearchExceptionLogModel model)
        {
            var result = this._exceptionLogService.GetExceptionLogSurvey();
            List<ExceptionLogModel> ds = result.Data;
            var data = new
            {
                rows = ds,
                total = ds.Count
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}