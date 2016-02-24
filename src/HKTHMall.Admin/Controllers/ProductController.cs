using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models.Bra;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Services.Products;
using HKTHMall.Services.Sys;
using Newtonsoft.Json;
using HKTHMall.Services.Shipment;
using HKTHMall.Domain.Models;

namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    ///     产品
    /// </summary>

    [UserAuthorize]
    public class ProductController : HKBaseController
    {
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly IParameterSetService _parameterSetService;
        private readonly IProductService _productService;
        private readonly IShipmentService _shipmentService;
        public ProductController(IProductService productService,
            IBrandService brandService,
            ICategoryService categoryService,
            IParameterSetService parameterSetService,
            IShipmentService shipmentService)
        {
            this._productService = productService;
            this._brandService = brandService;
            this._categoryService = categoryService;
            this._parameterSetService = parameterSetService;
            this._shipmentService = shipmentService;
        }

        //
        // GET: /Product/
        public ActionResult Index()
        {
            IList<BrandModel> brandlist = this._brandService.GetAll(ACultureHelper.GetLanguageID).Data;

            this.ViewBag.Brands = new SelectList(brandlist, "BrandID", "BrandName", 0);

            List<CategoryModel> categoryList = this._categoryService.GetCategoryByParentId(0,
                ACultureHelper.GetLanguageID).Data;

            this.ViewBag.Categories =
                new SelectList(categoryList.Select(m => new { m.CategoryId, m.CategoryLanguageModel.CategoryName }),
                    "CategoryId", "CategoryName", 0);

            return this.View();
        }

        private IList<ResultCategoryModel> FormatCategory(List<ResultCategoryModel> categoryList,
            ResultCategoryModel parentCategory)
        {
            var parentId = 0;
            if (parentCategory != null && parentCategory.CategoryId.HasValue)
            {
                parentId = parentCategory.CategoryId.Value;
            }

            var cts = categoryList.FindAll(m => m.parentId == parentId);
            var list = new List<ResultCategoryModel>();

            foreach (var ct in cts)
            {
                if (parentCategory != null)
                {
                    ct.CategoryName = parentCategory.CategoryName + ">>" + ct.CategoryLanguageModel.CategoryName;
                }
                else
                {
                    ct.CategoryName = ct.CategoryLanguageModel.CategoryName;
                }

                list.Add(ct);
                list.AddRange(this.FormatCategory(categoryList, ct));
            }
            return list;
        }

        public ActionResult ProductComment()
        {
            return this.View();
        }

        public ActionResult ProductCommentCreate()
        {
            return this.View();
        }

        #region 产品 查询 添加 更新 复制

        /// <summary>
        ///     根据条件查询产品
        /// </summary>
        /// <param name="model">查询条件Model</param>
        /// <returns>Json数据</returns>
        public ActionResult Search(SearchProductModel model)
        {
            model.LanguageId = ACultureHelper.GetLanguageID;
            var result = this._productService.Search(model);

            return this.Json(new { rows = result.Data, total = result.Data.TotalCount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     审核
        /// </summary>
        /// <param name="id">产品Id</param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult ProductCheck(long id)
        {
            AddProductModel product = this._productService.GetProductById(id, ACultureHelper.GetLanguageID).Data;
            return this.PartialView(product);
        }

        /// <summary>
        ///     执行审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProductCheck(long ProductId, ProductStatus ProductStatus)
        {
            var result = this._productService.Check(ProductId, ProductStatus);

            var opera = string.Format("商品审核:{0},操作结果:{1}", "商品Id:" + ProductId + (result.IsValid ? "成功" : "失败"),
                result.Messages);

            LogPackage.InserAC_OperateLog(opera, "商品管理-审核");

            return this.Json(result);
        }
        /// <summary>
        ///     审核
        /// </summary>
        /// <param name="id">产品Id</param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult ProductsCheck(string[] pId)
        {           
            pId = Request.Params.GetValues(0);
           
            if (pId != null)
            {      
                    ViewBag.Type = "m";
                    ViewBag.ids =string.Join(",", pId);
            }           
            return this.PartialView();
        }

        /// <summary>
        ///     执行审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProductsCheck(string ProductIds, ProductStatus ProductStatus)
        {
            string[] ids = ProductIds.Split(',');
            long[] str2 = new long[ids.Length];
            for (int i=0;i<ids.Length;i++) {
                str2[i] = long.Parse(ids[i]);
            }
            var result = this._productService.Check(str2, ProductStatus);

            var opera = string.Format("商品审核:{0},操作结果:{1}", "商品Id:" + ProductIds + (result.IsValid ? "成功" : "失败"),
                result.Messages);

            LogPackage.InserAC_OperateLog(opera, "商品管理-审核");

            return this.Json(result);
        }
        /// <summary>
        ///     更新或删除产品
        /// </summary>
        /// <param name="id">productId</param>
        /// <returns>视图</returns>
        [HttpGet]
        public ActionResult AddOrUpdate(long? id)
        {
            var model = new AddProductModel
            {
                Product_LangModels = new List<ProductModel.Product_LangModel>
                {
                    new ProductModel.Product_LangModel(),
                    new ProductModel.Product_LangModel(),
                    new ProductModel.Product_LangModel(),
                    new ProductModel.Product_LangModel()
                }
            };
            this.ViewBag.fare = this._shipmentService.GetAllFareTemp().Data;
            if (id.HasValue)
            {
                model = this._productService.GetProductById(id.Value, ACultureHelper.GetLanguageID).Data;
                this.ViewBag.pics = model.ProductPicModels;
                this.ViewBag.items = model.SKU_SKUItemsModels;
                this.ViewBag.attrs = model.SKU_ProductAttributesModels;
                this.ViewBag.skus = model.SKU_ProductModels;
                
                this.ViewBag.parameters = model.ProductParametersModels
                    .GroupBy(m => m.GroupName)
                    .ToList();
            }
            //model.RebateDays = this._parameterSetService.GetParametePValueById(7529218793).Data != null ? Convert.ToInt32(this._parameterSetService.GetParametePValueById(7529218793).Data) : null;
            model.RebateRatio = this._parameterSetService.GetParametePValueById(7529218877).Data != null ? decimal.Parse(this._parameterSetService.GetParametePValueById(7529218877).Data) : null;
            return this.View(model);
        }

        /// <summary>
        ///     批量删除产品
        /// </summary>
        /// <param name="ids">产品ids</param>
        /// <returns>json结果</returns>
        public ActionResult Delete(IList<long> ids)
        {
            var result = this._productService.DeleteList(ids);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     添加产品
        /// </summary>
        /// <param name="model">添加内容</param>
        /// <returns>Json数据</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(AddProductModel model)
        {
            if (Convert.ToInt64(this._parameterSetService.GetParametePValueById(7529218804).Data) == 1)
            {
                //model.RebateDays = Convert.ToInt32(this._parameterSetService.GetParametePValueById(7529218793).Data);
                model.RebateRatio = decimal.Parse(this._parameterSetService.GetParametePValueById(7529218877).Data);
            }
            model.ProductId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
            model.CreateBy = UserInfo.CurrentUserName;
            model.CreateDT = DateTime.Now;
            model.Status = (int)ProductStatus.Submitting;
            model.MerchantID = Convert.ToInt64(this._parameterSetService.GetParametePValueById(4557557848).Data);

            model.ProdctRuleModel.ProductRuleId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
            model.ProdctRuleModel.SalesRuleId = 1;

            if (model.SKU_ProductModels != null && !model.SKU_ProductModels.Any())
            {
                model.SKU_ProductModels.Add(new ProductModel.AddSKU_ProductModel
                {
                    ProductId = model.ProductId.Value,
                    SKU_ProducId = MemCacheFactory.GetCurrentMemCache().Increment("logId"),
                    CreateBy = UserInfo.CurrentUserName,
                    CreateDT = DateTime.Now,
                    PurchasePrice = model.PurchasePrice ?? 0,
                    HKPrice = model.HKPrice,
                    MarketPrice = model.MarketPrice ?? 0,
                    SKUStr = "",
                    Stock = model.StockQuantity,
                    IsUseBool = true
                });
            }
            else
            {
                if (model.SKU_ProductModels != null && model.SKU_ProductModels.Any())
                {
                    model.StockQuantity = 0;
                    foreach (var item in model.SKU_ProductModels)
                    {
                        item.CreateBy = UserInfo.CurrentUserName;
                        item.CreateDT = DateTime.Now;
                        model.StockQuantity += item.Stock;
                        item.ProductId = model.ProductId.Value;
                        item.SKU_ProducId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                    }

                    var minP = model.SKU_ProductModels.FirstOrDefault(
                            m => m.HKPrice == model.SKU_ProductModels.Min(min => min.HKPrice));
                    if (minP != null)
                    {
                        model.HKPrice = minP.HKPrice;
                        model.PurchasePrice = minP.PurchasePrice;
                        model.MarketPrice = minP.MarketPrice;
                    }
                }
            }

            if (model.SKU_ProductAttributesModels != null && model.SKU_ProductAttributesModels.Any())
            {
                foreach (var item in model.SKU_ProductAttributesModels)
                {
                    item.CreateBy = UserInfo.CurrentUserName;
                    item.CreateDT = DateTime.Now;
                    item.ProductId = model.ProductId.Value;
                    item.SKU_ProductAttributesId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                }
            }

            if (model.SKU_SKUItemsModels != null && model.SKU_SKUItemsModels.Any())
            {
                foreach (var item in model.SKU_SKUItemsModels)
                {
                    item.CreateBy = UserInfo.CurrentUserName;
                    item.CreateDT = DateTime.Now;
                    item.ProductId = model.ProductId.Value;
                    item.SKU_SKUItemsId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                }
            }

            if (model.ProductPicModels != null && model.ProductPicModels.Any())
            {
                var isSetSort = false;
                foreach (var item in model.ProductPicModels.OrderBy(m => m.sort))
                {
                    if (!isSetSort)
                    {
                        item.sort = 1;
                        isSetSort = true;
                    }
                    item.ProductID = model.ProductId.Value;
                    item.Flag = 0;
                    item.ProductPicId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                }

                var productPicModel = model.ProductPicModels.OrderBy(m => m.sort).FirstOrDefault();
                if (productPicModel != null)
                    productPicModel.Flag = 1;
            }

            if (model.ProductParametersModels != null && model.ProductParametersModels.Count > 0)
            {
                foreach (var para in model.ProductParametersModels)
                {
                    para.ProductId = model.ProductId.Value;
                    para.ParametersId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                }
            }

            model.ProdctRuleModel.ProductId = model.ProductId.Value;

            var result = this._productService.Add(model);

            if (result.IsValid)
            {
                return this.RedirectToAction("Index");
            }

            model.ProductId = null;

            this.ViewBag.pics = model.ProductPicModels;
            this.ViewBag.items = model.SKU_SKUItemsModels;
            this.ViewBag.attrs = model.SKU_ProductAttributesModels;
            this.ViewBag.skus = model.SKU_ProductModels;

            return this.View("AddOrUpdate", model);
        }

        /// <summary>
        ///     更新产品
        /// </summary>
        /// <param name="model">更新内容</param>
        /// <returns>Json数据</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(UpdateProductModel model)
        {
            if (!model.ProductId.HasValue)
            {
                return this.RedirectToAction("Index");
            }

            model.UpdateDT = DateTime.Now;
            model.UpdateBy = UserInfo.CurrentUserName;

            if (model.SKU_ProductModels != null && !model.SKU_ProductModels.Any())
            {
                var oldSku_Products =
                    ((AddProductModel)
                        this._productService.GetProductById(model.ProductId.Value, ACultureHelper.GetLanguageID).Data)
                        .SKU_ProductModels;
                if (oldSku_Products.Count == 1)
                {
                    var oldSku_product = oldSku_Products.SingleOrDefault();
                    if (string.IsNullOrEmpty(oldSku_product.SKUStr))
                    {
                        model.SKU_ProductModels.Add(new ProductModel.UpdateSKU_ProductModel
                        {
                            ProductId = oldSku_product.ProductId,
                            SKU_ProducId = oldSku_product.SKU_ProducId,
                            UpdateBy = UserInfo.CurrentUserName,
                            UpdateDT = DateTime.Now,
                            PurchasePrice = model.PurchasePrice ?? 0,
                            HKPrice = model.HKPrice,
                            MarketPrice = model.MarketPrice ?? 0,
                            SKUStr = "",
                            Stock = model.StockQuantity,
                            IsUseBool = true
                        });
                    }
                    else
                    {
                        model.SKU_ProductModels.Add(new ProductModel.UpdateSKU_ProductModel
                        {
                            ProductId = model.ProductId.Value,
                            SKU_ProducId = MemCacheFactory.GetCurrentMemCache().Increment("logId"),
                            UpdateBy = UserInfo.CurrentUserName,
                            UpdateDT = DateTime.Now,
                            PurchasePrice = model.PurchasePrice ?? 0,
                            HKPrice = model.HKPrice,
                            MarketPrice = model.MarketPrice ?? 0,
                            SKUStr = "",
                            Stock = model.StockQuantity,
                            IsUseBool = true
                        });
                    }
                }
                else
                {
                    model.SKU_ProductModels.Add(new ProductModel.UpdateSKU_ProductModel
                    {
                        ProductId = model.ProductId.Value,
                        SKU_ProducId = MemCacheFactory.GetCurrentMemCache().Increment("logId"),
                        UpdateBy = UserInfo.CurrentUserName,
                        UpdateDT = DateTime.Now,
                        PurchasePrice = model.PurchasePrice ?? 0,
                        HKPrice = model.HKPrice,
                        MarketPrice = model.MarketPrice ?? 0,
                        SKUStr = "",
                        Stock = model.StockQuantity,
                        IsUseBool = true
                    });
                }
            }
            else
            {
                model.StockQuantity = 0;
                foreach (var item in model.SKU_ProductModels)
                {
                    item.UpdateBy = UserInfo.CurrentUserName;
                    item.UpdateDT = DateTime.Now;
                    item.ProductId = model.ProductId.Value;
                    model.StockQuantity += item.Stock;
                    if (item.SKU_ProducId == 0)
                    {
                        item.SKU_ProducId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                    }
                }

                var minP = model.SKU_ProductModels.FirstOrDefault(
                            m => m.HKPrice == model.SKU_ProductModels.Min(min => min.HKPrice));
                if (minP != null)
                {
                    model.HKPrice = minP.HKPrice;
                    model.PurchasePrice = minP.PurchasePrice;
                    model.MarketPrice = minP.MarketPrice;
                }
            }

            if (model.SKU_ProductAttributesModels != null && model.SKU_ProductAttributesModels.Any())
            {
                foreach (var item in model.SKU_ProductAttributesModels)
                {
                    item.UpdateBy = UserInfo.CurrentUserName;
                    item.UpdateDT = DateTime.Now;
                    item.ProductId = model.ProductId.Value;
                    if (item.SKU_ProductAttributesId == 0)
                    {
                        item.SKU_ProductAttributesId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                    }
                }
            }


            if (model.SKU_SKUItemsModels != null && model.SKU_SKUItemsModels.Any())
            {
                foreach (var item in model.SKU_SKUItemsModels)
                {
                    item.UpdateBy = UserInfo.CurrentUserName;
                    item.UpdateDT = DateTime.Now;
                    item.ProductId = model.ProductId.Value;
                    if (!item.SKU_SKUItemsId.HasValue || item.SKU_SKUItemsId.Value == 0)
                    {
                        item.SKU_SKUItemsId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                    }
                }
            }


            if (model.ProductPicModels != null && model.ProductPicModels.Any())
            {
                var isSetSort = false;

                foreach (var item in model.ProductPicModels.OrderBy(m => m.sort))
                {
                    if (!isSetSort)
                    {
                        item.sort = 1;
                        isSetSort = true;
                    }
                    item.ProductID = model.ProductId.Value;
                    item.Flag = 0;
                    if (item.ProductPicId == 0)
                    {
                        item.ProductPicId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                    }
                }
                var productPicModel = model.ProductPicModels.OrderBy(m => m.sort).FirstOrDefault();
                if (productPicModel != null)
                    productPicModel.Flag = 1;
            }

            if (model.ProductParametersModels != null && model.ProductParametersModels.Count > 0)
            {
                foreach (var para in model.ProductParametersModels)
                {
                    para.ProductId = model.ProductId.Value;
                    para.ParametersId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                }
            }

            var result = this._productService.Update(model);

            if (result.IsValid)
            {
                var opera = string.Format("修改商品:{0},操作结果:{1}", JsonConvert.SerializeObject(model), result.Messages);
                LogPackage.InserAC_OperateLog(opera, "商品管理-修改");

                return this.RedirectToAction("Index");
            }

            this.ViewBag.pics = model.ProductPicModels;
            this.ViewBag.items = model.SKU_SKUItemsModels;
            this.ViewBag.attrs = model.SKU_ProductAttributesModels;
            this.ViewBag.skus = model.SKU_ProductModels;

            return this.View("AddOrUpdate", model);
        }
        [HttpPost]       
        public JsonResult Copy(long? id)
        {
            if (id.HasValue)
            {
              ResultModel  result = this._productService.CopyProductById(id.Value);
              return this.Json(result);
            }
            return this.Json(new ResultModel() { IsValid = false, Messages = new List<string>() {" the [id] is empty"} });
        }
        #endregion
    }
}