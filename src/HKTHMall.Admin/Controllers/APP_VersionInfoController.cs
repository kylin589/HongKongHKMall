using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Core;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.APP;
using HKTHMall.Domain.Models;
using HKTHMall.Services.APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class APP_VersionInfoController : Controller
    {
         public static readonly string ImagePath = HKSJ.Common.ConfigHelper.GetConfigString("ImagePath");
        public static readonly string ImageHeader = HKSJ.Common.ConfigHelper.GetConfigString("ImageHeader");
        
       
        
        private IAPP_VersionInfoService _aPP_VersionInfoService;

        public APP_VersionInfoController(IAPP_VersionInfoService aPP_VersionInfoService)
        {
            
            this._aPP_VersionInfoService = aPP_VersionInfoService;
            
        }

        //
        //APP版本信息
        public ActionResult Index()
        {
            ViewBag.ImagePath = ImagePath;
            return View();
        }

        /// <summary>
        /// 列表APP_VersionInfo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchAPP_VersionInfoModel model)
        {
            
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            
            var result = this._aPP_VersionInfoService.GetAPP_VersionInfoList(model);
            List<APP_VersionInfoModel> ds = result.Data;
            var data = new
            {

                rows = ds,
                total = ds.Count,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// APP_VersionInfo加载数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            APP_VersionInfoModel model = null;
            ViewBag.IsTrueModel = SelectCommon.GetIsTrueModel();
            ViewBag.GetAppModel = SelectCommon.GetAppModel();
            if (id.HasValue)
            {
                SearchAPP_VersionInfoModel spmodel = new SearchAPP_VersionInfoModel();
                spmodel.ID = id.Value;
                spmodel.PagedIndex = 0;
                spmodel.PagedSize = 100;

                var result = this._aPP_VersionInfoService.GetAPP_VersionInfoList(spmodel).Data as List<APP_VersionInfoModel>;
                
                if (result != null && result.Count > 0)
                {
                    model = result[0];
                }
            }
            else
            {
                model = new APP_VersionInfoModel();
                
            }
            
            return PartialView(model);
        }

        /// <summary>
        /// APP_VersionInfo 表新增,修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(APP_VersionInfoModel model)
        {
            var opera="";
            ViewBag.IsTrueModel = SelectCommon.GetIsTrueModel();
            ViewBag.GetAppModel = SelectCommon.GetAppModel();
            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();

                if (model.ID != 0)
                {
                    model.UpdateBy = UserInfo.CurrentUserName;//asuser.UserName;
                    model.UpdateDT = DateTime.Now;
                    resultModel.Messages = new List<string>() { this._aPP_VersionInfoService.UpdateAPP_VersionInfo(model).IsValid != false ? "Modify success" : "Modify failed" };
                    opera = string.Format("修改APP_VersionInfo:{0},操作结果:{1}",JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "APP_VersionInfo-修改");
                }
                else
                {
                    
                    model.CreateBy = UserInfo.CurrentUserName;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = UserInfo.CurrentUserName;
                    model.UpdateDT = DateTime.Now;


                    resultModel.Messages = new List<string>() { this._aPP_VersionInfoService.AddAPP_VersionInfo(model).IsValid != false ? "Add success" : "Add failed" };
                    opera = string.Format("添加APP_VersionInfo:{0},操作结果:{1}",JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "APP_VersionInfo-添加");
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

        public JsonResult DeleteAPP_VersionInfo(int? id)
        {
            var model = new APP_VersionInfoModel();
            var resultModel = new ResultModel();
            if (id.HasValue)
            {
                model.ID = id.Value;
                var result = this._aPP_VersionInfoService.DeleteAPP_VersionInfo(model).IsValid;
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

                opera = " bannerId:" + model.ID + ",结果:" + resultModel.Messages;
                LogPackage.InserAC_OperateLog(opera, "删除");
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