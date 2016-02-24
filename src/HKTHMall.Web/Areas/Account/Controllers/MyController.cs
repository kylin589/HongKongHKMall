using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Domain.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Collection;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Domain.WebModel.Models.Search;
using HKTHMall.Services.AC;
using HKTHMall.Services.Common;
using HKTHMall.Services.LoginLog;
using HKTHMall.Services.Orders;
using HKTHMall.Services.Products;
using HKTHMall.Services.WebLogin;
using HKTHMall.Services.WebProducts;
using HKTHMall.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrCms.Framework.Mvc.Extensions;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Services.Users;
using HKTHMall.Domain.AdminModel.Models.Orders;
using ZJ_UserBalanceModel = HKTHMall.Domain.WebModel.Models.Login.ZJ_UserBalanceModel;

using IZJ_UserBalanceService = HKTHMall.Services.Users.IZJ_UserBalanceService;
using HKTHMall.Core.Extensions;
using Webdiyer.WebControls.Mvc;


namespace HKTHMall.Web.Areas.Account.Controllers
{
    [Authorize]
    public class MyController : BaseController
    {
        private readonly IComplaintsService _ComplaintsService;
        private readonly IOrderService _orderService;
        private readonly ITHAreaService _THAreaService;
        private readonly IProductSearchListService _IProductSearchListService;
        private readonly IProductService _IProductService;
        private readonly ILoginService _LoginService;
        private readonly IYH_UserLoginLogService _LoginLogService;
        private readonly IReturnProductInfoService _ReturnProductInfoService;
        private readonly IProductCommentService _IProductCommentService;
        private readonly IZJ_UserBalanceChangeLogService _zJ_UserBalanceChangeLogService;
        private readonly IOrderTrackingLogService _IOrderTrackingLogService;
        private readonly IZJ_RebateService _IZJ_RebateService;
        /// <summary>
        /// 商品评论服务
        /// </summary>
        private readonly ISP_ProductCommentService _SP_ProductCommentService;
        private readonly IZJ_WithdrawOrderService _zJ_WithdrawOrderService;
        private readonly HKTHMall.Services.Users.IZJ_UserBalanceService _zJ_UserBalanceService;

        private readonly IUserAddressService _userAddressService;

        public MyController(IComplaintsService complaintsService, IOrderService orderService, ITHAreaService thAreaService, IProductSearchListService iProductSearchListService, IProductService iProductService, ILoginService loginService, IYH_UserLoginLogService loginLogService, IReturnProductInfoService returnProductInfoService, IProductCommentService iProductCommentService, IZJ_UserBalanceChangeLogService zJUserBalanceChangeLogService
            , ISP_ProductCommentService spProductCommentService, IZJ_WithdrawOrderService zJWithdrawOrderService, IZJ_UserBalanceService zJUserBalanceService, IUserAddressService userAddressService, IOrderTrackingLogService iOrderTrackingLogService, IZJ_RebateService iZJ_RebateService)
        {
            _ComplaintsService = complaintsService;
            _orderService = orderService;
            _THAreaService = thAreaService;
            _IProductSearchListService = iProductSearchListService;
            _IProductService = iProductService;
            _LoginService = loginService;
            _LoginLogService = loginLogService;
            _ReturnProductInfoService = returnProductInfoService;
            _IProductCommentService = iProductCommentService;
            _zJ_UserBalanceChangeLogService = zJUserBalanceChangeLogService;
            _SP_ProductCommentService = spProductCommentService;
            _zJ_WithdrawOrderService = zJWithdrawOrderService;
            _zJ_UserBalanceService = zJUserBalanceService;
            _userAddressService = userAddressService;
            _IOrderTrackingLogService = iOrderTrackingLogService;
            _IZJ_RebateService = iZJ_RebateService;
        }




