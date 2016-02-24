using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HKTHMall.Core
{
    /// <summary>字符串处理.</summary>
    /// <remarks></remarks>
    /// <author>Yun 2015-04-19 11:54:02</author>
    public class StringUtility
    {
        /// <summary>反序列化JSON</summary>
        /// <remarks>无throw异常方法</remarks>
        /// <Author>2014/8/16 9:06 PANYUN-PC Yun</Author>
        /// <typeparam name="T"></typeparam>
        /// <param name="strSrc">The STR SRC.</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(String strSrc) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(strSrc);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>JSON序列化</summary>
        /// <remarks></remarks>
        /// <author>Yun 2015-01-29 21:22:01</author>
        /// <param name="obj">The object.</param>
        /// <returns>String.</returns>
        public static String SerializeObject(Object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return String.Empty;
            }
        }


        /// <summary>日期转换为 yyyy-MM-dd HH:mm:ss 格式</summary>
        /// <remarks></remarks>
        /// <author>PanYun HX1501345 2015-04-22 11:13:05</author>
        /// <param name="dt">The dt.</param>
        /// <returns>String.</returns>
        public static String formatDate(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
