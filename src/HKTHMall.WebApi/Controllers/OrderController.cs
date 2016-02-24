using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using BrCms.Framework.Logging;
using HKTHMall.Core;
using HKTHMall.Core.Extensions;
using HKTHMall.Core.Security;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Services.Orders;
using HKTHMall.Services.Orders.MQ;
using HKTHMall.Services.Products;
using HKTHMall.Services.ShoppingCart;
using HKTHMall.Services.Users;
using HKTHMall.Services.YHUser;
using HKTHMall.WebApi.Models.Request;
using HKTHMall.WebApi.Models.Result;
using Newtonsoft.Json;
using HKTHMall.Services.Common.MultiLangKeys;


namespace HKTHMall.WebApi.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IEncryptionService _enctyptionService;
        private readonly IFavoritesService _favoritesService;
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;
        private readonly IPaymentOrderService _paymentOrderService;
        private readonly IReturnProductInfoService _returnProductInfoService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUserAddressService _userAddressService;
        private readonly IYH_UserService _userService;

        public OrderController(IEncryptionService enctyptionService, IFavoritesService favoritesService, ILogger logger,
            IOrderService orderService, IPaymentOrderService paymentOrderService,
            IReturnProductInfoService returnProductInfoService, IShoppingCartService shoppingCartService,
            IUserAddressService userAddressService, IYH_UserService userService)
        {
            this._enctyptionService = enctyptionService;
            this._favoritesService = favoritesService;
            this._logger = logger;
            this._orderService = orderService;
            this._paymentOrderService = paymentOrderService;
            this._returnProductInfoService = returnProductInfoService;
            this._shoppingCartService = shoppingCartService;
            this._userAddressService = userAddressService;
            this._userService = userService;
        }

        #region 5.1.获取全部订单列表（周博）

        /// <summary>
        ///     5.1.获取全部订单列表（周博）
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetOrderList(RequestOrderModel requestModel)
        {
            var responseOrder = new ApiPagingResultModel();
            var resultmodel = new ResultModel();
            if (requestModel != null)
            {
                try
                {
                    var view = new SearchOrderView();
                    view.UserID = Convert.ToInt64(this._enctyptionService.RSADecrypt(requestModel.userId));
                    view.s = (OrderEnums.OrderStatus) requestModel.orderStatus;
                    view.LanguageID = requestModel.lang;
                    view.page = requestModel.pageNo;
                    view.pageSize = requestModel.pageSize;
                    view.d = OrderEnums.TimeSpanType.All;
                    //状状为10时查询待评价订单
                    if (requestModel.orderStatus == 10)
                    {
                        resultmodel = this._orderService.GetPagingEvaluationOrdersIntoWeb(view);
                    }
                    else
                    {
                        resultmodel = this._orderService.GetPagingOrdersIntoWeb(view);
                    }
                    responseOrder.flag = resultmodel.IsValid ? 1 : 0;
                    responseOrder.msg = resultmodel.IsValid
                        ? CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_DELETESUCCESS", requestModel.lang)
                        : CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", requestModel.lang);
                    responseOrder.totalSize = resultmodel.Data.TotalCount;
                    var arrayList = new ArrayList();
                    ArrayList detailsArrayList = null;
                    Dictionary<string, object> dictMain = null;
                    Dictionary<string, object> dictDetails = null;
                    if (resultmodel.Data != null)
                    {
                        foreach (OrderView orderView in resultmodel.Data)
                        {
                            detailsArrayList = new ArrayList();
                            dictMain = new Dictionary<string, object>();
                            dictMain.Add("orderID", orderView.OrderID);
                            dictMain.Add("orderStatus", orderView.OrderStatus);
                            dictMain.Add("merchantID", orderView.MerchantID);
                            dictMain.Add("shopName", orderView.YH_MerchantInfoView.ShopName);
                            dictMain.Add("expressMoney", orderView.ExpressMoney);
                            dictMain.Add("productNumber",
                                (orderView.OrderDetailViews.Count > 0 ? orderView.OrderDetailViews.Count : 0));
                            dictMain.Add("totalAmount", orderView.TotalAmount);
                            dictMain.Add("orderDate", DateTimeExtensions.DateTimeToUnixTimestamp(orderView.OrderDate));
                            dictMain.Add("payChannel", orderView.PayChannel);

                            if (orderView.OrderDetailViews.Count > 0)
                            {
                                foreach (var orderDetailsView in orderView.OrderDetailViews)
                                {
                                    dictDetails = new Dictionary<string, object>();
                                    dictDetails.Add("orderDetailsID", orderDetailsView.OrderDetailsID);
                                    dictDetails.Add("productId", orderDetailsView.ProductId);
                                    dictDetails.Add("skuId", orderDetailsView.SKU_ProducId);
                                    dictDetails.Add("productName", orderDetailsView.ProductName);
                                    dictDetails.Add("picUrl",
                                        HtmlExtensions.GetImagesUrl(orderDetailsView.PicUrl, 128, 128));
                                    dictDetails.Add("salesPrice", orderDetailsView.SalesPrice);
                                    dictDetails.Add("quantity", orderDetailsView.Quantity);
                                    dictDetails.Add("skuName", orderDetailsView.SkuName);
                                    dictDetails.Add("isComment", orderDetailsView.Iscomment);
                                    detailsArrayList.Add(dictDetails);
                                }
                            }
                            dictMain.Add("productArray", detailsArrayList);
                            arrayList.Add(dictMain);
                        }
                        responseOrder.rs = arrayList;
                    }
                }
                catch (Exception ex)
                {
                    responseOrder.flag = 0;
                    responseOrder.msg = CultureHelper.GetAPPLangSgring("HOME_SHOPPING_ERROR", requestModel.lang);
                }
            }
            else
            {
                responseOrder.flag = 0;
                responseOrder.msg = CultureHelper.GetAPPLangSgring("HOME_SHOPPING_ERROR", requestModel.lang);
            }
            var jsonStr = JsonConvert.SerializeObject(responseOrder);
            return new HttpResponseMessage
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "application/json")
            };
        }

        #endregion

        #region 5.2.订单详情（周博）

        /// <summary>
        ///     5.2.订单详情（周博）
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetOrderDetails(RequestOrderDetailsModel requestModel)
        {
            var responseOrder = new ApiPagingResultModel();
            if (requestModel != null)
            {
                try
                {
                    var view = new SearchOrderDetailView();
                    view.OrderID = requestModel.orderNumber;
                    view.UserID = Convert.ToInt64(this._enctyptionService.RSADecrypt(requestModel.userId));
                    view.LanguageID = requestModel.lang;
                    var resultmodel = this._orderService.GetOrderDetailIntoWebBy(view);
                    responseOrder.flag = resultmodel.IsValid ? 1 : 0;
                    responseOrder.msg = resultmodel.IsValid
                        ? CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_DELETESUCCESS", requestModel.lang)
                        : CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", requestModel.lang);
                    var arrayList = new ArrayList();
                    var dictMain = new Dictionary<string, object>();
                    Dictionary<string, object> dictDetails = null;

                    if (resultmodel.Data != null)
                    {
                        OrderView orderView = resultmodel.Data;
                        dictMain.Add("orderID", orderView.OrderID);
                        dictMain.Add("orderStatus", orderView.OrderStatus);
                        dictMain.Add("merchantID", orderView.MerchantID);
                        dictMain.Add("shopName", orderView.YH_MerchantInfoView.ShopName);
                        dictMain.Add("expressMoney", orderView.ExpressMoney);
                        dictMain.Add("productNumber",
                            (orderView.OrderDetailViews.Count > 0 ? orderView.OrderDetailViews.Count : 0));
                        dictMain.Add("totalAmount", orderView.TotalAmount);
                        dictMain.Add("orderDate", DateTimeExtensions.DateTimeToUnixTimestamp(orderView.OrderDate));
                        dictMain.Add("paidDate",
                            (orderView.PaidDate == null
                                ? ""
                                : DateTimeExtensions.DateTimeToUnixTimestamp(Convert.ToDateTime(orderView.PaidDate))
                                    .ToString()));
                        dictMain.Add("remark", orderView.Remark);
                        dictMain.Add("payType", orderView.PayType);
                        dictMain.Add("payChannel", orderView.PayChannel);

                        if (orderView.OrderAddressView != null)
                        {
                            dictMain.Add("receiverName", orderView.OrderAddressView.Receiver);
                            dictMain.Add("receiverAddress",
                                this._userAddressService.GetTHAreaAreaName(orderView.OrderAddressView.THAreaID,
                                    requestModel.lang).Data + orderView.OrderAddressView.DetailsAddress);
                            dictMain.Add("receiverPhone", orderView.OrderAddressView.Mobile);
                            dictMain.Add("receiverTelephone", orderView.OrderAddressView.Phone);
                            dictMain.Add("postalCode", orderView.OrderAddressView.PostalCode);
                        }
                        if (orderView.OrderDetailViews.Count > 0)
                        {
                            foreach (var orderDetailsView in orderView.OrderDetailViews)
                            {
                                dictDetails = new Dictionary<string, object>();
                                dictDetails.Add("orderDetailsID", orderDetailsView.OrderDetailsID);
                                dictDetails.Add("productId", orderDetailsView.ProductId);
                                dictDetails.Add("skuId", orderDetailsView.SKU_ProducId);
                                dictDetails.Add("productName", orderDetailsView.ProductName);
                                dictDetails.Add("picUrl", HtmlExtensions.GetImagesUrl(orderDetailsView.PicUrl, 128, 128));
                                dictDetails.Add("salesPrice", orderDetailsView.SalesPrice);
                                dictDetails.Add("quantity", orderDetailsView.Quantity);
                                dictDetails.Add("skuName", orderDetailsView.SkuName);
                                dictDetails.Add("isComment", orderDetailsView.Iscomment);
                                dictDetails.Add("isReturn", orderDetailsView.IsReturn);
                                arrayList.Add(dictDetails);
                            }
                        }
                        dictMain.Add("productArray", arrayList);
                    }
                    responseOrder.rs = dictMain;
                }
                catch (Exception ex)
                {
                    responseOrder.flag = 0;
                    responseOrder.msg = CultureHelper.GetAPPLangSgring("HOME_SHOPPING_ERROR", requestModel.lang);
                }
            }
            else
            {
                responseOrder.flag = 0;
                responseOrder.msg = CultureHelper.GetAPPLangSgring("HOME_SHOPPING_ERROR", requestModel.lang);
            }
            var jsonStr = JsonConvert.SerializeObject(responseOrder);
            return new HttpResponseMessage
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "application/json")
            };
        }

        #endregion

        #region 5.3.取消订单状态（周博）

        /// <summary>
        ///     5.3.取消订单状态（周博）
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateOrder(RequestOrderDetailsModel requestModel)
        {
            var responseOrder = new ApiResultModel();
            if (requestModel != null)
            {
                var resultModel = new ResultModel();
                var searchOrderDetailView = new SearchOrderDetailView();
                searchOrderDetailView.UserID = Convert.ToInt64(this._enctyptionService.RSADecrypt(requestModel.userId));
                searchOrderDetailView.OrderID = requestModel.orderNumber;
                searchOrderDetailView.OrderStatus = requestModel.orderStatus;
                searchOrderDetailView.LanguageID = requestModel.lang;
                resultModel = this._orderService.CancelOrderBy(searchOrderDetailView);
                responseOrder.flag = resultModel.IsValid ? 1 : 0;
                responseOrder.msg = resultModel.IsValid
                    ? CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_DELETESUCCESS", requestModel.lang)
                    : CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", requestModel.lang);
            }
            else
            {
                responseOrder.flag = 0;
                //参数错误
                responseOrder.msg = CultureHelper.GetAPPLangSgring("HOME_SHOPPING_ERROR", requestModel.lang);
            }
            var jsonStr = JsonConvert.SerializeObject(responseOrder);
            return new HttpResponseMessage
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "application/json")
            };
        }

        #endregion

        //#region 获取账号余额

        //[HttpPost]
        //public IHttpActionResult GetUserBalance()
        //{

        //}
        //#endregion

        #region 5.4.确认收货（周博）

        /// <summary>
        ///     5.4.确认收货（周博）
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AffirmReceiving(RequestOrderDetailsModel requestModel)
        {
            var responseOrder = new ApiResultModel();
            if (requestModel != null)
            {
                var searchOrderDetailView = new SearchOrderDetailView();
                searchOrderDetailView.UserID = Convert.ToInt64(this._enctyptionService.RSADecrypt(requestModel.userId));
                searchOrderDetailView.OrderID = requestModel.orderNumber;
                searchOrderDetailView.OrderStatus = requestModel.orderStatus;
                var resultModel = this._orderService.OutTimeReceivingOrder(searchOrderDetailView);
                responseOrder.flag = resultModel.IsValid ? 1 : 0;
                //成功 失败
                responseOrder.msg = resultModel.IsValid
                    ? CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_DELETESUCCESS", requestModel.lang)
                    : CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", requestModel.lang);
            }
            else
            {
                responseOrder.flag = 0;
                //参数错误
                responseOrder.msg = CultureHelper.GetAPPLangSgring("HOME_SHOPPING_ERROR", requestModel.lang);
            }
            var jsonStr = JsonConvert.SerializeObject(responseOrder);
            return new HttpResponseMessage
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "application/json")
            };
        }

        #endregion

        #region 5.5.订单结算（伍锐）

        /// <summary>
        ///     5.5.订单结算（伍锐）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Clearing(RequestOrderInfoModel model)
        {
            try
            {
                var userId = this._enctyptionService.RSADecrypt(model.UserId);

                //获取商品信息，以商家分组
                List<ComInfo> comInfos =
                    this._shoppingCartService.getGoodsGroupByCom(1.ToString(), model.lang, userId).Data;

                //获取订单运费
                if (model.UserAddressId.HasValue && model.UserAddressId > 0)
                {
                    //获取订单运费
                    this._orderService.GetOrdersExpressMoney(comInfos, model.UserAddressId.Value);
                }
                var resultModel = new ApiResultModel
                {
                    rs = comInfos,
                    flag = 1
                };
                return this.Ok(resultModel);
            }
            catch (Exception ex)
            {
                this._logger.Error(this.GetType().FullName,
                    string.Format("类名:{0} 方法名称:{1} 错误信息:{2}", this.GetType().FullName, "Clearing", ex.Message));

                return this.Ok(new ApiResultModel
                {
                    flag = 0,
                    msg = CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", model.lang)
                });
            }
        }

        #endregion

        #region 5.6.（立即购买）获取商品信息(伍锐)

        /// <summary>
        ///     5.6.（立即购买）获取商品信息(伍锐)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetGoodsInfo(RequestGoodsInfoModel model)
        {
            var result = new ApiResultModel();
            try
            {
                List<GoodsInfoModel> productList =
                    this._shoppingCartService.GetGoodsInfo(model.StrGoodsIds, model.StrSkuNumber, model.Lang).Data;

                var hash = new Hashtable();
                for (var i = 0; i < model.StrGoodsIds.Count; i++)
                {
                    hash.Add(model.StrGoodsIds[0], model.StrCounts[i]);
                }

                //设置立即购买数量
                foreach (var goodsInfoModel in productList)
                {
                    goodsInfoModel.Count = Convert.ToInt32(hash[goodsInfoModel.GoodsId]);
                }

                //以商家分组的商品集合
                var comInfos = productList.GroupBy(x => new {x.ComId, x.ComName}).Select(x => new ComInfo
                {
                    ComId = x.Key.ComId,
                    ComName = x.Key.ComName,
                    Goods = productList.Where(y => y.ComId == y.ComId).ToList()
                }).ToList();


                //收货地址不为空，则获取运费
                if (model.UserAddressId.HasValue && model.UserAddressId.Value > 0)
                {
                    //获取订单运费
                    this._orderService.GetOrdersExpressMoney(comInfos, model.UserAddressId.Value);
                }

                result.flag = 1;
                result.rs = comInfos;

                return this.Ok(result);
            }
            catch (Exception)
            {
                return this.Ok(new ApiResultModel
                {
                    flag = 0,
                    msg = CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", model.Lang)
                });
            }
        }

        #endregion

        #region 5.7.获取订单地址（李霞）

        /// <summary>
        ///     5.7.获取订单地址（李霞）
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <param name="UserType">用户类型</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetUserAddress(RequestUserAddressModel model)
        {
            var apiResult = new ApiResultModel();
            if (model == null)
            {
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int) LanguageType.defaultLang);
                    //参数错误
                return this.Ok(apiResult);
            }
            try
            {
                var userId = Convert.ToInt64(this._enctyptionService.RSADecrypt(model.UserId));
                var result = this._userAddressService.GetUserAllAddress(new SearchUserAddressModel {UserID = userId},
                    model.lang);
                if (result.IsValid)
                {
                    var userAddress = (result.Data as List<UserAddress>).Select(x => new
                    {
                        userAddressId = x.UserAddressId,
                        userID = x.UserID,
                        detailsAddress = x.DetailsAddress,
                        receiver = x.Receiver,
                        tHAreaID = x.THAreaID,
                        postalCode = x.PostalCode,
                        mobile = x.Mobile,
                        phone = x.Phone,
                        flag = x.Flag,
                        email = x.Email,
                        shengTHAreaID = x.ShengTHAreaID,
                        shiTHAreaID = x.ShiTHAreaID,
                        shengAreaName = x.ShengAreaName,
                        shiAreaName = x.ShiAreaName,
                        quAreaName = x.QuAreaName,
                        address =
                            this.FormatAddress(model.lang, x.ShengAreaName, x.ShiAreaName, x.QuAreaName,
                                x.DetailsAddress)
                    });
                    apiResult.rs = userAddress;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", model.lang);
                    apiResult.flag = 1;
                }
                else
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", model.lang);
                }
                return this.Ok(apiResult);
            }
            catch (Exception ex)
            {
                this._logger.Error(this.GetType().FullName,
                    string.Format("类名:{0} 方法名称:{1} 错误信息:{2}", this.GetType().FullName, "Clearing", ex.Message));
                return this.Ok(new ApiResultModel
                {
                    flag = 0,
                    msg = CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", model.lang)
                });
            }
        }

        #endregion

        #region 5.8.生成订单（伍锐）

        /// <summary>
        ///     5.8.生成订单（伍锐）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GenerateOrder([FromBody] RequestOrderInfoViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return this.Ok(new ApiResultModel
                    {
                        flag = 0
                    });
                }
                var addOrderInfoView = model.To<AddOrderInfoView>();

                addOrderInfoView.PayChannel = (int) OrderEnums.PayChannel.Omise; //默认为omise支付
                addOrderInfoView.PayType = (int) OrderEnums.PayType.ThirdPay;
                addOrderInfoView.PaidType = (int) OrderEnums.PaidType.Mall;
                addOrderInfoView.OrderSource = (int) OrderEnums.OrderSource.Mobile;
                addOrderInfoView.PaymentOrderId = MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString();

                model.UserId = this._enctyptionService.RSADecrypt(model.UserId);

                //把支付单信息放入缓存（存放时间为2天）
                var isSuccessed = MemCacheFactory.GetCurrentMemCache()
                    .AddCache("ZF" + addOrderInfoView.PaymentOrderId, addOrderInfoView, 2*24*60);

                var data = new ApiResultModel
                {
                    flag = isSuccessed ? 1 : 0,
                    rs = new
                    {
                        paymentOrderId = addOrderInfoView.PaymentOrderId
                    }
                };

                if (isSuccessed)
                {
                    OrderMQ.SendMsgToMQ(addOrderInfoView.PaymentOrderId);
                }

                return this.Ok(data);
            }
            catch (Exception ex)
            {
                this._logger.Error(this.GetType().FullName,
                    string.Format("类名:{0} 方法名称:{1} 错误信息:{2}", this.GetType().FullName, "Clearing", ex.Message));
                return this.Ok(new ApiResultModel
                {
                    flag = 0,
                    msg = CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", model.lang)
                });
            }
        }

        #endregion

        #region 5.9.获取订单详情（伍锐）

        /// <summary>
        ///     5.9.获取订单详情（伍锐）
        /// </summary>
        /// <param name="userId">用户ID(加密)</param>
        /// <param name="paymentOrderId">支付单号</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetPaymentOrderDetail(RequestPaymentOrderDatilModel model)
        {
            var userid = this._enctyptionService.RSADecrypt(model.UserId);

            var requestPaymentOrder = new PaymentOrderView
            {
                PaymentOrderID = model.PaymentOrderId,
                UserID = Convert.ToInt64(userid)
            };

            var result = this._paymentOrderService.GetPaymentOrderBy(requestPaymentOrder);

            //只有未支付的订单才处理
            if (!result.IsValid || result.Data == null || result.Data.Flag != (int) (OrderEnums.PaymentFlag.NonPaid))
            {
                result.IsValid = true;
                result.Messages.Add(CultureHelper.GetAPPLangSgring("NO_PAY", model.Lang));
                return this.Ok(result);
            }

            return this.Ok(result);
        }

        #endregion

        #region 5.10.订单处理结果（伍锐）

        /// <summary>
        ///     5.10.订单处理结果（伍锐）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult OrderProcessResult(RequestOrderProcessModel model)
        {
            var tempResult =
                MemCacheFactory.GetCurrentMemCache().GetCache<TempResultModel>("DDJG" + model.PaymentOrderId);
            var result = tempResult == null ? null : tempResult.ConvertToResultModel(tempResult);
            var addOrderInfoView =
                MemCacheFactory.GetCurrentMemCache().GetCache<AddOrderInfoView>("ZF" + model.PaymentOrderId);

            if (result == null && addOrderInfoView != null)
            {
                result = new ResultModel
                {
                    Status = (int) OrderEnums.GenerateOrderFailType.Processing,
                    IsValid = true
                };
            }
            else if (result == null)
            {
                result = new ResultModel
                {
                    Status = (int) OrderEnums.GenerateOrderFailType.Fail,
                    IsValid = false
                };
            }
            else
            {
                //订单不是待处理中状态,就清除缓存中的订单状态数据
                if (result.Status != (int) OrderEnums.GenerateOrderFailType.Processing)
                {
                    MemCacheFactory.GetCurrentMemCache().ClearCache("DDJG" + model.PaymentOrderId);
                }
            }

            var apiResultModel = new ApiResultModel
            {
                flag = 1,
                rs = new ResponseOrderProcessModel
                {
                    Status = result.Status
                }
            };

            switch ((OrderEnums.GenerateOrderFailType) result.Status)
            {
                case OrderEnums.GenerateOrderFailType.Processing:
                    apiResultModel.msg = CultureHelper.GetAPPLangSgring("PROCESSING", model.Lang);
                    break;
                case OrderEnums.GenerateOrderFailType.Success:
                    apiResultModel.msg = CultureHelper.GetAPPLangSgring("SUCCESS", model.Lang);
                    break;
                case OrderEnums.GenerateOrderFailType.Fail:
                    apiResultModel.msg = string.IsNullOrEmpty(result.Message)
                        ? CultureHelper.GetAPPLangSgring("FAILURE", model.Lang)
                        : result.Message;
                    break;
                case OrderEnums.GenerateOrderFailType.NotAddress:
                    apiResultModel.msg = CultureHelper.GetAPPLangSgring("NOT_ADDRESS", model.Lang);
                    break;
                case OrderEnums.GenerateOrderFailType.NotStock:
                    apiResultModel.msg = CultureHelper.GetAPPLangSgring("MONEY_SHOPPINGCART_INSUFFICIENTINVENTORY",
                        model.Lang);
                    break;
                case OrderEnums.GenerateOrderFailType.UnShelve:
                    apiResultModel.msg = CultureHelper.GetAPPLangSgring("MONEY_SHOPPINGCART_COMMODITIESHAVESHELVES",
                        model.Lang);
                    break;
                case OrderEnums.GenerateOrderFailType.ParamError:
                    apiResultModel.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.Lang);
                    break;
            }

            return this.Ok(apiResultModel);
        }

        #endregion

        #region 5.11.订单退款处理(周博)

        /// <summary>
        ///     5.11.订单退款处理(周博)
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Refund(RequestOrderDetailsModel requestModel)
        {
            var responseOrder = new ApiResultModel();
            var userId = Convert.ToInt64(this._enctyptionService.RSADecrypt(requestModel.userId));

            List<OrderDetailsModel> detailsModelList =
                this._orderService.GetOrderDetailsById(Convert.ToInt64(requestModel.orderDetailsID), userId, 1).Data;
            if (detailsModelList != null && detailsModelList.Count > 0)
            {
                var model = new ReturnProductInfoModel();
                model.UserID = userId;
                model.OrderDetailsID = Convert.ToInt64(requestModel.orderDetailsID);
                model.ReturnOrderID = MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString();
                model.OrderID = detailsModelList[0].OrderID;
                model.ProductId = detailsModelList[0].ProductId;
                model.ReturntNumber = detailsModelList[0].Quantity;
                model.TradeAmount = detailsModelList[0].SalesPrice*detailsModelList[0].Quantity;
                model.RefundAmount = model.TradeAmount;
                model.ReasonType = 1;
                model.Discription = " ";
                model.ReturnType = 2;
                model.ReturnStatus = 1;
                model.CreateTime = DateTime.Now;
                var resultModel = this._returnProductInfoService.AddReturnProductInfo(model, requestModel.lang);
                responseOrder.flag = resultModel.IsValid ? 1 : 0;
                responseOrder.msg = resultModel.Message;
            }
            else
            {
                responseOrder.flag = 0;
                responseOrder.msg = CultureHelper.GetAPPLangSgring("MY_ORDER_NO_EXCEPTION", requestModel.lang);
            }
            var jsonStr = JsonConvert.SerializeObject(responseOrder);
            return new HttpResponseMessage
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "application/json")
            };
        }

        #endregion

        #region 5.12.撤销退款(周博)

        /// <summary>
        ///     5.12.撤销退款(周博)
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RevokeRefund(RequestOrderDetailsModel requestModel)
        {
            var responseOrder = new ApiResultModel();
            if (requestModel != null)
            {
                var infoModel = new ReturnProductInfoModel();
                infoModel.UserID = Convert.ToInt64(this._enctyptionService.RSADecrypt(requestModel.userId));
                infoModel.ReturnOrderID = requestModel.returnOrderID;
                var resultModel = this._returnProductInfoService.UndoReturnProductInfoBH(infoModel, requestModel.lang);
                responseOrder.flag = resultModel.IsValid ? 1 : 0;
                responseOrder.msg = resultModel.Message;
            }
            else
            {
                responseOrder.flag = 0;
                responseOrder.msg = CultureHelper.GetAPPLangSgring("HOME_SHOPPING_ERROR", requestModel.lang);
            }
            var jsonStr = JsonConvert.SerializeObject(responseOrder);
            return new HttpResponseMessage
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "application/json")
            };
        }

        #endregion

        #region 5.13.售后列表(刘文宁)有修改

        /// <summary>
        ///     5.13.售后列表(刘文宁)有修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AfterSaleList(RequestAfterSaleListModel model)
        {
            var result = new ResultModel();
            var apiResult = new ApiPagingResultModel();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.userId))
                {
                    var userId = this._enctyptionService.RSADecrypt(model.userId);

                    if (!string.IsNullOrEmpty(userId))
                    {
                        result = this._orderService.GetAfterSaleList(userId, model.pageNo, model.pageSize, model.lang);
                        if (result.IsValid)
                        {
                            if (result.Data != null)
                            {
                                apiResult.flag = 1;
                                apiResult.rs = result.Data;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", model.lang);
                                    //查询成功
                                apiResult.totalSize = Convert.ToInt32(result.Messages[0]);
                            }
                            else
                            {
                                apiResult.flag = 0;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("NO_DATA", model.lang); //暂无数据
                            }
                        }
                        else
                        {
                            apiResult.flag = 0;
                            apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", model.lang); //查询失败
                        }
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("USER_ID_IllEGAL", model.lang); //用户ID不合法
                    }
                }
                else
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                }
            }
            else
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int) LanguageType.defaultLang);
                    //参数不能为空
            }
            return this.Ok(apiResult);
        }

        #endregion

        #region 5.14 交易密码验证 (樊利民)

        /// <summary>
        ///     验证交易密码
        /// </summary>
        /// <param name="requestModel">请求验证交易密码模型</param>
        /// <returns></returns>
        public IHttpActionResult ValidPayPassword(RequestValidPayPasswordModel requestModel)
        {
            requestModel.UserId = this._enctyptionService.RSADecrypt(requestModel.UserId);
            var tempResult = new ResultModel();
            UserInfoViewForPayment userInfoView =
                this._userService.GetYH_UserForPayment(Convert.ToInt64(requestModel.UserId)).Data;
            tempResult = this._userService.GetYH_UserForPaymentMessage(userInfoView, new UserInfoViewForPayment
            {
                UserID = userInfoView.UserID,
                PayPassWord = this._enctyptionService.RSADecrypt(requestModel.PayPassword),
                LanguageId = requestModel.Lang
            });
            var result = new ApiResultModel
            {
                flag = tempResult.IsValid ? 1 : 0,
                msg = tempResult.Message
            };
            return this.Ok(result);
        }

        #endregion

        #region 获取运费 （刘泉）

        [HttpPost]
        public IHttpActionResult GetPostagePrice(RequestTotalPostagePriceModel model)
        {
            var response = new ApiResultModel();
            try
            {
                if (model == null)
                {
                    response.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int) LanguageType.defaultLang);
                        //参数不能为空
                    return this.Ok(response);
                }

                if (model.userAddressId == null || model.userAddressId == 0)
                {
//收货地址不存在
                    response.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int) LanguageType.defaultLang);
                        //参数不能为空
                    return this.Ok(response);
                }
                if (model.comInfos == null || model.comInfos.Count == 0)
                {
//无商品
                    response.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int) LanguageType.defaultLang);
                        //参数不能为空
                    return this.Ok(response);
                }
                var addressId = model.userAddressId;

                var re = new TotalPostagePriceResult();
                re.comInfos = new List<PostagePrice>();
                //循环获取每个商家订单运费
                foreach (var item in model.comInfos)
                {
                    var postagePrice = new PostagePrice();
                    var comInfo = new ComInfo();
                    comInfo.ComId = item.ComId;
                    postagePrice.ComId = item.ComId;
                    var goods = item.GoodsInfo;
                    if (goods != null)
                    {
                        comInfo.Goods = goods.Select(x => new GoodsInfoModel
                        {
                            Count = x.Count,
                            Weight = x.Weight,
                            FreeShipping = x.FreeShipping
                        }).ToList();
                    }
                    postagePrice.ExpressMoney = this._orderService.GetOrderExpressMoney(comInfo, addressId).Data;
                    re.comInfos.Add(postagePrice);
                }
                if (re.comInfos.Count > 0)
                {
                    //计算整个订单总运费
                    re.TotalExpressMoney = re.comInfos.Sum(x => x.ExpressMoney);
                    response.rs = re;

                    response.flag = 1;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", model.lang); //查询成功
                }
                else
                {
                    response.flag = 1;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", model.lang); //查询成功
                }
            }
            catch (Exception ex)
            {
                response.flag = 0;
                response.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", model.lang);
                    //"获取惠卡推荐异常!" + ex;
                NLogHelper.GetCurrentClassLogger().Error("获取运费!" + ex);
            }
            return this.Ok(response);
            //var jsonStr = JsonConvert.SerializeObject(response);
            //return new HttpResponseMessage { Content = new StringContent(jsonStr, Encoding.UTF8, "application/json") };
        }

        #endregion
    }
}