using HKTHMall.Admin.common;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.banner;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Services.Banner;
using HKTHMall.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Admin.Filters;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.Products;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class BannerProductController : HKBaseController
    {
        public static readonly string ImagePath = HKSJ.Common.ConfigHelper.GetConfigString("ImagePath");
        public static readonly string ImageHeader = HKSJ.Common.ConfigHelper.GetConfigString("ImageHeader");
        
        private GetBannerAndBannerProduct con = new GetBannerAndBannerProduct();
        
        private IbannerProductService _bannerProductService;
       
        public BannerProductController(IbannerProductService bannerProductService)
        {
            
            this._bannerProductService = bannerProductService;
            
        }
        /// <summary>
        /// BannerProduct首页
        /// </summary>
        /// <param name="IdentityStatus"></param>
        /// <returns></returns>
        public ActionResult Index(int? IdentityStatus)
        {

            ViewData["BannerPlaceCodeModel1"] = null;
            if (IdentityStatus.HasValue&&IdentityStatus.Value == 2)
            {
                ViewData["BannerPlaceCodeModel1"] = con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, IdentityStatus.Value);
            }
            ViewBag.ImagePath = ImagePath;
            ViewBag.titleName = con.GetBannerProducIdentityStatusName(IdentityStatus.HasValue == true ? IdentityStatus.Value : 1);
            ViewBag.dentityStatus = IdentityStatus.HasValue == true ? IdentityStatus.Value : 1;
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
            //标识ID 1首页轮播banner,2首页楼层banner,3 分类频道轮播banner,4分类频道楼层banner
            model.IdentityStatus = model.IdentityStatus == null ? 1 : model.IdentityStatus;

            model.LanguageID = ACultureHelper.GetLanguageID;
            int total = 0;
            //查询列表 由于是3表连接,关联关键字是商品ID,默认条件是商品语音为中文,如果查不到,请查看bannerProduct,Product_Lang
            //var result = this._bannerProductService.GetBannerProduct(model);
            var result = this._bannerProductService.GetBannerProduct(model,out total);
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
        public ActionResult Create(long? id, int identityStatus)
        {
            bannerProductModel model = null;
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
                if (result != null&& result.Count>0)
                {
                    model = result[0];
                }
            }
            else
            {
                model = new bannerProductModel();
                model.IdentityStatus = identityStatus;//添加控制位置是否显示出来
            }
            ViewData["BannerPlaceCodeModel1"] = null;
            if (model.IdentityStatus==2)
            {
                ViewData["BannerPlaceCodeModel1"] = con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, model.IdentityStatus);
            }
            
            //ViewData["IdentityModelBannerCreate"] = con.GetIdentityNameList(new int[] { 1 });
            return PartialView(model);
        }

        /// <summary>
        /// 验证推荐位置（分类）是否跟商品的分类相同
        /// </summary>
        /// <param name="bannerProductId">ID</param>
        /// <param name="PlaceCode">推荐位置</param>
        /// <param name="identityStatus">标识ID</param>
        /// <returns></returns>
        public bool IsCategoryId(long productId, int PlaceCode, int identityStatus)
        {
            bool bl = false;
            int total = 0;
            //查询列表 由于是3表连接,关联关键字是商品ID,默认条件是商品语音为中文,如果查不到,请查看bannerProduct,Product_Lang

            try
            {
                ProductModel modelp = GetBannerProductByProductId(productId);

                if (modelp != null)
                {

                    if (modelp.CategoryId == PlaceCode)
                    {
                        bl = true;
                    }
                }
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
            ProductModel modelp = _productService.GetBannerProductByProductId(productId).Data;
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
            if (model.IdentityStatus == 2)
            {
                ViewData["BannerPlaceCodeModel1"] = con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, model.IdentityStatus);
            }
            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();



                ProductModel modelp = GetBannerProductByProductId(model.productId);
                if (modelp==null)
                {
                    resultModel.Messages = new List<string>() { "Invalid product ID" };
                    return Json(resultModel, JsonRequestBehavior.AllowGet);
                }

                if (model.IdentityStatus == 2 && !IsCategoryId(model.productId, model.PlaceCode, model.IdentityStatus))
                {
                    ViewBag.messges = "推荐位置（分类）跟商品的分类不相同";
                    return PartialView(model);
                }

                if (model.bannerProductId != 0)
                {
                    model.UpdateBy = UserInfo.CurrentUserName;//asuser.UserName;
                    model.UpdateDT = DateTime.Now;

                    resultModel=this._bannerProductService.UpdateBannerProduct(model);
                    resultModel.Messages = new List<string>() { resultModel.Data > 0 ? "Modify success" : "Modify failed" };

                    var opera = string.Format("修改首页轮播banner:{0},操作结果:{1}",JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "广告管理--首页楼层商品-修改");
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

                    var opera = string.Format("添加首页轮播banner:{0},操作结果:{1}",JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "广告管理--首页楼层商品-添加");
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
        public string DeleteBanner(long? bannerId)
        {
            bannerProductModel model = new bannerProductModel();
            if (bannerId.HasValue)
            {
                model.bannerProductId = bannerId.Value;
                var result = this._bannerProductService.DeleteBannerProduct(model).IsValid;
                return result == true ? "Delete success！" : "Delete failed！";
            }

            return "Delete failed！";

        }

        /// <summary>
        /// Banner上下移动Sorts（用于列表排序显示）
        /// </summary>
        /// <param name="bannerId"></param>
        /// <param name="sx">1上移动,2下移动</param>
        /// <returns></returns>
        public string UpdateSorts(long? bannerId, int sx, long Sorts, int IdentityStatus, int BannerPlaceCode)
        {
            bannerProductModel model = new bannerProductModel();
            bannerProductModel modelsx = new bannerProductModel();
            if (bannerId.HasValue)
            {
                //UserInfoModel asuser = UserInfo.GetLoginUserInfo();
                //查询
                SearchbannerProductModel spmodel = new SearchbannerProductModel();
                spmodel.bannerProductId = bannerId.Value;
                spmodel.IdentityStatus = IdentityStatus;
                int total = 0;
                var result1 = this._bannerProductService.GetBannerProduct(spmodel, out total).Data as List<bannerProductModel>;
                
                if (result1 != null && result1.Count > 0)
                {

                    bannerProductModel models = new bannerProductModel();
                    models = result1[0];
                    bannerId = models.bannerProductId;
                    BannerPlaceCode = models.PlaceCode;
                    Sorts = models.Sorts;
                    IdentityStatus = models.IdentityStatus;
                }

                List<bannerProductModel> ds = null;
                if (sx == 1)
                {
                    //获取触发行的上一行数据
                    var resultlist = this._bannerProductService.GetBannerProduct(Sorts, 1, IdentityStatus, bannerId.Value, 1, BannerPlaceCode);
                    ds = resultlist.Data;
                    if (ds != null && ds.Count > 0)
                    {
                        modelsx = ds[ds.Count-1];
                        modelsx.UpdateBy = UserInfo.CurrentUserName;
                        modelsx.UpdateDT = DateTime.Now;
                        var msorts = modelsx.Sorts;
                        modelsx.Sorts = Sorts ;//触发行的上一行,把它的排序减一
                        //this._bannerService.UpdateBanner(smodel);
                        model.Sorts = msorts;//触发行修改的排序
                    }
                    else
                    {
                        return "This is top line";
                    }
                }
                else
                {
                    //获取触发行的下一行数据
                    var resultlist = this._bannerProductService.GetBannerProduct(Sorts, 2, IdentityStatus, bannerId.Value, 1, BannerPlaceCode);
                    ds = resultlist.Data;
                    if (ds != null && ds.Count > 0)
                    {
                        modelsx = ds[0];
                        modelsx.UpdateBy = UserInfo.CurrentUserName;
                        modelsx.UpdateDT = DateTime.Now;
                        var msorts = modelsx.Sorts;
                        modelsx.Sorts = Sorts;//触发行的下一行,把它排序加一
                        //this._bannerService.UpdateBanner(smodel1);
                        model.Sorts = msorts;//触发行修改的排序
                    }
                    else
                    {
                        return "This is last line!";
                    }
                }

                model.bannerProductId = bannerId.Value;
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;

                var result = this._bannerProductService.UpdateSorts(model, modelsx).IsValid;
                return result == true ? "Move success！" : "Move failed！";
            }

            return "Move failed！";
        } 
	}
}