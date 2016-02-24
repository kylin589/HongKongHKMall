using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autofac;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using HKTHMall.Core;
using HKTHMall.Core.Configuration;
using HKTHMall.Core.Extensions;
using HKTHMall.Core.Security;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Services.Orders;
using HKTHMall.Services.YHUser;
using HKTHMall.WebApi.Models;
using PayPal.Api;
using HKTHMall.WebApi.Models.Request;
using HKTHMall.WebApi.Models.Result;

using HKTHMall.Services.Common;

namespace HKTHMall.WebApi.Controllers
{
    /// <summary>
    /// 支付接口
    /// </summary>
    public class PaymentController : ApiController
    {
        /// <summary>
        /// 支付服务对象
        /// </summary>
        private IPaymentOrderService _paymentOrderService;

        /// <summary>
        /// 订单服务实体
        /// </summary>
        private readonly IOrderService _orderService;

        /// <summary>
        /// 加密服务实体
        /// </summary>
        private readonly IEncryptionService _enctyptionService;

        /// <summary>
        /// 用户服务
        /// </summary>
        private readonly IYH_UserService _userService;

        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="paymentOrderService">支付服务对象</param>
        /// <param name="orderService">订单服务实体</param>
        /// <param name="enctyptionService">加密服务实体</param>
        /// <param name="userService"></param>
        /// <param name="logger">日志对象</param>
        public PaymentController(IPaymentOrderService paymentOrderService, IOrderService orderService, IEncryptionService enctyptionService, IYH_UserService userService, ILogger logger)
        {
            _paymentOrderService = paymentOrderService;
            _orderService = orderService;
            _enctyptionService = enctyptionService;
            _userService = userService;
            _logger = logger;
        }


        #region 7.1.获取支付单号（樊利民）

        /// <summary>
        /// 7.1.获取支付单号（樊利民）
        /// </summary>
        /// <param name="requestModel">请求实体</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult PaymentCart(RequestGeneratePaymentOrderModel requestModel)
        {
            var tempResult = this._orderService.AgainPaymentOrder(new OrderView
            {
                OrderID = requestModel.OrderId,
                UserID = Convert.ToInt64(_enctyptionService.RSADecrypt(requestModel.UserId))
            });
            ApiResultModel result = new ApiResultModel()
            {
                flag = tempResult.Flag,
                rs = new
                    {
                        paymentOrderId = tempResult.Data
                    },
                msg = tempResult.Flag == 1 ? "获取支付单号成功" : "获取支付单号失败"
            };
            return this.Ok(result);
        }

        #endregion

        #region 7.2.支付成功操作（樊利民）

        /// <summary>
        /// 7.2.支付成功操作（樊利民）
        /// </summary>
        /// <param name="requestModel">请求支付实体</param>
        /// <returns>支付结果</returns>
        [HttpPost]
        public IHttpActionResult PaymentSuccess(RequestPaymentOrdersModel requestModel)
        {
            ResultModel resultModel = new ResultModel();
            PaymentOrderView requestPaymentOrder = new PaymentOrderView()
            {
                PaymentOrderID = requestModel.PaymentOrderId,
                UserID = Convert.ToInt64(_enctyptionService.RSADecrypt(requestModel.UserId))
            };
            ResultModel tempResult = _paymentOrderService.GetPaymentOrderBy(requestPaymentOrder);
           
            //支付单必须为未支付,并且支付单实际支付金额和支付单金额相等
            if (!tempResult.IsValid || tempResult.Data == null || tempResult.Data.Flag != (int)(OrderEnums.PaymentFlag.NonPaid) || requestModel.RealAmount != tempResult.Data.ProductAmount)
            {
                resultModel.Status = -1;
            }
            else
            {
                resultModel = this.VerifyPayPalPayment(requestModel, tempResult.Data);
            }
            switch (resultModel.Status)
            {
                default:
                case 0:
                    resultModel.Messages.Add("更新失败");
                    break;
                case 1:
                    resultModel.Messages.Add("更新成功");
                    break;
                case -1:
                    resultModel.Messages.Add("参数非法");
                    break;
                case -2:
                    resultModel.Messages.Add("没有查到相关订单记录");
                    break;
            }
            ApiResultModel result = new ApiResultModel()
            {
                flag = resultModel.Status == 1 ? 1 : 0,
                msg = resultModel.Messages[0]
            };
            return Ok(result);
        }

