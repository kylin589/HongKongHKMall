using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Autofac;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using HKTHMall.Core;
using HKTHMall.Core.ActiveMQ;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Orders;
using BrCms.Framework.Mvc.Extensions;


namespace HKTHMall.Services.Orders.MQ
{
    /// <summary>
    /// 订单消息队列（生成端）
    /// </summary>
    public class OrderMQ
    {
        /// <summary>
        /// 消息队列对象
        /// </summary>
        private static MQEntity MQEntity;

        /// <summary>
        /// 消息队列配置
        /// </summary>
        private static MQConfigSection mqConfig;

        /// <summary>
        /// 消息队列生产者
        /// </summary>
        private static IMessageProducer Producer;

        /// <summary>
        /// 日志对象
        /// </summary>
        private static ILogger _logger = BrEngineContext.Current.Resolve<ILogger>();

        /// <summary>
        /// 订单服务对象
        /// </summary>
        private static IOrderService _orderService = BrEngineContext.Current.Resolve<IOrderService>();


        /// <summary>
        /// 静态构造函数
        /// </summary>
        static OrderMQ()
        {
            try
            {

                mqConfig = MQConfigSection.GetConfig();

                MQEntity = new MQEntity();
                MQEntity.Topic = mqConfig.TopicName;
                MQEntity.Uri = "failover:tcp://" + mqConfig.Host + ":" + mqConfig.Port;
                MQEntity.UserName = mqConfig.UserName;
                MQEntity.Password = mqConfig.Password;
                MQEntity.Start();
                Producer = MQEntity.CreateProducerQueue(MQEntity.Topic);
                Producer.DeliveryMode = MsgDeliveryMode.Persistent;

                _logger.Error(typeof(OrderMQ), "*****************初始化订单消息队列成功 ******************");
            }
            catch (Exception ex)
            {
                _logger.Error(typeof(OrderMQ), "*****************初始化订单消息队列失败 ******************");
            }
        }


        /// <summary>
        /// 加入订单处理队列
        /// </summary>
        /// <param name="paymentOrderId">订单号</param>
        public static void SendMsgToMQ(string paymentOrderId)
        {
            try
            {
                IMessage msg = Producer.CreateTextMessage();
                ((ITextMessage)msg).Text = paymentOrderId;
                Producer.Send(msg);
                _logger.Error(typeof(OrderMQ), string.Format("订单编号【{0}】已加入订单处理队列", paymentOrderId));
            }
            catch (Exception ex)
            {
                _logger.Error(typeof(OrderMQ), ex.Message);
            }
        }


        /// <summary>
        /// 订单队列消费者的处理方法
        /// </summary>
        /// <param name="receivedMsg">接收到的消息</param>
        public static void OnReceiveOrderMessage(IMessage receivedMsg)
        {
            try
            {
                ITextMessage msg = receivedMsg as ITextMessage;

                _logger.Error(typeof(OrderMQ), string.Format("订单处理队列收到消息【{0}】", msg.Text));

                long paymentOrderId;
                if (Int64.TryParse(msg.Text, out paymentOrderId))
                {
                    //订单处理
                    OrderHandler(paymentOrderId);
                }
                else
                {
                    _logger.Error(typeof(OrderMQ), string.Format("OnReceiveOrderMessage: 转换成Int64失败"));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(typeof(OrderMQ), "OnReceiveOrderMessage:" + ex.Message);
            }
        }

        /// <summary>
        /// 订单处理
        /// </summary>
        /// <param name="paymentOrderId">支付单号</param>
        private static void OrderHandler(long paymentOrderId)
        {
            ResultModel result = null;
            try
            {
                AddOrderInfoView addOrderInfoView =
                    MemCacheFactory.GetCurrentMemCache().GetCache<AddOrderInfoView>("ZF" + paymentOrderId);
                if (addOrderInfoView == null)
                {
                    _logger.Error(typeof(OrderMQ), string.Format("未找到支付编号【{0}】的订单信息", paymentOrderId));
                    return;
                }

                switch ((OrderEnums.PurchaseType)addOrderInfoView.PurchaseType)
                {
                    case OrderEnums.PurchaseType.Normal:
                    default:
                        result = _orderService.GenerateNormalOrder(addOrderInfoView);
                        break;
                    case OrderEnums.PurchaseType.Outright:
                        result = _orderService.GenerateOutrightOrder(addOrderInfoView);
                        break;
                }

                //订单处理成功
                if (result.IsValid && result.Status == (int)OrderEnums.GenerateOrderFailType.Success)
                {

                    _logger.Error(typeof(OrderMQ), string.Format("支付编号【{0}】的订单已经成功处理", paymentOrderId));

                }
                else
                {
                    _logger.Error(typeof(OrderMQ),
                        string.Format("支付编号【{0}】的订单失败:{1}", paymentOrderId,
                            EnumDescription.GetFieldText((OrderEnums.GenerateOrderFailType)result.Status)));
                }
            }
            catch (Exception ex)
            {
                result = new ResultModel()
                {
                    Status = (int)OrderEnums.GenerateOrderFailType.Fail,
                };
                _logger.Error(typeof(OrderMQ), string.Format("支付编号【{0}】的订单处理异常:{1}", paymentOrderId, ex.Message));
            }
            finally
            {
               bool flag = MemCacheFactory.GetCurrentMemCache().AddCache("DDJG" + paymentOrderId, result, 24 * 60);
                MemCacheFactory.GetCurrentMemCache().ClearCache("ZF" + paymentOrderId);
            }
        }

    }
}
