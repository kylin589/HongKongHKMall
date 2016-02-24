using HKTHMall.Admin.Filters;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        public ActionResult Index()
        {
            return View();
        }

        #region 查询反馈信息列表

        /// <summary>
        /// 查询反馈信息列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-12</remarks>
        public JsonResult List(SearchFeedbackModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var list = _feedbackService.Select(model);
            var data = new { rows = list.Data, total = list.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}