using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Domain.WebModel.Models.Search;
using HKTHMall.Services.Products;
using HKTHMall.Services.Common.MultiLangKeys;


namespace HKTHMall.Web.Controllers
{
    public class SearchController : BaseController
    {

        private ICategoryService _ICategoryService;
        private IProductSearchListService _IProductSearchListService;
        private IProductService _IProductService;
        public SearchController(ICategoryService iCategoryService, IProductSearchListService iProductSearchListService,
                                IProductService iProductService)
        {
            _ICategoryService = iCategoryService;
            _IProductSearchListService = iProductSearchListService;
            _IProductService = iProductService;


        }

        // GET: Search
        public ActionResult Index(KeyWordsSearch model)
        {             
            model.languageId = CultureHelper.GetLanguageID();
            List<int> idlist = new List<int>();
            List<CategorysModel> caModel = _ICategoryService.GetCateById(model.cateId, model.languageId).Data;//根据点击的分类获取分类信息
            if(caModel.Count>0)
            {
                if (model.type == "1")//点击的是一级分类
                {
                    ViewBag.yiCateName = caModel[0].CategoryName;//一级分类名称
                    ViewBag.yiCateID = caModel[0].CategoryId;//一级分类ID
                    List<CategorysModel> list1 = _ICategoryService.GetCateByPid(caModel[0].CategoryId, model.languageId).Data;//二级分类                    
                    if (list1.Count > 0)
                    {
                        for (int j = 0; j < list1.Count; j++)
                        {
                            List<CategorysModel> list2 = _ICategoryService.GetCateByPid(list1[j].CategoryId, model.languageId).Data;//三级分类
                            if (list2.Count > 0)
                            {
                                for (int i = 0; i < list2.Count; i++)
                                {
                                    idlist.Add(list2[i].CategoryId);
                                }
                            }
                        }
                    }
                }
                else if (model.type == "2")//点击的是二级分类
                {
                    List<CategorysModel> yimodel = _ICategoryService.GetCateById(caModel[0].parentId, model.languageId).Data;
                    ViewBag.yiCateName = yimodel[0].CategoryName;//一级分类名称             
                    ViewBag.yiCateID = yimodel[0].CategoryId;//一级分类ID
                    ViewBag.erCateName = caModel[0].CategoryName;//二级分类名称
                    ViewBag.erCateID = caModel[0].CategoryId;//二级分类ID
                    List<CategorysModel> list1 = _ICategoryService.GetCateByPid(model.cateId, model.languageId).Data;
                    if (list1.Count > 0)
                    {
                        for (int i = 0; i < list1.Count; i++)
                        {
                            idlist.Add(list1[i].CategoryId);
                        }
                    }
                }
                else if (model.type == "3")
                {
                    List<CategorysModel> ermodel = _ICategoryService.GetCateById(caModel[0].parentId, model.languageId).Data;
                    ViewBag.erCateName = ermodel[0].CategoryName;//二级分类名称    
                    ViewBag.erCateID = ermodel[0].CategoryId;
                    List<CategorysModel> yimodel = _ICategoryService.GetCateById(ermodel[0].parentId, model.languageId).Data;
                    ViewBag.yiCateName = yimodel[0].CategoryName;//一级分类名称    
                    ViewBag.yiCateID = yimodel[0].CategoryId;
                    ViewBag.sanCateName = caModel[0].CategoryName;//三级分类名称
                    ViewBag.sanCateID = caModel[0].CategoryId;//三级分类ID
                    idlist.Add(model.cateId);
                }
                else
                {
                    return RedirectToAction("NotFound", "Home");
                }
                var userModel = GetUserResultModel();
                long userId = 0;
                if (userModel != null)
                {
                    userId = userModel.UserID;
                }
                int[] ilist = idlist.ToArray();
                if (ilist.Length > 0)
                {
                    int count = 0;
                    var result = _IProductSearchListService.GetAllSearchList(model, ilist,userId, out count).Data;
                    model.AllCount = count;
                    ViewBag.products = result;
                }
                ViewBag.type = model.type;
                ViewData.Add("searchModel", model);

                ///获取惠卡推广数据
                var productData = _IProductService.GetTopRecommend(CultureHelper.GetLanguageID());
                List<ProductInfo> productList = productData.Data;
                if (productList.Count > 0)
                {
                    productList.ForEach(a =>
                    {
                        a.Guid = System.Guid.NewGuid().ToString();
                        //处理价格显示促销中的
                        if (a.SalesRuleId > 1 & a.StarDate <= DateTime.Now & a.EndDate >= DateTime.Now)
                        {
                            var price = a.HKPrice;
                            a.MarketPrice = a.HKPrice;
                            a.HKPrice = a.Discount == 0 ? price : price * a.Discount;
                        }
                    });
                    var data = productList.OrderByDescending(a => a.Guid).Take(4);
                    ViewBag.ProductData = data;
                }
            }                      
            else
            {
                return RedirectToAction("NotFound", "Home");
            }           
            return View();
        }
         
    }
}