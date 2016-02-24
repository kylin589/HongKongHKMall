using AutoMapper;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel;
using HKTHMall.Domain.AdminModel.Models.banner;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.AdminModel.Models.Keywork;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.WebModel.Models.Index;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Domain.WebModel.Models.Search;
using HKTHMall.Services.Banner;
using HKTHMall.Services.Keywork;
using HKTHMall.Services.Products;
using HKTHMall.Services.SKU;
using HKTHMall.Services.WebProducts;
using HKTHMall.Services.New;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Domain.Models.banner;
using HKTHMall.Web.Common;

namespace HKTHMall.Web.Controllers
{
    public class HomeController : BaseController
    {
        private IbannerService _IbannerService;
        private ICategoryService _ICategoryService;
        private IProductService _IProductService;
        private IProductSearchListService _IProductSearchListService;
        private IFloorKeywordService _IFloorKeywordService;
        private IProductPicService _IProductPicService;
        private IProductCommentService _IProductCommentService;
        private IbannerProductService _IbannerProductService;
        private IProductRuleService _IProductRuleService;
        private IFloorConfigService _IFloorConfigService;
        private ISKU_ProductService _ISKU_ProductService;
        private ISKU_ProductAttributesService _ISKU_ProductAttributesService;
        private ISKU_SKUItems _ISKU_SKUItems;
        private IProductConsultService _productConsultService;
        private INewInfoService _newInfoService;
        private IBrandService _brandService;
        //
        // GET: /Home/
        public HomeController(IbannerService ibannerService, ICategoryService iCategoryService,
            IProductService iProductService, IProductSearchListService iProductSearchListService,
            IFloorKeywordService iFloorKeywordService, IProductPicService iProductPicService,
            IProductCommentService iProductCommentService, IbannerProductService ibannerProductService,
            IProductRuleService iProductRuleService, IFloorConfigService iFloorConfigService, ISKU_ProductService iSKU_ProductService,
            ISKU_ProductAttributesService iSKU_ProductAttributesService, ISKU_SKUItems iSKU_SKUItems, IProductConsultService productConsultService,
            INewInfoService iNewInfoService, IBrandService iBrandServcie)
        {
            _IbannerService = ibannerService;
            _ICategoryService = iCategoryService;
            _IProductService = iProductService;
            _IProductSearchListService = iProductSearchListService;
            _IFloorKeywordService = iFloorKeywordService;
            _IProductPicService = iProductPicService;
            _IProductCommentService = iProductCommentService;
            _IbannerProductService = ibannerProductService;
            _IProductRuleService = iProductRuleService;
            _IFloorConfigService = iFloorConfigService;
            _ISKU_ProductService = iSKU_ProductService;
            _ISKU_ProductAttributesService = iSKU_ProductAttributesService;
            _ISKU_SKUItems = iSKU_SKUItems;
            _productConsultService = productConsultService;
            _newInfoService = iNewInfoService;
            _brandService = iBrandServcie;
        }

        public ActionResult Default()
        {
            return View();
        }



        #region Index 数据初始化
        public ActionResult Index()
        {
            MemCacheFactory.GetCurrentMemCache()
                    .AddCache("ZF001", "123", 1);
            ///获取推广广告
            ViewBag.BannerData = _IbannerService.GetTopBanner(6, 1, 0).Data;
            ViewBag.Advertisement = _IbannerService.GetTopBanner(1, 7, 0).Data.Count == 0 ? new bannerModel() : _IbannerService.GetTopBanner(1, 7, 0).Data[0];
            ///獲取新聞公告
            ViewBag.NewsData = _newInfoService.GetIndexNews(5, 1, null, true).Data;
            return View();
        }
        #endregion
        /// <summary>
        /// 热销排行
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _SellingList()
        {
            ViewBag.list = _IProductService.GetSellingList(6, CultureHelper.GetLanguageID()).Data;
            return PartialView();
        }

        [ValidateInput(false)]
        public ActionResult Search(KeyWordsSearch model)
        {
            if (model.k == "" || model.k == null)
            {
                model.AllCount = 0;
                ViewBag.products = null;
                ViewData.Add("searchModel", model);
                return View();
            }
            model.languageId = CultureHelper.GetLanguageID();
            long userId = 0;
            var userModel = GetUserResultModel();
            if (userModel != null)
            {
                userId = userModel.UserID;
            }
            int count = 0;
            var result = _IProductSearchListService.GetProductSearchListNew(model, userId, out count).Data;
            model.AllCount = count;
            ViewBag.products = result;
            ViewData.Add("searchModel", model);
            return View();
        }

