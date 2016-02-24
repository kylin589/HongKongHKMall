using HKTHMall.Admin.common;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Admin.Filters;
using HKTHMall.Core.Utils;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class ReturnProductInfoController : Controller
    {
        private IReturnProductInfoService _return_GoodsService;

        public ReturnProductInfoController(IReturnProductInfoService return_GoodsService)
        {
            this._return_GoodsService = return_GoodsService;
        }

        //
        // GET: /Return_Goods/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列表Return_Goods
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchReturnProductInfoModel model)
        {
            //SearchAC_OperateLogModel logmodel = new SearchAC_OperateLogModel();
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            model.LanguageID = ACultureHelper.GetLanguageID;


            
            //查询列表 
            var result = this._return_GoodsService.GetReturnProductInfoList(model);
            List<ReturnProductInfoModel> ds = result.Data;
            var data = new
            {

                rows = ds,
                total = result.Data.TotalCount,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Return_Goods加载数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(string id)
        {
            ReturnProductInfoModel model = new ReturnProductInfoModel();
            if (! string.IsNullOrEmpty(id))
            {

                //查询列表 
                //List<Return_GoodsModel> result = this._return_GoodsService.GetReturn_GoodsById(id).Data;
                SearchReturnProductInfoModel model1 = new SearchReturnProductInfoModel();
                model1.ReturnOrderID = id;
                model1.PagedIndex = 0;
                model1.PagedSize = 100;
                var result = this._return_GoodsService.GetReturnProductInfoList(model1);
                List<ReturnProductInfoModel> ds = result.Data;
                if (ds != null && ds.Count > 0)
                {
                    model = ds[0];
                }
                
            }
            return PartialView(model);
        }

        /// <summary>
        /// 审核通过修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ReturnProductInfoModel model)
        {
            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();
                model.UpdateTime = DateTime.Now;
                model.AuditUser = UserInfo.CurrentUserName;
                if (model.ReturnStatus == 2)
                {
                    //通过,只修改退货表状态
                    resultModel = this._return_GoodsService.UpdateReturnProductInfo(model);
                }
                else if (model.ReturnStatus == 3)
                {
                    model.RefundAmount = 0;//驳回不需要保存退款金额
                    //申请驳回 修改退货表状态 和 订单表 退款标识 修改成 已处理 和 订单明细表 退货状态 改成 审核未通过
                    resultModel = this._return_GoodsService.UpdateReturnProductInfoBH(model);
                    
                }



                resultModel.Messages = new List<string> { resultModel.IsValid == true ? "Review success！" : "Review failed！" };
                var opera = string.Empty;
                //opera += "修改状态UpdateTime:" + model.UpdateTime + ",ReturnStatus:" + model.ReturnStatus + ",ReturnOrderID:" + model.ReturnOrderID + ",ReturnText:" + model.ReturnText + ",RefundAmount" + model.RefundAmount + ",结果:" + result.IsValid;
                opera = string.Format("审核退换货记录:{0},操作结果:{1}",JsonConverts.ToJson(model), resultModel.Messages);
                LogPackage.InserAC_OperateLog(opera, "审核退换货记录");
                

                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }

        /// <summary>
        /// 删除Return_Goods
        /// </summary>
        /// <param name="ProductCommentId">评论ID</param>
        /// <returns></returns>
        public JsonResult DeleteSP_ProductComment(string ReturnOrderID)
        {
            ReturnProductInfoModel model = new ReturnProductInfoModel();
            var resultModel = new ResultModel();
            if (!string.IsNullOrEmpty(ReturnOrderID))
            {
                model.ReturnOrderID = ReturnOrderID;
                var result = this._return_GoodsService.DeleteReturnProductInfo(model);
                var opera = string.Empty;
                opera += "删除退换货记录 ReturnOrderID:" + model.ReturnOrderID + ",结果:" + result.IsValid;
                LogPackage.InserAC_OperateLog(opera, "修改退换货记录");
                resultModel = LogPackage.GetResulMessagest(result.IsValid, new List<string> { result.IsValid == true ? "Delete success" : "Delete failed" });
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultModel = LogPackage.GetResulMessagest(false, new List<string> { "Invalid return or change product record ID" });

            }

            return Json(resultModel, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="ProductCommentId">评论ID</param>
        /// <returns></returns>
        public JsonResult UpdateReturnProductInfoSH(string ReturnOrderID, int ReturnStatus)
        {
            ReturnProductInfoModel model = new ReturnProductInfoModel();
            var resultModel = new ResultModel();
            if (!string.IsNullOrEmpty(ReturnOrderID))
            {
                model.ReturnOrderID = ReturnOrderID;
                model.Receiver = UserInfo.CurrentUserName;
                model.ReturnStatus = ReturnStatus;
                model.DeliveryDate = DateTime.Now;
                var result = this._return_GoodsService.UpdateReturnProductInfoSH(model);
                var opera = string.Empty;
                opera += "确认收货 ReturnStatus=4, ReturnOrderID:" + model.ReturnOrderID + ",结果:" + result.IsValid;
                LogPackage.InserAC_OperateLog(opera, "确认收货");
                resultModel = LogPackage.GetResulMessagest(result.IsValid, new List<string> { result.IsValid == true ? "Confirm received success" : "Confirm received failed" });
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultModel = LogPackage.GetResulMessagest(false, new List<string> { "Confirm receipt ID is invalid" });

            }

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

       /// <summary>
        /// 确认退款
       /// </summary>
       /// <param name="ReturnOrderID">退款标识</param>
       /// <param name="ReturnStatus">退款状态</param>
       
       /// <returns></returns>
        public JsonResult UpdateReturnProductInfoTK(string ReturnOrderID, int ReturnStatus)
        {
            ReturnProductInfoModel model = new ReturnProductInfoModel();
            var resultModel = new ResultModel();

            if (!string.IsNullOrEmpty(ReturnOrderID))
            {
                model.ReturnOrderID = ReturnOrderID;
                model.ReturnStatus = ReturnStatus;
                model.RefundPerson = UserInfo.CurrentUserName;
                model.RefundDate = DateTime.Now;

                //resultModel = this._return_GoodsService.UpdateReturnProductInfoTK(model);
                //修改退货表 状态 5已退款
                //订单明细表 退货状态 改成 2已退款
                //订单表 退款标识 修改成 已处理
                //订单表 状态改成8交易关闭（需要判断订单明细表里的相关产品数据都是已经退款状态）
                //给退款用户的账户余额添加退款 费用
                SearchReturnProductInfoModel spmodel = new SearchReturnProductInfoModel();
                spmodel.ReturnOrderID = ReturnOrderID;
                spmodel.PagedIndex = 0;
                spmodel.PagedSize = 20;
                var result = this._return_GoodsService.GetReturnProductInfoList(spmodel);
                List<ReturnProductInfoModel> ds = result.Data;
                if (ds.Count>0)
                {
                    model = ds[0];
                    model.ReturnOrderID = ReturnOrderID;
                    model.ReturnStatus = ReturnStatus;
                    model.RefundPerson = UserInfo.CurrentUserName;
                    model.RefundDate = DateTime.Now;
                }
                
                resultModel = this._return_GoodsService.UpdateReturnProductInfoTK(model);

                
                

                var opera = string.Empty;
                opera += "确认退款 ReturnStatus=5, ReturnOrderID:" + model.ReturnOrderID + ",结果:" + resultModel.IsValid;
                LogPackage.InserAC_OperateLog(opera, "修改退换货记录");
                resultModel = LogPackage.GetResulMessagest(resultModel.IsValid, new List<string> { resultModel.IsValid == true ? "Confirm refund success" : "Confirm refund failed" });
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultModel = LogPackage.GetResulMessagest(false, new List<string> { "Invalid return or change product record ID" });

            }

            return Json(resultModel, JsonRequestBehavior.AllowGet);

        }
	}
}