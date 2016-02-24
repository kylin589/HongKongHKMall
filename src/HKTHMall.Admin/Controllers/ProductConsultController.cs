using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel;
using HKTHMall.Domain.AdminModel.Products;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class ProductConsultController : HKBaseController
    {
        private readonly IProductConsultService _productConsultService;
        public ProductConsultController(IProductConsultService productConsultService)
        {
            this._productConsultService = productConsultService;
        }

        // GET: ProductConsult
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 商品咨询列表页
        ///  zhoub 20150827
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public JsonResult List(SearchProductConsultModel searchModel)
        {
            var result = _productConsultService.GetPagingProductConsult(searchModel, ACultureHelper.GetLanguageID);
            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 商品咨询回复数据查询
        /// zhoub 20150827
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            ProductConsultModel model = new ProductConsultModel();
            if (id.HasValue)
            {
                SearchProductConsultModel smodel = new SearchProductConsultModel();
                smodel.ProductConsultId = id.Value;
                smodel.PagedIndex = 0;
                smodel.PagedSize = 100;
                //查询列表 
                List<ProductConsultModel> List = this._productConsultService.GetPagingProductConsult(smodel, ACultureHelper.GetLanguageID).Data;
                if (List != null && List.Count > 0)
                {
                    model = List[0];
                }
            }
            return PartialView(model);
        }

        /// <summary>
        /// 商品咨询回复
        /// zhoub 20150827
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ProductConsultModel model)
        {
            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();
                model.ReplyBy = UserInfo.CurrentUserName;
                model.ReplyDT = DateTime.Now;
                resultModel = this._productConsultService.ReplyProductConsult(model, ACultureHelper.GetLanguageID);
                resultModel.Messages = new List<string> { resultModel.IsValid == true ? "Success！" : "Failed！" };
                string opera = string.Format("商品咨询回复:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                LogPackage.InserAC_OperateLog(opera, "Product--Product Consult-Reply");
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }

        /// <summary>
        /// 删除商品咨询 
        /// zhoub 20150827
        /// </summary>
        /// <param name="ParamenterID">用户Id</param>
        /// <returns></returns>
        public JsonResult Delete(long? productConsultId)
        {
            ResultModel resultModel = new ResultModel();
            if (productConsultId.HasValue)
            {
                resultModel = _productConsultService.DeleteProductConsult(productConsultId.Value, ACultureHelper.GetLanguageID);
                resultModel.Messages = new List<string> { resultModel.IsValid == true ? "Success！" : "Failed！" };
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "主键Id有误" };
            }
            string opera = string.Format("商品咨询删除:{0},操作结果:{1}", productConsultId, resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "Product--Product Consult-Reply");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
    }
}