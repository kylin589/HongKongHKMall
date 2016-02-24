using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Core.Extensions
{
    /// <summary>
    /// 语言切换时的时间转换变大问题
    /// zhoub 20150813
    /// </summary>
    public static class DateTimeExtensions
    {
        public static string DateTimeToString(this DateTime time, string formatter="yyyy-MM-dd HH:mm:ss", string zhName = "zh-CN")
        {
            return time.ToString(formatter, new System.Globalization.CultureInfo(zhName));
        }

        /// <summary>
        /// 日期转换成unix时间戳
        /// zhoub 20150817
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime.AddHours(-8) - start).TotalSeconds);
        }

        ///// <summary>
        ///// unix时间戳转换成日期
        ///// zhoub 20150817
        ///// </summary>
        ///// <param name="unixTimeStamp">时间戳（秒）</param>
        ///// <returns></returns>
        //public static DateTime UnixTimestampToDateTime(this DateTime target, long timestamp)
        //{
        //    var start = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
        //    return start.AddSeconds(timestamp);
        //}
    }
}
