using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Services.Orders;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class PaymentOrderController : HKBaseController
    {
        private readonly IPaymentOrderService _paymentOrderService;

        public PaymentOrderController(IPaymentOrderService paymentOrderService)
        {
            _paymentOrderService = paymentOrderService;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 查询订单支付信息表列表

        /// <summary>
        /// 查询订单支付信息表列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        public JsonResult List(SearchPaymentOrderModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var paramList =
                _paymentOrderService.Select(model);
            var data = new { rows = paramList.Data, total = paramList.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}