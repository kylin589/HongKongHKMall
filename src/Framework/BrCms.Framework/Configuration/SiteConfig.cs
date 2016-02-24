
using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace BrCms.Framework.Configuration
{
    public class SiteConfig
    {
        private static SiteConfig _configuration;

        public static SiteConfig Configuration
        {
            get { return _configuration ?? (_configuration = new SiteConfig()); }
        }

        public SiteConfig()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                ConfigurationManager.AppSettings["site.Config"]));
            var siteNode = xmlDocument.SelectSingleNode("site");
            if (siteNode == null)
            {
                throw new Exception("请配置siteNode节点。");
            }
            this.Create(siteNode);
        }

        private void Create(XmlNode section)
        {
            var pluginNode = section.SelectSingleNode("plugin");
            if (pluginNode != null && pluginNode.Attributes != null)
            {
                var attribute = pluginNode.Attributes["enabled"];
                if (attribute != null)
                    this.PluginEnabled = Convert.ToBoolean(attribute.Value);
            }
            var containerNameNode = section.SelectSingleNode("system");
            if (containerNameNode != null && containerNameNode.Attributes != null)
            {
                var attribute = containerNameNode.Attributes["name"];
                if (attribute != null)
                    this.ContainerName = attribute.Value;
            }
        }

        /// <summary>
        /// 是否开启插件功能。
        /// </summary>
        public bool PluginEnabled { get; private set; }

        /// <summary>
        /// 密匙容器名称
        /// </summary>
        public string ContainerName { get; private set; }
    }
}
