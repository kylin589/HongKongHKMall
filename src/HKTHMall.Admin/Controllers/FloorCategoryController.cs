using System;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Services.Products;
using HKTHMall.Domain.Models;
using System.Collections.Generic;
using HKTHMall.Admin.common;
using HKTHMall.Core.Utils;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class FloorCategoryController : HKBaseController
    {
        private readonly IFloorCategoryService _floorCategoryService;
        private readonly ICategoryService _categoryService;

        public FloorCategoryController(IFloorCategoryService floorCategoryService, ICategoryService categoryService)
        {
            _floorCategoryService = floorCategoryService;
            _categoryService = categoryService;
        }

        //
        // GET:
        public ActionResult Index()
        {
            return View();
        }


        #region 查询楼层显示分类列表

        /// <summary>
        ///     查询楼层显示分类列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchFloorCategoryModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var paramList =
                _floorCategoryService.Select(new SearchFloorCategoryModel
                {
                    PagedIndex = model.PagedIndex,
                    PagedSize = model.PagedSize,
                    CategoryId = model.CategoryId,
                    ThreeCategoryId = model.ThreeCategoryId,
                    LanguageID = ACultureHelper.GetLanguageID,
                    ParentID = model.ParentID
                });
            var data = new { rows = paramList.Data, total = paramList.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除楼层显示分类

        /// <summary>
        ///     删除楼层显示分类
        /// </summary>
        /// <param name="floorCategoryId">楼层显示分类Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(long? floorCategoryId)
        {
            var resultModel = new ResultModel();
            if (floorCategoryId.HasValue)
            {
                var result = _floorCategoryService.Delete(floorCategoryId.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete category display success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            var opera = string.Format("删除楼层显示分类 floorCategoryId:{0},操作结果:{1}", floorCategoryId, resultModel.IsValid ? "成功" : "失败");
            LogPackage.InserAC_OperateLog(opera, "广告管理--楼层显示分类");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建楼层显示分类

        /// <summary>
        ///创建楼层显示分类
        /// </summary>
        /// <param name="model">楼层显示分类对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(FloorCategoryModel model)
        {
            var admin = UserInfo.CurrentUserName;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (model.FloorCategoryId != 0)
                {
                    var result = _floorCategoryService.Update(model);
                    resultModel.Messages = new List<string>
                    {
                        result.Data > 0 ? "Modified floor display classification success" : "Modified floor display classification failure"
                    };
                    string opera = string.Format("修改楼层显示分类:{0},操作结果:{1}",JsonConverts.ToJson(model), result.IsValid ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "广告管理--楼层显示分类");
                }
                else
                {
                    model.FloorCategoryId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.AddUsers = admin;
                    model.AddTime = DateTime.Now;
                    model.Place = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    var result = _floorCategoryService.Add(model);
                    if (result.IsValid)
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { "Add category display success" };
                    }
                    else
                    {
                        resultModel.IsValid = false;
                        resultModel.Messages = new List<string> { result.Messages[0] };
                    }

                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DrowFirstList(null);
                GetCategory(model.DCategoryId);
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
            FloorCategoryModel model = null;
            if (id.HasValue)
            {
                var result = _floorCategoryService.GetFloorCategoryById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;

                    var categoryThress = _categoryService.GetCategoryById(model.CategoryId).Data;
                    ViewBag.categoryThress = categoryThress[0];
                    var categoryTwo = _categoryService.GetCategoryById(categoryThress[0].parentId).Data;
                    ViewBag.CategoryTowId = categoryTwo[0].CategoryId;
                    ViewBag.CategoryTow = this._categoryService.GetCategoriesByParentId(ACultureHelper.GetLanguageID, categoryTwo[0].parentId);
                }
            }
            else
            {
                model = new FloorCategoryModel();
                model.Place = 1;
            }

            DrowFirstList(model);
            return PartialView(model);
        }

        #endregion

        private void DrowFirstList(FloorCategoryModel model)
        {
            var Categories = this._categoryService.GetCategoriesByParentId(ACultureHelper.GetLanguageID, 0);
            //状态
            var states = new Dictionary<int, string>();
            var list = new List<SelectListItem>();
            if (Categories != null)
            {
                foreach (var item in Categories)
                {
                    var info = new SelectListItem();
                    if (model != null)
                    {
                        if (model.DCategoryId == item.CategoryId)
                        {
                            info.Selected = true;
                        }
                    }
                    info.Value = item.CategoryId.ToString();
                    info.Text = item.CategoryName;
                    list.Add(info);
                }
            }
            ViewData["DCategoryIds"] = list;
        }

        #region 请求二级或三级分类查询
        /// <summary>
        /// 请求二级或三级分类查询
        /// </summary>
        /// <param name="parenId"></param>
        /// <returns></returns>
        public JsonResult GetCategory(int? parenId)
        {
            if (parenId.HasValue)
            {
                var Categories = this._categoryService.GetCategoriesByParentId(ACultureHelper.GetLanguageID, parenId.Value);
                return Json(Categories, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 楼层显示分类排序
        /// <summary>
        /// 楼层显示分类排序
        /// </summary>
        /// <param name="floorCategoryId">楼层显示分类排序ID</param>
        /// <param name="rowId">行数下标</param>
        /// <param name="sortType">1、上移；2、下移</param>
        /// <returns></returns>
        public JsonResult UpdatePlace(long? floorCategoryId, int rowId, int sortType, int ParentID, int PagedIndex, int PagedSize)
        {
            var resultModel = new ResultModel();
            resultModel.Messages = new List<string> { "Move success." };
            if (floorCategoryId.HasValue)
            {
                List<FloorCategoryModel> paramList =
                 _floorCategoryService.Select(new SearchFloorCategoryModel
                 {
                     PagedIndex = PagedIndex,
                     PagedSize = PagedSize,
                     ParentID = ParentID,
                     LanguageID = ACultureHelper.GetLanguageID
                 }).Data;
                if (paramList != null && paramList.Count > 0)
                {
                    switch (sortType)
                    {
                        case 1://上移
                            if (rowId > 0)
                            {
                                _floorCategoryService.UpdatePlace(paramList[rowId].FloorCategoryId, (int)paramList[rowId - 1].Place);
                                _floorCategoryService.UpdatePlace(paramList[rowId - 1].FloorCategoryId, (int)paramList[rowId].Place);
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
                                _floorCategoryService.UpdatePlace(paramList[rowId].FloorCategoryId, (int)paramList[rowId + 1].Place);
                                _floorCategoryService.UpdatePlace(paramList[rowId + 1].FloorCategoryId, (int)paramList[rowId].Place);
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
    }
}