        [ValidateInput(false)]
        public ActionResult IndexFirst(KeyWordsSearch model)
        {
            model.languageId = CultureHelper.GetLanguageID();
            List<CateList> esjlist = new List<CateList>();
            List<CategorysModel> caModel = _ICategoryService.GetCateById(model.cateId, model.languageId).Data;//获取一级分类名称
            if (caModel.Count > 0)
            {
                ViewBag.CateName = caModel[0].CategoryName;
                List<CategorysModel> list1 = _ICategoryService.GetCateByPid(model.cateId, model.languageId).Data;//二级分类   
                List<int> idlist = new List<int>();
                if (list1.Count > 0)
                {
                    for (int j = 0; j < list1.Count; j++)//二级分类
                    {
                        CateList catelist = new CateList();
                        catelist.CategoryId = list1[j].CategoryId;
                        catelist.CategoryName = list1[j].CategoryName;
                        List<CategorysModel> list2 = _ICategoryService.GetCateByPid(list1[j].CategoryId, model.languageId).Data;
                        catelist.cateLlist = list2;
                        esjlist.Add(catelist);
                        if (list2.Count > 0)
                        {
                            for (int i = 0; i < list2.Count; i++)
                            {
                                idlist.Add(list2[i].CategoryId);
                            }
                        }

                    }
                }
                ViewBag.ErSanJi = esjlist;
                int[] ilist = idlist.ToArray();
                if (ilist.Length > 0)
                {
                    int count = 0;
                    var result = _IProductSearchListService.GetProductSearchList(model, ilist, out count);
                    model.AllCount = count;
                    List<SearchModel> productlist = result.Data;
                    if (productlist.Count > 0)
                    {
                        productlist.ForEach(a =>
                        {
                            //处理价格显示促销中的
                            if (a.SalesRuleId > 1 & a.StarDate <= DateTime.Now & a.EndDate >= DateTime.Now)
                            {
                                var price = a.HKPrice;
                                a.MarketPrice = a.HKPrice;
                                a.HKPrice = a.Discount == 0 ? price : price * a.Discount;
                            }
                        });

                        if (model.st == SearchType.PriceDesc)
                        {
                            var data = productlist.OrderByDescending(a => a.HKPrice);
                            ViewBag.products = data;
                        }
                        else if (model.st == SearchType.PriceAsc)
                        {
                            var data = productlist.OrderBy(a => a.HKPrice);
                            ViewBag.products = data;
                        }
                        else
                        {
                            ViewBag.products = productlist;
                        }


                        // ViewData.Add("products", data);
                    }
                }
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
                return View("NotFound");
            }
            return View();
        }

        /// <summary>
        /// 获取子分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetsubCate(int id)
        {
            List<CategoryModel> catelist = _ICategoryService.GetCategoriesByParentId(CultureHelper.GetLanguageID(), id);
            return Json(catelist, JsonRequestBehavior.AllowGet);
        }


