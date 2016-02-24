using HKTHMall.Core;
using HKTHMall.Services.Common.MultiLangKeys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace HKTHMall.Web.Common
{
    /// <summary>
    /// 邮件模板类型
    /// </summary>
    public enum MailTempType 
    {
        /// <summary>
        /// 设置交易密码邮件模板
        /// </summary>
        SetPaymentPassword,
        /// <summary>
        /// 激活账户邮件模板
        /// </summary>
        ActiveAccount,
        /// <summary>
        /// 找回密码邮件模板
        /// </summary>
        RestPassword,
        MailStyle
    }
    public class MailTempHelper
    {
        public static string GetMailTemp(MailTempType MailType)
        {
            if (MailType != MailTempType.MailStyle)
                return FileReadByCacheDependency(HttpContext.Current.Server.MapPath("~/MailTemp/" + MailType + "_" + CultureHelper.GetCurrentCulture() + ".html"));
            else
                return FileReadByCacheDependency(HttpContext.Current.Server.MapPath("~/MailTemp/" + MailType + ".css"));
        }

        /// <summary>
        /// 使用文件依赖缓存读取文本文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static string FileReadByCacheDependency(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return "";
            }
            Cache cache = System.Web.HttpRuntime.Cache;
            object filestr = cache.Get(filepath);
            if (filestr == null)
            {
                CacheDependency dp = new CacheDependency(filepath);
                string str = ReadFile(filepath);
                cache.Insert(filepath, str, dp);
                return str;
            }
            else
            {
                return filestr.ToString();
            }
        }

        /// <summary>
        /// 使用异步读取文本文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return "";
            }
            return File.ReadAllText(filepath,Encoding.UTF8);
        }
    }
}