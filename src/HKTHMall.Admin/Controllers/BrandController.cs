using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HKSJ.Common;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.UploadFile;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Bra;
using HKTHMall.Services.Products;
using HKTHMall.Admin.common;
using HKTHMall.Core.Utils;
using HKTHMall.Admin.Filters;
using HKTHMall.Domain.Enum;
using System.Web;
using HKSJ.Common.FastDFS;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class BrandController : HKBaseController
    {
        public static readonly string imagePath = ConfigHelper.GetConfigString("ImagePath");
        private readonly IBrandService _brandService;
        private readonly UploadFile uf = new UploadFile();

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET: 
        public ActionResult Index()
        {
            return View();
        }

        #region 查询商品品牌

        /// <summary>
        ///     查询商品品牌
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchBrandModel model)
        {
            try
            {
                model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
                var list =
                    _brandService.Select(
                        new SearchBrandModel
                        {
                            PagedIndex = model.PagedIndex,
                            PagedSize = model.PagedSize,
                            BrandName = model.BrandName,
                            BrandState = model.BrandState,
                            LanguageID = ACultureHelper.GetLanguageID
                        });
                var data = new { rows = list.Data, total = list.Data.TotalCount };
                var json = Json(data, JsonRequestBehavior.AllowGet);
                return json;
            }
            catch (Exception ex)
            {
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除商品品牌

        /// <summary>
        ///     删除商品品牌
        /// </summary>
        /// <param name="ParamenterID">系统参数Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(int? brandId)
        {
            var resultModel = new ResultModel();
            if (brandId.HasValue)
            {
                var result = _brandService.Delete(brandId.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete brand success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete brand failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            var opera = string.Format("删除商品品牌 brandId:{0},操作结果:{1}", brandId, resultModel.IsValid ? "成功" : "失败");
            LogPackage.InserAC_OperateLog(opera, "商品管理--品牌管理");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建商品品牌

        /// <summary>
        ///     创建商品品牌
        /// </summary>
        /// <param name="model">系统参数对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(BrandModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                var name =UserInfo.CurrentUserName;
                if (model.BrandID != 0)
                {
                    model.Brand_Lang = new List<Brand_Lang>
                    {
                        new Brand_Lang {BrandID = model.BrandID, BrandName = model.ZhBrandName, LanguageID = (int)LanguageType.zh_CN},
                        new Brand_Lang {BrandID = model.BrandID, BrandName = model.EnBrandName, LanguageID = (int)LanguageType.en_US},
                        new Brand_Lang {BrandID = model.BrandID, BrandName = (model.TaiBrandName==null?"":model.TaiBrandName), LanguageID = (int)LanguageType.th_TH},
                        new Brand_Lang {BrandID = model.BrandID, BrandName = model.HongkongBrandName, LanguageID = (int)LanguageType.zh_HK}//add by liujc
                    };
                    model.UpdateBy = name;
                    model.UpdateDT = DateTime.Now;
                    var result = _brandService.Update(model);
                    resultModel.Messages = new List<string>
                    {
                        result.Data > 0 ? "Modify brand success" : "Modify brand failed"
                    };
                    var opera = string.Format("修改商品品牌:BrandID={0},操作结果:{1}", model.BrandID, result.Data > 0 ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "商品管理--品牌管理");
                }
                else
                {
                    model.AddUsers = name;
                    model.AddTime = DateTime.Now;
                    model.UpdateBy = name;
                    model.UpdateDT = DateTime.Now;

                    //1中文
                    var brandLangList = new List<Brand_Lang>();
                    var bl = new Brand_Lang();
                    bl.BrandName = model.ZhBrandName;
                    bl.LanguageID = (int)LanguageType.zh_CN;
                    brandLangList.Add(bl);
                    //2英语
                    bl = new Brand_Lang();
                    bl.BrandName = model.EnBrandName;
                    bl.LanguageID = (int)LanguageType.en_US;
                    brandLangList.Add(bl);
                    //3泰语
                    bl = new Brand_Lang();
                    bl.BrandName = (model.TaiBrandName==null?"":model.TaiBrandName);
                    bl.LanguageID = (int)LanguageType.th_TH;
                    brandLangList.Add(bl);

                    //add by liujc
                    bl = new Brand_Lang();
                    bl.BrandName = model.HongkongBrandName;
                    bl.LanguageID = (int)LanguageType.zh_HK;
                    brandLangList.Add(bl);

                    model.Brand_Lang = brandLangList;
                    var resut = _brandService.Add(model).IsValid;
                    resultModel.Messages = new List<string> { resut ? "Add brand success" : "Add brand failed" };
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            DrowList(null);
            return PartialView(model);
        }

        #endregion

        #region 加载数据

        /// <summary>
        ///     加载数据
        /// </summary>
        /// <param name="id">对象Id</param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            BrandModel model = null;
            var result = new ResultModel();
            if (id.HasValue)
            {
                result = _brandService.GetBrandById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data[0];
                    //if (model.Brand_Lang != null && model.Brand_Lang.Count == 3) del by liujc
                    if (model.Brand_Lang != null)
                    {
                        foreach (var item in model.Brand_Lang)
                        {
                            //update by liujc
                            if (item.LanguageID == (int)LanguageType.zh_CN)
                            {
                                model.ZhBrandName = item.BrandName;
                            }
                            if (item.LanguageID == (int)LanguageType.en_US)
                            {
                                model.EnBrandName = item.BrandName;
                            }
                            if (item.LanguageID == (int)LanguageType.th_TH)
                            {
                                model.TaiBrandName = item.BrandName;
                            }
                            if(item.LanguageID == (int)LanguageType.zh_HK)
                            {
                                model.HongkongBrandName = item.BrandName;
                            }
                        }
                    }
                    ViewBag.ImgUrl =  model.BrandUrl;
                    ViewBag.ShowImgUrl = imagePath + model.BrandUrl;
                }
            }
            else
            {
                model = new BrandModel();
            }

            DrowList(model);
            return PartialView(model);
        }

        #endregion

        private void DrowList(BrandModel model)
        {
            //状态
            var states = new Dictionary<int, string>();
            states.Add(1, "Enable");
            states.Add(2, "Disable");
            var list = new List<SelectListItem>();
            if (states != null)
            {
                foreach (var item in states)
                {
                    var info = new SelectListItem();
                    if (model != null && model.BrandState != 0)
                    {
                        if (model.BrandState == item.Key)
                        {
                            info.Selected = true;
                        }
                    }
                    else
                    {
                        if (item.Key == 1)
                        {
                            info.Selected = true;
                        }
                    }
                    info.Value = item.Key.ToString();
                    info.Text = item.Value;
                    list.Add(info);
                }
            }
            ViewData["states"] = list;
        }

        /// <summary>
        ///     上传图片（商品）
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
                //System.Drawing.Image image = System.Drawing.Image.FromStream(mes);
                //int H = image.Height;
                //int W = image.Width;
                string fileName = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, bt, "jpg");
                //fileName = new UploadFile().GetThumbsImage(fileName, 660, 600);
                return fileName;
            }
        }

        #region 更新商品品牌状态
        /// <summary>
        /// 更新商品品牌状态
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateState(int? brandID,int type, int brandState)
        {
            var resultModel = new ResultModel();
            if (brandID.HasValue)
            {
                var result = this._brandService.UpdateState(brandID.Value, brandState).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string>() { type == 1 ? "Enable success" : "Disable success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string>() { "Failed" };
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// 根据分类Id获取品牌
        /// </summary>
        /// <param name="id">分类Id</param>
        /// <returns>品牌列表</returns>
        public JsonResult GetBrandByCategoryById(int? id)
        {
            var result = new ResultModel();
            if (id.HasValue)
            {
                 result = this._brandService.GetBrandByCategoryId(id.Value, ACultureHelper.GetLanguageID);
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add("Parameter error");
            }
           
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}