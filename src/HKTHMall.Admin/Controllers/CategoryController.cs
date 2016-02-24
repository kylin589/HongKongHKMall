using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Products;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class CategoryController : HKBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        //
        // GET: /Category/
        public ActionResult Index()
        {
            return this.View();
        }

        public JsonResult GetCategoryById(int? id)
        {
            var result = new ResultModel();
            if (id.HasValue)
            {
                result = this._categoryService.GetCategoryById(id.Value);
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add("ID is empty");
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 隐藏分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult HideCategory(int? id)
        {
            var result = new ResultModel();
            if (id.HasValue)
            {
                result = this._categoryService.HideCategoryById(id.Value);
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add("ID is empty");
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParentCategoryListByChildernCategoryId(int? id)
        {
            var result = new ResultModel();
            if (id.HasValue)
            {
                result = this._categoryService
                    .GetParentCategoryListByChildernCategoryId(id.Value,
                        ACultureHelper.GetLanguageID);
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add("ID is empty");
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategoryByGrade(int id)
        {
            var result = this._categoryService.GetCategoryByGrade(id, ACultureHelper.GetLanguageID);
            return this.Json(result);
        }

        public JsonResult GetCategoryByParentId(int? id)
        {
            var result = new ResultModel();
            if (id.HasValue)
            {
                result = this._categoryService.GetCategoryByParentId(id.Value, ACultureHelper.GetLanguageID);
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add("Parameter error");
            }

            return this.Json(result);
        }

        public JsonResult GetCategoryTree()
        {
            var result = this._categoryService.GetCategoriesByCategoryToTree(ACultureHelper.GetLanguageID);

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     获取全部分类
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCategoryAll()
        {
            var result = this._categoryService.GetAll(ACultureHelper.GetLanguageID);

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult AddOrUpdate(int? id, int? parentId, int? ParentGrade)
        {
            var model = new CategoryModel();
            if (id.HasValue)
            {
                var result = this._categoryService.GetCategoryById(id.Value);
                if (result.IsValid)
                {
                    List<CategoryModel> models = result.Data;
                    return this.PartialView(models.FirstOrDefault());
                }
            }

            if (parentId.HasValue)
            {
                model.parentId = parentId.Value;
            }

            if (ParentGrade.HasValue)
            {
                model.ParentGrade = ParentGrade.Value;
            }

            return this.PartialView(model);
        }

        [HttpPost]
        public JsonResult Add(AddCategoryModel model)
        {
            model.AddDate = DateTime.Now;
            model.AddUser = UserInfo.CurrentUserName;
            model.Grade = model.ParentGrade + 1;
            if (model.Grade == 3)
            {
                model.CategoryTypeModel = new CategoryTypeModel
                {
                    CategoryTypeId = MemCacheFactory.GetCurrentMemCache().Increment("logId")
                };
            }

            var result = this._categoryService.AddCategory(model);

            var opera = string.Format("添加商品分类:{0},操作结果:{1}", JsonConverts.ToJson(model), result.Messages);
            LogPackage.InserAC_OperateLog(opera, "商品分类-添加");

            return this.Json(result);
        }

        [HttpPost]
        public ActionResult Update(UpdateCategoryModel model)
        {
            model.UpdateBy = UserInfo.CurrentUserName;
            model.UpdateDT = DateTime.Now;

            var result = this._categoryService.UpdateCategory(model);

            var opera = string.Format("修改商品分类:{0},操作结果:{1}", JsonConverts.ToJson(model), result.Messages);
            LogPackage.InserAC_OperateLog(opera, "商品分类-修改");

            return this.Json(result);
        }
    }
}