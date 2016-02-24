using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.User;
using HKTHMall.Services.AC;
using HKTHMall.Services.Orders;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Admin.Filters;
using HKTHMall.Admin.common;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.Enum;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.WebModel.Models.YH;
using HKTHMall.Services.Users;
using Autofac;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class OrderController : HKBaseController
    {
        private readonly IOrderService _acOrederService;
        private readonly IOrderTrackingLogService _orderTrackingLogService;
        private readonly IPurchaseOrderSerivce _PurchaseOrderSerivce;

        public OrderController(IOrderService acOrederService, IOrderTrackingLogService orderTrackingLogService, IPurchaseOrderSerivce PurchaseOrderSerivce)
        {
            _acOrederService = acOrederService;
            _orderTrackingLogService = orderTrackingLogService;
            _PurchaseOrderSerivce = PurchaseOrderSerivce;
        }

        public ActionResult Index(string userId)
        {
            

            return View();
        }

        public ActionResult OrderDetails(string orderId)
        {
            ViewData["orderId"] = orderId;
            return View();
        }

        public ActionResult OrderPurchase(string orderId)
        {
            SearchPurchaseOrderModel model = new SearchPurchaseOrderModel();
            ViewData["orderId"] = orderId;
            model.PagedSize = 100;
            model.PagedIndex = 0;
            model.OrderID = orderId;
            var list = _PurchaseOrderSerivce.Select(model);
            return PartialView(list.Data);
        }

        /// <summary>
        /// 订单列表页
        /// zhoub 20150713
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public JsonResult List(SearchOrderModel searchModel)
        {
            var result = _acOrederService.GetPagingOrder(searchModel);

            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateOrderStatus(string orderId, int status, string expressOrder)
        {
            var resultModel = new ResultModel();
            if (!string.IsNullOrEmpty(orderId))
            {
                var result = _acOrederService.UpdateOrderStatus(orderId, status).Data;
                var resultExpressOrder = _acOrederService.UpdateExpressOrder(orderId, expressOrder).Data;
                if (result > 0 && resultExpressOrder > 0)
                {
                    //订单发货记录
                    OrderTrackingLogModel orderTrackingLogOne = new OrderTrackingLogModel();
                    orderTrackingLogOne.OrderID = orderId;
                    orderTrackingLogOne.OrderStatus = (int)HKTHMall.Domain.Enum.OrderEnums.OrderStatus.WaitReceiving;
                    orderTrackingLogOne.TrackingContent = "待收货";
                    orderTrackingLogOne.CreateTime = DateTime.Now;
                    orderTrackingLogOne.CreateBy = UserInfo.CurrentUserName;
                    _orderTrackingLogService.AddOrderTrackingLog(orderTrackingLogOne);
                    resultModel.Messages = new List<string> { "Success." };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Failed." };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error." };
            }
            string opera = string.Format("订单发货:{0},操作结果:{1}", "{orderId:" + orderId + ",status:" + status + ",expressOrder:" + expressOrder + "}", resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "Order--Orders-Deliver goods");

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 货到付款取消订单
        /// zhoub 20150909
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult CancelOrder(string orderId)
        {
            SearchOrderDetailView searchModel = new SearchOrderDetailView()
            {
                LanguageID = 1,
                OrderID = orderId,

            };
            var result = _acOrederService.CancelOrderBy(searchModel);
            string opera = string.Format("货到付款取消订单:{0},操作结果:{1}", "{orderId:" + orderId + "}", result.Messages);
            LogPackage.InserAC_OperateLog(opera, "Order--Orders-Cancel");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更改邮费
        /// 刘文宁 20160115
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="expressFee"></param>
        /// <returns></returns>
        public JsonResult ModifyExpressFee(string orderId, decimal expressFee)
        {
            if (expressFee < 0)
            {
                return Json("邮费不能小于0", JsonRequestBehavior.AllowGet);
            }
            OrderModel model =_acOrederService.GetOrderByOrderID(orderId).Data;
            decimal oldExpressFee = model.ExpressMoney;
            model.TotalAmount = model.TotalAmount - model.ExpressMoney + expressFee;
            model.ExpressMoney = expressFee;

            var result = _acOrederService.UpdateOrderExpressMoney(model, oldExpressFee);
            if (!result.IsValid)
            {
                return Json("更新失败", JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 货到付款确认收货
        /// zhoub 20150910
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult ConfirmReceipt(string orderId)
        {
            SearchOrderDetailView model = new SearchOrderDetailView();
            model.OrderStatus = (int)OrderEnums.OrderStatus.WaitReceiving;
            model.OrderID = orderId;
            var result = _acOrederService.OutTimeReceivingOrder(model);
            string opera = string.Format("货到付款确认收货:{0},操作结果:{1}", "{orderId:" + orderId + "}", result.Messages);
            LogPackage.InserAC_OperateLog(opera, "Order--Orders-Confirm receipt");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 订单详情页
        /// zhoub 20150713
        /// </summary>
        /// <returns></returns>
        public JsonResult DetailsList(string orderId)
        {
            var result = _acOrederService.GetPagingOrderDetails(Convert.ToInt64(orderId), ACultureHelper.GetLanguageID);

            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DaYin(string orderId, int type)
        {
            ObjesToPdf.OrderPdf(orderId, type);
            string path = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath")) + "dayin.pdf";
            return File(path, "application/pdf", "pdf print download");

            
        }

        
        public ActionResult DaYins(string orderId)
        {
            var url1 = HKSJ.Common.ConfigHelper.GetConfigString("htmlPath") + "/AC_User/SelectDaYin?orderId=" + orderId;// System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("htmlPath"));
            string fileName = orderId;
            try
            {

                //ObjesToPdf.CreatPdf(url1, orderId); 如果下面的插件打印失败，请放开此方法,并且修改SelectDaYin.cshtml的注释1注释，注释2放开 wuyf 2015-9-23
                ExportFile.HtmlToPdf(fileName, url1);
            }
            catch (Exception ex)
            {

                var opera = string.Format("插件打印出问题开始调用另外打印方法:{0},操作结果:{1}", "打印页面再AC_User，调用在order控制器" + url1, ex.Message);
                LogPackage.InserAC_OperateLog(opera, "打印页面");
                try
                {
                    //调用非插件打印方法
                    ObjesToPdf.CreatPdf(url1, orderId);
                }
                catch (Exception ex1)
                {
                    opera = string.Format("非插件打印出问题:{0},操作结果:{1}", "打印页面再AC_User，调用在order控制器" + url1, ex1.Message);
                    LogPackage.InserAC_OperateLog(opera, "打印页面");
                    string path = System.Web.HttpContext.Current.Server.MapPath(HKSJ.Common.ConfigHelper.GetConfigString("PdfPath")) + "dayin.pdf";
                    return File(path, "application/pdf", "pdf print download");

                }

            }
            return View();
            
        }


        public ActionResult SelectDaYin(string orderId)
        {
            ViewBag.orderId = orderId;
            dynamic order;
            YH_MerchantInfoView YH_MerchantInfo = new YH_MerchantInfoView();
            string orderStatusStr = "";
            string payTypeStr = "";
            var list = new List<HKTHMall.Domain.AdminModel.Models.Orders.OrderDetailsModel>();
            OrderModel model = new OrderModel();
            string Address = "";
            var imgpath = "";
            try
            {
                imgpath = Code.BarCode.GetBarCode.GetTxm(orderId);
                ObjesToPdf.Orderinfo(orderId, 3, out order, out YH_MerchantInfo, out orderStatusStr, out payTypeStr);
                model = order as OrderModel;
                //订单分页详情(商品的信息)
                list = _acOrederService.GetPagingOrderDetails(Convert.ToInt64(orderId), 3).Data as List<HKTHMall.Domain.AdminModel.Models.Orders.OrderDetailsModel>;

                IUserAddressService userAddressService = BrCms.Framework.Infrastructure.BrEngineContext.Current.Resolve<IUserAddressService>();
                //省市区
                var userAddress = userAddressService.GetTHAreaAreaName(model.THAreaID, 3).Data;
                Address = userAddress + model.DetailsAddress;
            }
            catch (Exception ex)
            {

                var opera = string.Format("显示打印详情错误:{0},操作结果:{1}", ex.Message, "失败");
                LogPackage.InserAC_OperateLog(opera, "PDF");
            }

            ViewBag.imgpath = imgpath;
            ViewBag.list = list;
            ViewBag.ordermodel = model;
            ViewBag.YH_MerchantInfo = YH_MerchantInfo;
            ViewBag.orderStatusStr = orderStatusStr;
            ViewBag.payTypeStr = payTypeStr;
            ViewBag.Address = Address;

            //return PartialView();
            return View();
        }

       

    }
}