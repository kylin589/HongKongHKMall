using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Results;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Validators.SKU;
using HKTHMall.Services.SKU;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    /// 商品规格属性
    /// </summary>
    [UserAuthorize]
    public class SKU_AttributesController : HKBaseController
    {
        /// <summary>
        /// 规格服务类
        /// </summary>
        private readonly ISKU_AttributesService _SkuAttributesService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iSkuAttributesService"></param>
        public SKU_AttributesController(ISKU_AttributesService iSkuAttributesService)
        {
            _SkuAttributesService = iSkuAttributesService;
        }

        /// <summary>
        /// 首页
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
        public JsonResult List(SearchSKU_AttributesModel searchModel)
        {
            searchModel.IsSKU = 1;
            searchModel.AttributeName = string.IsNullOrEmpty(searchModel.AttributeName) ? null : searchModel.AttributeName.Trim();

            var result = _SkuAttributesService.GetPagingSKU_Attributess(searchModel);

            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加、编辑规格项
        /// </summary>
        /// <param name="id">规格值Id</param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            SKU_AttributesModel model = null;

            if (id.HasValue)
            {
                var result = this._SkuAttributesService.GetSKU_AttributesById(id.Value);
                if (result.Data != null)
                {

                    model = result.Data;
                }
            }
            if (model == null)
            {
                model = new SKU_AttributesModel();

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
        /// 保存规格项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SKU_AttributesModel model)
        {
            if (ModelState.IsValid)
            {
                ResultModel resultModel = null;
                if (model.AttributeId == 0)
                {
                    model.CreateBy = UserInfo.CurrentUserName;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = model.CreateBy;
                    model.UpdateDT = model.CreateDT;
                    model.IsSKU = 1;

                    resultModel = _SkuAttributesService.AddStandardSKU_Attributes(model);

                }
                else
                {
                    model.UpdateBy = UserInfo.CurrentUserName;
                    model.UpdateDT = DateTime.Now;
                    model.IsSKU = 1;
                    resultModel = _SkuAttributesService.UpdateStandardSKU_Attributes(model);
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }

        /// <summary>
        /// 验证规格值是否被使用
        /// </summary>
        /// <param name="valueId"></param>
        /// <returns></returns>
        public ActionResult ValueIsUsed(int valueId)
        {
            ResultModel resultModel = _SkuAttributesService.CheckValueIsUsed(valueId);
            string messsage = resultModel.IsValid ? "This specification is quoted by products, cannot be  deleted！" : "Delete success";
            resultModel.Messages.Add(messsage);
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 规格选择页
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Selector(string paramsData)
        {
            //已选中的规格值等相关集合
            List<Tuple<long, bool>> tupleData = new List<Tuple<long, bool>>();

            if (!string.IsNullOrEmpty(paramsData))
            {

                var arrayParams = paramsData.Split(',');
                if (arrayParams.Length > 0)
                {
                    foreach (var param in arrayParams)
                    {
                        int index = param.IndexOf('&');
                        long attrId = long.Parse(param.Substring(0, index));            //已选中的规格
                        bool isUse = int.Parse(param.Substring(index + 1)) != 0;        //是否可以删除,此处用来是否可以不选中
                        tupleData.Add(new Tuple<long, bool>(attrId, isUse));
                    }
                }

            }
            ViewBag.ValueData = tupleData;
            var result = _SkuAttributesService.GetAllSKU_AttributesBy(true);

            if (Request.IsAjaxRequest())
            {
                return PartialView(result.Data);
            }
            else
            {
                return View(result.Data);

            }
        }

        /// <summary>
        /// 属性值数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ValuesData(long id)
        {
            var result = _SkuAttributesService.GetAttributeValuesById(id);
            List<SKU_AttributeValuesModel> values = null;
            if (result.IsValid)
            {
                values = result.Data;
            }
            if (values == null)
            {
                values = new List<SKU_AttributeValuesModel>();
            }
            var data = values.Select(x => new
            {
                ValueStr = x.ValueStr,
                ImageUrl = string.IsNullOrEmpty(x.ImageUrl) ? "" : ViewBag.RootImage + x.ImageUrl
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}