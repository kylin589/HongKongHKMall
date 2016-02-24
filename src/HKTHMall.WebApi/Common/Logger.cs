using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using HKSJ.Common.FastDFS;

namespace HKTHMall.WebApi.Common
{
    /// <summary>
    /// 日志写入代码
    /// </summary>
    public static class Logger
    {
        private static string baseDirName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message"></param>
        public static void Write(string category, string message)
        {
            string dirName = Path.Combine(baseDirName, category);
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);

            string fileName = Path.Combine(dirName, DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine("-------------header----------------   time:" + DateTime.Now.ToString());
                sw.WriteLine(message);
                sw.WriteLine("-------------footer----------------   time:" + DateTime.Now.ToString());
                sw.Flush();
                sw.Close();
            }
        }

        //public static void Write(string format,params object[] args)
        //{
        //    string message = string.Format(format,args);
        //    Logger.Write(message);
        //}

        public static void Write(string message)
        {
            Logger.Write("page_error", message);
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            Logger.Error("error", ex);
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="category"></param>
        /// <param name="ex"></param>
        public static void Error(string category, Exception ex)
        {
            Util util = new Util();
            string err =
                    "url:" + HttpContext.Current.Request.Url + "\r\n" +
                    "IP:" + util.GetIP() + " \r\n" +
                    "UrlReferrer:" + HttpContext.Current.Request.UrlReferrer.AbsoluteUri + " \r\n" +
                    "date time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " \r\n" +
                    "error Source: " + ex.Source + "\r\n" +
                    "error Message: " + ex.Message + "\r\n" +
                  "stack Trace: " + ex.StackTrace + "\r\n";

            Logger.Write(category, err);
        }



        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            Logger.Write("debug", message);
        }
    }
}