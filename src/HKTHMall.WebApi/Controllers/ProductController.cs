using BrCms.Framework.Collections;
using HKTHMall.Core;
using HKTHMall.Core.Extensions;
using HKTHMall.Core.Security;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.AdminModel.Products;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.WebModel.Models.Collection;
using HKTHMall.Domain.WebModel.Models.Index;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Services.Common;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Services.Orders;
using HKTHMall.Services.Products;
using HKTHMall.Services.SKU;
using HKTHMall.Services.WebProducts;
using HKTHMall.Services.YHUser;
using HKTHMall.WebApi.Common;
using HKTHMall.WebApi.Models;
using HKTHMall.WebApi.Models.Request;
using HKTHMall.WebApi.Models.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;


namespace HKTHMall.WebApi.Controllers
{
    public class ProductController : BaseController
    {
        private IEncryptionService _enctyptionService;
        private ISKU_AttributesService _attributesService;
        private ISP_ProductCommentService _spProductCommentService;
        private IProductService _productService;
        private IProductRuleService _IProductRuleService;
        private IProductService _IProductService;
        private IProductCommentService _IProductCommentService;
        private IProductPicService _IProductPicService;
        private ISKU_ProductService _ISKU_ProductService;
        private IMyCollectionService _MyCollectionService;
        private ISKU_ProductAttributesService _ISKU_ProductAttributesService;
        private ISKU_AttributesService _SkuAttributesService;
        private IOrderService _orderService;
        private IProductConsultService _productConsultService;
        private ICategoryService _CategoryService;


        public ProductController(ISP_ProductCommentService spProductCommentService, IProductService productService, ISKU_AttributesService _attributesService, IEncryptionService _enctyptionService, IOrderService orderService
            , IProductRuleService productRuleService
            , IProductService iProductService
            , IProductCommentService iProductCommentService
            , IProductPicService iProductPicService
            , ISKU_ProductService iSKU_ProductService
            , IMyCollectionService myCollectionService
            , ISKU_ProductAttributesService iSKU_ProductAttributesService
            , ISKU_AttributesService iSkuAttributesService
            , IProductConsultService iPproductConsultService
            , ICategoryService iCategoryService)
        {
            _spProductCommentService = spProductCommentService;
            this._productService = productService;
            this._attributesService = _attributesService;
            this._enctyptionService = _enctyptionService;
            this._IProductRuleService = productRuleService;
            this._IProductService = iProductService;
            this._IProductCommentService = iProductCommentService;
            this._IProductPicService = iProductPicService;
            this._ISKU_ProductService = iSKU_ProductService;
            this._MyCollectionService = myCollectionService;
            this._ISKU_ProductAttributesService = iSKU_ProductAttributesService;
            this._SkuAttributesService = iSkuAttributesService;
            this._orderService = orderService;
            this._productConsultService = iPproductConsultService; 
            this._CategoryService = iCategoryService;
        }


        #region 4.1.商品搜索（朱志容）
        /// <summary>
        /// 4.1.商品搜索（朱志容）
        /// </summary>
        /// <param name="searchProduct"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SearchProduct(RequestSearchProduct searchProduct)
        {
            ApiPagingResultModel response = new ApiPagingResultModel();
            try
            {
                if (searchProduct == null)
                {
                    response.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 3);// 参数不能为空
                    return Ok(response);
                }
                ProductSearchListService productService = new ProductSearchListService();
                int totalSize = 0;
                Domain.WebModel.Models.Search.KeyWordsSearch keyWords = new Domain.WebModel.Models.Search.KeyWordsSearch()
                {
                    k = searchProduct.keyword,
                    languageId = searchProduct.lang,
                    Page = searchProduct.pageNo,
                    PageSize = searchProduct.pageSize
                };
                var result = productService.GetProductSearchList(keyWords, out totalSize);
                List<ResponseProductModel> list = new List<ResponseProductModel>();
                if (result.Data != null)
                {
                    foreach (var item in result.Data)
                    {
                        float money = (float)item.HKPrice;
                        float marketPrice = (float)item.MarketPrice;
                        if (item.Discount!=0)
                        {
                            money = (float)item.HKPrice * (float)item.Discount;
                            marketPrice = (float)item.HKPrice;
                        }
                        ResponseProductModel pro = new ResponseProductModel()
                        {
                            hKPrice =(float)Math.Round(money,0),
                            marketPrice = marketPrice,
                            picAddress = HtmlExtensions.GetImagesUrl(item.PicUrl, 268, 268),
                            productId = item.ProductId,
                            productName = item.ProductName
                        };
                        list.Add(pro);
                    }
                }
                response.totalSize = totalSize;
                if (result.IsValid)
                {
                    response.flag = 1;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", searchProduct.lang);//查询成功
                }
                else
                {
                    response.flag = 0;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", searchProduct.lang);//查询失败
                }
                response.rs = list;
            }
            catch (Exception ex)
            {
                response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", searchProduct.lang); //"根据关键字获取产品异常!" + ex;
                NLogHelper.GetCurrentClassLogger().Error("根据关键字获取产品异常!" + ex);
            }
            return Ok(response);
        }
        #endregion