        #endregion

        #region 7.3 Omise 泰国支付(樊利民)

        [HttpPost]
        public IHttpActionResult OmisePayment(RequestOmisePaymentModel requestModel)
        {
            ApiResultModel result = new ApiResultModel()
            {
                flag = 0
            };

            //解密UserId
            requestModel.UserId = _enctyptionService.RSADecrypt(requestModel.UserId);
            requestModel.OmiseToken = _enctyptionService.RSADecrypt(requestModel.OmiseToken);

            PaymentOrderView searchPaymentOrder = new PaymentOrderView()
            {
                PaymentOrderID = requestModel.PaymentOrderId,
                UserID = long.Parse(requestModel.UserId)
            };

            //查找支付单
            ResultModel paymentResult = _paymentOrderService.GetPaymentOrderBy(searchPaymentOrder);


            //只有未支付的订单才处理
            if (!paymentResult.IsValid || paymentResult.Data == null ||
                (paymentResult.Data.Flag != (int)(OrderEnums.PaymentFlag.NonPaid)))
            {
                return this.Ok(result);
            }
            else
            {
                PaymentOrderView paymentOrder = paymentResult.Data;
                paymentOrder.OrderNO = string.Empty;
                paymentOrder.RechargeAmount = 0;
                //更新支付通道
                if (!UpdatePayChannel(paymentOrder, (int)OrderEnums.PayChannel.Omise))
                {
                    return this.Ok(result);
                }
            }

            ResultModel tempResult = OmisePaymentService.OmisePayment(new OmisePaymentService.OmisePaymentModel()
             {
                 PaymentOrderId = requestModel.PaymentOrderId,
                 Amount = requestModel.Amount,
                 Token = requestModel.OmiseToken,
                 UserId = requestModel.UserId
             });

            result.flag = tempResult.Status == 1 ? 1 : 0;
            result.msg = tempResult.Messages[0];

            return this.Ok(result);

        }

        #endregion

        #region 7.4 余额支付(樊利民)

        [HttpPost]
        public IHttpActionResult BalancePayment(RequestBalancePaymentModel requestModel)
        {

            long userId = long.Parse(_enctyptionService.RSADecrypt(requestModel.UserId));

            string payPassword = _enctyptionService.RSADecrypt(requestModel.PayPassword);

            ApiResultModel resultModel = new ApiResultModel()
            {
                flag = 0
            };

            PaymentOrderView searchPaymentOrder = new PaymentOrderView()
            {
                PaymentOrderID = requestModel.PaymentOrderId.ToString(),
                UserID = userId
            };

            //查找支付单
            ResultModel paymentResult = _paymentOrderService.GetPaymentOrderBy(searchPaymentOrder);

            //只有未支付的订单才处理
            if (!paymentResult.IsValid || paymentResult.Data == null ||
                (paymentResult.Data.Flag != (int)(OrderEnums.PaymentFlag.NonPaid)))
            {

            }
            else
            {

                PaymentOrderView paymentOrder = paymentResult.Data;

                //更新支付通道
                if (!UpdatePayChannel(paymentOrder, (int)OrderEnums.PayChannel.Balance))
                {
                    return this.Ok(resultModel);
                }


                //余额支付结果
                ResultModel balanceResult = _paymentOrderService.PaymentBalanceOrder(paymentOrder,
                                        new UserInfoViewForPayment()
                                        {
                                            LanguageId = requestModel.Lang,
                                            UserID = userId,
                                            PayPassWord = payPassword
                                        }, true);

                if (balanceResult.IsValid)
                {
                    paymentOrder.RealAmount = paymentOrder.ProductAmount;

                    //余额支付成功之后更新支付订单
                    if (_paymentOrderService.PaymentOrder(paymentOrder).IsValid)
                    {
                        resultModel.flag = 1;
                    }
                }
                else
                {
                    resultModel.msg = balanceResult.Messages.Count > 0 ? balanceResult.Messages[0] : string.Empty;
                }


            }
            return this.Ok(resultModel);

        }

