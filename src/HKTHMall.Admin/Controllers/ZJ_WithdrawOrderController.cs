using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Services.Sys;
using HKTHMall.Services.Users;
using HKTHMall.Admin.common;
using HKTHMall.Core.Utils;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class ZJ_WithdrawOrderController : HKBaseController
    {
        private readonly IZJ_WithdrawOrderService _zJ_WithdrawOrderService;

        public ZJ_WithdrawOrderController(IZJ_WithdrawOrderService zJ_WithdrawOrderService)
        {
            _zJ_WithdrawOrderService = zJ_WithdrawOrderService;
        }

        // GET: ParameterSet
        public ActionResult Index()
        {
            return View();
        }

        #region 查询提现订单列表

        /// <summary>
        ///     查询提现订单列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchZJ_WithdrawOrderModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var list = _zJ_WithdrawOrderService.Select(model);
            var data = new { rows = list.Data, total = list.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建提现订单信息

        /// <summary>
        /// 创建提现订单信息
        /// </summary>
        /// <param name="model">提现订单对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ZJ_WithdrawOrderModel model)
        {
            var name = UserInfo.CurrentUserName;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (!string.IsNullOrEmpty(model.OrderNO))
                {
                    switch (model.WithdrawResult)
                    {
                        case 2:
                        case 3:
                            //审核信息
                            model.Verifier = name;
                            model.VerifyDT = DateTime.Now;
                            break;
                        case 4:
                        case 5:
                            //打款信息
                            model.Remitter = name;
                            model.RemittanceDT = DateTime.Now;
                            break;
                        default:
                            break;
                    }
                    var result =  _zJ_WithdrawOrderService.Update(model);
                    if (result.IsValid)
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { "Success" };
                    }
                    else
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { result.Messages[0] };
                    }
                    var opera = string.Format("审核提现信息:OrderNO={0},操作结果:{1}", model.OrderNO, resultModel.IsValid ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "订单业务--提现订单");
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }

        #endregion

        #region 加载数据

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="id">对象提现订单Id</param>
        /// <returns></returns>
        public ActionResult Create(string id)
        {
            ZJ_WithdrawOrderModel model = null;
            if (!string.IsNullOrEmpty(id))
            {
                var result = _zJ_WithdrawOrderService.GetZJ_WithdrawOrderById(id);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new ZJ_WithdrawOrderModel();
            }
            DrowList(model);
            return PartialView(model);
        }

        #endregion


        /// <summary>
        /// 操作名称
        /// </summary>
        /// <param name="model"></param>
        private void DrowList(ZJ_WithdrawOrderModel model)
        {
            var states = new Dictionary<int, string>();
            if (model != null)
            {
                switch (model.WithdrawResult)
                {
                    case 1:
                        ViewData["WithdrawResult"] = new List<SelectListItem>() { new SelectListItem(){
                         Selected= true, Text="Review success",Value="2"},new SelectListItem(){
                         Selected= false, Text="Review failed",Value="3"}};
                        break;
                    case 2:
                        ViewData["WithdrawResult"] = new List<SelectListItem>() { new SelectListItem(){
                         Selected= true, Text="Pay success",Value="4"},new SelectListItem(){
                         Selected= false, Text="Pay failed",Value="5"}};
                        break;
                    default:
                        break;
                }
            }
            else
            {
                ViewData["WithdrawResult"] = new List<SelectListItem>() { new SelectListItem(){
                         Selected= true, Text="Review success",Value="2"},new SelectListItem(){
                         Selected= false, Text="Review failed",Value="3"}};
            }
        }
    }
}