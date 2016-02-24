using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.Shipment;
using HKTHMall.Services.Shipment;
using HKTHMall.Services.Sys;
using HKTHMall.Services.AC;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Domain.Models;
using BrCms.Framework.Collections;


namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class ShipmentController : Controller
    {
        private readonly IParameterSetService _parameterSetService;
        private readonly IShipmentService _shipmentService;
        private readonly ITHAreaService _thAreaService;

        public ShipmentController(IShipmentService shipmentService, IParameterSetService parameterSetService, ITHAreaService thAreaService)
        {
            this._shipmentService = shipmentService;
            this._parameterSetService = parameterSetService;
            this._thAreaService = thAreaService;
        }

        // GET: Shipment
        public ActionResult Index()
        {

            return this.View();
        }

        public JsonResult Address(int id)
        {
            return Json(_thAreaService.GetTHAreaByID(1, id).Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Addresssub(int id)
        {
            return Json(_thAreaService.GetTHAreaByParentID(1, id).Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(Paged model)
        {
            var result = this._shipmentService.FareTempPaged(model);
            return this.Json(new { rows = result.Data, total = result.Data.TotalCount }, JsonRequestBehavior.AllowGet);
        }

        public ViewResult AddOrUpdate(long? id)
        {
            YF_FareTempModel model = new YF_FareTempModel();           
            if (id.HasValue)
            {
                model = this._shipmentService.GetFareTemp(id.Value).Data;
            }
            return View(model);
        }

        public ActionResult Add(YF_FareTempModel model)
        {
            try
            {
                if (model.IsFreeShip==2)
                {
                    model.InitialAmount = 0;
                    model.InitialValue = 0;
                    model.AdditiveAmount = 0;
                    model.AdditiveValue = 0;
                    model.IsFreeValue = 0;
                }
                if (model.FareTempID > 0)
                {
                    this._shipmentService.UpdateFareTemp(model);
                }
                else
                {                    
                    this._shipmentService.AddFareTemp(model);
                }
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }           
        }

        public JsonResult SetDefault(int id)
        {
            var result = this._shipmentService.SetDefault(id);
            return this.Json(result);
        }

        public ActionResult Update(YF_FareTemplateModel model)
        {
            var result = this._shipmentService.UpdateShipment(model);
            if (result.IsValid)
            {
                var opera = string.Format("修改运费模板:{0},操作结果:{1}", JsonConverts.ToJson(model), result.Messages);
                LogPackage.InserAC_OperateLog(opera, "运费模板-修改");

                return this.RedirectToAction("Index");
            }
            return this.View("AddOrUpdate", model);
        }

        public JsonResult Delete(IList<int> ids)
        {
            if(ids==null||ids.Count==0)
            {
                 return this.Json( new ResultModel());
            }
            var result = this._shipmentService.DeleteShipment(ids);
            return this.Json(result);
        }


    }
}