        [ValidateInput(false)]
        public ActionResult IndexALL(KeyWordsSearch model)
        {
            model.languageId = CultureHelper.GetLanguageID();
            List<CategorysModel> catemodel = _ICategoryService.GetCateById(model.cateId, model.languageId).Data;
            if (catemodel.Count > 0)
            {
                ViewBag.CateName = catemodel[0].CategoryName;//分类名称
                //获取商品分类信息
                List<int> idlist = new List<int>();
                if (model.type == "2")//如果点击二级分类
                {
                    List<CategorysModel> yimodel = _ICategoryService.GetCateById(catemodel[0].parentId, model.languageId).Data;
                    ViewBag.yiCateName = yimodel[0].CategoryName;            //一级分类             
                    ViewBag.yiCateID = yimodel[0].CategoryId;
                    List<CategorysModel> list1 = _ICategoryService.GetCateByPid(model.cateId, model.languageId).Data;
                    if (list1.Count > 0)
                    {
                        for (int i = 0; i < list1.Count; i++)
                        {
                            idlist.Add(list1[i].CategoryId);
                        }
                    }
                }
                else if (model.type == "3")//点击三级分类
                {
                    List<CategorysModel> ermodel = _ICategoryService.GetCateById(catemodel[0].parentId, model.languageId).Data;
                    ViewBag.erCateName = ermodel[0].CategoryName;
                    ViewBag.erCateID = ermodel[0].CategoryId;
                    List<CategorysModel> yimodel = _ICategoryService.GetCateById(ermodel[0].parentId, model.languageId).Data;
                    ViewBag.yiCateName = yimodel[0].CategoryName;
                    ViewBag.yiCateID = yimodel[0].CategoryId;
                    idlist.Add(model.cateId);
                }
                else
                {
                    return View("NotFound");
                }
                int[] ilist = idlist.ToArray();
                if (ilist.Length > 0)
                {
                    int count = 0;
                    var result = _IProductSearchListService.GetProductSearchList(model, ilist, out count);
                    model.AllCount = count;
                    List<SearchModel> productlist = result.Data;
                    if (productlist.Count > 0)
                    {
                        productlist.ForEach(a =>
                        {
                            //处理价格显示促销中的
                            if (a.SalesRuleId > 1 & a.StarDate <= DateTime.Now & a.EndDate >= DateTime.Now)
                            {
                                var price = a.HKPrice;
                                a.MarketPrice = a.HKPrice;
                                a.HKPrice = a.Discount == 0 ? price : price * a.Discount;
                            }
                        });
                        if (model.st == SearchType.PriceDesc)
                        {
                            var data = productlist.OrderByDescending(a => a.HKPrice);
                            ViewBag.products = data;
                        }
                        else if (model.st == SearchType.PriceAsc)
                        {
                            var data = productlist.OrderBy(a => a.HKPrice);
                            ViewBag.products = data;
                        }
                        else
                        {
                            ViewBag.products = productlist;
                        }
                        // ViewData.Add("products", productlist);
                    }

                }
                ViewBag.type = model.type;
                ViewData.Add("searchModel", model);
            }
            else
            {
                return View("NotFound");
            }
            return View();
        }

