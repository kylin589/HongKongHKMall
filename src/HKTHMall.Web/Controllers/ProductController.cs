using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.WebModel.Models.Collection;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Services.Products;
using HKTHMall.Services.WebProducts;
using HKTHMall.Services.YHUser;
using HKTMall.Web;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.Models.Bra;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.WebModel.Models.Search;
using HKTHMall.Services.Banner;


namespace HKTHMall.Web.Controllers
{
    public class ProductController : BaseController
    {
        /// <summary>
        /// 商品评价类
        /// </summary>
        private IProductCommentService _ProductCommentService;

        private IMyCollectionService _MyCollectionService;
        private ICategoryService _CategoryService;
        private IBrandService _BrandService;
        private IProductService _ProductService;
        private int langId = CultureHelper.GetLanguageID();
        private IbannerService _IbannerService;

        public ProductController(IProductCommentService ProductCommentService, IMyCollectionService MyCollectionService, ICategoryService CategoryService, IBrandService BrandService, IProductService ProductService, IbannerService bannerService)
        {
            _ProductCommentService = ProductCommentService;
            _MyCollectionService = MyCollectionService;
            _CategoryService = CategoryService;
            _BrandService = BrandService;
            _ProductService = ProductService;
            _IbannerService = bannerService;
        }

        /// <summary>
        /// 产品页面 商品评价
        /// </summary>
        /// <param name="key">商品ID</param>
        /// <param name="commentLevel">评价等级（0全部,1好评,2中评,3差评）</param>
        [HttpPost]
        [Ajax]
        public ActionResult PrdAppraise(long key, int commentLevel = 0, int page = 1)
        {
            SearchSP_ProductCommentModel model = new SearchSP_ProductCommentModel();
            model.ProductId = key;
            model.PagedIndex = page - 1;
            model.PagedSize = 10;
            model.CheckStatus = 2;
            model.CommentLevel = commentLevel;
            model.LanguageID = CultureHelper.GetLanguageID();
            int totalcnt;
            var result = _ProductCommentService.GetProductCommentList(model,out totalcnt);
            List<ProductCommentModel> ds = result.Data;
            foreach (var pc in ds)
            {
                if (pc.IsAnonymous == 1)
                {
                    pc.Phone = pc.Phone.Substring(0, 3) + "***" + pc.Phone.Substring(pc.Phone.Length - 4);
                }
            }
            ViewBag.Page = page;
            ViewBag.Count = totalcnt;
            //if (ds.Count > 0)
            //{
            //    ViewBag.Data = ds;
            //}
            return View(ds);
        }

        public ActionResult CommetList(long key, int commentLevel = 0, int pageIndex = 1, int pageSize = 10)
        {
            SearchSP_ProductCommentModel model = new SearchSP_ProductCommentModel();
            model.ProductId = key;
            model.PagedIndex = pageIndex - 1;
            model.PagedSize = pageSize;
            //model.CheckStatus = 2;
            model.CommentLevel = commentLevel;
            model.LanguageID = CultureHelper.GetLanguageID();
            int totalcnt;
            var result = _ProductCommentService.GetProductCommentList(model,out totalcnt);
            List<ProductCommentModel> ds = result.Data;
            if (result.Data.Count > 0)
            {
                foreach (var pc in ds)
                {
                    if (pc.IsAnonymous == 1 && !string.IsNullOrEmpty(pc.Email))
                    {
                        //pc.Phone = pc.Phone.Substring(0, 3) + "***" + pc.Phone.Substring(pc.Phone.Length - 4);
                        var a=pc.Email.Split('@');
                        pc.Email = pc.Email.Replace(a[0], "***");
                    }
                }
            }
            ViewBag.ProductCommetCount = totalcnt;

            return PartialView("_CommetList", ds);
        }

        public ActionResult _CommetLabels(long productId)
        {
            List<CommentLablesList> list = CommentLablesList.ReturnList(_ProductCommentService.GroupLabelsByProduct(productId));
            return PartialView(list);
        }


