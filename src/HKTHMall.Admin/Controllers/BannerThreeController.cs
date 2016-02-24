using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models.banner;
using HKTHMall.Services.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HKTHMall.Domain.Models;
using HKTHMall.Admin.common;
using HKTHMall.Core;
using HKTHMall.Domain.Models.User;
using HKTHMall.Admin.Filters;
using Newtonsoft.Json;

using HKTHMall.Core.Utils;
using HKTHMall.Admin.Models;
using HKTHMall.Services.Products;
using HKTHMall.Domain.AdminModel.Models.Products;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class BannerThreeController : HKBaseController
    {
        private IbannerService _bannerService;
        private IbannerProductService _bannerProductService;
        private GetBannerAndBannerProduct con = new GetBannerAndBannerProduct();
        
        public static readonly string ImagePath = HKSJ.Common.ConfigHelper.GetConfigString("ImagePath");
        public static readonly string ImageHeader = HKSJ.Common.ConfigHelper.GetConfigString("ImageHeader");
        public BannerThreeController(IbannerService bannerService, IbannerProductService bannerProductService)
        {
            this._bannerService = bannerService;
            this._bannerProductService = bannerProductService;
        }

        #region Banner 表操作
        /// <summary>
        /// Banner首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()//int? IdentityStatus
        {
            //2015-9-22 不用了，目前是分开的，不合并了
            //ViewData["BannerPlaceCodeModel"] = null;
            //if (IdentityStatus.HasValue && IdentityStatus.Value == 2)
            //{
            //    ViewData["BannerPlaceCodeModel"] = con.GetBannerProducPlaceCodeNameList(CultureHelper.GetLanguageID, 0, IdentityStatus.Value);
            //}
            ViewBag.ImagePath = ImagePath;
            ViewBag.titleName = con.GetIdentityStatusName(7);//IdentityStatus.HasValue == true ? IdentityStatus.Value : 1
            ViewBag.dentityStatus = 7;//IdentityStatus.HasValue == true ? IdentityStatus.Value : 1
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
            List<bannerModel> ds = new List<bannerModel>();
            model.PlaceCode = 0;
            model.IdentityStatus = 7;
            //查询banner图片表
            var result = this._bannerService.GetBanner(model);
            ds = result.Data;
            model.IdentityStatus = 8;
            List<bannerModel> ds2 = this._bannerService.GetBanner(model).Data;
            ds = ds.Union(ds2).ToList();

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
            bannerModel model = null;
            var identityStatus=7;
            ViewBag.ImagePath = ImagePath;//图片前半段地址
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
                model.IdentityStatus = identityStatus;//添加控制位置是否显示出来
            }
           
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
        
        public ActionResult Create(bannerModel model)
        {
            ViewData["BannerPlaceCodeModel1"] = null;
            ViewBag.ImagePath = ImagePath;//图片前半段地址
            var opera = string.Empty;
            //2015-9-22 wuyf IdentityStatus默认是1
            //if (model.IdentityStatus== 2)
            //{
            //    ViewData["BannerPlaceCodeModel1"] = con.GetBannerProducPlaceCodeNameList(CultureHelper.GetLanguageID, 0, model.IdentityStatus);
            //}
            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();
                if ( model.ProductId>0)
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

                    resultModel=this._bannerService.UpdateThreeBanner(model);
                    resultModel.Messages = new List<string>() { resultModel.Data > 0 ? "Modify success" : "Modify failed" };

                    opera = string.Format("修改首页右侧banner:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "广告管理--首页右侧banner-修改");
                }
                else
                {
                    model.bannerId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.CreateBy = UserInfo.CurrentUserName;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = UserInfo.CurrentUserName;
                    model.UpdateDT = DateTime.Now;
                    model.Sorts = MemCacheFactory.GetCurrentMemCache().Increment("commonId");

                    resultModel=this._bannerService.AddBanner(model);
                    resultModel.Messages = new List<string>() { resultModel.Data != 0 ? "Add success" : "Add failed" };
                    opera = string.Format("添加首页右侧banner:{0},操作结果:{1}",JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "广告管理--首页右侧banner-添加");
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            ViewBag.Ts = "有未填项";
            return PartialView(model);
        }

        /// <summary>
        /// 删除Banner
        /// </summary>
        /// <param name="bannerId"></param>
        /// <returns></returns>
        [UserAuthorize]
       public JsonResult Delete(long? bannerId)
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

                var opera = string.Empty;
                
                opera = " bannerId:" + model.bannerId +",结果:" + resultModel.Messages;
                LogPackage.InserAC_OperateLog(opera, "广告管理--首页右侧banner-删除");
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Parameter ID error" };
            }

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 测试编辑器图片上传
        /// </summary>
        /// <returns></returns>
        public ActionResult KindEditor()
        {
            return View();
        }

          /// <summary>
          /// 删除图片
          /// </summary>
          /// <param name="groupName"></param>
          /// <param name="fileName"></param>
          /// <returns></returns>
         
        public JsonResult DeleteImgs(string groupName, string fileName)
        {
            
            var resultModel = new ResultModel();
            try
            {
                //FastDFSClient.RemoveFile(groupName, fileName);
                //resultModel.IsValid = true;
                //resultModel.Messages = new List<string> { "删除成功" };

                //var filePath = Path.Combine(this.Server.MapPath("http://192.168.16.218/pm1/"), fileName);
                //System.IO.File.Delete(filePath);
            }
            catch (Exception ex)
            {

                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { ex.ToString() };
            }
           

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
	}
}