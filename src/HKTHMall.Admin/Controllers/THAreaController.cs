using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Services.AC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Admin.Filters;
using HKTHMall.Admin.common;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class THAreaController : HKBaseController
    {
        private readonly ITHAreaService _thAreaService;

        public THAreaController(ITHAreaService thAreaService)
        {
            _thAreaService = thAreaService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        /// <summary>
        /// 区域树形数据获取
        /// zhoub 20150709
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTHAreaTree()
        {
            var result = _thAreaService.GetTHAreaByLanguageIdToTree(ACultureHelper.GetLanguageID);

            return this.Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据区域ID获取单个数据
        /// zhoub 20150709
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetTHArea_langByTHAreaID(int id)
        {
            var result = _thAreaService.GetTHArea_langByTHAreaID(id);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加区域信息
        /// zhoub 20150709.update by liujc
        /// </summary>
        /// <returns></returns>
        public JsonResult AddTHArea(int? areaId, string cAreaName, string eAreaName, string tAreaName,string hAreaName, string shortName, int areaType)
        {
            var resultModel = new ResultModel();
            var result = _thAreaService.AddTHArea(areaId.Value, cAreaName, eAreaName, tAreaName, hAreaName, shortName, areaType);
            resultModel.Messages = result.Messages as List<string>;
            string opera = string.Format("区域信息添加:{0},操作结果:{1}", "{areaId:" + areaId + ",cAreaName:" + cAreaName + ",eAreaName:" + eAreaName + ",tAreaName:" + tAreaName + ",hAreaName:" + hAreaName + ",shortName:" + shortName + ",areaType:" + areaType + "}", resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "System--Region-AddTHArea");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改区域信息
        /// zhoub 20150709.update by liujc 增加hAreaName
        /// </summary>
        /// <returns></returns>
        public JsonResult EditTHArea(int? areaId, string cAreaName, string eAreaName, string tAreaName,string hAreaName, string shortName, int areaType)
        {
            var resultModel = new ResultModel();
            if (areaId.HasValue)
            {
                var result = _thAreaService.EditTHArea(areaId.Value, cAreaName, eAreaName, tAreaName, hAreaName, shortName, areaType);
                resultModel.Messages = result.Messages as List<string>;
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Region ID error" };
            }
            string opera = string.Format("区域信息修改:{0},操作结果:{1}", "{areaId:" + areaId + ",cAreaName:" + cAreaName + ",eAreaName:" + eAreaName + ",tAreaName:" + tAreaName + ",hAreaName"+hAreaName + ",shortName:" + shortName + ",areaType:" + areaType + "}", resultModel.IsValid);
            LogPackage.InserAC_OperateLog(opera, "System--Region-EditTHArea");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 删除区域信息
        /// zhoub 20150709
        /// </summary>
        /// <returns></returns>
        public JsonResult DelTHArea(int? thAreaId)
        {
            var resultModel = new ResultModel();
            if (thAreaId.HasValue)
            {
                var result = _thAreaService.DelTHArea(thAreaId.Value);
                resultModel.Messages = result.Messages as List<string>;
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Region ID error" };
            }
            string opera = string.Format("区域信息删除:{0},操作结果:{1}", thAreaId, resultModel.IsValid);
            LogPackage.InserAC_OperateLog(opera, "System--Region-DelTHArea");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
    }
}