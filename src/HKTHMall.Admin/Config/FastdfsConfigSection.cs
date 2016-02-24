using BrCms.Framework.Logging;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Autofac;
using BrCms.Framework.Infrastructure;

namespace HKTHMall.Admin.Config
{
    public class FastdfsConfigSection : ConfigurationSection
    {
        private static ILogger logger = BrEngineContext.Current.Resolve<ILogger>();

        public static FastdfsConfigSection GetConfig()
        {
            FastdfsConfigSection section = null;
            try
            {
                section = (FastdfsConfigSection)ConfigurationManager.GetSection("FastDFS/FastdfsConfig");
            }
            catch (Exception ex)
            {
                logger.Error("HKTHMall.Admin.Config.FastdfsConfigSection",ex.Message,ex);
                throw new ConfigurationErrorsException("Section FastdfsConfig is error.");
            }

            if (section == null)
            {
                logger.Equals("Section FastdfsConfig is not found.");
                throw new ConfigurationErrorsException("Section FastdfsConfig is not found.");
            }

            return section;
        }

        public static FastdfsConfigSection GetConfig(string sectionName)
        {
            FastdfsConfigSection section = null;
            try
            {
                section = (FastdfsConfigSection)ConfigurationManager.GetSection(sectionName);
            }
            catch (Exception ex)
            {
                logger.Error("HKTHMall.Admin.Config.FastdfsConfigSection", ex.Message, ex);
                throw new ConfigurationErrorsException("Section " + sectionName + " is error.");
            }

            if (section == null)
            {
                logger.Equals("Section " + sectionName + " is not found.");
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
            }

            return section;
        }

        /// <summary>
        /// FastDFS图片服务器IP
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
        /// tracker端口
        /// </summary>
        [ConfigurationProperty("Port", IsRequired = false, DefaultValue = 22122)]
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
        /// 组名
        /// </summary>
        [ConfigurationProperty("GroupName", IsRequired = false)]
        public string GroupName
        {
            get
            {
                return (string)base["GroupName"];
            }
            set
            {
                base["GroupName"] = value;
            }
        }

        /// <summary>
        /// 浏览图片路径
        /// </summary>
        [ConfigurationProperty("ImagePath", IsRequired = false)]
        public string ImagePath
        {
            get
            {
                return (string)base["ImagePath"];
            }
            set
            {
                base["ImagePath"] = value;
            }
        }

        /// <summary>
        /// 图片文件大小限制,单位:K
        /// </summary>
        [ConfigurationProperty("ImageMaxSize", IsRequired = false, DefaultValue = 512)]
        public int ImageMaxSize
        {
            get
            {
                return (int)base["ImageMaxSize"];
            }
            set
            {
                base["ImageMaxSize"] = value;
            }
        }

        /// <summary>
        /// 图片最大高宽限制
        /// </summary>
        [ConfigurationProperty("ImageMaxHeightOrWidth", IsRequired = false, DefaultValue = 1024)]
        public int ImageMaxHeightOrWidth
        {
            get
            {
                return (int)base["ImageMaxHeightOrWidth"];
            }
            set
            {
                base["ImageMaxHeightOrWidth"] = value;
            }
        }

        /// <summary>
        /// 普通文件大小限制,单位:K
        /// </summary>
        [ConfigurationProperty("FileMaxSize", IsRequired = false, DefaultValue = 10240)]
        public int FileMaxSize
        {
            get
            {
                return (int)base["FileMaxSize"];
            }
            set
            {
                base["FileMaxSize"] = value;
            }
        }

        /// <summary>
        /// 连接池大小
        /// </summary>
        [ConfigurationProperty("Pool", IsRequired = false, DefaultValue = 100)]
        public int Pool
        {
            get
            {
                return (int)base["Pool"];
            }
            set
            {
                base["Pool"] = value;
            }
        }
    }
}