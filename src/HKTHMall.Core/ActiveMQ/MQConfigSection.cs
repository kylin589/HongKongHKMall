using System;
using System.Configuration;
using NLog;

namespace HKTHMall.Core.ActiveMQ
{
    /// <summary>
    /// 消息队列配置
    /// </summary>
    public sealed class MQConfigSection : ConfigurationSection
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static MQConfigSection GetConfig()
        {
            MQConfigSection section = null;
            try
            {
                section = (MQConfigSection)ConfigurationManager.GetSection("ActiveMQ/MQConfig");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " Section ActiveMQ/MQConfig is error.");
                throw new ConfigurationErrorsException("Section ActiveMQ/MQConfig is error.");
            }

            if (section == null)
            {
                logger.Error("Section ActiveMQ/MQConfig is not found.");
                throw new ConfigurationErrorsException("Section ActiveMQ/MQConfig is not found.");
            }

            return section;
        }

        public static MQConfigSection GetConfig(string sectionName)
        {
            MQConfigSection section = null;
            try
            {
                section = (MQConfigSection)ConfigurationManager.GetSection(sectionName);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + "Section " + sectionName + " is error.");
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
            }

            if (section == null)
            {
                logger.Error("Section " + sectionName + " is not found.");
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
            }

            return section;
        }

        /// <summary>
        /// 消息队列服务器IP
        /// </summary>
        [ConfigurationProperty("Host", IsRequired = true)]
        public string Host
        {
            get
            {
                return (string)base["Host"];
            }
            set
            {
                base["Host"] = value;
            }
        }

        /// <summary>
        /// 消息队列服务器端口
        /// </summary>
        [ConfigurationProperty("Port", IsRequired = false, DefaultValue = 61616)]
        public int Port
        {
            get
            {
                return (int)base["Port"];
            }
            set
            {
                base["Port"] = value;
            }
        }

        /// <summary>
        /// 消息队列服务器用户名
        /// </summary>
        [ConfigurationProperty("UserName", IsRequired = false)]
        public string UserName
        {
            get
            {
                return (string)base["UserName"];
            }
            set
            {
                base["UserName"] = value;
            }
        }

        /// <summary>
        /// 消息队列服务器用户名密码
        /// </summary>
        [ConfigurationProperty("Password", IsRequired = false)]
        public string Password
        {
            get
            {
                return (string)base["Password"];
            }
            set
            {
                base["Password"] = value;
            }
        }

        /// <summary>
        /// 自动重启
        /// </summary>
        [ConfigurationProperty("Debug", IsRequired = false, DefaultValue = false)]
        public bool Debug
        {
            get
            {
                return (bool)base["Debug"];
            }
            set
            {
                base["Debug"] = value;
            }
        }

        /// <summary>
        /// 消息队列名
        /// </summary>
        [ConfigurationProperty("TopicName", IsRequired = false)]
        public string TopicName
        {
            get
            {
                return (string)base["TopicName"];
            }
            set
            {
                base["TopicName"] = value;
            }
        }

        /// <summary>
        /// 消息ClientId
        /// </summary>
        [ConfigurationProperty("ClientId", IsRequired = false)]
        public string ClientId
        {
            get
            {
                return (string)base["ClientId"];
            }
            set
            {
                base["ClientId"] = value;
            }
        }
    }
}
