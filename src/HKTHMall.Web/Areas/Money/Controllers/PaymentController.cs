using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using HKTHMall.Core;
using HKTHMall.Core.Config;
using HKTHMall.Core.Payment.LinePay;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Sys;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Services.Common;
using HKTHMall.Services.Orders;
using HKTHMall.Services.Sys;
using HKTHMall.Services.YHUser;
using HKTHMall.Web.Common;
using HKTHMall.Web.Controllers;
using Omise;
using PayPal;
using PayPal.Api;

using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Services.Banner;
using APIContext = PayPal.APIContext;
using ConfigManager = PayPal.Manager.ConfigManager;
using OAuthTokenCredential = PayPal.OAuthTokenCredential;


namespace HKTHMall.Web.Areas.Money.Controllers
{
    /// <summary>
    /// 支付控制器
    /// </summary>
    public class PaymentController : BaseController
    {
        /// <summary>
        /// 支付单服务
        /// </summary>
        private readonly IPaymentOrderService _paymentOrderService;

        /// <summary>
        /// 订单服务实体
        /// </summary>
        private readonly IOrderService _orderService;

        /// <summary>
        /// 用户服务
        /// </summary>
        private readonly IYH_UserService _userService;

        /// <summary>
        /// 系统参数服务
        /// </summary>
        private readonly IParameterSetService _parameterSetService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="paymentOrderService"></param>
        /// <param name="orderService"></param>
        /// <param name="userService"></param>
        /// <param name="parameterSetService"></param>
        public PaymentController(IPaymentOrderService paymentOrderService, IOrderService orderService, IYH_UserService userService, IParameterSetService parameterSetService)
        {
            _paymentOrderService = paymentOrderService;
            _orderService = orderService;
            _userService = userService;
            _parameterSetService = parameterSetService;
        }




        /// <summary>
        /// 选择支付页面
        /// </summary>
        /// <param name="paymentOrderId">支付单号</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult PaymentAction(string paymentOrderId)
        {
            if (string.IsNullOrEmpty(paymentOrderId))
            {
                return RedirectToAction("NotFound", "Home", new { area = string.Empty });
            }

            ResultModel resultModel =
                _paymentOrderService.GetPaymentActionData(
                    new PaymentOrderView() { PaymentOrderID = paymentOrderId, UserID = this.UserID },
                    CultureHelper.GetLanguageID());

            if (!resultModel.IsValid || resultModel.Data == null || resultModel.Data.PaymentOrderView.Flag != (int)OrderEnums.PaymentFlag.NonPaid)
            {
                return RedirectToAction("NotFound", "Home", new { area = string.Empty });
            }
            //用户信息（用于余额支付）
            resultModel.Data.UserInfoViewForPayment = _userService.GetYH_UserForPayment(this.UserID).Data;

            //151****4610
            //resultModel.Data.UserInfoViewForPayment.Phone = resultModel.Data.UserInfoViewForPayment.Phone.Substring(0, 3)
            //    + "***"
            //    + resultModel.Data.UserInfoViewForPayment.Phone.Substring(resultModel.Data.UserInfoViewForPayment.Phone.Length - 4);

            return View(resultModel.Data);
        }

        [Authorize]
        //[HttpPost]
        public ActionResult PaymentPostAction(PaymentActionPostView paymentActionPostView)
        {

            if (string.IsNullOrEmpty(paymentActionPostView.PaymentOrderId))
            {
                return RedirectToAction("NotFound", "Home", new { area = string.Empty });
            }
            PaymentOrderView searchPaymentOrder = new PaymentOrderView()
            {
                PaymentOrderID = paymentActionPostView.PaymentOrderId,
                UserID = UserID
            };

            //查找支付单
            ResultModel paymentResult = _paymentOrderService.GetPaymentOrderBy(searchPaymentOrder);


            //只有未支付的订单才处理
            if (!paymentResult.IsValid || paymentResult.Data == null || (paymentResult.Data.Flag != (int)(OrderEnums.PaymentFlag.NonPaid)))
            {
                return RedirectToAction("NotFound", "Home", new { area = string.Empty });
            }



            //支付单实体
            PaymentOrderView paymentOrder = paymentResult.Data;
            int balance = (int)OrderEnums.PayChannel.Balance;


            bool isSuccess = true;


            if (
                //余额支付但不是混合支付 且与原支付通道不一致需要更新支付方式
                !(balance == paymentActionPostView.PayChannel && (paymentActionPostView.PayChannel2 == 0 || paymentActionPostView.PayChannel != paymentActionPostView.PayChannel2))
                //当前支付通道与原支付通道不一致需要更新支付方式
               && paymentOrder.PayChannel != paymentActionPostView.PayChannel)
            {
                paymentOrder.PayChannel = paymentActionPostView.PayChannel;
                paymentOrder.OrderNO = string.Empty;
                paymentOrder.RechargeAmount = 0;
                isSuccess = _paymentOrderService.UpdatePayChannel(paymentOrder).IsValid;
            }

            if (!isSuccess)
            {
                TempData["Result"] = new ResultModel()
                {
                    Data = paymentActionPostView.PaymentOrderId
                };
                return RedirectToAction("Fail");
            }


            switch ((OrderEnums.PayChannel)paymentActionPostView.PayChannel)
            {
                case OrderEnums.PayChannel.PayPal:

                    ParameterSetModel parameter =
                        _parameterSetService.GetParameterSetById(ParameterSetExtension.PAYPAL_API_TYPE).Data;

                    //是否调用PayPal REST API ，默认为Classic API（sandbox升级 测试环境不能使用）
                    bool isCallRESTAPI = parameter != null && parameter.PValue == "1";

                    if (isCallRESTAPI)
                    {
                        return this.PayPalPayment(paymentOrder);
                    }
                    else
                    {
                        return this.SetExpressCheckout(paymentOrder);
                    }
                case OrderEnums.PayChannel.Omise:
                    return RedirectToAction("OmisePayment", new { paymentOrderId = paymentOrder.PaymentOrderID });
                case OrderEnums.PayChannel.Balance:
                    //余额支付 两个通道都是余额支付、说明足够余额支付此单
                    if (paymentActionPostView.PayChannel == paymentActionPostView.PayChannel2)
                    {
                        return this.PaymentBalance(paymentActionPostView, paymentOrder, true);
                    }
                    else //混合支付
                    {
                        return this.PaymentMixture(paymentActionPostView, paymentOrder);
                    }
                case OrderEnums.PayChannel.COD:
                    //货到付款
                    if (_paymentOrderService.PaymentCODOrder(paymentOrder).IsValid)
                    {
                        TempData["PaymentOrder"] = paymentOrder;
                        return RedirectToAction("CODSuccess", "Payment", new { area = "Money" });
                    }
                    else
                    {
                        TempData["Result"] = new ResultModel()
                        {
                           Data=paymentActionPostView.PaymentOrderId
                        };
                        return RedirectToAction("Fail");
                    }
                case OrderEnums.PayChannel.LinePay:
                    return this.LinePayReserve(paymentOrder);
                default:
                    return RedirectToAction("NotFound", "Home", new { area = string.Empty });
            }
        }


