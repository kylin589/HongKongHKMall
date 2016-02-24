using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Users;
using HKTHMall.Domain.Models.User;
using HKTHMall.Services.AC.Impl;
using HKTHMall.Services.Sys;
using HKTHMall.Services.AC;
using HKTHMall.Core;
using System.Security.Cryptography;
using System.Web.Security;
using HKTHMall.Services.Products;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.Models.Bra;
using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Core.Utils;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class Brand_CategoryController : HKBaseController
    {
        private ICategoryService _categoryService;
        private IBrand_CategoryService _brand_CategoryService;

        public Brand_CategoryController(ICategoryService categoryService, IBrand_CategoryService brand_CategoryService)
        {
            this._categoryService = categoryService;
            this._brand_CategoryService = brand_CategoryService;
        }

        /// <summary>
        /// 品牌关联列表页
        /// zhoub 20150708
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int languageId, int brandID, string brandName)
        {
            ViewData["brandID"] = brandID;
            ViewData["languageId"] = ACultureHelper.GetLanguageID;
            ViewData["brandName"] = brandName;
            return View();
        }

        /// <summary>
        /// 商品分类级联json数据获取
        /// zhoub 20150708
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public JsonResult GetParentList(int languageId, int parentId)
        {
            var list = _categoryService.GetCategoriesByParentId(ACultureHelper.GetLanguageID, parentId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 添加品牌关联保存
        /// zhoub 20150709
        /// </summary>
        /// <returns></returns>
        public JsonResult AddBrand_Category(int? brandID, int? categoryId)
        {
            Brand_CategoryModel model = new Brand_CategoryModel();
            ResultModel resultModel = new ResultModel();
            if (brandID.HasValue && categoryId.HasValue)
            {
                model.Brand_CategoryId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                model.BrandID = (int)brandID;
                model.CategoryId = (int)categoryId;
                model.AddUser = UserInfo.CurrentUserName;
                model.AddDate = DateTime.Now;
                model.IsEnable = true;
                var result = this._brand_CategoryService.AddBrand_Category(model);
                if (result.IsValid)
                {
                    //添加品牌关联成功
                    resultModel.Messages = new List<string>() { "Add brand association success" };
                }
                else
                {
                    //该品牌关联已经存在.
                    resultModel.Messages = new List<string>() { "The brand association already exists." };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string>() { "Key ID error" };
            }
            string opera = string.Format("品牌关联添加:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "Product--Brands-Add");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 品牌关联列表页
        /// zhoub 20150707
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public JsonResult Brand_CategoryList(SearchBrandModel searchModel)
        {
            var result = _brand_CategoryService.GetPagingBrand_Category(searchModel);

            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount,
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除品牌关联
        /// zhoub 20150709
        /// </summary>
        /// <param name="ParamenterID">用户Id</param>
        /// <returns></returns>
        public JsonResult DeleteBrand_Category(string ParamenterID,string brandID)
        {
            ResultModel resultModel = new ResultModel();
            if (!string.IsNullOrEmpty(ParamenterID) && !string.IsNullOrEmpty(brandID))
            {
                var result = this._brand_CategoryService.DeleteBrand_Category(ParamenterID, brandID);
                resultModel.IsValid = result.IsValid;
                resultModel.Messages =result.Messages as List<string>;
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string>() { "Key ID error" };
            }
            string opera = string.Format("品牌关联删除:{0},操作结果:{1}", "{ParamenterID:" + ParamenterID + ",brandID:" + brandID + "}", resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "Product--Brands-Delete");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
    }
}