        #region 加载购买产品详细列表页
        /// <summary>
        /// 加载购买产品详细列表页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>addded by martin</remarks>
        public ActionResult Shopping(SearchConsle searchmodel,bool show=false,long id = 0)
        {
            if (id < 0)
            {
                return View("NoProduct");
            }
            SearchProductModel searchModel = new SearchProductModel();
            searchModel.ProductId = id;
            searchModel.LanguageId = CultureHelper.GetLanguageID();
            searchModel.PagedIndex = 0;
            searchModel.PagedSize = 1;
            //searchModel.Status = ProductStatus.HasUpShelves.GetHashCode();
            searchModel.Status = ProductStatus.HasUpShelves;//刘宏文改
            ResultModel result = null;
            if(show==false)
            {
                result = _IProductService.SearchProduct(searchModel);
            }
            else
            {
                result = _IProductService.SearchProductShow(searchModel);
            }
            

            int count = 0;
            searchmodel.languageID = CultureHelper.GetLanguageID();
            searchmodel.productId = id;
            List<ProductConsultModel> consul = _productConsultService.GetConsulList(searchmodel, out count).Data;
            searchmodel.AllCount = count;
            if (count > 0)
            {
                for (int i = 0; i < consul.Count; i++)
                {
                    if (consul[i].IsAnonymous == 1)
                    {
                        consul[i].Phone = consul[i].Phone.Substring(0, 3) + "***" + consul[i].Phone.Substring(consul[i].Phone.Length - 4);
                    }

                }
            }
            ViewData.Add("searchModel", searchmodel);
            ViewData.Add("consuls", consul);

            ProductInfo model = result.Data;
            if (model == null)
            {
                return View("NoProduct");
            }
            var getCommnetCount = _IProductCommentService.GetCount(model.ProductId).Data;
            model.Imglist = _IProductPicService.GetImageListByProductIdNoPage(id, 1).Data;   //获取截图
            model.GetCommentCount = getCommnetCount.Count > 0 ? getCommnetCount[0] : null;   //获取评论总数
            model.AvgCommentRate = _IProductCommentService.GetProductCommentAvgRate(id);  //平均评分
            model.SKU_ProductList = (List<HKTHMall.Domain.WebModel.Models.Product.SKU_ProductModel>)_ISKU_ProductService.GetSKU_ProductById(id).Data;                                            //获取产品总库存
            model.SKU_ProductAttributesAndSKU_AttributeValuesList = _ISKU_ProductAttributesService.GetSKU_ProductAttributesAndSKU_AttributeValuesById(id).Data;     //获取产品规格值
            model.SKU_SKUItemsAndSKU_AttributeValuesList = _ISKU_SKUItems.GetSKU_SKUItemsAndSKU_AttributeValuesById(id).Data;                                   //产品扩展属性

            if (model.SKU_ProductAttributesAndSKU_AttributeValuesList.Count > 0)
            {
                model.ProductCategoryTypeForSKU_AttributesList = _IProductService.GetProductCategoryTypeForSKU_Attributes(id).Data;  //获取产品类型规格参数总值
            }
            model.ProductParametersList = _IProductService.GetProductSpecifications(id).Data;
            model.IndexExplosionList = _IProductRuleService.GetPromotionProductForId(id, CultureHelper.GetLanguageID()).Data;     //促销
            ///获取库存
            Dictionary<string, SKU_DataInfo> list = new Dictionary<string, SKU_DataInfo>();
            foreach (var skuProductModel in model.SKU_ProductList)
            {
                SKU_DataInfo dataInfo = new SKU_DataInfo();
                dataInfo.HKPrice = skuProductModel.HKPrice;
                dataInfo.MarketPrice = skuProductModel.MarketPrice;
                dataInfo.SKUStr = skuProductModel.SKUStr;
                dataInfo.SKU_ProducId = skuProductModel.SKU_ProducId;
                dataInfo.Stock = skuProductModel.Stock;
                dataInfo.IsUse = skuProductModel.IsUse == 1 ? true : false;
                //if (skuProductModel.Stock > 0)
                {
                    list.Add(skuProductModel.SKUStr, dataInfo);
                }
            }
            model.Data = JsonConverts.ToJson(list);

            int isLogin = 0;
            if (GetUserResultModel() != null)
            {
                isLogin = 1;
            }
            ProductPath productPath = new ProductPath();
            object Ojson = MemCacheFactory.GetCurrentMemCache().GetCache(id + "_product_path" + CultureHelper.GetLanguageID());
            productPath = (ProductPath)JsonConverts.Json2Obj((Ojson == null ? "" : Ojson.ToString()), productPath.GetType());
            if (productPath == null)
            {
                var plist = _IProductService.GetProductPath(id, CultureHelper.GetLanguageID()).Data;
                productPath = plist.Count > 0 ? (ProductPath)plist[0] : new ProductPath();
                ViewBag.Path = productPath;
                MemCacheFactory.GetCurrentMemCache().AddCache(id + "_product_path" + CultureHelper.GetLanguageID(), ViewBag.Path);
            }
            else
            {
                ViewBag.Path = productPath;
            }
            ViewBag.islogin = isLogin;
            return View(model);

        }

        /// <summary>
        /// 咨询列表
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        public PartialViewResult _ConsultList(long productId = 0)
        {
            List<GroupModel> result = _productConsultService.GetConsultCountGroup(productId).Data as List<GroupModel>;
            List<SearchConsle> _listsearch = new List<SearchConsle>(){
                new SearchConsle(1,0),
                new SearchConsle(2,0),
                new SearchConsle(3,0),
                new SearchConsle(4,0),
                new SearchConsle(5,0),
                new SearchConsle(6,0),
                new SearchConsle(7,0)
            };

            if (result != null)
            {
                foreach (SearchConsle x in _listsearch)
                {
                    foreach (GroupModel s in result)
                    {
                        if (x.contype == s.contype)
                        {
                            x.AllCount += s.AllCount;
                        }
                    }
                }

                foreach (GroupModel x in result)
                {
                    _listsearch[0].AllCount += x.AllCount;
                    //if (x.contype < 2 || x.contype > 6)
                    //    _listsearch[6].AllCount += x.AllCount;
                }
            }

            _listsearch[0].languageID = CultureHelper.GetLanguageID();
            _listsearch[0].productId = productId;

            ViewData.Add("searchModellist", _listsearch);
            return PartialView("_ConsultList");
        }