        #endregion

        #region 7.5 混合支付(樊利民)

        [HttpPost]
        public IHttpActionResult MixturePayment(RequestMixturePaymentModel requestModel)
        {

            ApiResultModel resultModel = new ApiResultModel()
            {
                flag = 0
            };

            int[] pays = { (int)OrderEnums.PayChannel.Omise, (int)OrderEnums.PayChannel.PayPal };

            if (!pays.Contains(requestModel.PayChannel))
            {
                return this.Ok(resultModel);
            }


            long userId = long.Parse(_enctyptionService.RSADecrypt(requestModel.UserId));

            YH_UserInfoModel userInfo = _userService.GetUserInfo(userId, requestModel.Lang).Data;

            //用户信息（用户支付）
            UserInfoViewForPayment userInfoView = _userService.GetYH_UserForPayment(userId).Data;

            PaymentOrderView searchPaymentOrder = new PaymentOrderView()
            {
                PaymentOrderID = requestModel.PaymentOrderId.ToString(),
                UserID = userId
            };

            //查找支付单
            ResultModel paymentResult = _paymentOrderService.GetPaymentOrderBy(searchPaymentOrder);

            //只有未支付的订单才处理
            if (!paymentResult.IsValid || paymentResult.Data == null ||
                (paymentResult.Data.Flag != (int)(OrderEnums.PaymentFlag.NonPaid)))
            {
                return this.Ok(resultModel);
            }

            var paymentOrder = paymentResult.Data;

            //需要充值部分的支付单号
            string paymentOrderID = string.Empty;

            //充值单号
            string orderNO = string.Empty;

            AccountRechargeWebs arwb = new AccountRechargeWebs();

            //充值参数类
            HKTHMall.Domain.WebModel.Models.AccountRecharge.AccountRechargeModel armodel = new HKTHMall.Domain.WebModel.Models.AccountRecharge.AccountRechargeModel();
            armodel.Account = userInfo.account;
            armodel.AddOrCutAmount = paymentOrder.ProductAmount - userInfoView.ConsumeBalance;
            armodel.AddOrCutType = 1;
            armodel.RechargeChannel = requestModel.PayChannel;
            armodel.UserID = userId;

            var result = arwb.InsertAddZJ_RechargeOrder(armodel, ERechargeOrderPrefix.Mixture, out orderNO, out paymentOrderID);

            //混合支付 将充值单号、充值金额插入支付单号中
            paymentOrder.OrderNO = orderNO;
            paymentOrder.RechargeAmount = armodel.AddOrCutAmount;
            paymentOrder.PayChannel = (int)OrderEnums.PayChannel.Balance;

            if (result.IsValid)
            {
                //更新支付单号，让它与充值单号关联
                ResultModel tempPaymentResult = _paymentOrderService.UpdatePayChannel(paymentOrder);
                if (tempPaymentResult.IsValid)
                {

                    PaymentOrderView rechargePaymentOrder =
                        _paymentOrderService.GetPaymentOrderBy(new PaymentOrderView()
                        {
                            PaymentOrderID = paymentOrderID,
                            UserID = userId
                        }).Data;
                    resultModel.flag = 1;
                    resultModel.rs = rechargePaymentOrder;

                }

            }

            return this.Ok(resultModel);

        }
        #endregion

        #region 7.6 货到付款

        [HttpPost]
        public IHttpActionResult CODPayment(RequestCODPaymentModel requestModel)
        {

            long userId = long.Parse(_enctyptionService.RSADecrypt(requestModel.UserId));

            ApiResultModel resultModel = new ApiResultModel()
            {
                flag = 0
            };



            PaymentOrderView searchPaymentOrder = new PaymentOrderView()
            {
                PaymentOrderID = requestModel.PaymentOrderId.ToString(),
                UserID = userId
            };

            //查找支付单
            ResultModel paymentResult = _paymentOrderService.GetPaymentOrderBy(searchPaymentOrder);

            //只有未支付的订单才处理
            if (!paymentResult.IsValid || paymentResult.Data == null ||
                (paymentResult.Data.Flag != (int)(OrderEnums.PaymentFlag.NonPaid)))
            {
                return this.Ok(resultModel);
            }
            else
            {
                PaymentOrderView paymentOrder = paymentResult.Data;
                //更新支付通道
                if (!UpdatePayChannel(paymentOrder, (int)OrderEnums.PayChannel.COD))
                {
                    return this.Ok(resultModel);
                }

                if (_paymentOrderService.PaymentCODOrder(paymentOrder).IsValid)
                {
                    resultModel.flag = 1;

                }
            }



            return this.Ok(resultModel);

        }

