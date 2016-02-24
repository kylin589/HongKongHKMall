using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Admin.Models;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Enum;
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
    public class BannerAppFloorController : HKBaseController
    {
        private IbannerService _bannerService;
        private IbannerProductService _bannerProductService;
        private GetBannerAndBannerProduct con = new GetBannerAndBannerProduct();
        
        public static readonly string ImagePath = HKSJ.Common.ConfigHelper.GetConfigString("ImagePath");
        public static readonly string ImageHeader = HKSJ.Common.ConfigHelper.GetConfigString("ImageHeader");
        public BannerAppFloorController(IbannerService bannerService, IbannerProductService bannerProductService)
        {
            this._bannerService = bannerService;
            this._bannerProductService = bannerProductService;
        }

        #region Banner 表操作
        /// <summary>
        /// Banner首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? IdentityStatus)
        {
            
            if (!IdentityStatus.HasValue)
            {
                IdentityStatus = (int)EIdentityStatus.AppFloor;
            }
            //商品一级分类

            ViewBag.BannerPlaceCodeModel = new SelectList(con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, IdentityStatus.Value), "PlaceCode", "PlaceCodeName");
            
            //图片地址的前段地址,http://198.168.1.1
            ViewBag.ImagePath = ImagePath;

            ViewBag.titleName = con.GetIdentityStatusName( IdentityStatus.Value );
            ViewBag.dentityStatus = IdentityStatus.Value;
            return View();
        }

        /// <summary>
        /// 列表Banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchbannerModel model)
        {
            //SearchAC_OperateLogModel logmodel = new SearchAC_OperateLogModel();
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            //标识ID 1首页轮播banner,2首页楼层banner,3 分类频道轮播banner,4分类频道楼层banner
            model.IdentityStatus = model.IdentityStatus <=0 ? 1 : model.IdentityStatus;


            //查询banner图片表
            var result = this._bannerService.GetBanner(model);
            List<bannerModel> ds = result.Data;
            var data = new
            {

                rows = ds,
                total = result.Data.TotalCount,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Banner加载数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BannerCreate(long? id, int identityStatus)
        {
            bannerModel model = null;
            ViewBag.ImagePath = ImagePath;
            if (id.HasValue)
            {
                ResultModel result = this._bannerService.GetBannerById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new bannerModel();
                model.IdentityStatus = identityStatus;//Add控制位置是否显示出来
            }
            ViewData["BannerPlaceCodeModel1"] = null;
            if (identityStatus == (int)EIdentityStatus.AppFloor)
            {
                ViewData["BannerPlaceCodeModel1"] = con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, identityStatus);
            }

            //ViewData["IdentityModelBannerCreate"] = con.GetIdentityNameList(new int[] { 1 });
            return PartialView(model);
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
        [UserAuthorize]

        public ActionResult BannerCreate(bannerModel model)
        {
            ViewData["BannerPlaceCodeModel1"] = null;
            ViewBag.ImagePath = ImagePath;
            var opera = string.Empty;
            if (model.IdentityStatus == (int)EIdentityStatus.AppFloor)
            {
                ViewData["BannerPlaceCodeModel1"] = con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, model.IdentityStatus);
            }
            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();

                if ( model.ProductId > 0)
                {
                    ProductModel modelp = GetBannerProductByProductId(model.ProductId);
                    if (modelp == null)
                    {
                        resultModel.Messages = new List<string>() { "Invalid product ID" };
                        return Json(resultModel, JsonRequestBehavior.AllowGet);
                    }

                }

                if (model.bannerId != 0)
                {
                    model.UpdateBy = UserInfo.CurrentUserName;
                    model.UpdateDT = DateTime.Now;
                    resultModel.Messages = new List<string>() { this._bannerService.UpdateBanner(model).Data > 0 ? "Modify success" : "Modify failed" };
                    opera = string.Format("修改APP首页楼层banner:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "广告管理--APP 首页楼层banner-修改");
                }
                else
                {
                    model.bannerId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.CreateBy = UserInfo.CurrentUserName;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = UserInfo.CurrentUserName;
                    model.UpdateDT = DateTime.Now;
                    model.Sorts = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    resultModel.Messages = new List<string>() { this._bannerService.AddBanner(model).Data != 0 ? "Add success" : "Add failed" };
                    opera = string.Format("添加APP 首页楼层banner:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "APP--APP首页楼层banner-添加");
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
        [UserAuthorize]
        public JsonResult DeleteBanner(long? bannerId)
        {
            bannerModel model = new bannerModel();
            var resultModel = new ResultModel();
            if (bannerId.HasValue)
            {
                model.bannerId = bannerId.Value;
                var result = this._bannerService.DeleteBanner(model).IsValid;
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
       
           [HttpPost]
        public JsonResult UpdateSorts(SortsModel modelst)
        {
            bannerModel model = new bannerModel();
            bannerModel modelsx = new bannerModel();
            var resultModel = new ResultModel();
            resultModel.Messages = new List<string> { "Parameter ID error" };
            resultModel.IsValid = false;
            if (modelst.bannerId >0)
            {
                //UserInfoModel asuser = UserInfo.GetLoginUserInfo();
                List<bannerModel> ds = null;
                //查询
                ResultModel result1 = this._bannerService.GetBannerById(modelst.bannerId);
                if (result1.Data != null)
                {
                    bannerModel models = new bannerModel();
                    models = result1.Data;
                    modelst.bannerId = models.bannerId;
                    modelst.BannerPlaceCode = models.PlaceCode;
                    modelst.Sorts = models.Sorts;
                    modelst.IdentityStatus = models.IdentityStatus;
                }
                if (modelst.sx == 1)
                {
                    //获取触发行的下一行数据
                    var resultlist = this._bannerService.GetBanner(modelst.Sorts, 1, modelst.IdentityStatus, modelst.bannerId, modelst.BannerPlaceCode);
                    ds = resultlist.Data;
                    if (ds != null && ds.Count > 0)
                    {
                        modelsx = ds[0];
                        modelsx.UpdateBy = UserInfo.CurrentUserName;
                        modelsx.UpdateDT = DateTime.Now;
                        var models = modelsx.Sorts;
                        modelsx.Sorts = modelst.Sorts;//
                        //this._bannerService.UpdateBanner(smodel);
                        model.Sorts = models;//触发行修改的排序
                    }
                    else
                    {

                        resultModel.Messages = new List<string> { "This is last line!！" };
                        return Json(resultModel, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    //获取触发行的上一行数据
                    var resultlist = this._bannerService.GetBanner(modelst.Sorts, 2, modelst.IdentityStatus, modelst.bannerId, modelst.BannerPlaceCode);
                    ds = resultlist.Data;
                    if (ds != null && ds.Count > 0)
                    {
                        modelsx = ds[ds.Count - 1];//获取最后一行
                        modelsx.UpdateBy = UserInfo.CurrentUserName;
                        modelsx.UpdateDT = DateTime.Now;
                        var models = modelsx.Sorts;
                        modelsx.Sorts = modelst.Sorts;//
                        //this._bannerService.UpdateBanner(smodel1);
                        model.Sorts = models;//触发行修改的排序
                    }
                    else
                    {

                        resultModel.Messages = new List<string> { "This is top line！" };
                        return Json(resultModel, JsonRequestBehavior.AllowGet);
                    }
                }

                model.bannerId = modelst.bannerId;
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;

                var result = this._bannerService.UpdateSorts(model, modelsx).IsValid;
                if (result)
                {
                    resultModel.Messages = new List<string> { "Move success！" };
                }
                else
                {
                    resultModel.Messages = new List<string> { "Move failed！" };
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}