        /// <summary>
        /// 提交咨询
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="content">咨询内容</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubmitConsult(long productId, string content, int contype)
        {
            var userModel = GetUserResultModel();
            if (userModel == null)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
            else
            {
                ProductConsult model = new ProductConsult();
                model.ProductId = productId;
                model.UserID = base.UserID;
                model.ConsultContent = content;
                model.ConsultDT = DateTime.Now;
                model.IsAnonymous = 0;
                model.CheckStatus = 1;
                model.ConsultType = contype;
                bool result = _productConsultService.AddConsult(model).IsValid;
                if (result)
                {
                    return Json("1", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("2", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitConsultGood(long ConsultID, int IsGood)
        {
            if (IsGood != 1 && IsGood != -1)
                return Content("0");

            if (base.UserID <= 0)
                return Content("0");

            UserConsult model = new UserConsult();
            model.ConsultID = ConsultID;
            model.UserID = base.UserID;
            model.IsGood = IsGood;

            ResultModel rs = _productConsultService.AddUserConsult(model);

            if (!rs.IsValid)
                return Content("0");
            else
                return Content(rs.Status.ToString());
        }

        [HttpPost]
        public ActionResult LoanConsultData(int contype, int count, long productid, int pageindex = 1, int pagesize = 5)
        {
            SearchConsle searchmodel = new SearchConsle();
            searchmodel.Page = pageindex;
            searchmodel.PageSize = pagesize;
            searchmodel.AllCount = count;
            searchmodel.contype = contype;
            searchmodel.productId = productid;

            List<ProductConsultModel> consul = _productConsultService.GetConsulList(searchmodel).Data;

            System.Text.StringBuilder builder = new System.Text.StringBuilder(null);
            System.Text.StringBuilder builderbody = new System.Text.StringBuilder(null);

            #region 生成分页

            long pageMaxSum = 10;
            long RecordCount = count;
            long curPage = pageindex;
            long PageSize = pagesize;
            long pageSum = count % pagesize > 0 ? count / pagesize + 1 : count / pagesize; //总页数

            if (pageSum < 1 || RecordCount <= 0) return Content("");

            if (curPage < 1) curPage = 1;
            if (curPage > pageSum) curPage = pageSum;

            long prev = (curPage < 2 ? 1 : curPage - 1);
            long next = (curPage >= pageSum ? pageSum : curPage + 1);

            builder.Append("<div class=\"ls_page\"><ul>");
            builder.AppendFormat("<li><a href=\"javascript:void(0)\" onclick=\"loadpagedata({0});\">< " + CultureHelper.GetLangString("ORDER_LIST_PREVIOUSPAGE") + "</a></li>", prev);

            long k = curPage - pageMaxSum / 2 - pageMaxSum % 2;
            if (k < 1)
            {
                k = 1;
            }
            long m = k + pageMaxSum - 1;
            if (m > pageSum)
            {
                m = pageSum;
            }

            for (long i = k; i <= m; i++)
            {
                if (i == curPage)
                    builder.AppendFormat("<li class=\"ls_page_on\"><a href=\"javascript:void(0)\" onclick=\"loadpagedata({0});\">{1}</a></li>", i, i);
                else
                    builder.AppendFormat("<li><a href=\"javascript:void(0)\" onclick=\"loadpagedata({0});\">{1}</a></li>", i, i);
            }

            builder.AppendFormat("<li><a href=\"javascript:void(0)\" onclick=\"loadpagedata({0});\">" + CultureHelper.GetLangString("ORDER_LIST_NEXTPAGE") + " > </a></li>", next);
            builder.Append("<span class=\"clearfix\"></span></ul></div>");

            #endregion

            #region 生成内容

            if (consul.Count == 0) return Content("");

            builderbody.Append("<ul>");
            foreach (ProductConsultModel model in consul)
            {
                if (model.IsAnonymous == 1)
                {
                    model.Phone = model.Phone.Substring(0, 3) + "***" + model.Phone.Substring(model.Phone.Length - 4);
                }

                builderbody.Append("<li>");
                builderbody.Append("<div class=\"details_4_content_lf\"><div class=\"ls_question\"><i></i><span>");
                builderbody.Append(model.Phone);
                builderbody.Append("</span><div class=\"ls_question_nr\">");
                builderbody.Append(model.ConsultContent);
                builderbody.Append("</div></div>");

                if (!string.IsNullOrEmpty(model.ReplyContent))
                {
                    builderbody.Append("<div class=\"ls_answer\"><i></i><div class=\"ls_answer_nr\">");
                    builderbody.Append(model.ReplyContent);
                    builderbody.Append("</div></div>");
                }

                builderbody.Append("</div>");//details_4_content_lf结束

                builderbody.Append("<div class=\"details_4_content_rg\">");
                if (!string.IsNullOrEmpty(model.ReplyContent))
                {
                    builderbody.Append("<div class=\"ls_message_time\">");
                    if (model.ReplyDT.HasValue)
                    {
                        builderbody.Append(model.ReplyDT.Value.ToString("yyyy-MM-dd"));
                        builderbody.Append("<span></span>");
                        builderbody.Append(model.ReplyDT.Value.ToString("HH:mm:ss"));
                    }
                    builderbody.Append("</div>");
                    //zhoub 修改，对应禅道(81210)
                    //builderbody.Append("<dl class=\"ls_zan\"><dd onclick='zan(" + model.ProductConsultId + ",1);'>" + CultureHelper.GetLangString("HOME_INDEX_ZX_ISGOOD") +
                    //    "<span class='isgood_" + model.ProductConsultId + "'>(" + model.IsGood.ToString() + ")</span></dd><dd onclick='zan(" + model.ProductConsultId + ",-1);'>" + CultureHelper.GetLangString("HOME_INDEX_ZX_NOGOOD") +
                    //    "<span class='nogood_" + model.ProductConsultId + "'>(" + model.NotGood + ")</span></dd></dl>");
                }
                else
                {
                    builderbody.Append("<div class=\"ls_message_time\">");
                    builderbody.Append(model.ConsultDT.ToString("yyyy-MM-dd") + "<span></span>" + model.ConsultDT.ToString("HH:mm:ss") + "</div>");
                }

                builderbody.Append("</div>");//details_4_content_rg结束

                builderbody.Append("</li>");
            }

            builderbody.Append("</ul>");

            #endregion

            return Content(builderbody.ToString() + builder.ToString());
        }

        #endregion

        #region 加载未登录购物车列表

        /// <summary>获取以商家分组的商品信息.</summary>
        /// <remarks></remarks>
        /// <author></author>
        /// <param name="getChecked">The string goods ids.</param>
        /// <returns>ActionResult.</returns>
        public JsonResult GetShoppingCarProductList(string productIds, string productSkuIds)
        {
            var result = _IProductService.GetProductListByPrdouctId(productIds, productSkuIds);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    

        #endregion

        #region 惠卡推广产品列表
        /// <summary>
        /// 惠卡推广产品列表
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        /// <remarks>addded by martin</remarks>
        public ActionResult LikeGuess(int top = 5)
        {
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
                var data = productList.OrderByDescending(a => a.Guid).Take(top);
                ViewBag.ProductData = data;
            }

            return PartialView();
        }
        #endregion

        /// <summary>
        /// 商品详情页 推荐
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public ActionResult LikeAll(int top = 5, int typeid = 0)
        {
            var productData = _IProductService.GetTopRecommend(CultureHelper.GetLanguageID(), base.UserID);
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
                var data = productList.OrderByDescending(a => a.Guid).Take(top);
                ViewBag.ProductData = data;
            }

            if (typeid == 1)
            {
                ViewBag.ViewString = "shopping";
            }
            else
            {
                ViewBag.ViewString = "";
            }

            return PartialView();
        }

        /// <summary>
        /// 商品详情页 推荐
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public ActionResult TopList(long productId, int top = 6, int languageid = 4, long userId = 0)
        {
            var productData = _IProductService.GetTopRecommend(productId, top, CultureHelper.GetLanguageID(), base.UserID);
            List<ProductInfo> productList = productData.Data;
            if (productList != null && productList.Any())
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
            }
            return PartialView("_TopList", productList);
        }

        /// <summary>
        /// 商品详情页 获取惠卡推荐数据(其他类型的，不排序 随意推荐)
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public ActionResult RecommendOtherList(long productId, int top = 6, int languageid = 4)
        {
            var productData = _IProductService.GetTopRecommend(productId, top, CultureHelper.GetLanguageID());
            List<ProductInfo> productList = productData.Data;
            if (productList != null && productList.Any())
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
            }
            return PartialView("_RecommendOtherList", productList);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public PartialViewResult _RecommendList(int Width = 300, int Height = 130)
        {
            ///获取推广广告
            ViewBag.RecList = _IbannerService.GetTopBanner(1000, 8, 0).Data;
            ViewBag.Width = Width;
            ViewBag.Height = Height;
            return PartialView();
        }

        #region 详情页底部 惠卡推荐
        /// <summary>
        /// 惠卡推广产品列表
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        /// <remarks>addded by martin</remarks>
        public ActionResult HuikaRecommend(int top = 5)
        {
            ///获取惠卡推广数据
            var productData = _IProductService.GetTopRecommend(CultureHelper.GetLanguageID(), 6600000104);
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
                var data = productList.OrderByDescending(a => a.Guid).Take(top);
                ViewBag.ProductData = data;
            }

            return PartialView();
        }
        #endregion

        public ActionResult languageTest()
        {
            CultureHelper.GetLanguageID();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(System.Web.HttpContext.Current.Server.MapPath("~/EmailContent/EmailVerify.xml"));
            string strMailTitle = doc.GetElementsByTagName("TITILE")[0].InnerText;
            string strMailBody = doc.GetElementsByTagName("BODY")[0].InnerText;

            string phone = base.Account;
            strMailBody = strMailBody.Replace("$USERNAME$", phone);//用户账号替换

            string email = "840297082@qq.com";
            strMailBody = strMailBody.Replace("$Email$", email);

            string url = "http://www.kfd9999.com/user/realSecond?action=activate&key=24e574fd82c84965350b679c0bb45b4a&usercode=31889";
            strMailBody = strMailBody.Replace("$URL$", url);//换地址

            HKTHMall.Web.Common.Mail.sendMail("840297082@qq.com", strMailTitle, strMailBody);
            return View();
        }

        //[ChildActionOnly]
        public ActionResult Top()
        {
            ViewBag.Phone = base.Phone;
            return PartialView();

        }
        [ValidateInput(false)]
        public ActionResult SetCulture(string culture)
        {
            culture = CultureHelper.GetImplementedCulture(culture);
            HttpCookie cookie = Request.Cookies["_culture"];
            string url = "";
            if (Request.UrlReferrer != null)
            {
                url = Request.UrlReferrer.ToString();
            }
            else
            {
                url = Url.Action("Index", "Home");
            }

            if (cookie != null)
            {
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.Add(new TimeSpan(7, 0, 0, 0));// DateTime.Now.AddDays(7);
            }
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                // cookie.Expires = DateTime.Now.Add(new TimeSpan(7, 0, 0, 0));// DateTime.Now.AddDays(7);

                //cookie.Expires = DateTime.Now.AddDays(7);

            }
            Response.Cookies.Add(cookie);
            return Redirect(url);//RedirectToAction(url);
        }

        #region 获取关键字推送
        /// <summary>
        /// 获取关键字推送
        /// </summary>
        /// <param name="languageid"></param>
        /// <param name="counts"></param>
        /// <returns></returns>
        /// <remarks>addded by martin</remarks>
        //[OutputCache(Duration = 300, VaryByParam = "languageid;counts")]
        public ActionResult GetFloorKeyword(int languageid, int counts = 3)
        {
            var result = _IFloorKeywordService.GetTopList(languageid, counts);
            List<FloorKeywordModel> floorList = result.Data;
            ViewBag.FloorList = floorList;
            return PartialView();
        }
        #endregion

        /// <summary>
        /// 获取一级分类Banaer
        /// </summary>
        /// <param name="topCount">图片数</param>
        /// <param name="identityStatus">分类</param>
        /// <param name="placeCode">位置</param>
        /// <returns></returns>
        public ActionResult CateBanner(int topCount, int identityStatus, int placeCode)
        {
            var bannerData = _IbannerService.GetTopBanner(topCount, identityStatus, placeCode);
            ViewBag.BannerData = bannerData.Data;
            return PartialView();
        }


        #region 加载首页楼层数据信息
        /// <summary>
        /// 加载首页楼层数据信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>addded by martin</remarks>
        [OutputCache(Duration = 300, VaryByParam = "languageid;floorCount")]
        public PartialViewResult IndexFloor(int languageid, int floorCount)
        {
            FloorConfigModel categoryIdList = _IFloorConfigService.GetModelById(1).Data;
            string[] categoryStringList = categoryIdList.CateIdStr.Split(',');
            ///加载数据楼层数据
            List<IndexFloorInfo> InfoList = new List<IndexFloorInfo>();
            if (categoryStringList.Length > 0)
            {
                foreach (var s in categoryStringList)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        int categoryId = int.Parse(s);
                        IndexFloorInfo floorInfo = new IndexFloorInfo();

                        //languageid=CultureHelper.GetLanguageID()
                        //CategorysModel infomodel = _ICategoryService.GetCateById(categoryId, languageid).Data.Count > 0 ? _ICategoryService.GetCateById(categoryId, languageid).Data[0] : null;

                        ResultModel rmModel = _ICategoryService.GetCateById(categoryId, languageid);
                        CategorysModel infomodel = rmModel.Data.Count > 0 ? rmModel.Data[0] : null; ;
                        if (infomodel != null)
                        {
                            floorInfo.categoryId = infomodel.CategoryId;
                            floorInfo.categoryName = infomodel.CategoryName;
                            floorInfo.Place = infomodel.Place;
                        }
                        floorInfo.bannerList = _IbannerService.GetBannerByTimeDesc(null, 2, categoryId).Data;
                        floorInfo.bannerProductList = _IbannerProductService.GetTopBanner(7, 2, categoryId).Data;
                        floorInfo.categoryList = _ICategoryService.GetCategoriesByALL(languageid, categoryId).Data;
                        InfoList.Add(floorInfo);
                    }
                }
            }
            return PartialView(InfoList);
        }
        #endregion