        /// <summary>
        /// 产品页面 加入收藏
        /// </summary>
        /// <param name="key">商品ID</param>
        /// <param name="userId">用户ID</param>
        [Authorize]
        public ActionResult AddToCollection(long key, long userId = 0)
        {
            var msg = "";
            var tag = false;
            if (key <= 0)
            {
                msg = "PRODUCT_PLS_CHOOSE_PRODUCTS";//请选择商品！
                return Json(new { Data = false, Msg = msg }, JsonRequestBehavior.AllowGet);
            }
            FavoritesModel model = new FavoritesModel();
            model.FavoritesID = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
            model.UserID = base.UserID;
            model.ProductId = key;
            model.FavoritesDate = DateTime.Now;
            return Json(new { Data = _MyCollectionService.Add(model).IsValid, Msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 产品页面 刷新收藏
        /// </summary>
        /// <param name="key">商品ID</param>
        /// <param name="userId">用户ID</param>
        [Authorize]
        public ActionResult Iscollected(long key, long userId = 0)
        {
            var msg = "";
            //var tag = false;
            if (key <= 0)
            {
                msg = CultureHelper.GetLangString("PRODUCT_PLS_CHOOSE_PRODUCTS");//请选择商品！
                return Json(new { Data = false, Msg = msg }, JsonRequestBehavior.AllowGet);
            }
            FavoritesModel model = new FavoritesModel();
            model.UserID = base.UserID;
            model.ProductId = key;

            return Json(new { Data = _MyCollectionService.Find(model).IsValid, Msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Error()
        {
            return View();
        }



        /// <summary>
        /// 列表页zzr
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductList(int parentId=0, string cateName = "", int selectCategoryId = 973841123, int level = 3, int brandId = 0, string key = "")
        {
            if (parentId == 0 && key=="")
            {
                return View("~/Views/Home/NoProduct.cshtml");
            }
            ViewBag.ParentId = parentId;
            int langId = CultureHelper.GetLanguageID();
            //有搜索条件
            if (key != "")
            {
                #region 有搜索条件
                ViewBag.cateName = key;
                SearchBrandProductModel searchModel = new SearchBrandProductModel();
                searchModel.langId = langId;
                searchModel.ProductName = key;
                searchModel.categoryId = 0;
                searchModel.level = 0;
                searchModel.brandId = 0;
                int totalCount = 0;
                var result = _ProductService.SearchCategoryBrandProduct(searchModel, ref totalCount);
                ViewBag.key = "";
                //ViewBag.isSearch = "true";
                ViewBag.productList = result.Data;
                if (result.Data.Count == 0)
                {
                    ViewBag.isSearch = "true";                   
                    ViewBag.searchName = searchModel.ProductName;
                }
                else
                {
                    ViewBag.isSearch = "false";
                }
                #endregion
            }
            else
            {
                #region 从首页的分类中跳转
                var cateInfo = _CategoryService.GetCateById(parentId, langId).Data;
                ViewBag.cateName = cateInfo.Count>0?cateInfo[0].CategoryName:"";
                ViewBag.search = "false";
                var result = _CategoryService.GetCategoriesByCategoryToTree(langId, parentId);
                if (result.Data != null)
                {
                    if (brandId > 0)
                    {
                        var resultBrand=_BrandService.GetThirdCategoryId(parentId, brandId);
                        selectCategoryId = (int)resultBrand.Data;                   
                    }
                    ViewBag.categoryList = result.Data;
                }
             
                ViewBag.categoryId = selectCategoryId;
                ViewBag.level = level;
               
                ViewBag.brandId = brandId;
                #endregion
            }
            ///获取推广广告
            var bannerList=_IbannerService.GetTopBanner(1000, 8, 0).Data;
            ViewBag.RecList = bannerList;
            ViewBag.key = key;
            return View();
        }
        /// <summary>
        /// 根据分类ID获取品牌
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public ActionResult GetBrand(int categoryId = 0, int level = 0,int brandId=0)
        {
            int langId = CultureHelper.GetLanguageID();
            List<BrandModel> brandList = _BrandService.GetBrandByCategoryId(categoryId, langId, level).Data;
            ViewBag.brandList = brandList;
            ViewBag.categoryId = categoryId;
            ViewBag.brandId = brandId;
            ViewBag.level = level;
            return View();
        }
        /// <summary>
        /// 客户最终购买
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public ActionResult UserBuy(int categoryId = 0, int level = 0, string key = "")
        {
            int langId = CultureHelper.GetLanguageID();
            SearchBrandProductModel model = new SearchBrandProductModel();
            if (key != "")
            {
                model.ProductName = key;
                model.pageSize = 10;
            }
            else
            {
                model.pageSize = 6;
                model.categoryId = categoryId;
                model.level = level;
            }
            model.pageIndex = 1;
            model.langId = langId;
            model.status = 4;
            int totalCount = 10;
            var result = _ProductService.SearchCategoryBrandProduct(model, ref totalCount);
            ViewBag.buyList = result.Data;
            return View();
        }
        /// <summary>
        /// 产品排序及筛选条件
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="brandID"></param>
        /// <returns></returns>
        public ActionResult _ProductCondition()
        {
            return View();
        }
        /// <summary>
        /// 搜索无产品的页面
        /// </summary>
        /// <param name="isSearch"></param>
        /// <returns></returns>
        public ActionResult _NoProduct_Search(string searchName)
        {
            var tuJian = _ProductService.GetTopRecommend(langId, Convert.ToInt64(0), 5);
            ViewBag.tuJian = tuJian.Data;
            ViewBag.searchName = searchName;
            return View();
        }
   
        /// <summary>
        /// 产品列表刷新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult _Products(SearchBrandProductModel model)
        {
            if (model != null)
            {
                int langId = CultureHelper.GetLanguageID();
                int totalCount = 0;
                
                SearchBrandProductModel searchModel = model;
                searchModel.langId = langId;
                if (!string.IsNullOrEmpty(model.ProductName))
                {
                    searchModel.categoryId = 0;
                    searchModel.level = 0;
                    searchModel.brandId = 0;
                    var result = _ProductService.SearchCategoryBrandProduct(searchModel, ref totalCount);
                    ViewBag.key = "";
                    //ViewBag.isSearch = "true";
                    ViewBag.productList = result.Data;
                    if (result.Data.Count == 0)
                    {
                        ViewBag.isSearch = "true";
                        var tuJian = _ProductService.GetTopRecommend(langId, Convert.ToInt64(0), 4);
                        ViewBag.tuJian = tuJian.Data;
                        ViewBag.searchName = searchModel.ProductName;                       
                    }
                    int totalPage = (int)Math.Ceiling(Convert.ToDouble(totalCount) / Convert.ToDouble(model.pageSize));
                    ViewBag.pageTotal = totalPage;
                    ViewBag.productCount = result.Data.Count;
                   // ViewBag.isSearch = "true";
                }                
                else if(model.brandId!=0||(model.categoryId!=0&&model.level!=0))
                {
                    var result = _ProductService.SearchCategoryBrandProduct(searchModel, ref totalCount);
                    ViewBag.isSearch = "false";
                    ViewBag.productCount = result.Data.Count;
                    int totalPage = (int)Math.Ceiling(Convert.ToDouble(totalCount) / Convert.ToDouble(model.pageSize));
                    ViewBag.pageTotal = totalPage;                   
                    ViewBag.brandId = model.brandId;
                    ViewBag.categoryId = model.categoryId;
                    ViewBag.level = model.level;
                    ViewBag.pageIndex = model.pageIndex;
                    ViewBag.pageTotal = totalPage;
                    ViewBag.productList = result.Data;
                    ViewBag.key = model.ProductName;
                    ViewBag.startPrice = model.startPrice;
                    ViewBag.endPrice = model.endPrice;
                    ViewBag.sortSell = model.sortSell;
                    ViewBag.sorPrice = model.startPrice;
                }
            }
            return View();
        }
    }
}