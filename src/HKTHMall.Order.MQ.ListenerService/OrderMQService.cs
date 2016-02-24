using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Autofac;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using HKTHMall.Core.ActiveMQ;
using HKTHMall.Services.Orders.MQ;

namespace HKTHMall.Order.MQ.ListenerService
{
    /// <summary>
    /// 订单消息队列服务（消费端）
    /// </summary>
    public partial class OrderMQService : ServiceBase
    {
        /// <summary>
        /// 消息队列实体
        /// </summary>
        private static MQEntity MQEntity;

        /// <summary>
        /// 消息队列配置对象
        /// </summary>
        private static MQConfigSection mqConfig;

        /// <summary>
        /// 消费者
        /// </summary>
        private static IMessageConsumer Consumer;


        /// <summary>
        /// 日志对象
        /// </summary>
        private static ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderMQService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                _logger = BrEngineContext.Current.Resolve<ILogger>();
                mqConfig = MQConfigSection.GetConfig();
                MQEntity = new MQEntity();
                MQEntity.Uri = "failover:tcp://" + mqConfig.Host + ":" + mqConfig.Port;
                MQEntity.Topic = mqConfig.TopicName;
                MQEntity.UserName = mqConfig.UserName;
                MQEntity.Password = mqConfig.Password;
                MQEntity.Start();
                Consumer = MQEntity.CreateConsumerQueue(MQEntity.Topic);
                Consumer.Listener += new MessageListener(ConsumerListener);
                _logger.Error(typeof(OrderMQService), "*****************初始化监听订单消息队列成功******************");
            }
            catch (Exception ex)
            {
                _logger.Error(typeof(OrderMQService), "*****************初始化监听订单消息队列失败******************");
            }
        }

        protected override void OnStop()
        {
        }


        /// <summary>
        /// 监听处理函数
        /// </summary>
        /// <param name="message"></param>
        private static void ConsumerListener(IMessage message)
        {
            try
            {
                OrderMQ.OnReceiveOrderMessage(message);
            }
            catch (Exception ex)
            {
                _logger.Error(typeof(OrderMQService),
                    string.Format("*****************监听订单消息队列异常 {0}******************", ex.Message));
            }
        }
    }
}
