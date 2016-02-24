using HKTHMall.Admin.common;
using HKTHMall.Core;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class SP_ProductCommentController : Controller
    {
        private ISP_ProductCommentService _sp_ProductCommentService;
        public SP_ProductCommentController(ISP_ProductCommentService sp_ProductCommentService)
        {
            _sp_ProductCommentService = sp_ProductCommentService;
        }
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchSP_ProductCommentModel model)
        {
            //SearchAC_OperateLogModel logmodel = new SearchAC_OperateLogModel();
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            model.LanguageID =  ACultureHelper.GetLanguageID;


            int total = 0;
            //查询列表 
            var result = this._sp_ProductCommentService.GetSP_ProductCommentList(model);
            List<SP_ProductCommentModel> ds = result.Data;
            var data = new
            {

                rows = ds,
                total = result.Data.TotalCount,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 商品加载数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            SP_ProductCommentModel model = new SP_ProductCommentModel();
            if (id.HasValue)
            {
                SearchSP_ProductCommentModel smodel = new SearchSP_ProductCommentModel();
                smodel.ProductCommentId = id.Value;
                smodel.PagedIndex = 0;
                smodel.PagedSize = 100;
                //查询列表 
                
                List<SP_ProductCommentModel> List = this._sp_ProductCommentService.GetSP_ProductCommentList(smodel).Data;

                if (List != null && List.Count>0)
                {
                    model = List[0];
                }
                
            }
            return PartialView(model);
        }

        /// <summary>
        /// 商品新增,修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SP_ProductCommentModel model)
        {


            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();
                model.CheckBy = UserInfo.CurrentUserName;
                model.CheckDT = DateTime.Now;
                var result = this._sp_ProductCommentService.UpdateSP_ProductCommentCheckStatus(model);

                if (result != null)
                {

                    resultModel.Messages = new List<string> { result.IsValid == true ? "Review success！" : "Review failed！" };
                    var opera = string.Empty;
                    opera += "修改审核状态CheckStatus:" + model.CheckStatus + ",ProductCommentId:" + model.ProductCommentId + ",结果:" + result.IsValid;
                    LogPackage.InserAC_OperateLog(opera, "Review product comment");
                }

                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }

        /// <summary>
        /// 删除商品评论
        /// </summary>
        /// <param name="ProductCommentId">评论ID</param>
        /// <returns></returns>
        public JsonResult DeleteSP_ProductComment(long? ProductCommentId)
        {
            SP_ProductCommentModel model = new SP_ProductCommentModel();
            var resultModel = new ResultModel();
            if (ProductCommentId.HasValue)
            {
                model.ProductCommentId = ProductCommentId.Value;
                var result = this._sp_ProductCommentService.DeleteSP_ProductComment(model);
                var opera = string.Empty;
                opera += "删除商品评论 ProductCommentId:" + model.ProductCommentId + ",结果:" + result.IsValid;
                LogPackage.InserAC_OperateLog(opera, "Review product comment");
                resultModel = LogPackage.GetResulMessagest(result.IsValid, new List<string> { result.IsValid == true ? "Delete success" : "Delete failed" });
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultModel = LogPackage.GetResulMessagest(false, new List<string> { "Invalid comment ID " });

            }

            return Json(resultModel, JsonRequestBehavior.AllowGet);

        }
    }
}