using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Services.Orders;
using HKTHMall.Services.Products;
using System.Web.Mvc;
using HKTHMall.Domain.Models;
using HKTHMall.Core.UploadFile;
using HKSJ.Common;
using HKTHMall.Admin.common;
using HKTHMall.Core.Utils;
using HKTHMall.Admin.Filters;
using HKTHMall.Domain.Enum;
using HKSJ.Common.FastDFS;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class SalesProductController : HKBaseController
    {
        public static readonly string imagePath = ConfigHelper.GetConfigString("ImagePath");
        private readonly UploadFile uf = new UploadFile();

        private readonly ISalesProductService _salesProductService;
        private readonly IProductRuleService _productRuleService;
        private readonly IProductService _productService;

        public SalesProductController(ISalesProductService salesProductService, IProductRuleService productRuleService, IProductService productService)
        {
            _salesProductService = salesProductService;
            _productRuleService = productRuleService;
            _productService = productService;
        }

        // GET: ParameterSet
        public ActionResult Index()
        {
            return View();
        }

        #region 查询广告促销商品列表

        /// <summary>
        ///  查询广告促销商品列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-9</remarks>
        public JsonResult List(SearchSalesProductModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var list =
                _salesProductService.Select(new SearchSalesProductModel
                {
                    PagedIndex = model.PagedIndex,
                    PagedSize = model.PagedSize,
                    ProductName = model.ProductName,
                    LanguageID = ACultureHelper.GetLanguageID
                });
            var data = new { rows = list.Data, total = list.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除广告促销商品

        /// <summary>
        ///     删除广告促销商品
        /// </summary>
        /// <param name="salesProductId">广告促销商品Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-9</remarks>
        public JsonResult Delete(long? salesProductId)
        {
            var resultModel = new ResultModel();
            if (salesProductId.HasValue)
            {
                var result = _salesProductService.Delete(salesProductId.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete promotion item success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete promotion item failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Promotion item key Id is wrong" };
            }
            var opera = string.Format("删除广告促销商品 salesProductId:{0},操作结果:{1}", salesProductId, resultModel.IsValid ? "成功" : "失败");
            LogPackage.InserAC_OperateLog(opera, "系统管理--权限管理");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 编辑广告促销商品

        /// <summary>
        ///     编辑广告促销商品
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SalesProductModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = UserInfo.CurrentUserName;
                var resultModel = new ResultModel();
                if (model.SalesProductId != 0)
                {
                    model.UpdateBy = userName;
                    model.UpdateDT = DateTime.Now;
                    var resultUp = _salesProductService.Update(model);
                    if (resultUp.IsValid)
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { "Change promotion item success" };
                    }
                    else
                    {
                        resultModel.IsValid = false;
                        resultModel.Messages = new List<string> { resultUp.Messages[0] };
                    }
                    var opera = string.Format("修改广告促销商品参数:SalesProductId={0},操作结果:{1}", model.SalesProductId, resultModel.IsValid ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "广告管理--首页促销商品");
                }
                else
                {
                    model.SalesProductId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.CreateBy = userName;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = userName;
                    model.UpdateDT = DateTime.Now;
                    model.Sorts = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    var result = _salesProductService.Add(model);
                    if (result.IsValid)
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { "Add promotion item success" };
                    }
                    else
                    {
                        resultModel.IsValid = false;
                        resultModel.Messages = new List<string> { result.Messages[0] };
                    }

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
        /// <param name="id">对象系统Id</param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            SalesProductModel model = null;
            if (id.HasValue)
            {
                List<SalesProductModel> result = _salesProductService.GetSalesProductById(id.Value).Data;

                if (result != null && result.Count > 0)
                {
                    model = result[0];
                    ViewBag.ImgUrl = model.PicAddress;
                    ViewBag.ShowImg = imagePath + model.PicAddress;

                    //获取商品的信息
                    AddProductModel _addProductModel = _productService.GetSKU_ProductById(model.productId, ACultureHelper.GetLanguageID).Data;
                    if (_addProductModel != null)
                    {
                        model.HKPrice = ToolUtil.Round(_addProductModel.HKPrice, 2);
                        model.ProductName = _addProductModel.ProductName;
                        model.SalePrice = ToolUtil.Round(model.HKPrice * model.Discount, 2);
                        model.SKU_ProductModels = _addProductModel.SKU_ProductModels;
                    }
                }
            }
            else
            {
                model = new SalesProductModel();
                model.Sorts = 1;
            }
            // DrowList(model);



            return PartialView(model);
        }

        #endregion

        /// <summary>
        /// 上传图片（商品）
        /// </summary>
        /// <returns></returns>
        public string GetUpLoad()
        {
            HttpPostedFileBase postFile = Request.Files["upLoad"];
            System.IO.Stream s = postFile.InputStream;
            byte[] bt = new byte[postFile.ContentLength];
            System.IO.MemoryStream mes = new System.IO.MemoryStream(bt);
            s.Read(bt, 0, bt.Length);
            s.Close();
            int w1 = string.IsNullOrEmpty(Request["w"]) ? 0 : int.Parse(Request["w"]);
            int h1 = string.IsNullOrEmpty(Request["h"]) ? 0 : int.Parse(Request["h"]);
            string t = postFile.ContentType;
            string imageTypes = "image/pjpeg,image/jpeg,image/gif,image/png,image/x-png";
            if (imageTypes.IndexOf(t) < 0)
            {
                return "1";
            }
            else
            {
                s.Read(bt, 0, bt.Length);
                s.Close();
                System.Drawing.Image image = System.Drawing.Image.FromStream(mes);

                //int H = image.Height;
                //int W = image.Width;
                //if (H < h1 || W < w1)
                //{
                //    return "2";
                //}
                //else
                //{
                //    string fileName = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, bt, "jpg");
                //   // fileName = new UploadFile().GetThumbsImage(fileName, 660, 200);
                //    return fileName;
                //}

                string fileName = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, bt, "jpg");
                return fileName;
            }
        }

        #region 广告促销商品
        /// <summary>
        /// 广告促销商品
        /// </summary>
        /// <param name="SalesProductId">广告促销商品ID</param>
        /// <param name="rowId">行数下标</param>
        /// <param name="sortType">1、上移；2、下移</param>
        /// <returns></returns>
        public JsonResult UpdatePlace(long? SalesProductId, int rowId, int sortType, string ProductName, int PagedIndex, int PagedSize)
        {
            var resultModel = new ResultModel();
            resultModel.Messages = new List<string> { "Move success." };
            if (SalesProductId.HasValue)
            {
                List<SalesProductModel> paramList =
                 _salesProductService.Select(new SearchSalesProductModel
                 {
                     PagedIndex = PagedIndex,
                     PagedSize = PagedSize,
                     ProductName = ProductName,
                     LanguageID = ACultureHelper.GetLanguageID
                 }).Data;
                if (paramList != null && paramList.Count > 0)
                {
                    switch (sortType)
                    {
                        case 1://上移
                            if (rowId > 0)
                            {
                                _salesProductService.UpdatePlace(paramList[rowId].SalesProductId, (int)paramList[rowId - 1].Sorts);
                                _salesProductService.UpdatePlace(paramList[rowId - 1].SalesProductId, (int)paramList[rowId].Sorts);
                            }
                            else
                            {
                                resultModel.IsValid = false;
                                resultModel.Messages = new List<string> { "This is top line." };
                            }
                            break;
                        case 2://下移
                            if (rowId < (paramList.Count - 1))
                            {
                                _salesProductService.UpdatePlace(paramList[rowId].SalesProductId, (int)paramList[rowId + 1].Sorts);
                                _salesProductService.UpdatePlace(paramList[rowId + 1].SalesProductId, (int)paramList[rowId].Sorts);
                            }
                            else
                            {
                                resultModel.IsValid = false;
                                resultModel.Messages = new List<string> { "This is last line!." };
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Failed, ID error." };
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
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
                            model.SKU_ProductModels = _addProductModel.SKU_ProductModels;
                            model.ProductName = _addProductModel.ProductName;
                            model.SalePrice = ToolUtil.Round(model.HKPrice * _addProductModel.Discount, 2);
                         
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