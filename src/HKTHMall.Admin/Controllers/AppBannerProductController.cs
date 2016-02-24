using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Admin.Models;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.banner;
using HKTHMall.Services.Banner;
using HKTHMall.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class AppBannerProductController : HKBaseController
    {
        public static readonly string ImagePath = HKSJ.Common.ConfigHelper.GetConfigString("ImagePath");
        public static readonly string ImageHeader = HKSJ.Common.ConfigHelper.GetConfigString("ImageHeader");
        
        private GetBannerAndBannerProduct con = new GetBannerAndBannerProduct();
        
        private IbannerProductService _bannerProductService;

        public AppBannerProductController(IbannerProductService bannerProductService)
        {
            
            this._bannerProductService = bannerProductService;
            
        }
        //
        // GET: /AppBannerProduct App 楼层商品/ wuyf 2015-9-18
        public ActionResult Index()
        {
            var IdentityStatus = 3;
            ViewData["BannerPlaceCodeModel1"] = con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, IdentityStatus);
            
            ViewBag.ImagePath = ImagePath;
            ViewBag.titleName = "App floor Product";
            ViewBag.dentityStatus =IdentityStatus;
            return View();
        }

        /// <summary>
        /// 列表Banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchbannerProductModel model)
        {
            //SearchAC_OperateLogModel logmodel = new SearchAC_OperateLogModel();
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            //标识ID 1 首页右部的推荐商品，2首页楼层商品,3 APP楼层商品
            model.IdentityStatus = 3;


            int total = 0;
            //查询列表 由于是3表连接,关联关键字是商品ID,默认条件是商品语音为中文,如果查不到,请查看bannerProduct,Product_Lang
            //var result = this._bannerProductService.GetBannerProduct(model);
            var result = this._bannerProductService.GetBannerProduct(model, out total);
            List<bannerProductModel> ds = result.Data;
            var data = new
            {

                rows = ds,
                total = ds.Count,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Banner加载数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            bannerProductModel model = null;
            var identityStatus = 3;
            ViewBag.ImagePath = ImagePath;
            if (id.HasValue)
            {
                int total = 0;
                //查询列表 由于是3表连接,关联关键字是商品ID,默认条件是商品语音为中文,如果查不到,请查看bannerProduct,Product_Lang
                //var result = this._bannerProductService.GetBannerProduct(model);
                SearchbannerProductModel spmodel = new SearchbannerProductModel();
                spmodel.bannerProductId = id.Value;
                spmodel.IdentityStatus = identityStatus;
                var result = this._bannerProductService.GetBannerProduct(spmodel, out total).Data as List<bannerProductModel>;
                //Result result = this._bannerProductService.GetBannerProductById(id.Value);
                if (result != null && result.Count > 0)
                {
                    model = result[0];
                }
            }
            else
            {
                model = new bannerProductModel();
                model.IdentityStatus = identityStatus;//添加控制位置是否显示出来
            }
            
                ViewData["BannerPlaceCodeModel1"] = con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, model.IdentityStatus);
            

            //ViewData["IdentityModelBannerCreate"] = con.GetIdentityNameList(new int[] { 1 });
            return PartialView(model);
        }

        /// <summary>
        /// 验证推荐位置（分类）是否跟商品的分类相同
        /// </summary>
        /// <param name="bannerProductId">ID</param>
        /// <param name="PlaceCode">推荐位置</param>
        /// <param name="identityStatus">标识ID（目前没用，原来是作用于一个页面多个标识）</param>
        /// <returns></returns>
        public bool IsCategoryId(long productId, int PlaceCode, int identityStatus)
        {
            bool bl = false;

            //查询列表 由于是3表连接,关联关键字是商品ID,默认条件是商品语音为中文,如果查不到,请查看bannerProduct,Product_Lang

            try
            {
                CategoryService cs = new CategoryService();
                ProductModel modelp = GetBannerProductByProductId(productId);
                //根据推荐位置,获取位置的所有分类
                // List<ResultCategoryModel> ctmlist = cs.GetAll(CultureHelper.GetLanguageID).Data;
                List<ResultCategoryModel> ctmlist1 = cs.GetCategoriesByParentIds(ACultureHelper.GetLanguageID, PlaceCode).Data;
                if (modelp != null && ctmlist1 != null)
                {
                    for (int i = 0; i < ctmlist1.Count; i++)
                    {
                        if (modelp.CategoryId == ctmlist1[i].CategoryId && ctmlist1[i].AuditState)
                        {
                            bl = true; break;
                        }//if
                    }//for

                }//if
            }
            catch (Exception)
            {

                throw;
            }

            return bl;
        }

        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private static ProductModel GetBannerProductByProductId(long productId)
        {
            ProductService _productService = new ProductService();
            List<ProductModel> listmodel = _productService.GetBannerProductByProductId(productId).Data;
            ProductModel modelp = null;
            if (listmodel != null && listmodel.Count > 0)
            {
                modelp = listmodel[0];
            }

            return modelp;
        }

        /// <summary>
        /// Banner 表新增,修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(bannerProductModel model)
        {
            ViewData["BannerPlaceCodeModel1"] = null;
            ViewBag.ImagePath = ImagePath;
            model.IdentityStatus = 3;
            var opera = string.Empty;
           
                ViewData["BannerPlaceCodeModel1"] = con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, model.IdentityStatus);
            
            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();



                ProductModel modelp = GetBannerProductByProductId(model.productId);
                if (modelp == null)
                {
                    resultModel.Messages = new List<string>() { "Invalid product ID" };
                    return Json(resultModel, JsonRequestBehavior.AllowGet);
                }

                if ( !IsCategoryId(model.productId, model.PlaceCode, model.IdentityStatus))
                {
                    ViewBag.messges = "Recommended position (classification) with the classification of goods is not the same";
                    return PartialView(model);
                }

                if (model.bannerProductId != 0)
                {
                    model.UpdateBy = UserInfo.CurrentUserName;//asuser.UserName;
                    model.UpdateDT = DateTime.Now;
                    resultModel.Messages = new List<string>() { this._bannerProductService.UpdateBannerProduct(model).Data > 0 ? "Update success" : "Update failed" };
                    opera = string.Format("修改APP楼层商品:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "APP楼层商品-修改");
                }
                else
                {
                    model.bannerProductId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.CreateBy = UserInfo.CurrentUserName;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = UserInfo.CurrentUserName;
                    model.UpdateDT = DateTime.Now;
                    model.Sorts = MemCacheFactory.GetCurrentMemCache().Increment("commonId");

                    resultModel.Messages = new List<string>() { this._bannerProductService.AddBannerProduct(model).Data != 0 ? "Add success" : "Add failed" };
                    opera = string.Format("添加APP楼层商品:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "APP楼层商品-添加");
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }

        /// <summary>
        /// 删除Banner
        /// </summary>
        /// <param name="bannerId"></param>
        /// <returns></returns>
        public JsonResult DeleteBanner(long? bannerId)
        {
            bannerProductModel model = new bannerProductModel();



            var resultModel = new ResultModel();
            if (bannerId.HasValue)
            {
                model.bannerProductId = bannerId.Value;
                var result = this._bannerProductService.DeleteBannerProduct(model).IsValid;
                if (result)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete failed" };
                }

                var opera = string.Empty;
                opera = " bannerProductId:" + model.bannerProductId + ",ProductName:" + model.ProductName + ",PicAddress:" + model.PicAddress + ",PlaceCode=" + model.PlaceCode + ",结果:" + resultModel.Messages;
                LogPackage.InserAC_OperateLog(opera, "APP楼层商品-删除");
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Parameter ID error" };
            }

            return Json(resultModel, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Banner上下移动Sorts（用于列表排序显示）
        /// </summary>
        /// <param name="bannerId"></param>
        /// <param name="sx">1上移动,2下移动</param>
        /// <returns></returns>
        public string UpdateSorts(SortsModel modelst)
        {
            bannerProductModel model = new bannerProductModel();
            bannerProductModel modelsx = new bannerProductModel();
            if (modelst.bannerId > 0)
            {
                //UserInfoModel asuser = UserInfo.GetLoginUserInfo();
                List<bannerProductModel> ds = null;
                //查询
                SearchbannerProductModel spmodel = new SearchbannerProductModel();
                spmodel.bannerProductId = modelst.bannerId;
                spmodel.IdentityStatus = modelst.IdentityStatus;
                int total = 0;
                var result1 = this._bannerProductService.GetBannerProduct(spmodel, out total).Data as List<bannerProductModel>;

                if (result1 != null && result1.Count > 0)
                {

                    bannerProductModel models = new bannerProductModel();
                    models = result1[0];
                    modelst.bannerId = models.bannerProductId;
                    modelst.BannerPlaceCode = models.PlaceCode;
                    modelst.Sorts = models.Sorts;
                    modelst.IdentityStatus = models.IdentityStatus;
                }
                if (modelst.sx == 1)
                {
                    //获取触发行的下一行数据
                    var resultlist = this._bannerProductService.GetBannerProduct(modelst.Sorts, 1, modelst.IdentityStatus, modelst.bannerId, 1, modelst.BannerPlaceCode);
                    ds = resultlist.Data;
                    if (ds != null && ds.Count > 0)
                    {
                        modelsx = ds[0];
                        modelsx.UpdateBy = UserInfo.CurrentUserName;
                        modelsx.UpdateDT = DateTime.Now;
                        var msorts = modelsx.Sorts;
                        modelsx.Sorts = modelst.Sorts;//触发行的上一行,把它的排序减一
                        //this._bannerService.UpdateBanner(smodel);
                        model.Sorts = msorts;//触发行修改的排序
                    }
                    else
                    {
                        return "This is last line!";
                    }
                }
                else
                {
                    //获取触发行的上一行数据
                    var resultlist = this._bannerProductService.GetBannerProduct(modelst.Sorts, 2, modelst.IdentityStatus, modelst.bannerId, 1, modelst.BannerPlaceCode);
                    ds = resultlist.Data;
                    if (ds != null && ds.Count > 0)
                    {
                        modelsx = ds[ds.Count - 1];
                        modelsx.UpdateBy = UserInfo.CurrentUserName;
                        modelsx.UpdateDT = DateTime.Now;
                        var msorts = modelsx.Sorts;
                        modelsx.Sorts = modelst.Sorts;//触发行的下一行,把它排序加一
                        //this._bannerService.UpdateBanner(smodel1);
                        model.Sorts = msorts;//触发行修改的排序
                    }
                    else
                    {
                        return "This is top line";
                    }
                }

                model.bannerProductId = modelst.bannerId;
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;

                var result = this._bannerProductService.UpdateSorts(model, modelsx).IsValid;
                return result == true ? "Move success！" : "Move failed！";
            }

            return "Move failed！";
        } 

	}
}