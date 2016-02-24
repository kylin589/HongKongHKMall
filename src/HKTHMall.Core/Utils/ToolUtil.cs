using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

#region << 版 本 注 释 >>

/*

 * ========================================================================

 * Copyright(c) 2014-2011

 * ========================================================================

 *  

 * 【当前类文件的功能】

 *  

 *  

 * 作者:Eric   时间:2014-05-04 11:44:09

 * 文件名:ToolUtil

 * 版本:V1.0.0

 * 

 * 修改者:           时间:               

 * 修改说明:

 * ========================================================================

*/

#endregion

namespace HKTHMall.Core
{
    public class ToolUtil
    {

        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        /// 替换银行卡号中间信息（前7后4,中间信息以newValue字符代替）
        /// </summary>
        /// <param name="str">银行卡号</param>
        /// <param name="newValue">中间信息符换字符</param>
        /// <returns></returns>
        public static string GetCardInfo(string str, string newValue)
        {
            string temp = string.Empty;
            temp = str.Substring(0, 7);
            Char[] tempChar = str.ToCharArray(7, str.Length - 11);
            for (int i = 0; i < tempChar.Length; i++)
            {
                temp += newValue;
            }
            temp += str.Substring(str.Length - 4);
            return temp;
        }
       
        /// <summary>
        /// 正数四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static double Round(double value, int decimals)
        {
            if (value < 0)
            {
                return Math.Round(value + 5 / Math.Pow(10, decimals + 1), decimals, MidpointRounding.AwayFromZero);
            }
            else
            {
                return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
            }
        }
        /// <summary>
        /// 正数四舍五入
        /// </summary>
        /// <param name="d"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal Round(decimal d, int decimals)
        {
            if (d < 0)
            {
                return Math.Round(d + 5 / (decimal)Math.Pow(10, decimals + 1), decimals, MidpointRounding.AwayFromZero);
            }
            else
            {
                return Math.Round(d, decimals, MidpointRounding.AwayFromZero);
            }
        }
       

        /// <summary>
        ///     得到文件配置路径(配置文件地址,图片存放地址)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string getFilePath(string key)
        {
            return ConfigurationManager.AppSettings[key].Trim();
        }

        /// <summary>
        ///     获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string getUserIp()
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetRealIP()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //可能有代理    
            if (!string.IsNullOrWhiteSpace(result))
            {
                //没有"." 肯定是非IP格式   
                if (result.IndexOf(".") == -1)
                {
                    result = null;
                }
                else
                {
                    //有",",估计多个代理。取第一个不是内网的IP。   
                    if (result.IndexOf(",") != -1)
                    {
                        result = result.Replace(" ", string.Empty).Replace("\"", string.Empty);

                        string[] temparyip = result.Split(",;".ToCharArray());

                        if (temparyip != null && temparyip.Length > 0)
                        {
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                //找到不是内网的地址   
                                if (RegexUtil.IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];
                                }
                            }
                        }
                    }
                    //代理即是IP格式   
                    else if (RegexUtil.IsIPAddress(result))
                    {
                        return result;
                    }
                    //代理中的内容非IP   
                    else
                    {
                        result = null;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }
       
     
        

