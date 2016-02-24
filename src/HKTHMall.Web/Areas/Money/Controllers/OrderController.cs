using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Services.AC;
using HKTHMall.Services.Orders;
using HKTHMall.Services.Orders.MQ;
using HKTHMall.Services.ShoppingCart;
using HKTHMall.Services.Users;
using HKTHMall.Services.YHUser;
using HKTHMall.Web.Controllers;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.Web.Areas.Money.Controllers
{
    /// <summary>
    ///     订单相关
    /// </summary>
    [Authorize]
    public class OrderController : BaseController
    {
        /// <summary>
        ///     订单服务实体
        /// </summary>
        private readonly IOrderService _orderService;

        /// <summary>
        ///     购物车服务类
        /// </summary>
        private readonly IShoppingCartService _shoppingCartService;

        /// <summary>
        ///     区域服务
        /// </summary>
        private readonly ITHAreaService _thAreaService;

        /// <summary>
        ///     订单地址服务实体
        /// </summary>
        private readonly IUserAddressService _userAddressService;

        /// <summary>
        ///     用户服务
        /// </summary>
        private readonly IYH_UserService _userService;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="userAddressService"></param>
        /// <param name="thAreaService"></param>
        /// <param name="userService"></param>
        /// <param name="orderService"></param>
        /// <param name="shoppingCartService"></param>
        public OrderController(IUserAddressService userAddressService, ITHAreaService thAreaService,
            IYH_UserService userService, IOrderService orderService, IShoppingCartService shoppingCartService)
        {
            this._userAddressService = userAddressService;
            this._thAreaService = thAreaService;
            this._userService = userService;
            this._orderService = orderService;
            this._shoppingCartService = shoppingCartService;
        }

        /// <summary>
        ///     订单页
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderInfo()
        {
            //获取用户收货地址
            List<UserAddress> userAddress = this._userAddressService.GetUserAllAddress(
                new SearchUserAddressModel { UserID = this.UserID }, CultureHelper.GetLanguageID()).Data;

            if (userAddress != null && userAddress.Count > 0)
            {
                //默认收货地址
                var defaultAddress = userAddress.FirstOrDefault(x => x.Flag == 1);
                defaultAddress = defaultAddress ?? userAddress[0];
            }

            //泰国区域
            List<THArea_lang> thAreas = this._thAreaService.GetTHAreaByParentID(CultureHelper.GetLanguageID(), 0).Data;


            //用户信息（用于余额支付）
            UserInfoViewForPayment userInfoView = this._userService.GetYH_UserForPayment(this.UserID).Data;

            dynamic pageView = new ExpandoObject();
            pageView.UserAddresss = userAddress;
            pageView.THAreas = thAreas;
            //UserInfo = userInfoView,
            //UserPhone = this.Phone.Substring(0, 3) + "***" + Phone.Substring(Phone.Length - 4)

            return View(pageView);


            /*
            List<UserAddress> addressCollection = _userAddressService.GetUserAllAddress(
                new SearchUserAddressModel() { UserID = UserID }, CultureHelper.GetLanguageID()).Data;

            //地址集合
            ViewBag.AddressCollection = addressCollection;

            //默认地址
            var defaultAdress = addressCollection.FirstOrDefault(x => x.Flag == 1);

            //区域
            ViewData["thArea"] = _thAreaService.GetTHAreaByParentID(CultureHelper.GetLanguageID(), 0).Data;

            //收货地址
            ViewBag.Model = StringUtility.SerializeObject(new
            {
                //新增收货地址时的默认值
                DefaultValueOnCreateAddress = new
                {
                    ReceiverName = defaultAdress == null ? "" : defaultAdress.Receiver,
                    Phone = Phone
                }
            });

            //用户信息
            ViewBag.UserInfo = _userService.GetYH_UserForPayment(this.UserID).Data;

            //用户手机号
            ViewBag.Phone = Phone.Substring(0, 3) + "***" + Phone.Substring(Phone.Length - 4); ;

            return View();*/
        }

        /// <summary>
        ///     获取订单数据
        /// </summary>
        /// <param name="userAddressId"></param>
        /// <returns></returns>
        public JsonResult OrderData(long? userAddressId)
        {
            //获取商品信息，以商家分组
            List<ComInfo> comInfos =
                this._shoppingCartService.getGoodsGroupByCom(1.ToString(), CultureHelper.GetLanguageID(),
                    this.UserID.ToString()).Data;
            if (userAddressId.HasValue)
            {
                //获取订单运费
                this._orderService.GetOrdersExpressMoney(comInfos, userAddressId.Value);
            }


            return this.Json(comInfos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OutrightPurchaseOrderData(string productIds, string skuIds, string counts,
            long? userAddressId)
        {
            var lstGoodsId = StringUtility.DeserializeObject<List<string>>(productIds);
            var lstSku = StringUtility.DeserializeObject<List<string>>(skuIds);
            var lstCount = StringUtility.DeserializeObject<List<string>>(counts);

            //商品集合
            List<GoodsInfoModel> productList =
                this._shoppingCartService.GetGoodsInfo(lstGoodsId, lstSku, CultureHelper.GetLanguageID()).Data;
            var hash = new Hashtable();
            for (var i = 0; i < lstGoodsId.Count; i++)
            {
                hash.Add(lstGoodsId[i], lstCount[i]);
            }
            //设置立即购买数量
            productList.ForEach(x => x.Count = Convert.ToInt32(hash[x.GoodsId]));

            var tag = false;

            //商家不能购买自己的商品
            if (this.UserType == 1)
            {
                var products = productList.FindAll(o => o.MerchantID == this.UserID);
                foreach (var p in products)
                {
                    if (p.MerchantID == this.UserID)
                    {
                        tag = true;
                    }
                    productList.Remove(p);
                }
            }

            var comInfos = productList.GroupBy(x => new { x.ComId, x.ComName }).Select(x => new ComInfo
            {
                ComId = x.Key.ComId,
                ComName = x.Key.ComName,
                Goods = productList.Where(y => y.ComId == y.ComId).ToList()
            }).ToList();

            //收货地址不为空，则获取运费
            if (userAddressId.HasValue)
            {
                //获取订单运费
                this._orderService.GetOrdersExpressMoney(comInfos, userAddressId.Value);
            }

            return
                this.Json(
                    new
                    {
                        Data = comInfos,
                        Msg =
                            (tag ? CultureHelper.GetLangString("SHOPPINGCART_PRODUCTOR_CAN_NOT_BUY_SELF_PRODUCT") : "")
                    },
                    JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     生成订单
        /// </summary>
        /// <param name="strPostData"></param>
        /// <returns></returns>
        public ActionResult GenerateOrder(string orderData)
        {
            //生成订单数据
            var addOrderInfoView = StringUtility.DeserializeObject<AddOrderInfoView>(orderData);

            addOrderInfoView.LanguageId = CultureHelper.GetLanguageID();
            addOrderInfoView.UserId = this.UserID;
            addOrderInfoView.OrderSource = (int)OrderEnums.OrderSource.Web;
            addOrderInfoView.PaidType = (int)OrderEnums.PaidType.Mall; //支付单类型
            addOrderInfoView.PaymentOrderId = MemCacheFactory.GetCurrentMemCache().Increment("commonId").ToString();
            //获取商品信息，以商家分组
            List<ComInfo> comInfos =
                this._shoppingCartService.getGoodsGroupByCom(1.ToString(), CultureHelper.GetLanguageID(),
                    this.UserID.ToString()).Data;
            if (addOrderInfoView.ReceiverAddressId != 0)
            {
                //获取订单运费
                var expressFee = this._orderService.GetOrdersExpressMoney(comInfos, addOrderInfoView.ReceiverAddressId);
            }
            //addOrderInfoView.MerchantViews = comInfos.Select(x => new HKTHMall.Domain.WebModel.Models.Orders.AddOrderInfoView.MerchantView()
            //     {
            //         MerchantID = x.ComId,
            //         Remark = "",                    
            //         Goods = x.Goods.Select(y => new HKTHMall.Domain.WebModel.Models.Orders.AddOrderInfoView.GoodsView()
            //         {
            //             ProductID = y.GoodsId,
            //             ProductNumber = y.Count.ToString(),
            //             SkuNumber = y.SkuNumber
            //         }) .ToList<AddOrderInfoView.GoodsView>()

            //     }).ToList<AddOrderInfoView.MerchantView>();

            //操作结果
            var data = new object();
            if (addOrderInfoView != null)
            {
                //把支付单信息放入缓存（存放时间为2天）
                var isSuccessed = MemCacheFactory.GetCurrentMemCache()
                    .AddCache("ZF" + addOrderInfoView.PaymentOrderId, addOrderInfoView, 2 * 24 * 60);

                data = new
                {
                    status = isSuccessed ? 1 : 0,
                    paymentOrder = addOrderInfoView.PaymentOrderId
                };

                if (isSuccessed)
                {
                    OrderMQ.SendMsgToMQ(addOrderInfoView.PaymentOrderId);
                }
            }
            return new JsonResult { Data = data };
        }

        /// <summary>
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult AgainPaymentOrder(string orderId)
        {
            var result = this._orderService.AgainPaymentOrder(new OrderView { OrderID = orderId, UserID = this.UserID });
            if (result.IsValid)
            {
                return this.RedirectToAction("PaymentAction", "Payment", new { paymentOrderId = result.Data });
            }
            return this.Redirect("~/Account/My/Order");
        }

        /// <summary>
        ///     支付
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns></returns>
        public ActionResult Payment(string paymentOrderId)
        {
            this.ViewBag.PaymentOrderId = paymentOrderId;
            return this.View();
        }

        /// <summary>
        ///     订单处理结果
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns></returns>
        public ActionResult OrderProcessResult(string paymentOrderId)
        {
            var tempResult = MemCacheFactory.GetCurrentMemCache().GetCache<TempResultModel>("DDJG" + paymentOrderId);

            var result = tempResult == null ? null : tempResult.ConvertToResultModel(tempResult);

            var addOrderInfoView = MemCacheFactory.GetCurrentMemCache()
                .GetCache<AddOrderInfoView>("ZF" + paymentOrderId);

            //处理中
            if (result == null && addOrderInfoView != null)
            {
                result = new ResultModel
                {
                    Status = (int)OrderEnums.GenerateOrderFailType.Processing,
                    IsValid = false
                };
            }

            //订单处理失败
            else if (result == null && addOrderInfoView == null)
            {
                result = new ResultModel
                {
                    Status = (int)OrderEnums.GenerateOrderFailType.Fail,
                    IsValid = false
                };
            }
            else
            {
                //订单不是待处理中状态,就清除缓存中的订单状态数据
                if (result.Status != (int)OrderEnums.GenerateOrderFailType.Processing)
                {
                    MemCacheFactory.GetCurrentMemCache().ClearCache("DDJG" + paymentOrderId);
                }
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Address()
        {
            List<UserAddress> addressCollection = this._userAddressService.GetUserAllAddress(
                new SearchUserAddressModel { UserID = this.UserID }, CultureHelper.GetLanguageID()).Data;
            dynamic model = new ExpandoObject();
            model.UserAddresss = addressCollection;

            return PartialView(model);
        }

        [Authorize]
        public JsonResult GetCurrentUserEmail()
        {
            var result = new { Email = this.Email };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}