        #region 加载爆款信息数据
        /// <summary>
        /// 加载爆款信息数据
        /// </summary>
        /// 
        /// <returns></returns>
        /// <remarks>addded by martin</remarks>

        //[OutputCache(Duration = 300)]
        //public PartialViewResult IndexActivity(int languageid, int topCount = 50)
        //{
        //    List<IndexExplosion> explosionList = _IProductRuleService.GetIndexExplosion(languageid, topCount).Data;
        //    return PartialView(explosionList);
        //}


        //[OutputCache(Duration = 300)]
        /// <summary>
        /// add by liujc
        /// </summary>
        /// <param name="languageid"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public PartialViewResult _HotList(int languageid = 4, int topCount = 4)
        {
            List<IndexExplosion> listx = _IProductRuleService.GetIndexExplosion(languageid, topCount, true).Data;
            List<IndexExplosion> listy = _IProductRuleService.GetIndexExplosion(languageid, topCount, false).Data;

            List<List<IndexExplosion>> list = new List<List<IndexExplosion>>();
            list.Add(listx);
            list.Add(listy);

            return PartialView(list);
        }
        #endregion

        #region 错误,404页面
        public ActionResult NotFound()
        {
            //Response.Status = "404 Not Found";
            //Response.StatusCode = 404;



            return View();
        }
        #endregion

