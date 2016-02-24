using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Suppliers.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IPurchaseOrderSerivce _IPurchaseOrderSerivce;

        /// <summary>
        /// 构造函数
        /// zhoub 20150928
        /// </summary>
        /// <param name="userAddress"></param>
        /// <param name="thAreaService"></param>
        /// <param name="loginService"></param>
        /// <param name="bankService"></param>
        /// <param name="userBankService"></param>
        public PurchaseOrderController(IPurchaseOrderSerivce iPurchaseOrderSerivce)
        {
            _IPurchaseOrderSerivce = iPurchaseOrderSerivce;
        }


        /// <summary>
        /// 采购订单视图
        /// zhoub 20150928
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Index(SearchPurchaseOrderModel model)
        {
            int totalCount = 0;
            int productQuantity = 0;
            model.LanguageID = 1;
            model.SupplierId = 5728471707;
            model.PagedIndex = 1;
            model.PagedSize = 5;
            ResultModel result = _IPurchaseOrderSerivce.GetSuppliersPagingPurchaseOrder(model, out totalCount, out productQuantity);
            return View();
        }

        /// <summary>
        /// 采购订单数据获取
        /// zhoub 20150928
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchPurchaseOrderModel model)
        {
            int totalCount = 0;
            int productQuantity = 0;
            model.LanguageID = 1;
            model.SupplierId = 5728471707;
            model.PagedIndex = 1;
            model.PagedSize = 5;
            ResultModel result = _IPurchaseOrderSerivce.GetSuppliersPagingPurchaseOrder(model, out totalCount, out productQuantity);
            var data = new { rows = result.Data, total = totalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}