        #region 4.2.商品列表（朱志容）
        /// <summary>
        /// 4.2.商品列表（朱志容）
        /// </summary>
        /// <param name="productModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetProductList(RequestProductModel productModel)
        {
            ApiPagingResultModel response = new ApiPagingResultModel();
            try
            {
                if (productModel == null || productModel.categoryId < 1)
                {
                    response.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", 3); //参数错误
                    return Ok(response);
                }
                ProductSearchListService productService = new ProductSearchListService();
                int totalSize = 0;
                int st = 0;
             
                switch (productModel.orderBy)
                {
                    case 1:
                        st = 0;
                        break;
                    case 2:
                        st = 20;
                        break;
                    case 3:
                        st = 30;
                        break;
                    case 4:
                        st = 40;
                        break;
                }
                HKTHMall.Domain.WebModel.Models.Search.KeyWordsSearch search = new Domain.WebModel.Models.Search.KeyWordsSearch()
                {
                    languageId = (int)productModel.lang,
                    Page = productModel.pageNo,
                    PageSize = productModel.pageSize,
                    st = (SearchType)st,
                    cateId = (int)productModel.categoryId
                };
                List<int> cateIds = new List<int>();
                //传入一级分类ID
                if (productModel.level == 1)
                {
                    #region 传入一级分类ID
                    //获取分类二级ID
                    var parentIds = _CategoryService.GetCategoryByParentId(productModel.categoryId, productModel.lang);
                    if (parentIds.Data != null)
                    {
                        foreach (var item in parentIds.Data)
                        {
                            //获取分类三级ID
                            var cates = _CategoryService.GetCategoryByParentId((int)item.CategoryId, productModel.lang);
                            if (cates.Data != null)
                            {
                                foreach (var id in cates.Data)
                                {
                                    cateIds.Add(id.CategoryId);
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (productModel.level == 2)
                {
                    #region 传入二级分类ID
                    //获取分类三级ID
                    var cates = _CategoryService.GetCategoryByParentId((int)productModel.categoryId, productModel.lang);
                    if (cates.Data != null)
                    {
                        foreach (var id in cates.Data)
                        {
                            cateIds.Add(id.CategoryId);
                        }
                    }
                    #endregion
                }
                else//默认传入三级分类ID
                {
                    cateIds.Add((int)productModel.categoryId);
                }
                var result = productService.GetProductSearchList(search, cateIds.ToArray(), out totalSize);
                List<ResponseProductModel> list = new List<ResponseProductModel>();
                if (result.Data != null)
                {
                    foreach (var item in result.Data)
                    {
                        float money = (float)item.HKPrice;
                        float marketPrice = (float)item.MarketPrice;
                        if (item.Discount != 0)
                        {
                            money = (float)item.HKPrice * (float)item.Discount;
                            marketPrice = (float)item.HKPrice;
                        }
                        ResponseProductModel pro = new ResponseProductModel()
                        {
                            hKPrice = (float)Math.Round(money, 0),
                            marketPrice = marketPrice,
                            picAddress = HtmlExtensions.GetImagesUrl(item.PicUrl, 268, 268),
                            productId = item.ProductId,
                            productName = item.ProductName
                        };
                        list.Add(pro);
                    }
                }
                List<ResponseProductModel> listRe = list;
                //对打折后的商品排序
                if (productModel.orderBy == 3)
                {
                    listRe = list.OrderByDescending(x => x.hKPrice).ToList();
                }
                else if (productModel.orderBy == 4)
                {
                     listRe = list.OrderBy(x => x.hKPrice).ToList();
                }
                response.totalSize = totalSize;
                if (result.IsValid)
                {
                    response.flag = 1;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", productModel.lang);//查询成功
                }
                else
                {
                    response.flag = 0;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", productModel.lang);//查询失败
                }                
                response.rs = listRe;
            }
            catch (Exception ex)
            {
                response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", productModel.lang); //"获取产品列表异常!" + ex;
                NLogHelper.GetCurrentClassLogger().Error("获取产品列表异常!" + ex);
            }
            return Ok(response);
            //var jsonStr = JsonConvert.SerializeObject(response);
            //return new HttpResponseMessage { Content = new StringContent(jsonStr, Encoding.UTF8, "application/json") };
        }
        #endregion

        #region 4.3.商品详情(刘翊，朱志容)
        /// <summary>
        /// 4.3.商品详情(刘翊)
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetProductInfo(RequestProductInfoModel requestModel)
        {
            var responseModel = new ResponseProductInfoModel();
            if (requestModel == null || requestModel.productId <= 0)
            {
                responseModel.flag = 0;
                responseModel.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", requestModel.lang);//参数错误
            }
            else
            {
                requestModel.userId = this._enctyptionService.RSADecrypt(requestModel.userId);
                SearchProductModel searchModel = new SearchProductModel();
                searchModel.ProductId = requestModel.productId;
                searchModel.LanguageId = requestModel.lang;
                searchModel.PagedIndex = 0;
                searchModel.PagedSize = 1;
                searchModel.Status = ProductStatus.HasUpShelves;
                var result = _IProductService.SearchProduct(searchModel);
                ProductInfo model = result.Data;
                if (model == null)
                {
                    responseModel.flag = 1;
                    //responseModel.Results = new ResultProduct();
                }
                else
                {
                    var productresults = new ResultProduct();
                    productresults.ProductId = model.ProductId.ToString();
                    productresults.ProductName = model.ProductName;
                    productresults.Subheading = model.Subheading;
                    //productresults.Introduction = model.Introduction;
                    productresults.HkPrice = model.HKPrice;
                    productresults.MarketPrice = model.MarketPrice;
                    productresults.StockQuanTity = model.StockQuantity;
                    productresults.StatusType = model.Status == 4 ? 0 : 1;
                    productresults.IsPostage = 1;
                    productresults.BuyCount = model.SaleCount; //销售量
                    productresults.CreateDt = HKTHMall.Core.Utils.Utils.ConvertToLongMillisecond(model.CreateDT.ToString());           //创建时间
                    productresults.ServiceDt = HKTHMall.Core.Utils.Utils.ConvertToLongMillisecond(DateTime.Now.ToString()); // 服务器当前时间 (时间戳)
                    var getCommnetCount = _IProductCommentService.GetCount(model.ProductId).Data;
                    productresults.CommentCount = getCommnetCount.Count > 0 ? getCommnetCount[0].CommentTotal.ToString() : "0";   //获取评论总数
                    model.SKU_ProductList = _ISKU_ProductService.GetSKU_ProductById(requestModel.productId).Data;   //获取库存值

                    #region 获取截图
                    List<ProductPicModel> Imglist = _IProductPicService.GetImageListByProductIdNoPage(requestModel.productId, 1).Data;   //获取截图
                    if (Imglist != null && Imglist.Count > 0)
                    {
                        productresults.ProductImageArray = new List<ImageUrl>();
                        Imglist.ForEach(a =>
                        {
                            var img = new ImageUrl();
                            img.ImageUrls = HtmlExtensions.GetImagesUrl(a.PicUrl, 480, 480);
                            img.BigimageUrls = HtmlExtensions.GetImagesUrl(a.PicUrl, 640, 640);
                            productresults.ProductImageArray.Add(img);
                        });

                        var defaultimg = Imglist.FirstOrDefault(a => a.Flag == 1);
                        productresults.ImageUrl = HtmlExtensions.GetImagesUrl(defaultimg.PicUrl, 100, 100);            //商品图片
                        productresults.bigImageUrl = HtmlExtensions.GetImagesUrl(defaultimg.PicUrl, 640, 640);        //商品大图片

                    }
                    #endregion
                    //productresults.SkuItemses         //sku属性集
                    model.SKU_ProductAttributesAndSKU_AttributeValuesList =
                        _ISKU_ProductAttributesService.GetSKU_ProductAttributesAndSKU_AttributeValuesById(requestModel.productId).Data;
                    if (model.SKU_ProductAttributesAndSKU_AttributeValuesList != null && model.SKU_ProductAttributesAndSKU_AttributeValuesList.Count > 0)
                    {
                        productresults.SkuItemses = new List<SkuItems>();
                        model.SKU_ProductAttributesAndSKU_AttributeValuesList.ForEach(a =>
                        {
                            //判断属性ID是否被记录
                            if (productresults.SkuItemses.FindAll(c => c.AttributeId == a.AttributeId.ToString()).Count <= 0)
                            {
                                #region
                                var skuItems = new SkuItems();
                                skuItems.AttributeId = a.AttributeId.ToString();    //属性ID 
                                var resultAttribute = _SkuAttributesService.GetSKU_AttributesById(a.AttributeId);
                                //属性名称
                                if (resultAttribute.Data != null)
                                {
                                    SKU_AttributesModel attributesModel = resultAttribute.Data;
                                    skuItems.AttributeName = attributesModel.AttributeName;
                                }
                                else
                                {
                                    skuItems.AttributeName = "";
                                }
                                var valueList = model.SKU_ProductAttributesAndSKU_AttributeValuesList.FindAll(
                                     b => b.AttributeId == a.AttributeId);
                                if (valueList != null)
                                {
                                    //获取属性值
                                    skuItems.PropertyValues = new List<Values>();
                                    valueList.ForEach(b =>
                                    {
                                        var value = new Values();
                                        value.AttributeId = b.AttributeId.ToString();
                                        value.ImgUrl = string.IsNullOrEmpty(b.ImageUrl) ? "" : HtmlExtensions.GetImagesUrl(b.ImageUrl, 640, 440);
                                        value.ValueId = b.ValueId.ToString();
                                        value.ValueStr = b.ValueStr;
                                        //value.Sku
                                        if (model.SKU_ProductList != null)
                                        {
                                            string skuList = string.Empty;
                                            model.SKU_ProductList.ForEach(d =>
                                            {
                                                if (d.SKUStr.IndexOf(value.ValueId) != -1)
                                                {
                                                    skuList += string.Format("{0},", d.SKUStr);
                                                }
                                            });
                                            skuList = skuList.TrimEnd(',');
                                            value.Sku = skuList;
                                        }
                                        skuItems.PropertyValues.Add(value);
                                    });
                                }
                                //是否文字显示
                                skuItems.IsRelationImage = string.IsNullOrEmpty(a.ImageUrl) ? false : true;
                                productresults.SkuItemses.Add(skuItems);
                                #endregion
                            }
                        });
                    }
                    //是否收藏 0表示未收藏；1表示已收藏
                    #region 是否收藏
                    if (requestModel.userId != "" || requestModel.userId != "0")
                    {
                        FavoritesModel fmodel = new FavoritesModel();
                        fmodel.UserID = Convert.ToInt64(requestModel.userId);
                        fmodel.ProductId = requestModel.productId;
                        var results = _MyCollectionService.Find(fmodel);
                        if (results.IsValid)
                        {
                            List<FavoritesModel> FavoritesList = results.Data;
                            productresults.IsFavorites = FavoritesList[0].FavoritesID;
                        }
                        else
                        {
                            productresults.IsFavorites = 0;
                        }
                    }
                    else
                    {
                        productresults.IsFavorites = 0;
                    }
                    #endregion
                    //库存价格 
                    #region 库存价格
                    if (model.SKU_ProductList != null & model.SKU_ProductList.Count > 0)
                    {
                        productresults.SkuStockList = new List<SkuStock>();
                        model.SKU_ProductList.ForEach(a =>
                        {
                            var skuStock = new SkuStock();
                            skuStock.SkuProducId = a.SKU_ProducId.ToString();
                            skuStock.SkuNumber = a.SKUStr;
                            skuStock.SkuName = a.SkuName;
                            skuStock.MarketPrice = a.MarketPrice;
                            skuStock.hkPrice = a.HKPrice;
                            skuStock.Stock = a.Stock;
                            productresults.SkuStockList.Add(skuStock);
                        });
                    }
                    #endregion
                    //商家基础信息
                    productresults.CompanyUrl = "";
                    productresults.CompanyName = "HuikaMall";
                    productresults.CompanyTelephone = "02-635-5484";

                    //促销
                    #region 促销
                    List<IndexExplosion> IndexExplosionList = _IProductRuleService.GetPromotionProductForId(requestModel.productId, requestModel.lang).Data;     //促销
                    if (IndexExplosionList != null & IndexExplosionList.Count > 0)
                    {
                        productresults.IsSpecial = 1;
                        productresults.ExplosionRebate = IndexExplosionList[0].Discount;
                        productresults.StartDt = HKTHMall.Core.Utils.Utils.ConvertToLongMillisecond(IndexExplosionList[0].StarDate);      //爆款开始时间
                        productresults.EndDt = HKTHMall.Core.Utils.Utils.ConvertToLongMillisecond(IndexExplosionList[0].EndDate); //拼购结束时间 
                    }
                    else
                    {
                        productresults.IsSpecial = 0;
                    }
                    #endregion
                    
                    //咨询总数
                    productresults.ConsultCount = _productConsultService.GetConsultCount(requestModel.productId).Data;              

                    responseModel.flag = 1;
                    responseModel.msg = CultureHelper.GetAPPLangSgring("SUCCESS", requestModel.lang);
                    responseModel.Results = productresults;
                }
            }
            var jsonStr = JsonConvert.SerializeObject(responseModel);
            return new HttpResponseMessage { Content = new StringContent(jsonStr, Encoding.UTF8, "application/json") };
        }

        #endregion

        #region 4.4.商品规格参数(刘文宁)
        /// <summary>
        /// 4.4.商品规格参数(刘文宁)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetAttributeList(RequersAttributeListModel model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            if (model != null)
            {
                result = _attributesService.GetSKU_ProductSpecificationParameterById(model.productId, model.lang);
                if (result.IsValid)
                {
                    apiResult.flag = 1;
                    apiResult.rs = result.Data;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", model.lang);//查询成功
                    if (result.Messages != null)
                    {
                        if (result.Messages.Count > 0)
                        {
                            apiResult.msg = result.Messages[0];
                        }
                    }
                }
                else
                {
                    apiResult.flag = 0;
                    apiResult.msg = result.Messages[0];
                }
            }
            else
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang);//参数不能为空
            }
            return Ok(apiResult);
        }
        #endregion

        #region 4.5.发表评价（朱志容）
        /// <summary>
        /// 4.5.发表评价（朱志容）
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult CommentList(RequestOrderComment comment)
        {
            ApiResultModel response = new ApiResultModel();
            try
            {
                if (comment == null)
                {
                    response.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", 3);//参数错误
                    return Ok(response);
                }
                var userId = _enctyptionService.RSADecrypt(comment.userId);
                if (userId == "")
                {
                    response.msg = CultureHelper.GetAPPLangSgring("USER_ID_IllEGAL", comment.lang);//用户ID不合法
                    return Ok(response);
                }
                var orderStatus = _orderService.GetOrderStatus(comment.orderId, Convert.ToInt64(userId));
                //订单状态:-1:无效订单；2:待付款,3:待发货,4:待收货,5:已收货,6:已完成,7:已取消,8交易关闭
                if (orderStatus.Data == null || orderStatus.Data == -1 || orderStatus.Data == 7)
                {
                    response.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_My_OrderComplaints_ThereIsnoordernumber", comment.lang);//订单号不存在
                    return Ok(response);
                }
                var commentService = new SP_ProductCommentService();
                //获取已评论的订单
                var aa = commentService.GetOrderProductComments(new SearchOrderProductCommentView() { OrderID = comment.orderId.ToString(), LanguageID = comment.lang, UserID = Convert.ToInt64(userId) });
                List<OrderProductCommentView> listView = aa.Data;
                var list = new List<SP_ProductCommentModel>();
                int count = 0;//记录同个订单下已评论商品的数量             
                foreach (var item in comment.rq)
                {

                    if (string.IsNullOrEmpty(item.productId.ToString()))
                    {
                        response.msg = CultureHelper.GetAPPLangSgring("GOODS_ID_IllEGAL", comment.lang);//商品ID不合法
                        return Ok(response);
                    }
                    else if (string.IsNullOrEmpty(item.skuId.ToString()))
                    {
                        response.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", comment.lang);//参数错误
                        return Ok(response);
                    }
                    long productId = item.productId;
                    if (listView.Where(c => c.ProductId == productId && c.Iscomment == 1).Any())//判断产品是否已经评论过
                    {
                        count++;
                        continue;
                    }
                    var model = new SP_ProductCommentModel
                    {
                        OrderId = Convert.ToInt64(comment.orderId),
                        UserID = Convert.ToInt64(userId),
                        ProductId = productId,
                        IsAnonymous = (byte)(comment.isanonymous ? 0 : 1),
                        CommentDT = DateTime.Now,
                        CommentContent = item.commentcontent,
                        CommentLevel = (byte)(item.commentlevel),
                        SKU_ProducId = item.skuId
                    };
                    list.Add(model);
                }
                if (count == comment.rq.Count)
                {
                    response.flag = 0;
                    response.msg = CultureHelper.GetAPPLangSgring("COMMENT_HAVE", comment.lang);//该订单已经评论过了。 重复评论
                    return Ok(response);
                }
                var result = commentService.BatchAddSP_ProductComment(list);
                if (result.IsValid)
                {
                    response.flag = 1;
                    response.msg = CultureHelper.GetAPPLangSgring("COMMENT_SUCCESS", comment.lang);     //提交评论成功           
                }
                else
                {
                    response.flag = 0;
                    response.msg = CultureHelper.GetAPPLangSgring("COMMENT_FAILURE", comment.lang);
                    Logger.Debug("订单评论失败!" + string.Join(",", result.Messages));
                    //NLogHelper.GetCurrentClassLogger().Error("订单评论失败!" +string.Join(",",result.Messages));
                }
            }
            catch (Exception ex)
            {
                response.flag = 0;
                response.msg = CultureHelper.GetAPPLangSgring("COMMENT_FAILURE", comment.lang);// "批量提交订单评论失败!" + ex;
                Logger.Debug("批量提交订单评论失败!" + ex);
                //NLogHelper.GetCurrentClassLogger().Error("批量提交订单评论失败!" + ex);
            }
            return Ok(response);
        }
        #endregion

        #region 4.6.获取评价列表(吴育富，朱志容)
        /// <summary>
        /// 4.6.获取评价列表(吴育富)
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetProductComment(RequestProductCommentModel requestModel)
        {
            ResponseProductCommentModel responseModel = new ResponseProductCommentModel();
            responseModel.flag = 0;
            responseModel.msg = "失败";
            try
            {
                if (requestModel != null && requestModel.productId != null && requestModel.productId > 0)
                {
                    SearchSP_ProductCommentModel SPProductCommentModel = new SearchSP_ProductCommentModel();

                    SPProductCommentModel = new SearchSP_ProductCommentModel();
                    SPProductCommentModel.ProductId = requestModel.productId;
                    SPProductCommentModel.PagedIndex = requestModel.PagedIndex;
                    SPProductCommentModel.PagedSize = requestModel.PagedSize != 0 ? requestModel.PagedSize : 10;
                    SPProductCommentModel.LanguageID = requestModel.lang != 0 ? requestModel.lang : 1;
                    SPProductCommentModel.CheckStatus = 2;
                    SPProductCommentModel.typeLevel = requestModel.typeLevel;
                    ResultModel remodel = _spProductCommentService.GetSP_ProductCommentList(SPProductCommentModel);

                    responseModel.rs = remodel.Data;
                    responseModel.flag = remodel.IsValid == true ? 1 : 0;
                    responseModel.msg = remodel.IsValid == true ? "成功" : "失败";
                    responseModel.TotalCount = remodel.Data.TotalCount;
                    int AllCount = _spProductCommentService.GetPaingCommentCount(requestModel.productId.Value, -1).Data;
                    responseModel.AllCount = AllCount;
                    responseModel.goodCount = "100%";
                    responseModel.midCount = "0%";
                    responseModel.badCount = "0%";

                    if (requestModel.productId > 0 && AllCount > 0)
                    {
                        //1.  好评：所有好评数/该商品的所有评价数，无评价时，默认100%
                        //2.  中评：所有中评数/该商品的所有评价数，无评价时，默认0%
                        //3.  差评：所有差评数/该商品的所有评价数，无评价时，默认0%
                        //好评：>=4颗星
                        //中评：>=2颗星
                        //差评：>=0颗星

                        int sum = AllCount;
                        //好评：>=4颗星
                        int goodCount = _spProductCommentService.GetPaingCommentCount(requestModel.productId.Value,1).Data;
                        responseModel.GCount = goodCount;
                        //中评：>=2颗星
                        int midCount = _spProductCommentService.GetPaingCommentCount(requestModel.productId.Value, 2).Data;
                        responseModel.MCount = midCount;
                        //差评：>=0颗星
                        int badCount = _spProductCommentService.GetPaingCommentCount(requestModel.productId.Value, 3).Data;
                        responseModel.BCount = badCount;
                        if (sum > 0)
                        {
                            if (goodCount >= 0)
                            {
                                responseModel.goodCount = (((double)goodCount / sum) * 100).ToString("f0")+"%";
                            }
                            if (midCount >= 0)
                            {
                                responseModel.midCount = (((double)midCount / sum) * 100).ToString("f0")+"%";
                            }
                            if (badCount >= 0)
                            {
                                responseModel.badCount = (((double)badCount / sum) * 100).ToString("f0")+"%";
                            }
                        }
                    }

                }
                else
                {
                    responseModel.flag = 0;
                    responseModel.msg = "请输入正确参数";
                }
            }
            catch (Exception ex)
            {
                responseModel.msg = ex.ToString();
            }
            return this.Ok(responseModel);
        }



        #endregion

        #region 4.7.提交咨询 (朱志容)
        /// <summary>
        /// 提交商品咨询 （朱志容）
        /// </summary>
        /// <param name="consult"></param>
        /// <returns></returns>
         [HttpPost]
        public IHttpActionResult ConsultSubmit(RequestConsultSubmit consult)
        {
            ApiResultModel response = new ApiResultModel();
            if (consult == null)
            {
                response.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", 3);//参数错误
                return Ok(response);
            }
            try
            {
                var userId = _enctyptionService.RSADecrypt(consult.userId);
                if (userId == "")
                {
                    response.msg = CultureHelper.GetAPPLangSgring("USER_ID_IllEGAL", consult.lang);//用户ID不合法
                    return Ok(response);
                }

                if (string.IsNullOrEmpty(consult.content))
                {
                    response.msg = CultureHelper.GetAPPLangSgring("CONTENT_NOT_EMPTY", consult.lang);//内容不能为空
                    return Ok(response);
                }
                var checkResult = _productService.GetProduct(consult.productId, consult.lang);//查询商品ID是否合法
                List<HKTHMall.Domain.AdminModel.Models.Products.ProductModel> list = checkResult.Data;
                if (list.Count() == 0)
                {
                    response.msg = CultureHelper.GetAPPLangSgring("GOODS_ID_IllEGAL", consult.lang);//商品ID不合法
                    return Ok(response);
                }
                HKTHMall.Domain.AdminModel.ProductConsult pConsult = new Domain.AdminModel.ProductConsult()
                {
                    UserID = Convert.ToInt64(userId),
                    ProductId = Convert.ToInt64(consult.productId),
                    ConsultContent = consult.content,
                    ConsultDT = DateTime.Now,
                    IsAnonymous = consult.isanonymous ? 1 : 0,
                    CheckStatus = 1
                };
                var resultModel = _productConsultService.AddConsult(pConsult);
                if (resultModel.IsValid)
                {
                    response.flag = 1;
                    response.msg = CultureHelper.GetAPPLangSgring("SUCCESS", consult.lang);//商品咨询提交成功
                }
                else
                {
                    response.msg = CultureHelper.GetAPPLangSgring("FAILURE", consult.lang);//商品咨询提交失败
                    Logger.Debug("商品咨询提交失败!" + resultModel.Message);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.msg = CultureHelper.GetAPPLangSgring("FAILURE", consult.lang);// "批量提交订单评论失败!" + ex;
                Logger.Debug("商品咨询提交异常!" + ex);
                return Ok(response);
            }
        }
        #endregion

        #region 4.8 咨询列表(刘泉)
         [HttpPost]
        public IHttpActionResult ConsultList(RequestProductConsultModel model)
        {

            ApiPagingResultModel response = new ApiPagingResultModel();
            ResponseProductConsoltModel rs = new ResponseProductConsoltModel();
            try
            {
                if (model == null)
                {
                    response.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang);    //参数错误  
                    return Ok(response);
                }
                SearchConsle sc = new SearchConsle();
                sc.languageID = model.lang;
                sc.Page = model.pageNo;
                sc.PageSize = model.pageSize;
                sc.productId = model.productId;
                int total = 0;
                var result = _productConsultService.GetConsulList(sc, out total);
                List<ProductConsultModel> pageList = result.Data as List<ProductConsultModel>;

                List<ResponseProductConsoltModel> list = new List<ResponseProductConsoltModel>();
                if (pageList != null)
                {
                    foreach (var item in pageList)
                    {
                        ResponseProductConsoltModel pro = new ResponseProductConsoltModel()
                        {
                            userID = item.UserID,
                            nickName = item.IsAnonymous == 0 ? item.NickName : GetFormatName(item.Phone),
                            answer = item.ReplyContent,
                            question = item.ConsultContent,
                            consultDt =  ConvertsTime.DateTimeToTimeStamp(item.ConsultDT)

                        };
                        list.Add(pro);
                    }
                }
                response.totalSize = total;
                if (result.IsValid)
                {
                    response.flag = 1;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", model.lang);//查询成功
                }
                else
                {
                    response.flag = 0;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", model.lang);//查询失败
                }
                response.rs = list;
            }
            catch (Exception ex)
            {
                response.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang); //"根据关键字获取产品异常!" + ex;
                NLogHelper.GetCurrentClassLogger().Error(string.Format("类{0}中方法{1}执行失败,参数为{2},失败信息为{3}", this.GetType().FullName, "ConsultList", JsonConvert.SerializeObject(model), ex.ToString()));
            }
            return Ok(response);
        }

        /// <summary>
        /// 匿名处理
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
         private string GetFormatName(string p)
         {
             StringBuilder str = new StringBuilder();
             if (p.Length > 4)
             {
                 for (int i = 0; i < p.Length; i++)
                 {
                     if (i == 0 || i == 1 || i == p.Length-1 || i == p.Length - 2)
                     {
                         str.Append(p[i]);
                     }
                     else {
                         str.Append("*");
                     }
                 }
             }
             return str.ToString();
         }
        #endregion
    }

}