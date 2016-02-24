using HKTHMall.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Services.Products;
using HKTHMall.Domain.Models;
using HKTHMall.Admin.common;
using HKTHMall.Domain.Enum;
using HKTHMall.Core.Utils;
using HKTHMall.Admin.Filters;


namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    /// 商品促销控制器处理
    /// </summary>
    [UserAuthorize]
    public class ProductRuleController : HKBaseController
    {
        private readonly IProductRuleService _productRuleService;
        private readonly ISalesRuleService _salesRuleService;
        private readonly IProductService _productService;

        public ProductRuleController(IProductRuleService productRuleService, ISalesRuleService salesRuleService, IProductService productService)
        {
            _productRuleService = productRuleService;
            _salesRuleService = salesRuleService;
            _productService = productService;
        }

        public ActionResult Index()
        {
            DrowSalesRuleList();
            return View();
        }

        /// <summary>
        /// 加载促销规则下拉框信息
        /// </summary>
        private void DrowSalesRuleList()
        {
            #region 加载促销规则下拉框信息
            var salesRuleList = this._salesRuleService.GetSalesRuleList().Data;
            var list = new List<SelectListItem>();
            if (salesRuleList != null)
            {
                foreach (var item in salesRuleList)
                {
                    var info = new SelectListItem();
                    info.Value = item.SalesRuleId.ToString();
                    info.Text = item.RuleName;
                    list.Add(info);
                }
            }
            ViewData["SalesRuleIdList"] = list;
            #endregion
        }

        #region 查询商品促销信息列表

        /// <summary>
        /// 查询商品促销信息列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchProductRuleModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var list =
                _productRuleService.Select(new SearchProductRuleModel
                {
                    PagedIndex = model.PagedIndex,
                    PagedSize = model.PagedSize,
                    SalesRuleId = model.SalesRuleId,
                    ProductId = model.ProductId,
                    LanguageID = ACultureHelper.GetLanguageID
                });
            var data = new { rows = list.Data, total = list.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除商品促销信息

        /// <summary>
        ///     删除商品促销信息
        /// </summary>
        /// <param name="productRuleId">商品促销信息Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(long? productRuleId)
        {
            var resultModel = new ResultModel();
            if (productRuleId.HasValue)
            {
                var result = _productRuleService.Delete(productRuleId.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete promotion information success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete promotion information failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            var opera = string.Format("删除商品促销信息 ProductRuleId:{0},操作结果:{1}", productRuleId, resultModel.IsValid ? "成功" : "失败");
            LogPackage.InserAC_OperateLog(opera, "商品管理--商品促销");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建商品促销信息信息

        /// <summary>
        ///     创建商品促销信息信息
        /// </summary>
        /// <param name="model">商品促销信息对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ProductRuleModel model)
        {
            var admin = UserInfo.CurrentUserName;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                model.StarDate = DateTime.Parse(model.TempStarDate);
                model.EndDate = DateTime.Parse(model.TempEndDate);

                if (model.ProductRuleId != 0)
                {
                    switch (model.SalesRuleId)
                    {
                        case 1:
                            model.StarDate = DateTime.Now;
                            model.EndDate = DateTime.Now;
                            break;
                        default:
                            break;
                    }
                    model.SalesRuleId = (int)ESalesRule.Discount;
                    var resultUp = _productRuleService.Update(model);
                    if (resultUp.IsValid)
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { "Modify promotion information success" };
                    }
                    else
                    {
                        resultModel.IsValid = false;
                        resultModel.Messages = new List<string> { resultUp.Messages[0] };
                    }
                    var opera = string.Format("修改商品促销:ProductRuleId={0},操作结果:{1}", model.ProductRuleId, resultModel.IsValid ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "商品管理--商品促销");
                }
                else
                {
                    model.ProductRuleId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.SalesRuleId = (int)ESalesRule.Discount;
                    var result = _productRuleService.Add(model);
                    if (result.IsValid)
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { "Add promotion information success" };
                    }
                    else
                    {
                        resultModel.IsValid = false;
                        resultModel.Messages = new List<string> { result.Messages[0] };
                    }

                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DrowSalesRuleList();
            }
            return PartialView(model);
        }

        #endregion

        #region 加载数据

        /// <summary>
        ///     加载数据
        /// </summary>
        /// <param name="id">对象商品促销信息Id</param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            ProductRuleModel model = null;
            if (id.HasValue)
            {
                var result = _productRuleService.GetProductRuleById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                    model.TempStarDate = model.StarDate.ToString();
                    model.TempEndDate = model.EndDate.ToString();
                    AddProductModel _addProductModel = _productService.GetSKU_ProductById(model.ProductId, ACultureHelper.GetLanguageID).Data;
                    if (_addProductModel != null)
                    {
                        model.HKPrice = ToolUtil.Round(_addProductModel.HKPrice, 2);
                        model.ProductName = _addProductModel.ProductName;
                        model.SalePrice = ToolUtil.Round(model.HKPrice * _addProductModel.Discount, 2);
                        model.SKU_ProductModels = _addProductModel.SKU_ProductModels;
                    }
                }
            }
            else
            {
                model = new ProductRuleModel();
                //model.TempStarDate = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd hh:mm");
               // model.TempEndDate = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd hh:mm");
            }
            DrowSalesRuleList();
            return PartialView(model);
        }

        #endregion

        #region 查询商品是否存在

        /// <summary>
        ///   查询商品是否存在
        /// </summary>
        /// <param name="model">商品促销信息对象</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsExsitProduct(long? id)
        {
            if (id.HasValue)
            {
                AddProductModel _addProductModel = _productService.GetSKU_ProductById(id.Value, ACultureHelper.GetLanguageID).Data;
                ProductRuleModel model = new ProductRuleModel();
                if (_addProductModel != null)
                {
                    ProductStatus productStatus = (ProductStatus)Enum.Parse(typeof(ProductStatus), _addProductModel.Status.ToString());
                    switch (productStatus)
                    {
                        case ProductStatus.Uncommitted:
                            return Json("Uncommitted", JsonRequestBehavior.AllowGet);
                        case ProductStatus.Submitting:
                            return Json("Submitting", JsonRequestBehavior.AllowGet);
                        case ProductStatus.ExaminationNotThrough:
                            return Json("ExaminationNotThrough", JsonRequestBehavior.AllowGet);
                        case ProductStatus.HasUpShelves:
                            model.HKPrice = ToolUtil.Round(_addProductModel.HKPrice, 2);
                            model.ProductName = _addProductModel.ProductName;
                            model.SalePrice = ToolUtil.Round(model.HKPrice * model.Discount, 2);
                            model.SKU_ProductModels = _addProductModel.SKU_ProductModels;
                            var jsonObject = Json(model, JsonRequestBehavior.AllowGet);
                            return jsonObject;
                        case ProductStatus.HasUnderShelves:
                            return Json("HasUnderShelves", JsonRequestBehavior.AllowGet);
                        default:
                            break;
                    }

                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}