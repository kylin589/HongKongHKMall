using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Services.Orders;
using HKTHMall.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Web.Areas.Account.Controllers
{

    public class RightsController : BaseController
    {
        private readonly IComplaintsService _complaintsService;
        private readonly IReturnProductInfoService _returnProductInfoService;

        public RightsController(IComplaintsService complaintsService, IReturnProductInfoService returnProductInfoService)
        {
            _complaintsService = complaintsService;
            _returnProductInfoService = returnProductInfoService;
        }

        #region  我的投诉

        /// <summary>
        /// 我的投诉列表页
        /// zhoub 20150716
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Complain(SearchComplaintsModel model, int page = 1)
        {
            model.UserID = base.UserID;
            model.PagedIndex = page - 1;
            model.PagedSize = 10;
            var result = _complaintsService.GetPagingComplaints(model);
            ViewData.Add("Complain", result.Data);
            ViewData.Add("searchModel", model);
            ViewBag.Page = page;
            ViewBag.Count = result.Data.TotalCount;
            return View();
        }

        #endregion

        #region  退款管理

        /// <summary>
        /// 我的退款管理表页
        /// zhoub 20150716
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize]
        public ActionResult Index(SearchReturnProductInfoModel model, int page = 1)
        {
            model.UserID =base.UserID;
            model.PagedIndex = page - 1;
            model.PagedSize = 10;
            model.LanguageID = CultureHelper.GetLanguageID();
            var result = _returnProductInfoService.GetReturnProductInfoList(model);
            ViewData.Add("ProductInfo", result.Data);
            ViewData.Add("searchModel", model);
            ViewBag.Page = page;
            ViewBag.Count = result.Data.TotalCount;
            return View();
        }


        /// <summary>
        /// 撤消退款申请
        /// zhoub 20150815
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult UndoOrder()
        {
            ResultModel model = new ResultModel();
            if (!string.IsNullOrEmpty(Request.Params["returnOrderID"]))
            {
                ReturnProductInfoModel returnModel = new ReturnProductInfoModel();
                returnModel.ReturnOrderID = Request.Params["returnOrderID"];
                model = _returnProductInfoService.UndoReturnProductInfoBH(returnModel, CultureHelper.GetLanguageID());
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        } 

        #endregion
    }
}