        #region 我的惠卡
        // GET: Account/My
        /// <summary>
        /// 我的惠卡
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize]
        public ActionResult Index()
        {
            long userId = base.UserID;
            YH_UserModel userInfo = _LoginService.GetUserInfoById(userId).Data;
            //if (Settings.IsEnableEM)
            //{
            //    //查询用户密保激活状态 type 1 邮箱账号  2 手机号 
            //    ReqUserInfoActivateState ActivateState = new ReqUserInfoActivateState();
            //    ActivateState.mac = Core.Settings.GetMacAddress(); //获取本机mac地址
            //    ActivateState.ip = Core.Settings.GetIPAddress();   //获取本机ip地址
            //    ActivateState.sys_id = Core.Settings.EmSystemId;      //获取系统业务编号
            //    ActivateState.dev = Core.Settings.EmDev;//注册来源,1:网站
            //    ActivateState.type = 1;
            //    ActivateState.uid = base.UserID;
            //    var stateresponse = EmMethodManage.EmFindUpdateInstance.MsgQueryInfoActivateStateReq(ActivateState);
            //    userInfo.ActiveEmail = (byte)stateresponse.state;
            //}
            ViewBag.Account = userInfo.Account;
            ViewBag.Phone = userInfo.Phone;
            ViewBag.ImageUrl = userInfo.HeadImageUrl;
            ViewBag.NickName = userInfo.NickName;

            var result = _LoginLogService.GetUserLoginInfo(userId);
            List<YH_UserLoginLogModel> list = result.Data;
            if (result.Data.Count > 0)
            {
                ViewBag.LastLoginTime = list[0].LoginTime.Value.DateTimeToString();
                ViewBag.LastloginIP = list[0].IP == "" || list[0].IP == "::1" ? "127.0.0.1" : list[0].IP;
            }
            else
            {
                ViewBag.LastLoginTime = DateTime.Now.DateTimeToString();
                ViewBag.LastloginIP = "127.0.0.1";
            }
            //我的余额
            ZJ_UserBalanceModel userbalance = _zJ_UserBalanceService.GetZJ_UserBalanceById(base.UserID).Data;
            ViewBag.ConsumeBalance = userbalance != null ? ToolUtil.Round(userbalance.ConsumeBalance, 2) : 0;
            //待付款
            ViewBag.UnPay = _orderService.GetOrderByUserIDStatus(userId, 2).Data.Count;
            //待收货
            ViewBag.UnReceived = _orderService.GetOrderByUserIDStatus(userId, 4).Data.Count;
            //待评价
            ViewBag.UnComment = _orderService.GetOrderUnComment(userId).Data.Count;
            //今日返现
            var query = _zJ_UserBalanceChangeLogService.GetRebeatAmountByDate(this.UserID, DateTime.Now).Data.ToScalar();
            ViewBag.Rebeat = query == null ? 0 : query;

            //"初级", "中级", "高级"
            string[] Rarr = { CultureHelper.GetLangString("MY_PRIMARY"), CultureHelper.GetLangString("MY_INTERMEDIATE"), CultureHelper.GetLangString("MY_ADVANCED") };
            // string[] Rarr = {  CultureHelper.GetLangString("MY_INTERMEDIATE"), CultureHelper.GetLangString("MY_ADVANCED") };
            int coun = 0;
            string str = Rarr[coun];
            int percent = 33;
            if (!string.IsNullOrWhiteSpace(userInfo.PayPassWord))
            {
                percent = 33 + percent;
                coun = 1 + coun;
                str = Rarr[coun];
            }
            if (!string.IsNullOrWhiteSpace(userInfo.Email) && userInfo.ActiveEmail == 1)
            {
                percent = 34 + percent;
                coun = 1 + coun;
                str = Rarr[coun];
            }
            ViewBag.Rank = str;
            ViewBag.percent = percent + "%";
            ViewBag.RankCount = coun;

            return View();
        }

