using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Services.Products;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;

using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Domain.WebModel.Models.Search;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.Web.Controllers
{
    public class SharedController : BaseController
    {

        private ICategoryService _ICategoryService;
        private readonly IProductSearchListService _IProductSearchListService;
        // GET: Shared
        public SharedController(ICategoryService iCategoryService, IProductSearchListService iProductSearchListService)
        {
            _ICategoryService = iCategoryService;
            _IProductSearchListService = iProductSearchListService;
        }

        /// <summary>
        /// 加载菜单栏目
        /// zhoub 20150831 重写
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 300, VaryByParam = "languageid")]
        public PartialViewResult _PartialMenu(int languageid)
        {
            List<CategoryModel> categoryData = _ICategoryService.GetCategoriesByALL(languageid, 0).Data;   //默认加载泰文
            //zhoub 20150831 更改 之前js异步加载改为首次请求一次性加载
            //List<CategorysModel> cateOne = _ICategoryService.GetCateByPid(0, CultureHelper.GetLanguageID()).Data;
            List<CategorysModel> cateTwo = _ICategoryService.GetWebAll(CultureHelper.GetLanguageID()).Data;
            //ViewBag.CateOne = cateOne;
            ViewBag.CateTwo = cateTwo;
            return PartialView(categoryData);
        }

        public PartialViewResult _Footer()
        {
            KeyWordsSearch model = new KeyWordsSearch();
            model.languageId = CultureHelper.GetLanguageID();
            int count = 0;
            if (GetUserResultModel() != null)
            {
                var result = _IProductSearchListService.GetMyCollectionList(base.UserID, model, out count);
            }          
            ViewBag.Count = count;
            return PartialView();
        }
    }
}