        #region PayPal

        /// <summary>
        /// Paypal支付设置交易详情
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns></returns>
        [Authorize]
        private ActionResult SetExpressCheckout(PaymentOrderView paymentOrder)
        {
            //进入SetExpressCheckout
            Logger.Write("PayPal_Log", "进入SetExpressCheckout");
            try
            {
                //获取PayPal支付配置文件
                Dictionary<string, string> paypalConfig = PayPal.Manager.ConfigManager.Instance.GetProperties();
                SetExpressCheckoutRequestDetailsType ecDetails = new SetExpressCheckoutRequestDetailsType();
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/";

                ecDetails.ReturnURL = baseUrl + paypalConfig["ReturnUrl"];
                ecDetails.CancelURL = baseUrl + paypalConfig["CancelUrl"];
                ecDetails.ReqConfirmShipping = "0";
                ecDetails.AddressOverride = "0";
                ecDetails.NoShipping = "0";
                ecDetails.Custom = paymentOrder.PaymentOrderID;
                ecDetails.InvoiceID = paymentOrder.PaymentOrderID;

                ecDetails.SolutionType = SolutionTypeType.SOLE;    //结账流程

                //支付详情
                PaymentDetailsType paymentDetails = new PaymentDetailsType();
                ecDetails.PaymentDetails.Add(paymentDetails);

                double orderTotal = 0.0;       //商品费用+运费

                double itemTotal = 0.0;        //订单所有项目成本总和

                CurrencyCodeType currency = CurrencyCodeType.HKD;       //币种

                //运费
                //paymentDetails.ShippingTotal = new BasicAmountType(currency, "0.05");
                //orderTotal += Convert.ToDouble("0.05");
                //保险费用
                //paymentDetails.InsuranceTotal = new BasicAmountType(currency, "0.01");
                //paymentDetails.InsuranceOptionOffered = "true";
                //orderTotal += Convert.ToDouble("0.01");
                ////处理此订单的成本
                //paymentDetails.HandlingTotal = new BasicAmountType(currency, "0.02");
                //orderTotal += Convert.ToDouble("0.02");
                ////税收
                //paymentDetails.TaxTotal = new BasicAmountType(currency, "1.1");
                //orderTotal += Convert.ToDouble("1.1");

                //订单描述
                paymentDetails.OrderDescription = "";

                //收取款类型
                paymentDetails.PaymentAction = PaymentActionCodeType.SALE;
                //AddressType shipAddress = new AddressType();
                //shipAddress.Name = "樊利民";
                //shipAddress.Street1 = "龙华民乐科技园圆梦科技大夏";
                //shipAddress.CityName = "深圳市";
                //shipAddress.StateOrProvince = "广东省";
                //shipAddress.Country = CountryCodeType.CN;
                //shipAddress.PostalCode = "518000";
                //shipAddress.Phone = "18207678755";
                //ecDetails.PaymentDetails[0].ShipToAddress = shipAddress;



                PaymentDetailsItemType itemDetails = new PaymentDetailsItemType();
                itemDetails.Name = "NO:" + paymentOrder.PaymentOrderID;
                itemDetails.Amount = new BasicAmountType(currency, paymentOrder.ProductAmount.ToString());
                itemDetails.Quantity = 1;
                itemDetails.ItemCategory = ItemCategoryType.PHYSICAL;
                itemTotal += Convert.ToDouble(itemDetails.Amount.value) * itemDetails.Quantity.Value;

                //营业税
                //itemDetails.Tax = new BasicAmountType(currency, "0.03");
                //orderTotal += Convert.ToDouble("0.03");
                //itemDetails.Description = "华为P8,你的选择没有错";
                paymentDetails.PaymentDetailsItem.Add(itemDetails);

                orderTotal += itemTotal;
                paymentDetails.ItemTotal = new BasicAmountType(currency, itemTotal.ToString());
                paymentDetails.OrderTotal = new BasicAmountType(currency, orderTotal.ToString());

                paymentDetails.NotifyURL = baseUrl + paypalConfig["IPNListenerUrl"];
                ecDetails.LocaleCode = paypalConfig["LocaleCode"];
                ecDetails.cppHeaderImage = paypalConfig["LogoUrl"];

                SetExpressCheckoutRequestType request = new SetExpressCheckoutRequestType(ecDetails);
                request.SetExpressCheckoutRequestDetails = ecDetails;

                SetExpressCheckoutReq wrapper = new SetExpressCheckoutReq();
                wrapper.SetExpressCheckoutRequest = request;

                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();

                //响应类型
                SetExpressCheckoutResponseType setECResponse = service.SetExpressCheckout(wrapper);

                if (setECResponse != null)
                {
                    if (setECResponse.Ack.ToString().Trim().ToUpper().Equals("SUCCESS"))
                    {
                        Session["PayPalToken"] = setECResponse.Token;
                        Logger.Write("PayPal_Log", "获取Token正常,支付单号为:" + paymentOrder.PaymentOrderID);//获取Token正常,支付单号为:
                        return
                            Redirect(paypalConfig["RedirectUrl"] + setECResponse.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("PayPal_Log", "SetExpressCheckout:" + ex.Message);
            }

            TempData["Result"] = new ResultModel()
            {
                Data=paymentOrder.PaymentOrderID
            };
            return RedirectToAction("Fail");
        }

        /// <summary>
        /// PayPal支付第二步获取交易详情
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult GetExpressCheckoutDetails()
        {
            Logger.Write("PayPal_Log", "进入GetExpressCheckoutDetails");//进入GetExpressCheckoutDetails
            try
            {
                GetExpressCheckoutDetailsResponseType responseType = new GetExpressCheckoutDetailsResponseType();

                GetExpressCheckoutDetailsReq getExpressCheckoutDetails = new GetExpressCheckoutDetailsReq();

                string EcToken = (string)(Session["PayPalToken"]);

                GetExpressCheckoutDetailsRequestType requestType = new GetExpressCheckoutDetailsRequestType(EcToken);

                getExpressCheckoutDetails.GetExpressCheckoutDetailsRequest = requestType;

                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();

                responseType = service.GetExpressCheckoutDetails(getExpressCheckoutDetails);

                if (responseType != null)
                {
                    if (responseType.Ack.ToString().Trim().ToUpper().Equals("SUCCESS"))
                    {
                        string payerId = responseType.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerID;

                        var paymentdetail = responseType.GetExpressCheckoutDetailsResponseDetails.PaymentDetails.FirstOrDefault();
                        Dictionary<string, string> paypalParams = new Dictionary<string, string>();
                        paypalParams.Add("PayPalPayerId", payerId);
                        paypalParams.Add("Currency_Code", ((int)paymentdetail.OrderTotal.currencyID).ToString());
                        paypalParams.Add("Order_Total", paymentdetail.OrderTotal.value);
                        paypalParams.Add("Shipping_Total", paymentdetail.ShippingTotal.value);
                        paypalParams.Add("InvoiceID", paymentdetail.InvoiceID);
                        paypalParams.Add("TransactionId", paymentdetail.TransactionId);

                        Session["PayPalParams"] = paypalParams;
                        //获取交易详情正常,支付单号为:
                        Logger.Write("PayPal_Log", "获取交易详情正常,支付单号为:" + paymentdetail.InvoiceID);
                        return RedirectToAction("DoExpressCheckoutPayment");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("PayPal_Log", "GetExpressCheckoutDetails:" + ex.Message);
            }

            TempData["Result"] = new ResultModel();
            return RedirectToAction("Fail");
        }

        /// <summary>
        /// PayPal支付第三部
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult DoExpressCheckoutPayment()
        {
            Logger.Write("PayPal_Log", "进入DoExpressCheckoutPayment");
            //获取paypal配置文件中的参数
            Dictionary<string, string> paypalConfig = PayPal.Manager.ConfigManager.Instance.GetProperties();

            DoExpressCheckoutPaymentResponseType responseType = new DoExpressCheckoutPaymentResponseType();
            try
            {
                DoExpressCheckoutPaymentReq paymentReq = new DoExpressCheckoutPaymentReq();
                DoExpressCheckoutPaymentRequestDetailsType requestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
                requestDetails.Token = (string)(Session["PayPalToken"]);

                //获取paypal session中参数
                Dictionary<string, string> paypalParams = Session["PayPalParams"] as Dictionary<string, string>;

                requestDetails.PayerID = paypalParams["PayPalPayerId"];

                List<PaymentDetailsType> paymentDetailsList = new List<PaymentDetailsType>();
                PaymentDetailsType paymentDetails = new PaymentDetailsType();

                CurrencyCodeType currency_code_type = (CurrencyCodeType)int.Parse(paypalParams["Currency_Code"]);

                PaymentActionCodeType payment_action_type = PaymentActionCodeType.SALE;

                string total_amount = paypalParams["Order_Total"];

                BasicAmountType orderTotal = new BasicAmountType(currency_code_type, total_amount);

                paymentDetails.OrderTotal = orderTotal;
                paymentDetails.PaymentAction = payment_action_type;


                SellerDetailsType sellerDetails = new SellerDetailsType();
                sellerDetails.PayPalAccountID = paypalConfig["SellerEmail"];
                paymentDetails.SellerDetails = sellerDetails;


                paymentDetailsList.Add(paymentDetails);
                requestDetails.PaymentDetails = paymentDetailsList;

                DoExpressCheckoutPaymentRequestType paymentRequest = new DoExpressCheckoutPaymentRequestType(requestDetails);
                paymentReq.DoExpressCheckoutPaymentRequest = paymentRequest;

                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();

                responseType = service.DoExpressCheckoutPayment(paymentReq);
                if (responseType != null)
                {
                    if (responseType.Ack.ToString().Trim().ToUpper().Equals("SUCCESS"))
                    {
                        if (responseType.DoExpressCheckoutPaymentResponseDetails.PaymentInfo != null)
                        {
                            PaymentInfoType paymentInfo = responseType.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.FirstOrDefault();


                            if (paymentInfo.PaymentStatus == PaymentStatusCodeType.COMPLETED)
                            {
                                var paymentOrder = new PaymentOrderView()
                                {
                                    PaymentOrderID = paypalParams["InvoiceID"],
                                    outOrderId = paymentInfo.TransactionID,
                                    RealAmount = decimal.Parse(paymentInfo.GrossAmount.value)
                                };

                                //支付成功
                                Logger.Write("PayPal_Log", string.Format("支付成功  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));

                                ResultModel result = _paymentOrderService.PaymentOrder(paymentOrder);


                                //日志记录
                                if (result.IsValid)
                                {
                                    //支付成功,更新数据库成功
                                    Logger.Write("PayPal_Log", string.Format("支付成功,更新数据库成功 PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                                    return this.RedirectSuccess(paymentOrder.PaymentOrderID, result);
                                }
                                else
                                {
                                    //支付成功,更新数据库失败
                                    Logger.Write("PayPal_Log", string.Format("支付成功,更新数据库失败  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                                }


                            }

                            Logger.Write("PayPal_Log",
                                string.Format("PaymentOrderID={0},outOrderId={1},PaymentStatus={2}",
                                    paypalParams["InvoiceID"],
                                    paymentInfo.TransactionID,
                                    paymentInfo.PaymentStatus
                                    ));
                        }
                    }
                    else
                    {
                        List<ErrorType> errorMessages = responseType.Errors;
                        string errorMessage = "";
                        foreach (ErrorType error in errorMessages)
                        {
                            errorMessage += error.LongMessage;
                        }
                        Logger.Write("PayPal_Log", "DoExpressCheckoutPayment:" + errorMessage);
                    }
                }
                //进行交易处理失败
                Logger.Write("PayPal_Log", "DoExpressCheckoutPayment:进行交易处理失败");
            }
            catch (System.Exception ex)
            {
                Logger.Write("PayPal_Log", "DoExpressCheckoutPayment:" + ex.Message);
            }

            TempData["Result"] = new ResultModel();
            return RedirectToAction("Fail");
        }

        /// <summary>
        /// PayPal即时付款通知
        /// </summary>
        public void IPNListener()
        {
            //获取paypal配置文件中的参数
            Dictionary<string, string> paypalConfig = PayPal.Manager.ConfigManager.Instance.GetProperties();
            try
            {
                byte[] parameters = Request.BinaryRead(System.Web.HttpContext.Current.Request.ContentLength);

                if (parameters.Length > 0)
                {
                    IPNMessage ipn = new IPNMessage(paypalConfig, parameters);

                    bool isIpnValidated = ipn.Validate();
                    string transactionType = ipn.TransactionType;
                    NameValueCollection map = ipn.IpnMap;
                    if (isIpnValidated)
                    {
                        if (map.AllKeys.Contains("payment_status") && map.GetValues("payment_status").FirstOrDefault() == "Completed")
                        {
                            var paymentOrder = new PaymentOrderView()
                            {
                                PaymentOrderID = map.GetValues("invoice").FirstOrDefault(),
                                outOrderId = map.GetValues("txn_id").FirstOrDefault(),
                                RealAmount = decimal.Parse(map.GetValues("mc_gross").FirstOrDefault())
                            };
                            if (!string.IsNullOrEmpty(paymentOrder.PaymentOrderID) &&
                                !string.IsNullOrEmpty(paymentOrder.outOrderId) &&
                                paymentOrder.RealAmount != 0)
                            {
                                //支付成功
                                Logger.Write("PayPal_Log", string.Format("支付成功  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                                ResultModel result = _paymentOrderService.PaymentOrder(paymentOrder);
                                if (result.IsValid)
                                {
                                    //支付成功,更新数据库成功
                                    Logger.Write("PayPal_Log", string.Format("支付成功,更新数据库成功  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                                }
                                else
                                {
                                    //支付成功,更新数据库失败
                                    Logger.Write("PayPal_Log", string.Format("支付成功,更新数据库失败  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                                }
                            }
                        }
                    }
                    //消息通知没有通过验证
                    Logger.Write("PayPal_Log", "IPNListener:消息通知没有通过验证");
                }
            }
            catch (System.Exception ex)
            {
                Logger.Write("PayPal_Log", "IPNListener:" + ex.Message);
            }
        }

        [Authorize]
        public void TransactionDetails(string transactionId)
        {
            //获取paypal配置文件中的参数
            Dictionary<string, string> paypalConfig = PayPal.Manager.ConfigManager.Instance.GetProperties();


            GetTransactionDetailsRequestType request = new GetTransactionDetailsRequestType();
            request.TransactionID = transactionId;

            GetTransactionDetailsReq wrapper = new GetTransactionDetailsReq();

            wrapper.GetTransactionDetailsRequest = request;

            PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService(paypalConfig);


            GetTransactionDetailsResponseType transactionDetails = service.GetTransactionDetails(wrapper);

            processResponse(service, transactionDetails);

        }

        private void processResponse(PayPalAPIInterfaceServiceService service,
            GetTransactionDetailsResponseType response)
        {
            HttpContext CurrContext = System.Web.HttpContext.Current;
            CurrContext.Items.Add("Response_apiName", "GetTransactionDetails");
            CurrContext.Items.Add("Response_redirectURL", null);
            CurrContext.Items.Add("Response_requestPayload", service.getLastRequest());
            CurrContext.Items.Add("Response_responsePayload", service.getLastResponse());

            Dictionary<string, string> keyResponseParameters = new Dictionary<string, string>();
            keyResponseParameters.Add("Correlation Id", response.CorrelationID);
            keyResponseParameters.Add("API Result", response.Ack.ToString());

            if (response.Ack.Equals(AckCodeType.FAILURE) ||
                (response.Errors != null && response.Errors.Count > 0))
            {
                Response.Write("失败");

            }
            else
            {
                Response.Write("成功");

                //response.PaymentTransactionDetails.PaymentItemInfo

                //CurrContext.Items.Add("Response_error", null);
                //PaymentTransactionType transactionDetails = response.PaymentTransactionDetails;
                //keyResponseParameters.Add("Payment receiver", transactionDetails.ReceiverInfo.Receiver);
                //keyResponseParameters.Add("Payer", transactionDetails.PayerInfo.Payer);
                //keyResponseParameters.Add("Payment date", transactionDetails.PaymentInfo.PaymentDate);
                //keyResponseParameters.Add("Payment status", transactionDetails.PaymentInfo.PaymentStatus.ToString());
                //keyResponseParameters.Add("Gross amount",
                //    transactionDetails.PaymentInfo.GrossAmount.value +
                //    transactionDetails.PaymentInfo.GrossAmount.currencyID.ToString());

                //if (transactionDetails.PaymentInfo.SettleAmount != null)
                //{
                //    keyResponseParameters.Add("Settlement amount",
                //        transactionDetails.PaymentInfo.SettleAmount.value +
                //        transactionDetails.PaymentInfo.SettleAmount.currencyID.ToString());
                //}
            }
            //CurrContext.Items.Add("Response_keyResponseObject", keyResponseParameters);
        }


        #endregion

        #region Omise

        /// <summary>
        /// omise支付
        /// </summary>
        /// <param name="paymentOrderId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult OmisePayment(string paymentOrderId)
        {
          

            PaymentOrderView requestPaymentOrder = new PaymentOrderView()
            {
                PaymentOrderID = paymentOrderId,
                UserID = this.UserID
            };
            ResultModel result = _paymentOrderService.GetPaymentOrderBy(requestPaymentOrder);

            if (!result.IsValid || result.Data == null || result.Data.Flag != (int)(OrderEnums.PaymentFlag.NonPaid) || result.Data.PayChannel != (int)OrderEnums.PayChannel.Omise)
            {
                return RedirectToAction("NotFound", "Home", new { area = string.Empty });
            }
            PaymentOrderView paymentOrder = result.Data;
            ViewBag.PaymentOrderId = paymentOrder.PaymentOrderID;
            ViewBag.ProductAmount = paymentOrder.ProductAmount;
            return View("OmisePayment");
        }

        /// <summary>
        /// omise支付提交
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult OmisePayment(FormCollection collection)
        {

            ResultModel tempResult = OmisePaymentService.OmisePayment(new OmisePaymentService.OmisePaymentModel()
            {
                PaymentOrderId = Request.Form["paymentOrderId"],
                Token = Request.Form["omise_token"],
                Amount = decimal.Parse(Request.Form["amount"]),
                UserId = UserID.ToString()
            });

            tempResult.IsValid = tempResult.Status == 1;
            if (tempResult.IsValid)
            {
                return this.RedirectSuccess(Request.Form["paymentOrderId"], tempResult);
            }
            TempData["Result"] = new ResultModel()
            {
                Data = Request.Form["paymentOrderId"]
            };

            return RedirectToAction("Fail");
        }

        #endregion

        #region Balance

        /// <summary>
        /// 余额支付
        /// </summary>
        /// <param name="paymentActionPostView">提交的支付数据</param>
        /// <param name="paymentOrder">支付单数据</param>
        /// <param name="isCheckPayPassword">是否检查交易密码（如果是混合支付，充值回来是无需检查交易密码）</param>
        /// <returns></returns>
        [Authorize]
        private ActionResult PaymentBalance(PaymentActionPostView paymentActionPostView, PaymentOrderView paymentOrder, bool isCheckPayPassword)
        {

            Logger.Write("Balance_Log", string.Format("进行余额支付 paymentOrderId={0},RealAmount={1}", paymentOrder.PaymentOrderID, paymentOrder.ProductAmount));
            //余额支付结果
            var balanceResult = _paymentOrderService.PaymentBalanceOrder(paymentOrder,
                                    new UserInfoViewForPayment()
                                    {
                                        LanguageId = CultureHelper.GetLanguageID(),
                                        UserID = this.UserID,
                                        PayPassWord = isCheckPayPassword ? CodeHelper.GetMD5(paymentActionPostView.PayPassword) : ""
                                    }, isCheckPayPassword);

            if (balanceResult.IsValid)
            {
                paymentOrder.RealAmount = paymentOrder.ProductAmount;

                //余额支付成功之后更新支付订单
                if (_paymentOrderService.PaymentOrder(paymentOrder).IsValid)
                {
                    Logger.Write("Balance_Log", string.Format("余额支付成功 paymentOrderId={0},RealAmount={1}", paymentOrder.PaymentOrderID, paymentOrder.RealAmount));
                    if (!string.IsNullOrEmpty(this.Email) && RegexUtil.IsEmail(this.Email))
                    {
                        string ProductName = _paymentOrderService.GetProductNameForEmail(paymentOrder.PaymentOrderID, CultureHelper.GetLanguageID()).Data;
                        ProductName = string.IsNullOrEmpty(ProductName) ? "" : ProductName;
                        try
                        {
                            Mail.sendMail(this.Email, CultureHelper.GetLangString("ACCOUNT_BALANCECHANGE_NOTICE")
                                    , string.Format(CultureHelper.GetLangString("ACCOUNT_PAYSUCCESS_EMAIL"), this.Email, paymentOrder.RealAmount, ProductName, paymentOrder.CreateDT.ToString("yyyy-MM-dd HH:mm:ss")));
                        }
                        catch (Exception e)
                        {
                            Logger.Write(e.Message);
                        }
                    }
                    return this.RedirectSuccess(paymentOrder.PaymentOrderID, balanceResult);
                }
                else
                {
                    TempData["Result"] = new ResultModel()
                    {
                        Messages = balanceResult.Messages,
                        Data = paymentActionPostView.PaymentOrderId
                    };
                    return RedirectToAction("Fail");
                }
            }
            else
            {
                TempData["Result"] = new ResultModel()
                {
                    Messages = balanceResult.Messages,
                    Data=paymentActionPostView.PaymentOrderId
                };
                return RedirectToAction("Fail");
            }
        }

        /// <summary>
        /// 混合支付
        /// </summary>
        /// <param name="paymentActionPostView">提交的支付信息</param>
        /// <param name="paymentOrder">支付单对象</param>
        /// <returns></returns>
        [Authorize]
        private ActionResult PaymentMixture(PaymentActionPostView paymentActionPostView, PaymentOrderView paymentOrder)
        {

            int[] pays = { (int)OrderEnums.PayChannel.Omise, (int)OrderEnums.PayChannel.PayPal, (int)OrderEnums.PayChannel.LinePay };

            if (!pays.Contains(paymentActionPostView.PayChannel2))
            {
                //支付类型，不在允许列表中
                TempData["Result"] = new ResultModel()
                {
                    Data=paymentActionPostView.PaymentOrderId
                };
                return RedirectToAction("Fail");
            }


            //用户信息（用户支付）
            UserInfoViewForPayment userInfoView = _userService.GetYH_UserForPayment(UserID).Data;

            //检查用户是否删除、锁定等
            ResultModel userInfoResultModel = _userService.GetYH_UserForPaymentMessage(userInfoView, new UserInfoViewForPayment()
            {
                LanguageId = CultureHelper.GetLanguageID(),
                UserID = this.UserID,
                PayPassWord = CodeHelper.GetMD5(paymentActionPostView.PayPassword)
            });

            //用户、交易密码错误等信息
            if (!userInfoResultModel.IsValid)
            {
                TempData["Result"] = new ResultModel()
                {
                    Messages = userInfoResultModel.Messages,
                    Data = paymentActionPostView.PaymentOrderId
                };
                return RedirectToAction("Fail");
            }
            else
            {

                //用户余额大于支付单金额，无需充值，直接进行余额支付
                if (paymentOrder.ProductAmount - userInfoView.ConsumeBalance <= 0)
                {
                    return this.PaymentBalance(paymentActionPostView, paymentOrder, false);
                }

                //需要充值部分的支付单号
                string paymentOrderID = string.Empty;

                //充值单号
                string orderNO = string.Empty;

                AccountRechargeWebs arwb = new AccountRechargeWebs();

                //充值参数类
                HKTHMall.Domain.WebModel.Models.AccountRecharge.AccountRechargeModel armodel = new HKTHMall.Domain.WebModel.Models.AccountRecharge.AccountRechargeModel();
                armodel.Account = Account;
                armodel.AddOrCutAmount = paymentOrder.ProductAmount - userInfoView.ConsumeBalance;
                armodel.AddOrCutType = 1;
                armodel.RechargeChannel = paymentActionPostView.PayChannel2;
                armodel.UserID = UserID;

                var result = arwb.InsertAddZJ_RechargeOrder(armodel, ERechargeOrderPrefix.Mixture, out orderNO, out paymentOrderID);


                //混合支付 将充值单号、充值金额插入支付单号中
                paymentOrder.OrderNO = orderNO;
                paymentOrder.RechargeAmount = armodel.AddOrCutAmount;
                paymentOrder.PayChannel = (int)OrderEnums.PayChannel.Balance;

                Logger.Write("Mixture_Log",
                    string.Format("进行混合支付 充值单paymentOrderId={0},ProductAmount={1} 主单 paymentOrderId={2}", paymentOrderID, armodel.AddOrCutAmount, paymentOrder.PaymentOrderID));
                if (result.IsValid)
                {
                    //更新支付单号，让它与充值单号关联
                    ResultModel paymentResult = _paymentOrderService.UpdatePayChannel(paymentOrder);
                    if (paymentResult.IsValid)
                    {
                        return this.PaymentPostAction(new PaymentActionPostView()
                        {
                            PayChannel = paymentActionPostView.PayChannel2,
                            PaymentOrderId = paymentOrderID
                        });

                    }

                }
                TempData["Result"] = new ResultModel()
                {
                    Data=paymentActionPostView.PaymentOrderId
                };
                return RedirectToAction("Fail");
            }
        }


        #endregion

        #region LinePay

        [Authorize]
        private ActionResult LinePayReserve(PaymentOrderView paymentOrder)
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/";

            ReserveService service = new ReserveService();
            service.ReserveDetailReq = new ReserveDetailReq()
            {
                amount = paymentOrder.ProductAmount.ToString("0.00"),
                confirmUrl = baseUrl + "money/payment/linepayconfirm",
                currency = "THB",
                cancelUrl = baseUrl + "order/list.html",
                productImageUrl = "http://www.0066mall.com/Content/css/images/logo-th.png",
                orderId = paymentOrder.PaymentOrderID,
                productName = "product"
            };

            dynamic res = service.ReserveRequest();
            if (res.returnCode == "0000")
            {
                paymentOrder.outOrderId = res.info.transactionId.ToString();

                ResultModel resultModel = _paymentOrderService.Update(paymentOrder);
                if (resultModel.IsValid)
                {

                    Logger.Write("LinePay_Log", "returnCode=" + res.returnCode + ";transactionId=" + res.info.transactionId);
                    string url = res.info.paymentUrl.web;
                    return Redirect(url);
                }
            }

            Logger.Write("LinePay_Log", "returnCode=" + res.returnCode + ";returnMessage=" + res.returnMessage);
            TempData["Result"] = new ResultModel()
            {
                Data=paymentOrder.PaymentOrderID
            };
            return RedirectToAction("Fail");
        }

        [Authorize]
        public ActionResult LinePayConfirm()
        {

            string transactionId = Request.QueryString["transactionId"];


            ResultModel result = _paymentOrderService.GetPaymentOrderBy(OrderEnums.PayChannel.LinePay,
                transactionId, this.UserID);

            if (!result.IsValid || result.Data == null || result.Data.Flag != (int)(OrderEnums.PaymentFlag.NonPaid) && result.Data.PayChannel != (int)OrderEnums.PayChannel.LinePay)
            {
                return RedirectToAction("NotFound", "Home", new { area = string.Empty });
            }
            PaymentOrderView paymentOrder = result.Data;
            ConfirmService service = new ConfirmService();
            service.ConfirmDetailReq = new ConfirmDetailReq()
            {
                amount = paymentOrder.ProductAmount.ToString("0.00"),
                currency = "THB"
            };

            dynamic res = service.ConfirmRequest(transactionId);

            if (res.returnCode == "0000")
            {
                //支付成功
                Logger.Write("LinePay_Log", string.Format("支付成功  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));


                if (res.info.payInfo[0].amount != paymentOrder.ProductAmount)
                {
                    //支付成功
                    Logger.Write("PayPal_Log", string.Format("支付成功  但是支付金额和支付单金额不一致 PaymentOrderID={0},outOrderId={1},RealAmount={2},ProductAmount={3}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount, paymentOrder.ProductAmount));
                }
                else
                {
                    paymentOrder.RealAmount = paymentOrder.ProductAmount;
                    ResultModel tempResult = _paymentOrderService.PaymentOrder(paymentOrder);
                    //日志记录
                    if (result.IsValid)
                    {
                        //支付成功,更新数据库成功
                        Logger.Write("LinePay_Log", string.Format("支付成功,更新数据库成功 PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                        return this.RedirectSuccess(paymentOrder.PaymentOrderID, result);
                    }
                    else
                    {
                        //支付成功,更新数据库失败
                        Logger.Write("LinePay_Log", string.Format("支付成功,更新数据库失败  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                    }
                }
            }
            Logger.Write("LinePay_Log", "returnCode=" + res.returnCode + ";returnMessage=" + res.returnMessage);
            TempData["Result"] = new ResultModel()
            {
                Data = paymentOrder.PaymentOrderID
            };
            return RedirectToAction("Fail");
        }


        #endregion

        #region New PayPal

        /// <summary>
        /// New PayPal支付
        /// </summary>
        /// <param name="paymentOrder"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult PayPalPayment(PaymentOrderView paymentOrder = null)
        {

            Payment payment = null;
            PayPal.Api.APIContext apiContext = HKTHMall.Core.Payment.PayPalPay.REST.Common.Configuration.GetAPIContext();

            //如果为空，创建PayPal支付
            if (Request.Params["PayerID"] != null)
            {
                try
                {
                    payment = new Payment();
                    if (Request.Params["guid"] != null)
                    {
                        payment.id = (string)Session[Request.Params["guid"]];

                    }
                    PaymentExecution pymntExecution = new PaymentExecution();
                    pymntExecution.payer_id = Request.Params["PayerID"];

                    Payment executedPayment = payment.Execute(apiContext, pymntExecution);
                    if (!string.IsNullOrEmpty(executedPayment.id))
                    {
                        payment = Payment.Get(apiContext, executedPayment.id);
                    }
                    if (executedPayment.state == "approved" && !string.IsNullOrEmpty(executedPayment.id) &&
                        payment.state == "approved")
                    {
                        PayPal.Api.Transaction trans = payment.transactions.FirstOrDefault();

                        paymentOrder = new PaymentOrderView()
                       {
                           PaymentOrderID = trans.invoice_number,
                           outOrderId = executedPayment.id,
                           RealAmount = decimal.Parse(trans.amount.total)
                       };

                        //支付成功
                        Logger.Write("PayPal_Log", string.Format("New PayPal 支付成功  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));

                        //进行数据库更新操作
                        ResultModel result = _paymentOrderService.PaymentOrder(paymentOrder);


                        //日志记录
                        if (result.IsValid)
                        {
                            //支付成功,更新数据库成功
                            Logger.Write("PayPal_Log", string.Format("New PayPal 支付成功,更新数据库成功 PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                            return this.RedirectSuccess(paymentOrder.PaymentOrderID, result);
                        }
                        else
                        {
                            //支付成功,更新数据库失败
                            Logger.Write("PayPal_Log", string.Format("New PayPal 支付成功,更新数据库失败  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                        }
                    }
                    else
                    {
                        Logger.Write("PayPal_Log", string.Format("Payment id:{0}", executedPayment.id));
                    }

                }
                catch (Exception ex)
                {
                    Logger.Write("PayPal_Log", "New PayPal 回调错误:" + ex.Message);
                }

                TempData["Result"] = new ResultModel()
                {
                    Data=paymentOrder.PaymentOrderID
                };
                return RedirectToAction("Fail");
            }
            else
            {

                //创建PayPal支付
                Payer payer = new Payer();
                payer.payment_method = "paypal";

                Random rndm = new Random();
                var guid = Convert.ToString(rndm.Next(100000));


                string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority;

                RedirectUrls redirUrls = new RedirectUrls();
                redirUrls.cancel_url = baseURI + "order/list.html" + "?guid=" + guid;
                redirUrls.return_url = baseURI + Url.Action("PayPalPayment", "Payment") + "?guid=" + guid;

                Details details = new Details();
                details.tax = "0";              //税
                details.shipping = "0";        //运费
                details.subtotal = paymentOrder.ProductAmount.ToString();       //小计



                Amount amnt = new Amount();
                amnt.currency = "HKD";//"USD";//
                amnt.total = paymentOrder.ProductAmount.ToString();
                amnt.details = details;

                List<PayPal.Api.Transaction> transactionList = new List<PayPal.Api.Transaction>();

                PayPal.Api.Transaction tran = new PayPal.Api.Transaction();
                tran.description = "NO:" + paymentOrder.PaymentOrderID;
                tran.amount = amnt;
                tran.invoice_number = paymentOrder.PaymentOrderID;
                tran.custom = paymentOrder.PaymentOrderID;
                tran.notify_url = baseURI + "Money/Payment/IPNListener";
                transactionList.Add(tran);

                payment = new Payment();
                payment.intent = "sale";
                payment.payer = payer;
                payment.transactions = transactionList;
                payment.redirect_urls = redirUrls;
                try
                {

                    Logger.Write("PayPal_Log", "进入New Paypal 请求");

                    Payment createdPayment = payment.Create(apiContext);
                    var links = createdPayment.links.GetEnumerator();

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            Session.Add(guid, createdPayment.id);
                            return Redirect(lnk.href);      //跳转到PayPal支付
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Write("PayPal_Log", "进入New Paypal 请求错误:" + ex.Message);
                }
                TempData["Result"] = new ResultModel()
                {
                    Data=paymentOrder.PaymentOrderID
                };
                return RedirectToAction("Fail");
            }


        }


        #endregion

        /// <summary>
        /// 订单失败
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        public ActionResult Fail()
        {
            var result = TempData["Result"] as ResultModel;

            if (result == null || result.Data == null)
            {
                return Redirect("~/Account/My/Order");
            }

            var failResult = TempData["Result"] as ResultModel;
            if (result.Data != null && result.Data.GetType() == typeof(string))
            {
                List<string> orderIds = _orderService.GetOrderIdByPaymentOrderId(result.Data).Data;     //多订单不支持重新支付
                failResult.Data = orderIds != null && orderIds.Count == 1 ? orderIds.FirstOrDefault() : string.Empty;
            }
            ViewBag.Result = failResult;
            return View();
        }

        /// <summary>
        /// 订单成功
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        public ActionResult Success()
        {
            if (TempData["paymentOrderId"] == null)
            {
                return Redirect("~/Account/My/Order");
            }
            ViewBag.Order = _orderService.GetOrderDetailsByPaymentOrderId(TempData["paymentOrderId"].ToString()).Data;
            return View();
        }
        /// <summary>
        /// 跳转成功页面
        /// </summary>
        /// <param name="paymentOrderId">支付单ID</param>
        /// <param name="result">操作结果</param>
        /// <returns></returns>
        private ActionResult RedirectSuccess(string paymentOrderId, ResultModel result)
        {
            if (result.IsValid)
            {
                PaymentOrderView paymentOrder = _paymentOrderService.GetPaymentOrderBy(new PaymentOrderView()
                {
                    PaymentOrderID = paymentOrderId,
                    UserID = this.UserID
                }).Data;

                switch ((OrderEnums.PaidType)paymentOrder.PayType)
                {
                    case OrderEnums.PaidType.Mall:
                        TempData["paymentOrderId"] = paymentOrderId;
                        return RedirectToAction("Success");
                    case OrderEnums.PaidType.Recharge:

                        PaymentOrderView balancePayment =
                            _paymentOrderService.GetPaymentOrderByOrderNO(paymentOrder).Data;

                        //如果该支付单是混合支付的支付单，则要进行后续的余额支付
                        if (balancePayment != null && balancePayment.Flag == (int)OrderEnums.PaymentFlag.NonPaid)
                        {
                            return this.PaymentBalance(new PaymentActionPostView() { }, balancePayment, false);
                        }
                        else if (_paymentOrderService.IsCurrentRechargeOrder(paymentOrderId, ERechargeOrderPrefix.Mixture).IsValid)
                        {
                            TempData["Result"] = new ResultModel()
                            {
                                Data = paymentOrderId
                            };
                            return RedirectToAction("Fail");
                        }
                        else
                        {
                            return RedirectToAction("AccountRechargeSuccess", "../Account/My");

                        }
                    default:
                        return RedirectToAction("NotFound", "Home", new { area = string.Empty });
                }
            }
            return RedirectToAction("Success");
        }

        /// <summary>
        /// 订单生成成功
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult CODSuccess()
        {
            if (TempData["PaymentOrder"] == null)
            {
                return Redirect("~/Account/My/Order");
            }
            PaymentOrderView paymentOrder = TempData["PaymentOrder"] as PaymentOrderView;
            return View(paymentOrder);
        }

        /// <summary>
        /// 验证交易密码
        /// </summary>
        /// <param name="payPassword">交易密码</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ValidPayPassword(string payPassword)
        {
            ResultModel resultModel = new ResultModel();
            UserInfoViewForPayment userInfoView = _userService.GetYH_UserForPayment(this.UserID).Data;
            resultModel = _userService.GetYH_UserForPaymentMessage(userInfoView, new UserInfoViewForPayment()
            {
                UserID = this.UserID,
                PayPassWord = CodeHelper.GetMD5(payPassword),
                LanguageId = CultureHelper.GetLanguageID()
            });

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

    }
}