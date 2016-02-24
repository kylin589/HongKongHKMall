using System;
using System.Collections.Generic;
using System.Compat.Web;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Core.Translation
{
    /// <summary>
    /// 翻译处理类
    /// <remarks>added by jimmy,2015-7-1</remarks>
    /// </summary>
    public class TranData
    {
        #region 百度翻译
        /// <summary>
        /// 百度翻译
        /// </summary>
        /// <param name="strToTranslate">被翻译的对象</param>
        /// <param name="fromLanguage">从指定的语言</param>
        /// <param name="toLanguage">翻译到指定的语言</param>
        /// <returns>返回翻译的对象信息</returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public string BaiduTranslate(string strToTranslate, string fromLanguage, string toLanguage)
        {
            string translatedStr = "";
            string transRetHtml = "";
            string encodedStr = strToTranslate;
             //encodedStr = System.Web.HttpUtility.UrlEncode(strToTranslate);

            string googleTransBaseUrl = "http://openapi.baidu.com/public/2.0/bmt/translate?";
            StringBuilder sb = new StringBuilder();
            sb.Append(googleTransBaseUrl);
            sb.AppendFormat("client_id={0}", "bcBmF1cSGuTv9D99C4k0j0vd");
            sb.AppendFormat("&q={0}", encodedStr);
            sb.AppendFormat("&from={0}", fromLanguage);
            sb.AppendFormat("&to={0}", toLanguage);
            try
            {
                var webClient = new WebClient();
                var bytes = webClient.DownloadData(sb.ToString());
                transRetHtml = Encoding.UTF8.GetString(bytes);
                translatedStr = transRetHtml;
            }
            catch (Exception ex)
            {
                //todo 记录错误日志
            }
            return translatedStr;
        }
        #endregion
    }
}
