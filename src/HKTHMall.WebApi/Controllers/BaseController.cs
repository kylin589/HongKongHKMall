using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Configuration;
using HKTHMall.Domain.Enum;

namespace HKTHMall.WebApi.Controllers
{
    public class BaseController : ApiController
    {
        //浏览图片路径
        public readonly string ImagePath = ConfigurationManager.AppSettings["ImagePath"].ToString();
        //浏览用户图像路径
        public readonly string ImageHeader = ConfigurationManager.AppSettings["ImageHeader"].ToString();

        public string FormatAddress(int lang, string ShengAreaName, string ShiAreaName, string QuAreaName, string DetailsAddress)
        {
            string address = string.Empty;
            if (lang == 0)
            {
                lang = (int)LanguageType.defaultLang;
            }
            switch (lang)
            {
                case (int)LanguageType.en_US: 
                    address=DetailsAddress+","+QuAreaName+","+ShiAreaName+","+ShengAreaName; break;
                case (int)LanguageType.th_TH:
                    address=ShengAreaName+" "+ShiAreaName+" "+QuAreaName+" "+DetailsAddress;break;
                case    (int)LanguageType.zh_CN:
                    address=ShengAreaName+ShiAreaName+QuAreaName+DetailsAddress;break;
                default: break;
            }
            return address;
        }
    }
}