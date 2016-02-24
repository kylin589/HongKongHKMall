
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HKTHMall.WebApi.Common
{
    /// <summary>
    /// 常用工具类
    /// author:jasonhe
    /// date:20130325
    /// </summary>
    public class Util
    {

        #region 实现随机验证码
        /// <summary>
        /// 实现随机验证码
        /// </summary>
        /// <param name="n">显示验证码的个数</param>
        /// <returns>返回生成的随机数</returns>
        public string RandomNum(int n) //
        {
            //定义一个包括数字、大写英文字母和小写英文字母的字符串
            string strchar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            //将strchar字符串转化为数组
            //String.Split 方法返回包含此实例中的子字符串（由指定Char数组的元素分隔）的 String 数组。
            string[] VcArray = strchar.Split(',');
            string VNum = "";
            //记录上次随机数值,尽量避免产生几个一样的随机数           
            int temp = -1;
            //采用一个简单的算法以保证生成随机数的不同
            Random rand = new Random();
            for (int i = 1; i < n + 1; i++)
            {
                if (temp != -1)
                {
                    //unchecked 关键字用于取消整型算术运算和转换的溢出检查。
                    //DateTime.Ticks 属性获取表示此实例的日期和时间的刻度数。
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                //Random.Next 方法返回一个小于所指定最大值的非负随机数。
                int t = rand.Next(61);
                if (temp != -1 && temp == t)
                {
                    return RandomNum(n);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;//返回生成的随机数
        }
        #endregion

        #region 用来截取小数点后nleng位
        /// <summary>
        /// 用来截取小数点后nleng位
        /// </summary>
        /// <param name="sString">sString原字符串。</param>
        /// <param name="nLeng">nLeng长度。</param>
        /// <returns>处理后的字符串。</returns>
        public string VarStr(string sString, int nLeng)
        {
            int index = sString.IndexOf(".");
            if (index == -1 || index + nLeng >= sString.Length)
                return sString;
            else
                return sString.Substring(0, (index + nLeng + 1));
        }
        #endregion

        #region  获取用户IP地址
        /// <summary>
        /// 客服留言获取用户IP
        /// </summary>
        /// <returns></returns>
        public static string KeFu_GetIP()
        {
            string userIP;

            HttpRequest Request = HttpContext.Current.Request;

            // 如果使用代理,获取真实IP
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
            {
                try
                {
                    userIP = Request.ServerVariables["REMOTE_ADDR"];
                }
                catch
                {
                    return "unknown";
                }
            }
            else
            {
                try
                {
                    userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                catch
                {
                    return "unknown";
                }
            }
            if (userIP == null || userIP == "")
            {
                try
                {
                    userIP = Request.UserHostAddress;
                }
                catch
                {
                    return "unknown";
                }
            }

            return userIP;
        }


        /// <summary>
        ///获取ip地址
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result != null && result != String.Empty)
            {
                //可能有代理 
                if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式 
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”,估计多个代理。取第一个不是内网的IP。 
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIPAddress(temparyip[i])
                                && temparyip[i].Substring(0, 3) != "10."
                                && temparyip[i].Substring(0, 7) != "192.168"
                                && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];    //找到不是内网的地址 
                            }
                        }
                    }
                    else if (IsIPAddress(result)) //代理即是IP格式 
                        return result;
                    else
                        result = null;    //代理中的内容 非IP,取IP 
                }
            }

            string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (result == null || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            return result;

        }
        //校验ip地址
        public bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

        #endregion

        #region 转换成json格式


        public static string ToJson(DataTable dt)
        {

            if (dt == null || dt.Rows.Count == 0)
            {
                return "[]";
            }
            else
            {
                StringBuilder jsonBuilder = new StringBuilder();
                jsonBuilder.Append("[");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("\"");
                        jsonBuilder.Append(dt.Columns[j].ColumnName);
                        jsonBuilder.Append("\":\"");
                        jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("'", "%27").Replace("\n", "%0A").Replace("\r", "%0A"));
                        jsonBuilder.Append("\",");
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("]");
                return jsonBuilder.ToString();
            }
        }


        /// <summary>      
        /// DataSet转换成Json格式      
        /// </summary>      
        /// <param name="ds">DataSet</param>      
        /// <returns></returns>      
        public static string DataSetToJson(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return "[]";
            }
            else
            {
                StringBuilder json = new StringBuilder();
                foreach (DataTable dt in ds.Tables)
                {
                    json.Append("{\"");
                    json.Append(dt.TableName);
                    json.Append("\":");
                    json.Append(ToJson(dt));
                    json.Append("}");
                }
                return json.ToString();
            }
        }

        #endregion

        #region 快递查询接口
        /// <summary>
        /// 快递100查询接口
        /// </summary>
        /// <param name="code">快递code</param>
        /// <param name="nu">快递单号</param>
        /// <returns></returns>
        public static string Express100API(string code, string nu)
        {

            WebClient wClient = new WebClient();
            wClient.Encoding = Encoding.UTF8;
            var response = wClient.DownloadString("http://www.kuaidi100.com/query?type=" + code + "&postid=" + nu);
            return response;
        }
        #endregion

        /// <summary>
        /// 字段串是否为Null或为""(空)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StrIsNullOrEmpty(string str)
        {
            if (str == null || str.Trim() == string.Empty)
                return true;

            return false;
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }
        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

    }
}