        /// <summary>
        /// 惠卡商品推荐
        /// </summary>
        /// <param name="count">默认数量</param>
        /// <returns></returns>
        public ActionResult RecommendProducts(int count = 4)
        {
            //获取惠卡推广数据
            long userId = base.UserID;
            var productData = _IProductService.GetTopRecommend(CultureHelper.GetLanguageID(), userId);
            List<ProductInfo> productList = productData.Data;
            if (productList != null)
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
                var data = productList.OrderByDescending(a => a.Guid).Take(count);
                productList = data.ToList();
            }
            return PartialView(productList);
        }

        #endregion 我的惠卡

        #region 我的收藏
        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [Authorize]
        public ActionResult Collection(KeyWordsSearch model)
        {
            //UserID
            model.languageId = CultureHelper.GetLanguageID();
            int count = 0;
            var result = _IProductSearchListService.GetMyCollectionList(base.UserID, model, out count);
            model.AllCount = count;
            ViewData.Add("collects", result.Data);
            ViewData.Add("searchModel", model);
            return View();
        }


        /// <summary>
        /// 我的收藏删除
        /// </summary>
        /// <param name="collectionId"> </param>
        /// <returns></returns>
        /// 
        [Authorize]
        public JsonResult DeleteCollection(long collectionId)
        {
            var result = _IProductSearchListService.DeleteMyCollection(base.UserID, collectionId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion 我的收藏

        #region 我的评价
        /// <summary>
        /// 我的评价
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Review(SearchSP_ProductCommentModel model, int page = 1)
        {
            model.LanguageID = CultureHelper.GetLanguageID();
            model.UserID = base.UserID;
            model.PagedIndex = page - 1;
            model.PagedSize = 10;
            int totalcount;
            var result = _IProductCommentService.GetProductCommentList(model,out totalcount);
            ViewData.Add("comments", result.Data);
            ViewData.Add("searchModel", model);
            ViewBag.Page = page;
            ViewBag.Count = totalcount;
            return View();
        }
        #endregion 我的评价

        #region 订单列表

        /// <summary>
        /// 我的订单
        /// </summary>
        /// <param name="searchOrderView"></param>
        /// <returns></returns>
        public ActionResult Order(SearchOrderView searchOrderView)
        {
            searchOrderView.page = this.Request["page"] != null ? Convert.ToInt32(this.Request["page"]) : searchOrderView.page;
            //订单状态
            ViewBag.OrderStatus = ML_OrderStatus.GetLocalOrderStatusInto(CultureHelper.GetLanguageID(), new int[] { -1 }, searchOrderView.s);

            //时间间隔类型
            ViewBag.TimeSpanTypes = ML_TimeSpanTypes.GetLocalTimeSpanTypesInto(CultureHelper.GetLanguageID(), new int[] { -1 }, searchOrderView.d);

            searchOrderView.UserID = UserID;
            searchOrderView.LanguageID = CultureHelper.GetLanguageID();
           
            var result = _orderService.GetPagingOrdersIntoWeb(searchOrderView);
            ViewBag.Status = (int)searchOrderView.s;
            ViewBag.SearchOrderView = searchOrderView;

            PagedList<OrderView> model = new PagedList<OrderView>(result.Data, searchOrderView.page, searchOrderView.pageSize, result.Data.TotalCount);
            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView("_OrderList", model);
            }
            return this.View(model);
        }

        #endregion

        #region  订单投诉

        /// <summary>
        /// 订单投诉页
        /// zhoub 20150716
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderComplaints(string orderId)
        {
            var result = _orderService.GetOrderByOrderID(orderId);
            if (result.Data == null || (long)result.Data.UserID != this.UserID)
            {
                return View();
            }
            return View(result.Data);
        }

        /// <summary>
        /// 投诉订单保存
        /// zhoub 20150716
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public JsonResult SaveComplaints(string orderId, string context)
        {
            var resultModel = new ResultModel();
            if (!string.IsNullOrEmpty(orderId))
            {
                var result = _orderService.GetOrderByOrderID(orderId);
                if (result.Data != null)
                {
                    if (result.Data.ComplaintStatus == 0)
                    {
                        ComplaintsModel model = new ComplaintsModel();
                        model.ComplaintsID = MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString();
                        model.OrderID = orderId;
                        model.MerchantID = result.Data.MerchantID;
                        model.UserID = result.Data.UserID;
                        model.complainType = 1;
                        model.Content = context;
                        model.ComplaintsDate = DateTime.Now;
                        model.Flag = 1;
                        _ComplaintsService.AddComplaints(model);
                        resultModel.Messages = new List<string> { CultureHelper.GetLangString("MY_APPLICATION_HAS_BEEN_SUBMITTED") };//申请已提交
                    }
                    else
                    {
                        resultModel.IsValid = false;
                        resultModel.Messages = new List<string> { CultureHelper.GetLangString("MY_ORDERS_HAVE_BEEN_COMPLAINTS") };//该订单已经进行过投诉.
                    }
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { CultureHelper.GetLangString("MY_ORDER_NUMBER_NOT_EXIST") };//订单号不存在.
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { CultureHelper.GetLangString("MY_ORDER_NO_EXCEPTION") };//订单号异常.
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 订单详情

        /// <summary>
        /// 订单详情页
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult OrderDetail(string orderId)
        {

            if (string.IsNullOrEmpty(orderId))
            {
                return Redirect("/Account/My/Order");
            }

            HKTHMall.Domain.WebModel.Models.Orders.SearchOrderDetailView searchOrderDetail = new SearchOrderDetailView()
            {
                OrderID = orderId,
                UserID = UserID,
                LanguageID = CultureHelper.GetLanguageID()
            };
            var result =
                _orderService.GetOrderDetailIntoWebBy(searchOrderDetail);

            if (result.IsValid && result.Data != null)
            {



                //收货地址
                Dictionary<string, string> userAreas =
                    _THAreaService.GetSingleTierAreaNames(new SearchUserAddressModel() { THAreaID = result.Data.OrderAddressView.THAreaID },
                        CultureHelper.GetLanguageID()).Data;


                //收货地址
                ViewBag.UserAddress = AddressHelper.ShowUserAddress(userAreas["Country"],userAreas["Sheng"], userAreas["Shi"],
                    userAreas["Qu"], result.Data.OrderAddressView.DetailsAddress, CultureHelper.GetLanguageID());

                //商家成功
                Dictionary<string, string> merchantAreas = _THAreaService.GetSingleTierAreaNames(new SearchUserAddressModel() { THAreaID = result.Data.YH_MerchantInfoView.AreaID },
                        CultureHelper.GetLanguageID()).Data;
                ViewBag.MerchantArea = merchantAreas["Shi"];


                return View(result.Data);
            }
            else
            {
                return Redirect("/Account/My/Order");
            }


        }

        #endregion

        #region 我的财富
        /// <summary>
        /// 我的财富
        /// </summary>
        /// <returns></returns>
        /// <remarks>addded by jimmy,2015-7-21</remarks>
        public ActionResult Wealth()
        {
            //YH_UserModel model = loginuser;
            YH_UserModel userInfo = _LoginService.GetUserInfoById(base.UserID).Data;
            ZJ_UserBalanceModel userbalance = _zJ_UserBalanceService.GetZJ_UserBalanceById(base.UserID).Data;
            userInfo.ConsumeBalance = userbalance != null ? ToolUtil.Round(userbalance.ConsumeBalance, 2) : 0;
            //model.Account = userInfo != null ? userInfo.Account : string.Empty;
            return View(userInfo);
        }

        public ActionResult GetChangelogList(string msg, int page = 1)
        {
            int[] addOrCutTypeArry = { };
            switch (msg)
            {
                case "1": //充值记录
                    addOrCutTypeArry = new[] { 1, 15, 16 };
                    break;
                case "2": //消费记录
                    addOrCutTypeArry = new[] { 2, 4 };
                    break;
                case "3": //提现记录
                    addOrCutTypeArry = new[] { 3 };
                    break;
                case "4": //营收记录
                    addOrCutTypeArry = new[] { 5 };
                    break;
                case "5": //惠粉收益记录
                    addOrCutTypeArry = new[] { 10, 11, 12 };
                    break;
                case "6": //代理收益记录
                    addOrCutTypeArry = new[] { 6, 7, 8, 9, 13, 14 };
                    break;
            }
            SearchZJ_UserBalanceChangeLogModel model = new SearchZJ_UserBalanceChangeLogModel();
            model.UserID = UserID;
            model.AddOrCutTypeArry = addOrCutTypeArry;
            model.PagedIndex = page - 1;
            model.PagedSize = 10;
            model.LanguageId = CultureHelper.GetLanguageID();
            var result = _zJ_UserBalanceChangeLogService.GetUserBalanceChangeLogList(model);
            List<ZJ_UserBalanceChangeLogModel> ds = result.Data;
            ViewBag.Data = ds;
            ViewBag.Page = page;
            ViewBag.Count = result.Data.TotalCount;
            return PartialView();
        }

        #endregion

        #region 我的财富--提现金额
        /// <summary>
        /// 我的财富--提现金额
        /// </summary>
        /// <param name="withdrawAmount">提现金额</param>
        /// <returns></returns>
        /// <remarks>addded by jimmy,2015-7-21</remarks>
        public JsonResult ZJWithdrawOrder(decimal? withdrawAmount)
        {
            long userId = base.UserID;
            // YH_UserModel model = loginuser;

            var resultModel = new ResultModel();
            if (withdrawAmount.HasValue)
            {
                var result = this._zJ_WithdrawOrderService.AddZJ_WithdrawOrder(userId, withdrawAmount.Value, IOrderSource.WebSite);
                if (result.Data != null && result.IsValid)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string>() { CultureHelper.GetLangString("MY_WITHDRAW_APPLY_SUCCESS") };//提现申请成功
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string>() { result.Messages[0] };
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  订单退款

        /// <summary>
        /// 订单退款列表页
        /// zhoub
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderReturnProductInfo(long? orderDetailsID)
        {
            if (orderDetailsID.HasValue)
            {
                var result = _orderService.GetOrderDetailsById(Convert.ToInt64(orderDetailsID), base.UserID, CultureHelper.GetLanguageID());
                return View(result.Data);
            }
            return View();
        }

        /// <summary>
        /// 增加商品退货信息
        /// zhoub 20150721
        /// </summary>
        /// <returns></returns>
        public JsonResult AddReturnProductInfo()
        {
            ReturnProductInfoModel model = new ReturnProductInfoModel();
            model.ReturnOrderID = MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString();
            model.UserID = base.UserID;
            model.OrderID = Request.Params["orderID"];
            model.ProductId = Convert.ToInt64(Request.Params["productId"]);
            model.ReturntNumber = Convert.ToInt32(Request.Params["quantity"]);
            model.TradeAmount = Convert.ToDecimal(Request.Params["salesPrice"]) * model.ReturntNumber;
            model.RefundAmount = model.TradeAmount;
            model.ReasonType = Convert.ToInt32(Request.Params["reasonType"]);
            model.Discription = Request.Params["discription"];
            model.ReturnType = 2;
            model.ReturnStatus = 1;
            model.CreateTime = DateTime.Now;
            model.OrderDetailsID = Convert.ToInt64(Request.Params["orderDetailsID"]);
            var resultModel = _ReturnProductInfoService.AddReturnProductInfo(model, CultureHelper.GetLanguageID());
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 账户充值
        public ActionResult AccountRecharge()
        {
            if (UserID <= 0 || string.IsNullOrEmpty(Account))
            {
                Response.Redirect("/Login/Index", true);
            }
            else
            {
                ViewBag.UserID = UserID;
                ViewBag.Account = Account.Trim() == "" ? CultureHelper.GetLangString("MY_ACCOUNT_IS_EMPTY") : Account;//账户是空号
            }

            return View();
        }

        public ActionResult PostAccountRecharge()
        {
            var RechargeAmount = Convert.ToDecimal(Request.Form["RechargeAmount"]);//充值金额
            var Radiochecked = Convert.ToInt32(Request.Form["Recharge2"]);//第三方充值方式(支付通道)

            AccountRechargeWebs arwb = new AccountRechargeWebs();

            if (UserID <= 0 || string.IsNullOrEmpty(Account))
            {
                Response.Redirect("/Login/Index", true);
            }
            if (UserID > 0 && !string.IsNullOrEmpty(Account))
            {
                string OrderNO = "";
                string PaymentOrderID = "1215890119";
                //充值参数类
                HKTHMall.Domain.WebModel.Models.AccountRecharge.AccountRechargeModel armodel = new HKTHMall.Domain.WebModel.Models.AccountRecharge.AccountRechargeModel();
                armodel.Account = Account;
                armodel.AddOrCutAmount = RechargeAmount;
                armodel.AddOrCutType = 1;
                armodel.RechargeChannel = Radiochecked;
                armodel.UserID = UserID;

                var result = arwb.InsertAddZJ_RechargeOrder(armodel, ERechargeOrderPrefix.Normal, out OrderNO, out PaymentOrderID);//调用第三方接口前
                if (result.IsValid)
                {
                    return RedirectToAction("PaymentAction", "Payment", new { area = "Money", paymentOrderId = PaymentOrderID });
                }
                else
                {
                    //
                }
                //var result2 = arwb.AccountRechargeWeb(PaymentOrderID);//第三方支付成功后调用
            }

            Response.Redirect("/My/AccountRecharge", true);
            return View();
        }

        public ActionResult AccountRechargeSuccess()
        {
            return View();
        }

        #endregion#region 商品评价

        #region 商品评价

        /// <summary>
        /// 订单商品评价
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult TradeComment(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return Redirect("/Account/My/Order");
            }
            SearchOrderProductCommentView searchOrderProductCommentView = new SearchOrderProductCommentView()
            {
                OrderID = orderId,
                UserID = UserID,
                LanguageID = CultureHelper.GetLanguageID(),
            };
            var result = _SP_ProductCommentService.GetOrderProductComments(searchOrderProductCommentView);
            if (result.IsValid && result.Data != null)
            {

                return View(result.Data);
            }
            else
            {
                return Redirect("/Account/My/Order");
            }
        }


        /// <summary>
        /// 保存商品评价
        /// </summary>
        /// <param name="comments"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveComment(List<SP_ProductCommentModel> comments)
        {

            if (comments != null && comments.Count > 0)
            {
                var orderId = comments[0].OrderId;
                comments.ForEach(x =>
                {
                    x.UserID = base.UserID;
                    x.OrderId = orderId;
                    x.CommentDT = DateTime.Now;
                    x.CheckStatus = 0;
                });
            }
            var result = _SP_ProductCommentService.BatchAddSP_ProductComment(comments);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 评价列表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ActionResult TradeComments(SearchOrderProductCommentView searchModel)
        {
            searchModel.UserID = base.UserID;
            searchModel.LanguageID = CultureHelper.GetLanguageID();
            var result = _SP_ProductCommentService.GetPaingCommentsIntoWeb(searchModel);
            ViewBag.SearchCommentView = searchModel;
            return View(result.Data);
        }

        #endregion

        #region 取消订单

        [HttpPost]
        public ActionResult CancelOrder(string orderId)
        {
            SearchOrderDetailView searchModel = new SearchOrderDetailView()
            {
                UserID = base.UserID,
                LanguageID = CultureHelper.GetLanguageID(),
                OrderID = orderId
            };
            var result = _orderService.CancelOrderBy(searchModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 确认收货
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OutTimeReceivingOrder(string orderId)
        {
            SearchOrderDetailView searchModel = new SearchOrderDetailView()
            {
                UserID = base.UserID,
                OrderID = orderId,
                OrderStatus = (int)OrderEnums.OrderStatus.WaitReceiving
            };
            var result = _orderService.OutTimeReceivingOrder(searchModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 退款申请-黄主霞
        public ActionResult ApplyRefund(long? OrderDetailsId)
        {
            //ViewBag.Info = _orderService.GetDetaisInfo(OrderDetailsId).Data;
            //return View();
            if (OrderDetailsId.HasValue)
            {
                var result = _orderService.GetOrderDetailsById(OrderDetailsId.Value, base.UserID, CultureHelper.GetLanguageID());
                return View(result.Data);
            }
            return View();
        }
        [HttpPost]
        public JsonResult AddRefundInfo()
        {
            ReturnProductInfoModel model = new ReturnProductInfoModel();
            model.ReturnOrderID = MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString();
            model.UserID = base.UserID;
            model.OrderID = Request.Params["orderID"];
            model.ProductId = Convert.ToInt64(Request.Params["productId"]);
            model.ReturntNumber = Convert.ToInt32(Request.Params["quantity"]);
            model.TradeAmount = Convert.ToDecimal(Request.Params["salesPrice"]) * model.ReturntNumber;
            model.RefundAmount = model.TradeAmount;
            model.ReasonType = Convert.ToInt32(Request.Params["reasonType"]);
            model.Discription = Request.Params["discription"];
            model.ReturnType = 2;
            model.ReturnStatus = 1;
            model.CreateTime = DateTime.Now;
            model.OrderDetailsID = Convert.ToInt64(Request.Params["orderDetailsID"]);
            var resultModel = _ReturnProductInfoService.AddReturnProductInfo(model, CultureHelper.GetLanguageID());
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefundProcess(long id=0)
        {
            var result = _ReturnProductInfoService.GetReturnProductInfo(new SearchReturnProductInfoModel { OrderDetailsID = id, PagedIndex = 0, PagedSize = 1, UserID = base.UserID }).Data;
            if (result.Count > 0)
            {
                ViewBag.DetailsId=id;
                ViewBag.OrderId = result[0].OrderID;
                ViewBag.ReturnId = result[0].ReturnOrderID ;
                return View();
            }
            else
            {
                ViewBag.DetailsId = 0;
                ViewBag.OrderId = 0;
                ViewBag.ReturnId = 0;
                return View();
            }
            
        }
        [HttpPost]
        public JsonResult CancelRefund()
        {
            var resultModel = new ResultModel();
            resultModel.IsValid = false;

            var result = _ReturnProductInfoService.GetReturnProductInfo(new SearchReturnProductInfoModel { OrderDetailsID = Convert.ToInt64(Request.Params["_id"]), PagedIndex = 0, PagedSize = 1, UserID = base.UserID }).Data;
            if (result.Count > 0)
            {
                ReturnProductInfoModel model = new ReturnProductInfoModel();
                model.ReturnOrderID = result[0].ReturnOrderID;
                model.OrderID = result[0].OrderID;
                model.OrderDetailsID = result[0].OrderDetailsID;
                model.UserID = base.UserID;
                resultModel = _ReturnProductInfoService.DeleteReturnProductInfo(model);
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public PartialViewResult _BalanceList(int index=1,int pageSize=10)
        {
            var model=_zJ_UserBalanceChangeLogService.GetConsumeList(this.UserID,index-1,pageSize).Data;
            return PartialView(model);
        }

        public ActionResult Balance()
        {
            ViewBag.TotalPages = _zJ_UserBalanceChangeLogService.GetConsumeList(this.UserID, 0, 10).Data.TotalPages;
            ViewBag.Balance = _zJ_UserBalanceService.GetZJ_UserBalanceById(this.UserID).Data.ConsumeBalance;
            //var query = _IZJ_RebateService.GetSurplusRebeatAmount(this.UserID).Data.ToScalar(); 
            var query = _zJ_UserBalanceChangeLogService.GetRebeatAmountByDate(this.UserID, DateTime.Now.AddDays(-1)).Data.ToScalar();
            ViewBag.Rebeat = query==null?0:query;
            return View();
        }

        public ActionResult Rebate()
        {
            ViewBag.Count = _IZJ_RebateService.GetCountByUserID(this.UserID).Data;
            ViewBag.PageCount = ViewBag.Count / 10;
            var queryPaidAmount=_IZJ_RebateService.GetPaidAmount(this.UserID).Data;
            ViewBag.PaidAmount = queryPaidAmount==null?0:queryPaidAmount;
            var queryTotalAmount = _IZJ_RebateService.GetTotalAmount(this.UserID).Data;
            ViewBag.SurplusAmount = (queryTotalAmount==null?0:queryTotalAmount) - ViewBag.PaidAmount;
            var query = _zJ_UserBalanceChangeLogService.GetRebeatAmountByDate(this.UserID, DateTime.Now.AddDays(-1)).Data.ToScalar(); 
            ViewBag.RebateToday = query==null?0:query;             
            return View();
        }

        public PartialViewResult _RebateList(int index = 1, int pageSize = 10)
        {
            ResultModel result = _IZJ_RebateService.GetRebeatAmountList(this.UserID, index, pageSize, CultureHelper.GetLanguageID());
            ViewBag.List = result.Data;
            return PartialView();
        }
    }
}