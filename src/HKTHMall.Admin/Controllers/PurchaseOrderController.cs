using HKTHMall.Admin.common;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.Controllers
{
    public class PurchaseOrderController : HKBaseController
    {
        private readonly IPurchaseOrderSerivce _PurchaseOrderSerivce;
        private readonly IPurchaseOrderDetailsService _purchaseOrderDetailsService;

        public PurchaseOrderController(IPurchaseOrderSerivce PurchaseOrderSerivce, IPurchaseOrderDetailsService purchaseOrderDetailsService)
        {
            _PurchaseOrderSerivce = PurchaseOrderSerivce;
            _purchaseOrderDetailsService = purchaseOrderDetailsService;
        }


        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }


        #region 查询供应商采购单单列表

        /// <summary>
        ///     查询供应商采购单列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchPurchaseOrderModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var list = _PurchaseOrderSerivce.Select(model);
            var data = new { rows = list.Data, total = list.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 供应商采购单明细
        public ActionResult PurchaseOrderDetails(string purchaseOrderId)
        {
            ViewData["purchaseOrderId"] = purchaseOrderId;
            return View();
        }
        #endregion

        #region 查询供应商采购单单列表

        /// <summary>
        ///     查询供应商采购单列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult PurchaseOrderList(SearchPurchaseOrderDetailsModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var list = _purchaseOrderDetailsService.Select(model, ACultureHelper.GetLanguageID);
            var data = new { rows = list.Data, total = list.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 编辑投诉

        /// <summary>
        /// 编辑投诉
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(PurchaseOrderModel model)
        {
            var userName = UserInfo.CurrentUserName;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (!string.IsNullOrEmpty(model.PurchaseOrderId))
                {
                    model.DeliveryDate = DateTime.Now;
                    model.status = (int)EPurchaseOrderStatus.Received;
                    model.Deliveryer = userName;
                    var result = _PurchaseOrderSerivce.Update(model);
                    resultModel.Messages = new List<string>
                    {
                        result.Data > 0 ? "Handle purchaseOrder success" : "Handle purchaseOrder failed"
                    };
                    var opera = string.Format("处理供应商状态:{0},操作结果:{1}", JsonConverts.ToJson(model), result.Data > 0 ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "订单业务--供应商管理");
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
            PurchaseOrderModel model = null;
            if (!string.IsNullOrEmpty(id))
            {
                var result = _PurchaseOrderSerivce.GetPurchaseOrder(id);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new PurchaseOrderModel();
            }
            return PartialView(model);
        }

        #endregion
    }
}