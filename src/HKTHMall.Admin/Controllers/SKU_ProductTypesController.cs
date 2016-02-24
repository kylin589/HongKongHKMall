using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Models;
using HKTHMall.Services.SKU;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    /// 商品类型控制器
    /// </summary>
    [UserAuthorize]
    public class SKU_ProductTypesController : HKBaseController
    {
        /// <summary>
        /// 商品类型服务类
        /// </summary>
        private readonly ISKU_ProductTypesService _SKU_ProductTypesService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="skuProductTypesService"></param>
        public SKU_ProductTypesController(ISKU_ProductTypesService skuProductTypesService)
        {
            _SKU_ProductTypesService = skuProductTypesService;
        }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public JsonResult List(SearchSKU_ProductTypesModel searchModel)
        {
            searchModel.Name = string.IsNullOrEmpty(searchModel.Name) ? null : searchModel.Name;
            var result = _SKU_ProductTypesService.GetPagingSKU_ProductTypes(searchModel);

            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount,
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSku_ProductTypesByCategoryId(int? id)
        {
            ResultModel result = new ResultModel();

            if (id.HasValue)
            {
                result = this._SKU_ProductTypesService.GetSku_ProductTypesByCategoryId(id.Value);
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add("Parameter error");
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(int? id)
        {
            SKU_ProductTypesModel model = null;

            if (id.HasValue)
            {
                var result = this._SKU_ProductTypesService.GetSKU_ProductTypesById(id.Value);
                if (result.Data != null)
                {

                    model = result.Data;
                }
            }
            if (model == null)
            {
                model = new SKU_ProductTypesModel()
                {
                    UseExtend = 1,
                    UseParameter = 1,
                };

            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            else
            {
                return View(model);

            }

        }


        /// <summary>
        /// 保存商品类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SKU_ProductTypesModel model)
        {

            ResultModel resultModel = null;
            model.IsGoods = 1;
            if (model.SkuTypeId == 0)
            {
                model.CreateBy = UserInfo.CurrentUserName;
                model.CreateDT = DateTime.Now;
                model.UpdateBy = model.CreateBy;
                model.UpdateDT = model.CreateDT;
                resultModel = _SKU_ProductTypesService.AddSKU_ProductTypes(model);

            }
            else
            {
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;
                resultModel = _SKU_ProductTypesService.UpdateSKU_ProductTypes(model);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }


    }
}