using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Services.Products;
using HKTHMall.Admin.common;
using HKTHMall.Core.Utils;
using HKTHMall.Core.UploadFile;
using HKSJ.Common;
using HKTHMall.Admin.Filters;
using System.Web;
using HKSJ.Common.FastDFS;


namespace HKTHMall.Admin.Controllers
{

    /// <summary>
    /// 产品图
    /// <remarks>added by jimmy,2015-7-27</remarks>
    /// </summary>
    [UserAuthorize]
    public class ProductImageController : HKBaseController
    {
        public static readonly string imagePath = ConfigHelper.GetConfigString("ImagePath");
        private readonly UploadFile uf = new UploadFile();
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 查询产品图列表

        /// <summary>
        /// 查询产品图列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchProductImageModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var list =
                _productImageService.Select(model);
            var data = new { rows = list.Data, total = list.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除产品图

        /// <summary>
        ///     删除产品图
        /// </summary>
        /// <param name="ParamenterID">产品图Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(long? productImageId)
        {
            var resultModel = new ResultModel();
            if (productImageId.HasValue)
            {
                var result = _productImageService.Delete(productImageId.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete product picture success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete product picture failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            var opera = string.Format("删除产品图 productImageId:{0},操作结果:{1}", productImageId, resultModel.IsValid ? "成功" : "失败");
            LogPackage.InserAC_OperateLog(opera, "企业信息--产品图");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建产品图

        /// <summary>
        ///创建产品图
        /// </summary>
        /// <param name="model">产品图对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ProductImageModel model)
        {
            var admin = UserInfo.CurrentUserName;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (model.ProductImageId != 0)
                {
                    model.UpdateBy = admin;
                    model.UpdateDT = DateTime.Now;
                    model.linkUrl = !string.IsNullOrEmpty(model.linkUrl) && model.linkUrl.Length > 7 && model.linkUrl.Contains("http://") ? model.linkUrl : string.Empty;
                    var result = _productImageService.Update(model);
                    resultModel.Messages = new List<string>
                    {
                       result.Data > 0 ? "Modify product picture success" : "Modify product picture failed"
                    };
                    var opera = string.Format("修改产品图:{0},操作结果:{1}",JsonConverts.ToJson(model), result.Data > 0 ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "企业信息--产品图");

                }
                else
                {
                    model.ProductImageId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.CreateBy = admin;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = admin;
                    model.UpdateDT = DateTime.Now;
                    model.linkUrl = !string.IsNullOrEmpty(model.linkUrl) && model.linkUrl.Length > 7 && model.linkUrl.Contains("http://") ? model.linkUrl : string.Empty;
                    model.place = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    resultModel.Messages = new List<string>
                    {
                        _productImageService.Add(model).Messages.Count == 0 ? "Add product picture success" : "Add product picture failed"
                    };
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DrowList(null);
            }
            return PartialView(model);
        }

        #endregion

        #region 加载数据

        /// <summary>
        ///     加载数据
        /// </summary>
        /// <param name="id">对象系统Id</param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            ProductImageModel model = null;
            if (id.HasValue)
            {
                var result = _productImageService.GetProductImageById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                    model.linkUrl = !string.IsNullOrEmpty(model.linkUrl) ? model.linkUrl : "http://";
                    ViewBag.ImgUrl = model.ImageUrl;
                    ViewBag.ShowImgUrl = imagePath + model.ImageUrl;
                }
            }
            else
            {
                model = new ProductImageModel();
            }
            DrowList(model);
            return PartialView(model);
        }

        #endregion

        #region 上传图片（产品图）
        /// <summary>
        /// 上传图片（产品图）
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
                int H = image.Height;
                int W = image.Width;
                if (H < h1 || W < w1)
                {
                    return "2";
                }
                else
                {
                    string fileName = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, bt, "jpg");
                    //fileName = new UploadFile().GetThumbsImage(fileName, 660, 600);
                    return fileName;
                }
            }
        }
        #endregion

        #region 产品图
        /// <summary>
        /// 产品图
        /// </summary>
        /// <param name="rowId">行数下标</param>
        /// <param name="sortType">1、上移；2、下移</param>
        /// <returns></returns>
        public JsonResult UpdatePlace(long? productImageId, int rowId, int sortType, string productName, int ImageType, int PagedIndex, int PagedSize)
        {
            var resultModel = new ResultModel();
            resultModel.Messages = new List<string> { "Move success." };
            if (productImageId.HasValue)
            {
                List<ProductImageModel> paramList =
               _productImageService.Select(new SearchProductImageModel
               {
                   PagedIndex = PagedIndex,
                   PagedSize = PagedSize,
                   ProductName = productName,
                   ImageType = ImageType
               }).Data;
                if (paramList != null && paramList.Count > 0)
                {
                    switch (sortType)
                    {
                        case 1://上移
                            if (rowId > 0)
                            {
                                _productImageService.UpdatePlace(paramList[rowId].ProductImageId, (int)paramList[rowId - 1].place);
                                _productImageService.UpdatePlace(paramList[rowId - 1].ProductImageId, (int)paramList[rowId].place);
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
                                _productImageService.UpdatePlace(paramList[rowId].ProductImageId, (int)paramList[rowId + 1].place);
                                _productImageService.UpdatePlace(paramList[rowId + 1].ProductImageId, (int)paramList[rowId].place);
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


        /// <summary>
        /// 产品图类型
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>added by jimmy,2015-7-28</remarks>
        private void DrowList(ProductImageModel model)
        {
            //状态
            var states = new Dictionary<int, string>();
            states.Add(1, "Picture");
            states.Add(2, "Video");
            var list = new List<SelectListItem>();
            if (states != null)
            {
                foreach (var item in states)
                {
                    var info = new SelectListItem();
                    if (model != null && model.ImageType != 0)
                    {
                        if (model.ImageType == item.Key)
                        {
                            info.Selected = true;
                        }
                    }
                    info.Value = item.Key.ToString();
                    info.Text = item.Value;
                    list.Add(info);
                }
            }
            ViewData["ImageType"] = list;
        }
    }
}