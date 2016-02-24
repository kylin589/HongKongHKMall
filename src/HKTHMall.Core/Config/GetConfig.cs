using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.Core.Config
{
    public class GetConfig
    {
        public static string ImagePath()
        {
            string ImagePath = ConfigurationManager.AppSettings["ImagePath"];
            return ImagePath;
        }
        public static string Host()
        {
            string Host = ConfigurationManager.AppSettings["Host"];
            return Host;
        }
        public static string FullPath()
        {
            string FullPath = ImagePath();
            return FullPath;
        }

        public static string GetWebSite()
        {
            string web = "";
            web = ConfigurationManager.AppSettings["WebSite"];
            return web;
        }


        public static string ExpressKey()
        {
            string key = "";
            key = ConfigurationManager.AppSettings["Express100Key"];
            return key;
        }
        public static string ExpressUrl()
        {
            string url = "";
            url = ConfigurationManager.AppSettings["Express100Url"];
            return url;
        }

        /// <summary>
        /// 获取Omise支付私钥
        /// </summary>
        /// <returns>Omise支付私钥</returns>
        public static string OmiseSecretKey()
        {
            string key = "";
            key = ConfigurationManager.AppSettings["OmiseSecretKey"];
            return key;
        }

        /// <summary>
        /// 获取Omise支付公钥
        /// </summary>
        /// <returns>Omise支付公钥</returns>
        public static string OmisePublicKey()
        {
            string key = "";
            key = ConfigurationManager.AppSettings["OmisePublicKey"];
            return key;
        }
    }
}