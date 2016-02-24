using System;
using System.Collections.Generic;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace HKTHMall.Core.ActiveMQ
{

    /// <summary>
    /// 消息实体
    /// </summary>
    public class MQEntity
    {
        /// <summary>
        /// url地址
        /// </summary>
        private string uri;

        /// <summary>
        /// 主题
        /// </summary>
        private string topic;

        /// <summary>
        /// 用户
        /// </summary>
        private string userName;

        /// <summary>
        /// 密码
        /// </summary>
        private string password;

        /// <summary>
        /// 连接工厂
        /// </summary>
        private IConnectionFactory factory;

        /// <summary>
        /// 连接
        /// </summary>
        private IConnection connection;

        /// <summary>
        /// 会话
        /// </summary>
        private ISession session;

        /// <summary>
        /// url
        /// </summary>
        public string Uri
        {
            set { uri = value; }
            get { return uri; }
        }

        /// <summary>
        /// 主题
        /// </summary>
        public string Topic
        {
            set { topic = value; }
            get { return topic; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            set { userName = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set { password = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MQEntity()
        {
            factory = null;
            connection = null;
            session = null;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~MQEntity()
        {
            Close();
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            factory = new ConnectionFactory(uri);

            if (!string.IsNullOrEmpty(userName))
            {
                connection = factory.CreateConnection(userName, password);
            }
            else
            {
                connection = factory.CreateConnection();
            }
            connection.Start();
            session = connection.CreateSession();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (session != null)
            {
                session.Close();
            }
            if (connection != null)
            {
                connection.Stop();
                connection.Close();
            }
        }

        /// <summary>
        /// 创建生产者(主题)
        /// </summary>
        /// <param name="topicName"></param>
        public IMessageProducer CreateProducerTopic(string topicName)
        {
            return session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(topicName));
        }

        /// <summary>
        /// 创建生产者(主题)
        /// </summary>
        public IMessageProducer CreateProducerTopic()
        {
            return session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(topic));

        }

        /// <summary>
        /// 创建生产者(队列)
        /// </summary>
        /// <param name="queueName"></param>
        public IMessageProducer CreateProducerQueue(string queueName)
        {

            return session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(queueName));
        }

        /// <summary>
        /// 创建生产者(队列)
        /// </summary>
        public IMessageProducer CreateProducerQueue()
        {

            return session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(topic));
        }


        /// <summary>
        /// 创建消费者 (主题)
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        public IMessageConsumer CreateConsumerTopic(string topicName)
        {

            return session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(topicName));

        }

        /// <summary>
        /// 创建消费者 (主题)
        /// </summary>
        public IMessageConsumer CreateConsumerTopic(string topicName, string selector)
        {
            if (string.IsNullOrEmpty(selector))
            {

                return null;
            }
            return session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(topicName), selector, false);

        }

        /// <summary>
        /// 创建消费者 （队列）
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public IMessageConsumer CreateConsumerQueue(string queueName)
        {

            return session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(queueName));
        }


        /// <summary>
        /// 创建消费者 （队列）
        /// </summary>
        public IMessageConsumer CreateConsumerQueue(string queueName, string selector)
        {
            if (selector == "")
            {

                return null;
            }

            return session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(queueName), selector, false);
        }

        /// <summary>
        /// 熟悉
        /// </summary>
        public struct Property
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 值
            /// </summary>
            public string Value { get; set; }
        }


    }
}
