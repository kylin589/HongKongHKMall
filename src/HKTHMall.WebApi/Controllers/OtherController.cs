
using FluentValidation;
using HKSJ.Common.FastDFS;
using HKSJ.MidMessage.Protocol;
using HKSJ.MidMessage.Services;
using HKTHMall.Core;
using HKTHMall.Core.Utils;
using HKTHMall.Core.Security;
using HKTHMall.Domain.AdminModel.Models.banner;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.AdminModel.Models.Keywork;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.banner;
using HKTHMall.Domain.WebModel.Models.Index;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Services.AC;
using HKTHMall.Services.Banner;
using HKTHMall.Services.Keywork;
using HKTHMall.Services.Products;
using HKTHMall.Services.Version;
using HKTHMall.WebApi.Common;
using HKTHMall.WebApi.Models;
using HKTHMall.WebApi.Models.Request;
using HKTHMall.WebApi.Models.Result;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using HKTHMall.Services.Common;
using HKTHMall.Core.Extensions;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.WebApi.Controllers
{
    public class OtherController : BaseController
    {

        private IbannerService _IBannerService;
        private IProductRuleService _IProductRuleService;
        private IProductService _IProductService;
        private ICategoryService _ICategoryService;
        private ITHAreaService _ThAreaService;
        private IFloorConfigService _IFloorConfigService;
        private IbannerProductService _IBannerProductService;
        private IFloorKeywordService _IFloorKeywordService;
        private IVersionService _IVersionService;
        private readonly IEncryptionService enctyptionService;
        public OtherController(IbannerService ibannerService, IProductRuleService iproductRuleService,
            IProductService iProductService, ICategoryService categoryService, ITHAreaService thAreaService,
            IFloorConfigService iFloorConfigService, IbannerProductService iBannerProductService, IFloorKeywordService iFloorKeywordService,IVersionService iVersionService,IEncryptionService enctyptionService)
        {
            _IBannerService = ibannerService;
            _IProductRuleService = iproductRuleService;
            _IProductService = iProductService;
            _ICategoryService = categoryService;
            _ThAreaService = thAreaService;
            _IFloorConfigService = iFloorConfigService;
            _IBannerProductService = iBannerProductService;
            _IFloorKeywordService = iFloorKeywordService;
            _IVersionService=iVersionService;
            this.enctyptionService = enctyptionService;
        }

        #region 9.1.banner广告首页顶端广告栏（刘泉）
        /// <summary>       
        /// 9.1.banner广告首页顶端广告栏（刘泉）
        /// </summary>       
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetBannerList(RequestLang model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            apiResult.rs = new List<BannerResult>();
            if (model == null)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR",(int) LanguageType.defaultLang);    //参数错误  
            }
            else
            {
                try
                {
                    ///获取推广广告
                    result = _IBannerService.GetTopBanner(5, 5, 0);
                    if (result.IsValid)
                    {
                        apiResult.flag = 1;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", model.lang);  //操作成功
                        List<bannerModel> re = result.Data as List<bannerModel>;
                        apiResult.rs = re.Select(s => new BannerResult()
                        {
                            BannerId = s.bannerId,
                            BannerName = s.bannerName,
                            BannerPic =HtmlExtensions.GetThumbsImage(ImagePath + s.bannerPic,640,256),
                            BannerUrl = s.bannerUrl,
                            IdentityStatus = s.IdentityStatus,
                            PlaceCode = s.PlaceCode,
                            Sorts = s.Sorts,
                            productId = s.ProductId
                        }).ToList<BannerResult>();

                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);//服务器异常 
                    }

                }
                catch (Exception ex)
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);//服务器异常 
                    Logger.Write("EmLog", string.Format("类{0}中方法{1}执行失败,参数为{2},失败信息为{3}", this.GetType().FullName, "GetBannerList", "", ex.ToString()));
                }
            }
            var jsonResult = JsonConvert.SerializeObject(apiResult);
            return new HttpResponseMessage { Content = new StringContent(jsonResult, System.Text.Encoding.UTF8, "application/json") };
        }
        #endregion

        #region 9.2.限时爆款（首页爆款3个商品,和爆款列表方法已经合并）（刘泉）
        /// <summary>
        /// 9.2.限时爆款（首页爆款3个商品,和爆款列表方法已经合并）（刘泉）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetExplosiveGoods(RequestExplosiveGoods model)
        {
            var result = new ResultModel();
            var apiResult = new ApiPagingResultModel();
            ExplosiveResult r = new ExplosiveResult();
            r.aboutToStart = new List<ExplosiveGoodsResult>();
            r.inProgressList = new List<ExplosiveGoodsResult>();
            apiResult.rs = r;
            if (model == null)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang);    //参数错误  
            }
            else if (model.lang <= 0 || model.lang > 3)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang);    //参数错误    
            }
            else
            {
                try
                {
                    result = _IProductRuleService.GetIndexExplosionForApi(model.lang);
                    if (result.IsValid)
                    {
                        List<IndexExplosion> list = result.Data as List<IndexExplosion>;
                        List<ExplosiveGoodsResult> lst = list.Select(s => new ExplosiveGoodsResult()
                        {
                            productId = s.ProductId,
                            productName = s.ProductName,
                            hKPrice = s.HKPrice,
                            discount = s.Discount,
                            marketPrice = s.MarketPrice,
                            picAddress = ImagePath + s.PicAddress,
                            activityPrice = s.ActivityPrice,
                            starDate = ConvertsTime.DateTimeToTimeStamp(s.StarDate),
                            endDate = ConvertsTime.DateTimeToTimeStamp(s.EndDate),
                            serverDt = ConvertsTime.DateTimeToTimeStamp(DateTime.Now)

                        }).OrderBy(i => i.sorts).ToList<ExplosiveGoodsResult>();
                        if (lst.Count > 0)
                        {
                            long nowDate=ConvertsTime.DateTimeToTimeStamp(DateTime.Now);
                            int cnt = lst.Count();
                            List<ExplosiveGoodsResult> ip = lst.Where(x => x.starDate <= nowDate).ToList<ExplosiveGoodsResult>();
                            List<ExplosiveGoodsResult> ats = lst.Where(x => x.starDate > nowDate).ToList<ExplosiveGoodsResult>();

                            int cntats = ats.Count();
                            int cntip = ip.Count();
                            if (model.isFirstPage == 0)//首页>
                            {
                                if (cntats > 0)
                                {
                                    ats = ats.Take(model.bannerSize < cnt ? model.bannerSize : cnt).ToList<ExplosiveGoodsResult>();
                                    ///计算时间间隔差距,以及活动价格
                                    ats.ForEach(a =>
                                    {
                                        a.picAddress = HtmlExtensions.GetThumbsImage(a.picAddress, 254, 178);

                                    });
                                }
                                if (cntip > 0)
                                {
                                    ip = ip.Take(model.bannerSize < cnt ? model.bannerSize : cnt).ToList<ExplosiveGoodsResult>();
                                 
                                    ip.ForEach(a =>
                                    {
                                        a.picAddress = HtmlExtensions.GetThumbsImage(a.picAddress, 254, 178);

                                    });
                                }
                                r.ipTotal = cntip;
                                r.atsTotal = cntats;
                                r.inProgressList = ip;
                                r.aboutToStart = ats;
                                apiResult.rs = r;
                            }
                            else//列表
                            {
                                #region 即将进行
                                int tc = 0;//总页数
                                if (cntats % model.pageSize == 0)
                                {
                                    tc = cntats / model.pageSize;
                                }
                                else
                                {
                                    tc = cntats / model.pageSize + 1;
                                }
                                if (tc >= model.pageNo)
                                {
                                    //判断最后一页的个数
                                    int pSize = model.pageSize * (model.pageNo - 1) + model.pageSize + 1 > cntats ? cntats - model.pageSize * (model.pageNo - 1) : model.pageSize;
                                    r.aboutToStart = ats.GetRange(model.pageSize * (model.pageNo - 1), pSize);
                                }
                                r.atsTotal = cntats;
                                #endregion
                                #region 进行中
                                int tcip = 0;//总页数
                                if (cntip % model.pageSize == 0)
                                {
                                    tcip = cntip / model.pageSize;
                                }
                                else
                                {
                                    tcip = cntip / model.pageSize + 1;
                                }
                                if (tcip >= model.pageNo)
                                {
                                    //判断最后一页的个数
                                    int pSize = model.pageSize * (model.pageNo - 1) + model.pageSize + 1 > cntip ? cntip - model.pageSize * (model.pageNo - 1) : model.pageSize;
                                    r.inProgressList = ip.GetRange(model.pageSize * (model.pageNo - 1), pSize);
                                }
                                r.ipTotal = cntip;
                                #endregion
                                apiResult.rs = r;
                            }
                            apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", model.lang); //操作成功
                        }
                        else
                        {
                            apiResult.totalSize = 0;
                            apiResult.msg = CultureHelper.GetAPPLangSgring("NO_DATA", model.lang); //暂无数据
                        }
                        apiResult.flag = 1;
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);    //参数错误        
                    }
                }
                catch (Exception ex)
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);   //服务器异常         
                    Logger.Write("EmLog", string.Format("类{0}中方法{1}执行失败,参数为{2},失败信息为{3}", this.GetType().FullName, "GetExplosiveGoods", JsonConvert.SerializeObject(model), ex.ToString()));
                }
            }
            var jsonResult = JsonConvert.SerializeObject(apiResult);
            return new HttpResponseMessage { Content = new StringContent(jsonResult, System.Text.Encoding.UTF8, "application/json") };//服务器异常 
        }
        #endregion

        #region 9.3.首页商品分类（刘泉）
        /// <summary>
        /// 9.3.首页商品分类（刘泉）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetProductClassification(RequestLang model)
        {
            var apiResult = new ApiResultModel();
            if (model == null)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang);    //参数错误  
            }
            else if (model.lang <= 0 || model.lang > 3)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang);    //参数错误    
            }
            else
            {
                try
                {
                    //获取楼层分类信息
                    FloorConfigModel categoryIdList = _IFloorConfigService.GetModelById(1).Data;
                    string[] categoryStringList = categoryIdList.CateIdStr.Split(',');
                    ///加载数据楼层数据
                    List<IndexFloorInfo> InfoList = new List<IndexFloorInfo>();
                    if (categoryStringList.Length > 0)
                    {
                        List<FirstFloorResult> retList = new List<FirstFloorResult>();
                        foreach (var s in categoryStringList)
                        {
                            FirstFloorResult ret = new FirstFloorResult();
                            ret.bannerList = new List<FirstFloorBanner>();
                            ret.bannerProductList = new List<FirstFloorBannerProduct>();
                            if (!string.IsNullOrEmpty(s))
                            {
                                int categoryId = int.Parse(s);
                                ResultModel rmModel = _ICategoryService.GetCateByIdForApi(categoryId, model.lang);
                                CategorysModel infomodel = rmModel.Data.Count > 0 ? rmModel.Data[0] : null;
                                //楼层类型信息
                                if (infomodel != null)
                                {
                                    ret.categoryId = infomodel.CategoryId;
                                    ret.categoryName = infomodel.CategoryName;
                                    ret.place = infomodel.Place;
                                }
                                else
                                {
                                    continue;
                                }
                                //获取楼层左侧推荐数据
                                ResultModel bsModel = _IBannerService.GetTopBannerForApp(2, 6, categoryId);
                                List<bannerModel> bsList = bsModel.Data.Count > 0 ? bsModel.Data : null;
                                if (bsList != null)
                                {
                                     ret.bannerList = bsList.Select(bl => new FirstFloorBanner()
                                    {
                                        bannerName = bl.bannerName,
                                        bannerPic =  ImagePath + bl.bannerPic,
                                        bannerUrl = bl.bannerUrl,
                                        productId = bl.ProductId

                                    }).ToList<FirstFloorBanner>();
                                }
                                //else
                                //{
                                //    continue;
                                //}
                                //获取楼层右侧商品
                                ResultModel bpsModel = _IBannerProductService.GetTopBannerForApi(4, 3, categoryId,model.lang);
                                List<bannerProductModel> bpsList = bpsModel.Data.Count > 0 ? bpsModel.Data : null;
                                if (bpsList != null)
                                {
                                    ret.bannerProductList = bpsList.Select(bpl => new FirstFloorBannerProduct()
                                    {
                                        productId = bpl.productId,
                                        productName = bpl.ProductName,
                                        hKPrice = bpl.HKPrice,
                                        picAddress = ImagePath + bpl.PicAddress,                                        
                                        categoryId = bpl.CategoryId

                                    }).ToList<FirstFloorBannerProduct>();
                                }
                                //else
                                //{
                                //    continue;
                                //}
                                retList.Add(ret);
                            }
                        }
                        apiResult.rs = retList;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", model.lang); //操作成功
                    }
                    else
                    {
                        apiResult.msg = CultureHelper.GetAPPLangSgring("NO_DATA", model.lang); //操作成功
                    }
                    apiResult.flag = 1;
                }
                catch (Exception ex)
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);   //服务器异常         
                    Logger.Write("EmLog", string.Format("类{0}中方法{1}执行失败,参数为{2},失败信息为{3}", this.GetType().FullName, "GetProductClassification", JsonConvert.SerializeObject(model), ex.ToString()));
                }
            }
            var jsonResult = JsonConvert.SerializeObject(apiResult);
            return new HttpResponseMessage { Content = new StringContent(jsonResult, System.Text.Encoding.UTF8, "application/json") };//服务器异常 
        }
        #endregion

        #region 9.4.猜你喜欢（猜你喜欢首页随机10个）（刘泉）
        /// <summary>
        /// 9.4.猜你喜欢（猜你喜欢首页随机10个）（刘泉）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Other/GtGuessLike")]
        public HttpResponseMessage GtGuessLike(RequestLang model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            apiResult.rs = new List<GuessLikeResult>();
            if (model == null)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang);    //参数错误  
            }
            else if (model.lang <= 0 || model.lang > 3)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang);    //参数错误    
            }
            else
            {
                try
                {   //获取惠卡推广数据
                    var productData = _IProductService.GetTopRecommendForApi(model.lang);
                    if (productData.IsValid)
                    {
                        List<dynamic> productList = productData.Data;
                        if (productList != null)
                        {
                            List<GuessLikeResult> data = productList.Take(10)
                                .Select(s => new GuessLikeResult()
                            {
                                productId = s.ProductId,
                                productName = s.ProductName,
                                picAddress =HtmlExtensions.GetThumbsImage( ImagePath + s.PicUrl,268,268),
                                hKPrice = s.HKPrice,
                                marketPrice = s.MarketPrice,
                                discount = s.Discount,
                                starDate = s.StarDate,
                                endDate = s.EndDate
                                
                            }).ToList<GuessLikeResult>();
                            if (data.Count() > 0)
                            {
                                ///计算时间间隔差距,以及活动价格
                                data.ForEach(a =>
                                {
                                    a.isActivity = (a.endDate > DateTime.Now && a.discount!=0) ? true : false;
                                    a.activityPrice = (a.hKPrice * a.discount);
                                });
                            }
                            apiResult.rs = data;
                            apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", model.lang); //操作成功
                            apiResult.flag = 1;
                        }
                        else
                        {
                            apiResult.msg = CultureHelper.GetAPPLangSgring("NO_DATA", model.lang); //操作成功
                            apiResult.flag = 1;
                        }
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);    //参数错误
                    }
                }
                catch (Exception ex)
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);   //服务器异常         
                    Logger.Write("EmLog", string.Format("类{0}中方法{1}执行失败,参数为{2},失败信息为{3}", this.GetType().FullName, "GtGuessLike", JsonConvert.SerializeObject(model), ex.ToString()));
                }
            }
            var jsonResult = JsonConvert.SerializeObject(apiResult);
            return new HttpResponseMessage { Content = new StringContent(jsonResult, System.Text.Encoding.UTF8, "application/json") };//服务器异常 
        }
        #endregion

        #region 9.5.筛选（商品分类）（刘泉）
        /// <summary>
        /// 9.5.筛选（商品分类）（刘泉）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetCategoryList(RequestLang model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            apiResult.rs = new List<CategoryListResult>();
            if (model == null)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang);    //参数错误  
            }
            else if (model.lang <= 0 || model.lang > 3)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang);    //参数错误    
            }
            else
            {
                try
                {
                    result = _ICategoryService.GetWebAll(model.lang);
                    if (result.IsValid)
                    {
                        List<CategorysModel> list = result.Data as List<CategorysModel>;
                        apiResult.flag = 1;
                        if (list.Count() > 0)
                        {
                            List<CategoryListResult> lst = list.Select(s => new CategoryListResult()
                            {
                                categoryID = (int)s.CategoryId,
                                categoryName = s.CategoryName,                               
                                parentId = s.parentId,
                                childNode = new List<CategoryListResult>()
                            }).ToList<CategoryListResult>();
                            apiResult.rs = CreateTree(lst, 0);
                            apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", model.lang); //操作成功
                        }
                        else
                        {
                            apiResult.msg = CultureHelper.GetAPPLangSgring("NO_DATA", model.lang); //暂无数据
                        }
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);   //服务器异常
                    }
                }
                catch (Exception ex)
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);   //服务器异常         
                    Logger.Write("EmLog", string.Format("类{0}中方法{1}执行失败,参数为{2},失败信息为{3}", this.GetType().FullName, "GetCategoryList", JsonConvert.SerializeObject(model), ex.ToString()));
                }
            }
            var jsonResult = JsonConvert.SerializeObject(apiResult);
            return new HttpResponseMessage { Content = new StringContent(jsonResult, System.Text.Encoding.UTF8, "application/json") };//服务器异常 
        }
        #endregion

        #region 9.6.首页搜索默认关键字（马锋）
        /// <summary>
        /// 9.6.首页搜索默认关键字（马锋）
        /// </summary>
        /// <param name="request">输入参数</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetKeywordList(RequestGetKeywordList request)
        {
            //统一返回类型
            ApiResultModel result = new ApiResultModel();
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang);//参数不能为空
                return Ok(result);
            }
            //输入参数验证          
            var r = _IFloorKeywordService.GetTopList(request.lang, 100000);
            if (r.IsValid)
            {
                result.flag = 1;
                List<FloorKeywordModel> fkm = r.Data as List<FloorKeywordModel>;
                List<ResponseKeywordList> reu = new List<ResponseKeywordList>();
                fkm = r.Data as List<FloorKeywordModel>;
                foreach (var item in fkm)
                {
                    ResponseKeywordList a = new ResponseKeywordList();
                    a.keyWordName = item.KeyWordName;
                    reu.Add(a);
                }
                result.rs = reu;
            }
            result.msg = "";
            return Ok(result);
        }
        #endregion

        #region 9.7.惠卡推荐（朱志容）
        /// <summary>
        /// 9.7.惠卡推荐（朱志容）
        /// </summary>
        /// <param name="lang">语言:1:中文,2:英文,3:泰文</param>
        /// <param name="referrerSize">推荐商品数量</param>
        /// <returns></returns>
        [HttpPost]        
        public IHttpActionResult GetRecommendList(RequestRecommendModel recommend)
        {
            ApiPagingResultModel response = new ApiPagingResultModel();
            try
            {
                if (recommend == null)
                {
                    response.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang); //参数不能为空
                    return Ok(response);
                }
                //获取惠卡推广数据
                var productData = _IProductService.GetTopRecommendForApi(recommend.lang);
                List<dynamic> productList = productData.Data;
                if (productList != null)
                {
                    productList.ForEach(a =>
                    {
                        a.Guid = System.Guid.NewGuid().ToString();
                    });
                    var data = productList.OrderByDescending(a => a.Guid).Take(recommend.referrerSize);
                    productList = data.ToList();
                }              
                List<ResponseProductModel> list = new List<ResponseProductModel>();
                foreach (var item in productList)
                {
                    ResponseProductModel pro = new ResponseProductModel()
                    {
                        hKPrice = (float)item.HKPrice,
                        marketPrice = (float)item.MarketPrice,
                        picAddress = ImagePath + item.PicUrl,
                        productId = item.ProductId,
                        productName = item.ProductName,
                        isActivity=(item.EndDate > DateTime.Now && item.Discount != 0) ? true : false,
                        activityPrice = item.HKPrice * item.Discount
                    };
                    list.Add(pro);
                }
                response.totalSize = list.Count;
                response.rs = list;
                if (productData.IsValid)
                {
                    response.flag = 1;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", recommend.lang);//查询成功
                }
                else
                {
                    response.flag = 0;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", recommend.lang);//查询失败
                }
            }
            catch (Exception ex)
            {
                response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", recommend.lang); //"获取惠卡推荐异常!" + ex;
                NLogHelper.GetCurrentClassLogger().Error("获取惠卡推荐异常!" + ex);
            }
            return Ok(response);
            //var jsonStr = JsonConvert.SerializeObject(response);
            //return new HttpResponseMessage { Content = new StringContent(jsonStr, Encoding.UTF8, "application/json") };
        }
        #endregion

        #region 9.8.地区信息列表（刘泉）
        /// <summary>
        /// 9.8.地区信息列表（刘泉）
        /// </summary>
        /// <returns></returns>
        [HttpPost]      
        public HttpResponseMessage GetAddressList(RequestLang model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            apiResult.rs = new { };
            if (model == null)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang);    //参数错误  
            }
            else if (model.lang <= 0 || model.lang > 3)
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang);    //参数错误    
            }
            else
            {
                try
                {
                    result = _ThAreaService.GetTHAreaByLanguageIdToTreeApi(model.lang);
                    if (result.IsValid)
                    {
                        apiResult.rs = result.Data;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", model.lang); //操作成功
                        apiResult.flag = 1;
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);//服务器异常
                    }
                }
                catch (Exception ex)
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);   //服务器异常         
                    Logger.Write("EmLog", string.Format("类{0}中方法{1}执行失败,参数为{2},失败信息为{3}", this.GetType().FullName, "GetAddressList", JsonConvert.SerializeObject(model), ex.ToString()));
                }
            }
            var jsonResult = JsonConvert.SerializeObject(apiResult);
            return new HttpResponseMessage { Content = new StringContent(jsonResult, System.Text.Encoding.UTF8, "application/json") };//服务器异常 
        }
        #endregion

        #region 9.9.上传头像图片（马锋）
        /// <summary>
        /// 9.9.上传头像图片（马锋）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UploadImage()
        {
            NameValueCollection nameValues = HttpContext.Current.Request.Form;
            ApiResultModel result = new ApiResultModel();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (!Request.Content.IsMimeMultipartContent() || nameValues.Count == 0)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.zh_CN);//参数不能为空
                string str = serializer.Serialize(result);
                HttpResponseMessage hrm = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
                return hrm;
            }
            long userId = 0;
            if (nameValues["userId"] != null)
            {
                long.TryParse(enctyptionService.RSADecrypt(nameValues["userId"].ToString()), out userId);
            }
            var provider = new MultipartMemoryStreamProvider();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var item in provider.Contents)
                {
                    if (item.Headers.ContentDisposition.FileName != null)
                    {
                        Stream stream = await item.ReadAsStreamAsync();
                        Byte[] buffer = new Byte[stream.Length];
                        //从流中读取字节块并将该数据写入给定缓冲区buffer中
                        stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                        stream.Close();
                        stream.Dispose();

                        ResponseUploadImage ui = new ResponseUploadImage();
                        string extension = item.Headers.ContentDisposition.FileName.Substring(item.Headers.ContentDisposition.FileName.IndexOf('.') + 1, item.Headers.ContentDisposition.FileName.Length - item.Headers.ContentDisposition.FileName.IndexOf('.') - 2);
                        //ui.imageUrl = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, buffer, extension);   // ConfigurationManager.AppSettings["ImagePath"].ToString() 

                        RequestASFileData mfile = new RequestASFileData();
                        mfile.ext = extension;
                        mfile.type = 1;   //1:图片,2:语音,3:视频,4:文件  
                        mfile.tag = 1;//文件数据tlv对应tag值(写死)
                        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                        byte[] md5byte = md5.ComputeHash(buffer);
                        mfile.digest = System.BitConverter.ToString(md5byte).Replace("-", ""); //文件摘要
                        mfile.size = buffer.Length; //文件
                        RequestASFileUpload reqASFileUpInfo = new RequestASFileUpload();
                        reqASFileUpInfo.uid = userId;
                        reqASFileUpInfo.file = new RequestASFileData[] { mfile };
                        reqASFileUpInfo.type = 1; //1:MD5(标记加密方式)
                        reqASFileUpInfo.modle = 3; //1和2:IM模块,3:帐号模块
                        ResponseASFileUpload code = EmMethodManage.EmFASTDFSManageInstance.ASFileUpload(reqASFileUpInfo, buffer);
                        if (code.isOK)
                        {
                            result.flag = 1;
                            result.msg = "";
                            ui.imageUrl = code.rt[0].url;
                            result.rs = ui;
                        }
                        else
                        {
                            result.msg = code.ErrorMsg;
                        }
                        result.rs = ui;
                    }
                }
                string str = serializer.Serialize(result);
                HttpResponseMessage hrm = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
                return hrm;
            }
            catch (System.Exception e)
            {
                result.flag = 0;
                result.msg = e.Message;
                string str = serializer.Serialize(result);
                HttpResponseMessage hrm = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
                return hrm;
            }
        }
        #endregion

        #region 9.10.获取软件版本信息(朱志容)
        /// <summary>
        /// 9.10.获取软件版本信息(朱志容)
        /// </summary>
        /// <remarks>yaodunpei</remarks>
        /// <param name="packageName">包名</param>
        /// <param name="versionNo">版本编号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Other/GetVersionInfo")]
        public IHttpActionResult GetVersionInfo(RequestGetVersionInfo requestParam)
        {
            ApiResultModel result = new ApiResultModel();
            try
            {
                ResponseGetVersionInfo response = new ResponseGetVersionInfo();
                if (requestParam == null || string.IsNullOrEmpty(requestParam.packageName) || string.IsNullOrEmpty(requestParam.versionNo))
                {
                    result.flag = 0;
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang);// "参数有误";
                    return Ok(result);
                }
                int isForceUpdate = 0;
                requestParam.versionNo = requestParam.versionNo.Replace(".", string.Empty);
                int nVersionNo = Utils.StrToInt(requestParam.versionNo, -1);
                if (nVersionNo < 1)
                {
                    result.flag = 0;
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", requestParam.lang); //"版本号有误";
                    return Ok(result);
                }
                var appVersion = _IVersionService.GetMaxVersionNo(requestParam.packageName, requestParam.versionNo);
                if (appVersion == null)
                {
                    result.flag = 0;
                    result.msg = CultureHelper.GetAPPLangSgring("VERSION_NOT", requestParam.lang);// "无最新版本";
                    return Ok(result);
                }
                int toVersionNo = 0;
                if (!string.IsNullOrEmpty(appVersion.VersionNO))
                {
                    toVersionNo = Utils.StrToInt(appVersion.VersionNO, -1);
                }
                isForceUpdate = appVersion.IsForceUpdate;
                //用户版本如果是最新则无需强制更新
                if (nVersionNo >= toVersionNo)
                {
                    isForceUpdate = 0;
                }
                response.appName = appVersion.APPName;
                response.downloadURL = ConfigurationManager.AppSettings["ImagePath"] + appVersion.DownloadURL;
                response.fileSize = appVersion.FileSize;
                response.isForceUpdate = isForceUpdate.ToString();
                response.packageName = appVersion.PackageName;
                switch (requestParam.lang)
                {
                    case 1:
                        response.updateInfo = appVersion.UpdateInfo;
                        break;
                    case 2:
                        response.updateInfo = appVersion.UpdateInfoEN;
                        break;
                    case 3:
                        response.updateInfo = appVersion.UpdateInfoTH;
                        break;
                    default:
                        response.updateInfo = appVersion.UpdateInfoTH;//默认泰文
                        break;
                }               
                response.versionName = appVersion.VersionName;
                response.versionNo = appVersion.VersionNO;
                result.rs = response;
                result.flag = 1;
                result.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", requestParam.lang);//查询成功
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.flag = 0;
                result.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", requestParam.lang);// "获取版本更新异常!" + ex;
                NLogHelper.GetCurrentClassLogger().Error("获取版本更新异常!" + ex);
                return Ok(result);
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        ///     递归创建树
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<CategoryListResult> CreateTree(List<CategoryListResult> categories, int parentId)
        {
            List<CategoryListResult> list = categories.FindAll(m => m.parentId == parentId);

            List<CategoryListResult> childNode = new List<CategoryListResult>(); ;

            if (list.Any())
            {
                childNode = list.Select(m => new CategoryListResult()
                {
                    categoryID = m.categoryID,
                    categoryName = m.categoryName,
                    backcolor = m.backcolor,
                    parentId = m.parentId,
                    childNode = CreateTree(categories, m.categoryID)
                }).ToList<CategoryListResult>();
            }

            return childNode;
        }
        #endregion

    }
}