using HKTHMall.Admin.common;
using HKTHMall.Admin.Models;
using HKTHMall.Domain.AdminModel.Models.banner;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class FloorConfigController : Controller
    {
        private readonly IFloorConfigService _floorConfigService;
        private GetBannerAndBannerProduct con = new GetBannerAndBannerProduct();
        public FloorConfigController(IFloorConfigService floorConfigService)
        {
            _floorConfigService = floorConfigService;
        }
        //
        // GET: /FloorConfig/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchFloorConfigModel model)
        {
            //SearchAC_OperateLogModel logmodel = new SearchAC_OperateLogModel();
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
           


            //查询列表 
            var result = this._floorConfigService.GetFloorConfigList(model);
            List<FloorConfigModel> ds = CateIdStrName(result.Data);
            var data = new
            {

                rows = ds,
                total = result.Data.TotalCount,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 转换分类名称
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public List<FloorConfigModel> CateIdStrName(List<FloorConfigModel> ds)
        {
            if (ds==null)
            {
                return ds;
            }
            List<FloorConfigModel> list = ds;
            foreach (var item in ds)
            {
                List<BannerPlaceCodeModel> listBanner = con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, 2);//这里直接是首页 如有其它请修改0
               var CateIdStrs= item.CateIdStr.Split(',');//把字符串分成数组（一级分类）
               var PlaceCodeNames = string.Empty;
               foreach (var  CateIdStr in CateIdStrs)
               {
                   foreach (var banner in listBanner)
                   {
                       if (CateIdStr==banner.PlaceCode.ToString().Trim())
                       {
                           PlaceCodeNames += banner.PlaceCodeName+"&nbsp;&nbsp;";
                       }
                   }
               }
               item.CateIdStr = PlaceCodeNames;
            }
            return list;
        }

        /// <summary>
        /// Banner加载数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            FloorConfigModel model = new FloorConfigModel();
            
            if (id.HasValue)
            {
                SearchFloorConfigModel smodel = new SearchFloorConfigModel();
                smodel.FloorConfigId = id.Value;
                smodel.PagedIndex = 0;
                smodel.PagedSize = 100;
                //查询列表 
                List<FloorConfigModel> List = this._floorConfigService.GetFloorConfigList(smodel).Data;
                string CateIds=string.Empty;

                if (List != null && List.Count > 0)
                {
                    model = List[0];
                    CateIds = model.CateIdStr; //model.CateIdStr.Split(',');
                }
                ViewData["BannerPlaceCodeModel1"] = null;
                ViewData["BannerPlaceCodeModel1"] = con.GetBannerProducPlaceCodeNameList(ACultureHelper.GetLanguageID, 0, 2);

                ViewBag.CateIds = CateIds;
                
            }
            return PartialView(model);
        }

        /// <summary>
        /// Banner 表新增,修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(FloorConfigModel model)
        {
            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();


                var result = _floorConfigService.UpdateFloorConfig(model);

                resultModel.Messages = new List<string> { result.IsValid == true ? "Layer config success！" : "Layer config failed！" };
                var opera = string.Empty;
                opera += " FloorConfigId:" + model.FloorConfigId + ",CateIdStr:" + model.CateIdStr + ",结果:" + result;
                LogPackage.InserAC_OperateLog(opera, "广告管理-楼层配置-楼层配置修改");

                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }
	}
}