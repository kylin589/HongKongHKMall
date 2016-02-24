using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Core;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Domain.Models;
using HKTHMall.Services.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class NewInfoController : Controller
    {
        private INewInfoService _newInfoService;
        public static readonly string ImagePath = HKSJ.Common.ConfigHelper.GetConfigString("ImagePath");
        public static readonly string ImageHeader = HKSJ.Common.ConfigHelper.GetConfigString("ImageHeader");

        public NewInfoController(INewInfoService newInfoService)
        {

            this._newInfoService = newInfoService;

        }

        /// <summary>
        /// 新闻信息 wuyf
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.ImagePath = ImagePath;
            return View();
        }

        /// <summary>
        /// 列表Banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchNewInfoModel model)
        {

            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;



            //列表查询
            var result = this._newInfoService.GetNewInfoList(model);
            List<NewInfoModel> ds = result.Data;
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
            ViewBag.RootImage = ImagePath;
            NewInfoModel model = new NewInfoModel();
            if (id.HasValue)
            {
                ResultModel result = _newInfoService.GetNewsById(id.Value);
                if (result != null)
                {
                    model = result.Data; 
                }
            }
            return PartialView(model);
        }

        /// <summary>
        ///  新增,修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(NewInfoModel model)
        {
            var opera = string.Empty;

            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();
                if (model.NewInfoId != 0)
                {
                    model.UpdateBy = UserInfo.CurrentUserName;//asuser.UserName;
                    model.UpdateDT = DateTime.Now;
                    resultModel = this._newInfoService.UpdateNewInfos(model);
                    resultModel.Messages = new List<string>() { resultModel.Data > 0 ? "Modify success" : "Modify failed" };
                    opera = string.Format("修改新闻:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "企业信息--新闻信息-修改");
                }
                else
                {
                    model.NewInfoId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.CreateBy = UserInfo.CurrentUserName;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = UserInfo.CurrentUserName;
                    model.UpdateDT = DateTime.Now;
                    model.IsRecommend = 0;
                    

                    if (model.NewImage != null && model.NewImage.Trim() == ImagePath.Trim())
                    {
                        model.NewImage = null;//前台会根据空值,加载默认图片
                    }
                    resultModel = this._newInfoService.AddNewInfo(model);
                    resultModel.Messages = new List<string>() { resultModel.Data != 0 ? "Add success" : "Add failed" };

                    opera = string.Format("添加新闻:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "企业信息--新闻信息-添加");
                }
                if (resultModel.IsValid)
                {
                    Response.Redirect("/NewInfo/Index");
                }
                else
                {
                    ViewBag.Messages = resultModel.Messages;
                    return View(model);
                }
                //return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return View(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="NewInfoId"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult DeleteBanner(long? NewInfoId)
        {
            NewInfoModel model = new NewInfoModel();
            var resultModel = new ResultModel();
            if (NewInfoId.HasValue)
            {
                model.NewInfoId = NewInfoId.Value;
                var result = this._newInfoService.DeleteNewInfo(model).IsValid;
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
        /// 修改是否推荐
        /// </summary>
        /// <param name="NewInfoId"></param>
        /// <returns></returns>
        [HttpPost]
        //[UserAuthorize]
        public JsonResult UpdateIsRecommend(long? NewInfoId, int IsRecommend)
        {
            NewInfoModel model = new NewInfoModel();

            var resultModel = new ResultModel();
            if (NewInfoId.HasValue)
            {
                model.NewInfoId = NewInfoId.Value;
                model.IsRecommend = IsRecommend;
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;
                var result = this._newInfoService.UpdateNewInfo(model).IsValid;
                if (result)
                {
                    resultModel.IsValid = true;
                    if (IsRecommend==1)
                    {
                        resultModel.Messages = new List<string> { "Recommend success" };
                    }
                    else
                    {
                        resultModel.Messages = new List<string> { "Not Recommend success" };
                    }
                }
                else
                {
                    resultModel.IsValid = false;
                    if (IsRecommend == 1)
                    {
                        resultModel.Messages = new List<string> { "Recommend failed" };
                    }
                    else
                    {
                        resultModel.Messages = new List<string> { "Not Recommend failed" };
                    }
                    
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
    }
}