using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using BrCms.Framework.Mvc.Extensions;
using HKSJ.Common;
using HKTHMall.Core.Config;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Orders;
using Omise;

namespace HKTHMall.Services.Orders
{
    /// <summary>
    /// Omise 泰国支付
    /// </summary>
    public class OmisePaymentService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly static ILogger Logger = BrEngineContext.Current.Resolve<ILogger>();

        /// <summary>
        /// 支付服务对象
        /// </summary>
        private readonly static IPaymentOrderService _paymentOrderService = BrEngineContext.Current.Resolve<IPaymentOrderService>();

        /// <summary>
        /// Omise 泰国支付
        /// </summary>
        /// <param name="omisePaymentModel">omise支付对象</param>
        /// <returns>处理结果</returns>
        public static ResultModel OmisePayment(OmisePaymentService.OmisePaymentModel omisePaymentModel)
        {
            Logger.Error("Omise_Log", string.Format("进入OmisePayment PaymentOrderID:{0}", omisePaymentModel.PaymentOrderId));
            PaymentOrderView requestPaymentOrder = new PaymentOrderView()
            {
                PaymentOrderID = omisePaymentModel.PaymentOrderId,
                UserID = Convert.ToInt64(omisePaymentModel.UserId)
            };

            //处理结果
            OmisePaymentResultType resultCode = OmisePaymentResultType.Fail;
            ResultModel result = _paymentOrderService.GetPaymentOrderBy(requestPaymentOrder);
            if (!result.IsValid || result.Data == null || result.Data.Flag != (int)(OrderEnums.PaymentFlag.NonPaid) || omisePaymentModel.Amount != result.Data.ProductAmount)
            {
                resultCode = OmisePaymentResultType.ParamError;
                Logger.Error("Omise_Log", string.Format("参数异常 PaymentOrderID={0},UserId={1},Amount={2}", requestPaymentOrder.PaymentOrderID, requestPaymentOrder.UserID, omisePaymentModel.Amount));
            }
            else
            {
                requestPaymentOrder = result.Data;
                var chargeInfo = new ChargeCreateInfo();
                chargeInfo.Amount = (int)(requestPaymentOrder.ProductAmount * 100); //Create a charge with amount 100 THB, here we are passing with the smallest currency unit which is 10000 satangs  交易金额 必须是整数（真实金额*100）。
                chargeInfo.Currency = "THB";
                chargeInfo.Description = requestPaymentOrder.PaymentOrderID;//目前这个参数很有可能是我们的订单号或支付单号用这个。
                chargeInfo.Capture = true; //TRUE means auto capture the charge, FALSE means authorize only. Default is FALSE
                chargeInfo.CardId = omisePaymentModel.Token; //Token generated with Omise.js or Card.js
                //var client = new Omise.Client(YOUR_SECRET_KEY, [YOUR_PUBLIC_KEY]);
                //chargeInfo.ReturnUri = "http://localhost:54741/Home/complete";



                try
                {
                    Client c = new Client(GetConfig.OmiseSecretKey());//new Client("skey_test_50xql6uiv02pemu8z9o");
                    //chargeInfo
                    //c.ChargeService.CreateCharge(chargeInfo);
                    var charge = c.ChargeService.CreateCharge(chargeInfo);
                    //charge.Description 
                    //charge.FailureCode
                    //charge.Captured
                    if (charge.Captured && string.IsNullOrEmpty(charge.FailureCode))
                    {

                        var paymentOrder = new PaymentOrderView()
                        {
                            PaymentOrderID = charge.Description,
                            outOrderId = charge.TransactionId,
                            RealAmount = charge.Amount / 100.00m
                        };

                        requestPaymentOrder.RealAmount = paymentOrder.RealAmount;

                        if (paymentOrder.RealAmount != requestPaymentOrder.ProductAmount)
                        {
                            Logger.Error("Omise_Log", string.Format("支付成功,但是支付金额小于需要支付的金额 PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                            resultCode = OmisePaymentResultType.LessAmount;
                        }
                        else
                        {

                            //支付成功
                            Logger.Error("Omise_Log", string.Format("支付成功  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));

                            ResultModel updateResult = _paymentOrderService.PaymentOrder(paymentOrder);
                            //日志记录
                            if (updateResult.IsValid)
                            {



                                //支付成功,更新数据库成功
                                Logger.Error("Omise_Log", string.Format("支付成功,更新数据库成功 PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                                resultCode = OmisePaymentResultType.Success;
                            }
                            else
                            {
                                //支付成功,更新数据库失败
                                Logger.Error("Omise_Log", string.Format("支付成功,更新数据库失败  PaymentOrderID={0},outOrderId={1},RealAmount={2}", paymentOrder.PaymentOrderID, paymentOrder.outOrderId, paymentOrder.RealAmount));
                                resultCode = OmisePaymentResultType.UpdateDataError;
                            }
                        }

                    }
                    else
                    {
                        resultCode = OmisePaymentResultType.Fail;
                        Logger.Error("Omise_Log", string.Format("支付失败 PaymentOrderID:{0}", requestPaymentOrder.PaymentOrderID));
                    }

                    //var result = c.ChargeService.Capture(charge.Id);
                    //Client.ChargeService.CreateCharge(chargeInfo);
                    //charge.Id //是否是交易ID（这个字段可能性大一些）
                    //charge.TransactionId  是否是交易ID
                    // charge.LiveMode是否是生产环境
                    //charge.FailureCode错误代码
                    //charge.FailureMessage错误信息
                    //var result = c.ChargeService.GetCharge("chrg_test_4xso2s8ivdej29pqnhz");
                }
                catch (Exception ex)
                {
                    resultCode = OmisePaymentResultType.Fail;
                    Logger.Error("Omise_Log", string.Format("支付异常 Error:{0} PaymentOrderID:{1}", ex.Message, requestPaymentOrder.PaymentOrderID));
                }
            }
            ResultModel resultModel = new ResultModel()
            {
                Status = (int)resultCode,
                Data = requestPaymentOrder
            };
            resultModel.Messages.Add(EnumDescription.GetFieldText(resultCode));
            return resultModel;
        }


        /// <summary>
        /// omise支付实体
        /// </summary>
        public class OmisePaymentModel
        {
            public string UserId { get; set; }

            public string Token { get; set; }

            public decimal Amount { get; set; }

            public string PaymentOrderId { get; set; }

        }

        /// <summary>
        /// omise处理结果
        /// </summary>
        public enum OmisePaymentResultType
        {
            /// <summary>
            /// 成功
            /// </summary>
            [EnumDescription("成功", 1)]
            Success = 1,

            /// <summary>
            /// 失败
            /// </summary>
            [EnumDescription("失败", -1)]
            Fail = -1,

            /// <summary>
            /// 支付金额小于需要支付的金额
            /// </summary>
            [EnumDescription("支付金额小于需要支付的金额", -2)]
            LessAmount = -2,

            /// <summary>
            /// 更新数据库失败
            /// </summary>
            [EnumDescription("更新数据库失败", -3)]
            UpdateDataError = -3,
            /// <summary>
            /// 参数错误
            /// </summary>
            [EnumDescription("参数错误", -5)]
            ParamError = -5
        }

    }
}