        #endregion


        #region 私有方法

        /// <summary>
        /// 更新支付通道
        /// </summary>
        /// <param name="paymentOrder">支付单对象</param>
        /// <param name="currentPayChannel"></param>
        /// <returns></returns>
        private bool UpdatePayChannel(PaymentOrderView paymentOrder, int currentPayChannel)
        {
            bool isSuccess = true;
            if (paymentOrder.PayChannel != currentPayChannel)
            {
                paymentOrder.PayChannel = currentPayChannel;
                isSuccess = _paymentOrderService.UpdatePayChannel(paymentOrder).IsValid;
            }
            return isSuccess;
        }


        /// <summary>
        /// paypal验证此支付单是否有效
        /// </summary>
        /// <param name="requestModel">请求模型</param>
        /// <returns>结果</returns>
        private ResultModel VerifyPayPalPayment(RequestPaymentOrdersModel requestModel, PaymentOrderView paymentOrder)
        {

            long userId = long.Parse(_enctyptionService.RSADecrypt(requestModel.UserId));

            _logger.Error(typeof(PaymentController),
                             string.Format(
                                 "PayPal_Log 进入 VerifyPayPalPayment  PaymentOrderID={0},outOrderId={1},RealAmount={2}",
                                 requestModel.PaymentOrderId, requestModel.outOrderId, requestModel.RealAmount));
            ResultModel resultModel = new ResultModel();
            try
            {
                var apiContext = PayPalConfiguration.GetAPIContext();
                var payment = Payment.Get(apiContext, requestModel.outOrderId);
                Transaction trans = payment.transactions.FirstOrDefault();




                if (trans == null || trans.invoice_number != requestModel.PaymentOrderId || payment.state != "approved" || decimal.Parse(trans.related_resources[0].sale.amount.total) != paymentOrder.ProductAmount)
                {
                    resultModel.IsValid = false;
                    resultModel.Status = -2;    //没有查到相关订单记录
                }
                else
                {

                    paymentOrder.OrderNO = string.Empty;
                    paymentOrder.RechargeAmount = 0;

                    //更新支付通道
                    if (!UpdatePayChannel(paymentOrder, (int)OrderEnums.PayChannel.PayPal))
                    {
                        resultModel.Status = 0;
                        return resultModel;
                    }

                    _logger.Error(typeof(PaymentController),
                             string.Format(
                                 "PayPal_Log VerifyPayPalPayment 验证成功 PaymentOrderID={0},outOrderId={1},RealAmount={2}",
                                 requestModel.PaymentOrderId, requestModel.outOrderId, requestModel.RealAmount));
                    resultModel = _paymentOrderService.PaymentOrder(new PaymentOrderView()
                    {
                        PaymentOrderID = requestModel.PaymentOrderId,
                        outOrderId = requestModel.outOrderId,
                        RealAmount = decimal.Parse(trans.amount.total)
                    });
                    _logger.Error(typeof(PaymentController),
                             string.Format(
                                 "PayPal_Log VerifyPayPalPayment 验证成功 更新数据库{3} PaymentOrderID={0},outOrderId={1},RealAmount={2}",
                                 requestModel.PaymentOrderId, requestModel.outOrderId, requestModel.RealAmount, resultModel.IsValid ? "成功" : "失败"));
                    resultModel.Status = resultModel.IsValid ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(typeof(PaymentController), string.Format("PayPal_Log VerifyPayPalPayment ERROR:{0} PaymentOrderID={1},outOrderId={2},RealAmount={3}", ex.Message, requestModel.PaymentOrderId, requestModel.outOrderId, requestModel.RealAmount));
                resultModel.IsValid = false;
                resultModel.Status = 0;    //没有查到相关订单记录
            }
            return resultModel;
        }


        #endregion

    }
}
