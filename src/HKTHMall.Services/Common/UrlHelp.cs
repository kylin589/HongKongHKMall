using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Common
{
    public class UrlHelp
    {
        /// <summary>
        /// 检测url文件是否存在
        /// </summary>
        /// <param name="url">检测url文件是否存在</param>
        /// <returns></returns>
        public static bool UrlIsExist(string url)
        {
            System.Uri u = null;
            try
            {
                u = new Uri(url);
            }
            catch { return false; }
            bool isExist = false;
            System.Net.HttpWebRequest r = System.Net.HttpWebRequest.Create(u)
                                    as System.Net.HttpWebRequest;
            r.Method = "HEAD";
            try
            {
                System.Net.HttpWebResponse s = r.GetResponse() as System.Net.HttpWebResponse;
                if (s.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    isExist = true;
                }
            }
            catch (System.Net.WebException x)
            {
            }
            return isExist;
        }
    }
}