        /// <summary>
        /// 是否为允许上传的文件(第二个参数 允许上传的文件格式一定要写小写)
        /// </summary>
        /// <param name="picExtension">文件格式</param>
        /// <param name="extStr">允许上传的格式</param>
        /// <returns></returns>
        public static bool boolPicUpload(string picExtension, string extStr)
        {
            bool res = false;
            picExtension = picExtension.ToLower();
            // List<string> extList = new List<string>() { ".jpg", ".jpeg" };
            List<string> extList = new List<string>(extStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            foreach (var Item in extList)
            {
                if (picExtension == Item)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        /// <summary>
        /// 自动识别超链接
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Url_Rewrite(string Text)
        {
            //用?正?则?表?达?式?识?别?URL超?链?接?  
            Regex UrlRegex = new Regex(@"(http:\/\/([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //进?规?则?查?询?  
            //Url  
            MatchCollection matches = UrlRegex.Matches(Text);
            foreach (Match match in matches)
            {
                Text = Text.Replace(match.Value, string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", match.Value, match.Value));
            }
            ////用?正?则?表?达?式?识?别?Email地?址?  
            //Regex EmailRegex = new Regex(@"([a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //matches = EmailRegex.Matches(Text);
            //foreach (Match match in matches)
            //{
            //    Text = Text.Replace(match.Value, string.Format("<a href=mailto:{0}>{1}</a>", match.Value, match.Value));
            //}
            return Text;
        }
        /// <summary>
        /// 把字符串中的URL替换为空
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string UrlReplace(string Text)
        {
            Regex UrlRegex = new Regex(@"(http:\/\/([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //进?规?则?查?询?  
            //Url  
            MatchCollection matches = UrlRegex.Matches(Text);
            foreach (Match match in matches)
            {
                Text = Text.Replace(match.Value, "");
            }

            return Text;
        }
        /// <summary>
        /// 提取HTML代码中文字
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string StripHTML(string strHtml)
        {
            string[] aryReg =
                {
                  @"<script[^>]*?>.*?</script>",
                  @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>", @"([\r\n])[\s]+", 
                  @"&(quot|#34);", @"&(amp|#38);", @"&(lt|#60);", @"&(gt|#62);", 
                  @"&(nbsp|#160);", @"&(iexcl|#161);", @"&(cent|#162);", @"&(pound|#163);",
                  @"&(copy|#169);", @"&#(\d+);", @"-->", @"<!--.*\n"
                };

            string[] aryRep =
            {
              "", "", "", "\"", "&", "<", ">", "   ", "\xa1",  //chr(161),
              "\xa2",  //chr(162),
              "\xa3",  //chr(163),
              "\xa9",  //chr(169),
              "", "\r\n", ""
            };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");
            return strOutput;
        }

        public static string htmlspecialchars(string str)
        {
            str = str.Replace("&", "&amp;");
            //str = str.Replace("<", "&lt;");
            //str = str.Replace(">", "&gt;");
            str = str.Replace("\"", "&quot;");
            return str;
        }

        /// <summary>
        /// 判断客户端设备 
        /// </summary>
        /// <returns></returns>
        public static bool isMobileBrowser() 
            { 
             //GETS THE CURRENT USER CONTEXT 
            HttpContext context = HttpContext.Current; 
             //FIRST TRY BUILT IN ASP.NT CHECK 
            if (context.Request.Browser.IsMobileDevice) 
            { 
                return true; 
            } 
            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER 
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null) 
            { 
               return true; 
             } 
             //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP 
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null && context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap")) 
            { 
                return true; 
             } 
            //AND FINALLY CHECK THE HTTP_USER_AGENT 
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING 
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null) 
            { 
            //Create a list of all mobile types 
             string[] mobiles = new[]  { 
             "midp", "j2me", "avant", "docomo",  "novarra", "palmos", "palmsource",  "240x320", "opwv", "chtml",  "pda", "windows ce", "mmp/","blackberry", "mib/", "symbian",  "wireless", "nokia", "hand", "mobi", "phone", "cdm", "up.b", "audio",  "SIE-", "SEC-", "samsung", "HTC",  "mot-", "mitsu", "sagem", "sony" , "alcatel", "lg", "eric", "vx",  "NEC", "philips", "mmm", "xx",  "panasonic", "sharp", "wap", "sch",  "rover", "pocket", "benq", "java", "pt", "pg", "vox", "amoi", "bird", "compal", "kg", "voda",  "sany", "kdd", "dbt", "sendo",  "sgh", "gradi", "jb", "dddi",  "moto", "iphone" 
             };  //Loop through each item in the list created above 

             //and check if the header contains that text 
            foreach (string s in mobiles) 
            { 
                 if (context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower())) 
                 { 
                     return true; 
                 } 
            } 
           }   
           return false; 
         }

        /// <summary>
        /// 自动生成int型的随机数据 
        /// </summary>
        /// <param name="minvalue"></param>
        /// <param name="maxvalue"></param>
        /// <param name="myran"></param>
        /// <returns></returns>
        public static int GenForInt(int minvalue, int maxvalue, ref int myran)  
        {
            Random ran = new Random();
            int RandKey;
            do
            {
                RandKey = ran.Next(minvalue, maxvalue);
            } while (RandKey == myran);
            Console.WriteLine("RandKey:" + RandKey);
            myran = RandKey; //将本次的随机值赋给myran 
            return RandKey;
        }
        #region 时间转换 create 刘泉
        public static DateTime DefaultTime = new DateTime(1900, 1, 1);
        /// <summary>
        /// 时间转换为秒
        /// </summary>
        /// <param name="objDateTime"></param>
        /// <returns></returns>
        public static long DateTimeToToSecond(object objDateTime)
        {
            DateTime dateTime = StrToDateTime(objDateTime);
            if (dateTime == null || dateTime == DateTime.MinValue || dateTime == DefaultTime) return 0;
            DateTime timeStamp = new DateTime(1970, 1, 1);
            return ((dateTime.AddHours(-8).Ticks - timeStamp.Ticks) / (10000 * 1000));
        }
        /// <summary>
        /// 时间转换为秒
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToToSecond(object dateTime)
        {
            return DateTimeToToSecond(dateTime).ToString();
        }
        /// <summary>
        /// string型转换为DateTime型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime StrToDateTime(object objValue)
        {
            DateTime defValue = DefaultTime;
            if (objValue == null || string.IsNullOrEmpty(objValue.ToString())) return defValue;
            DateTime result = defValue;
            DateTime.TryParse(objValue.ToString(), out result);
            return result;
        }
        #endregion
    }
}