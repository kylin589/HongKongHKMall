using System;
using System.Text.RegularExpressions;
using Autofac;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;

namespace HKTHMall.Core
{
    public class RegexUtil
    {
        /// <summary>
        ///     判断输入的字符串是否是一个合法的Email地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            //^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$
            var pattern =
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        ///     判断是否IP地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIPAddress(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str) || str.Length < 7 || str.Length > 15)
                    return false;
                var regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}{1}";
                var regex = new Regex(regformat, RegexOptions.IgnoreCase);
                return regex.IsMatch(str);
                
            }
            catch (Exception ex)
            {
                var logger = BrEngineContext.Current.Resolve<ILogger>();
                logger.Error(typeof(RegexUtil),
                   string.Format("类名:{0}\t 方法:{1}\t 错误信息:{2}", "RegexUtil", "IsIPAddress",
                    ex.Message));
                return false;
            }
        }
    }
}