        public PartialViewResult _IndexFloor(int Lang = 4)
        {
            FloorConfigModel categoryIdList = _IFloorConfigService.GetModelById(1).Data;
            string[] categoryStringList = categoryIdList.CateIdStr.Split(',');
            ///加载数据楼层数据
            List<IndexFloorInfo> InfoList = new List<IndexFloorInfo>();
            if (categoryStringList.Length > 0)
            {
                foreach (var s in categoryStringList)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        int categoryId = int.Parse(s);
                        IndexFloorInfo floorInfo = new IndexFloorInfo();
                        ResultModel rmModel = _ICategoryService.GetCateById(categoryId, Lang);
                        CategorysModel infomodel = rmModel.Data.Count > 0 ? rmModel.Data[0] : null; ;
                        if (infomodel != null)
                        {
                            floorInfo.categoryId = infomodel.CategoryId;
                            floorInfo.categoryName = infomodel.CategoryName;
                            floorInfo.Place = infomodel.Place;
                        }
                        floorInfo.bannerList = _IbannerService.GetBannerByTimeDesc(null, 2, categoryId).Data;
                        floorInfo.bannerProductList = _IbannerProductService.GetTopBanner(6, 2, categoryId).Data;
                        floorInfo.categoryList = _ICategoryService.GetCategoriesByALL(Lang, categoryId).Data;
                        floorInfo.floorAd = _IbannerService.GetBannerByTimeDesc(1, 3, categoryId).Data.Count == 0 ? new bannerModel() : _IbannerService.GetBannerByTimeDesc(1, 3, categoryId).Data[0];
                        var CategoryIds = floorInfo.categoryList.Select(p => (int)p.CategoryId).ToList().ToArray();
                        floorInfo.brandList = _brandService.GetTopBrandTimeDesc(4, CategoryIds, Lang).Data;
                        InfoList.Add(floorInfo);
                    }
                }
            }
            return PartialView(InfoList);
        }


        /// <summary>
        /// 我的惠卡页面惠卡推荐
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        /// <remarks>addded by martin</remarks>
        public ActionResult MyHuikaRecommend(int top = 4)
        {
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
                var data = productList.OrderByDescending(a => a.Guid).Take(top);
                ViewBag.ProductData = data;
            }

            return PartialView();
        }

    }
}