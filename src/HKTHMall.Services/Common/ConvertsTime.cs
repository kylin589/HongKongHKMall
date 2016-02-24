using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Common
{
    public class ConvertsTime
    {
        
        /// <summary>
        /// DateTime转MySQL 13位长度UTC时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToTimeStamp(DateTime dateTime)
        {
            try
            {
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
                return (long)(dateTime - startTime).TotalMilliseconds;

               //DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);  //得到1970年的时间戳   
               //long timeStamp = (dateTime.ToUniversalTime().Ticks - time.Ticks) / 10000;      
               //return timeStamp;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// UTC Now转MySQL 13位长度UTC时间戳
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long UTCNowToTimeStamp()
        {
            try
            {
                return DateTimeToTimeStamp(DateTime.Now);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// UTC时间戳转本地DateTime
        /// </summary>
        /// <param name="timeStamp">MySQL 13位长度UTC时间戳</param>
        /// <returns></returns>
        public static DateTime TimeStampToDateTime(long timeStamp)
        {
            try
            {
                DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1,0,0,0));
                DateTime result = dateTimeStart.AddMilliseconds(timeStamp);
                return result;

                //DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timeStamp / 1000).ToLocalTime();
                //return result;
            }
            catch (Exception ex)
            {
                return System.DateTime.Now;
            }
        }
    }
}
