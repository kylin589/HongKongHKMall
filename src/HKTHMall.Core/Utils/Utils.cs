using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace HKTHMall.Core.Utils
{
    using System.Collections.Specialized;
    using System.Runtime.Serialization.Formatters.Binary;

    public class Utils
    {
        /// <summary>
        /// 正式环境状态
        /// </summary>
        public static int FORMAL_PLATFORM_STATUS = 1;
        /// <summary>
        /// 测试环境状态
        /// </summary>
        public static int TEST_PLATFORM_STATUS = 2;
        /// <summary>
        /// 预生产环境状态
        /// </summary>
        public static int EXPECT_PLATFORM_STATUS = 3;
        /// <summary>
        /// 支付渠道:余额支付
        /// </summary>
        public static int PAY_CHANNEL_HK = 0;
        /// <summary>
        /// 支付渠道:银联支付
        /// </summary>
        public static int PAY_CHANNEL_YL = 2;
        /// <summary>
        /// 支付渠道:微信支付
        /// </summary>
        public static int PAY_CHANNEL_WX = 6;
        /// <summary>
        /// 支付渠道:惠信钱包支付
        /// </summary>
        public static int PAY_CHANNEL_QB = 7;

        public static  DateTime DefaultTime = new DateTime(1900,1,1);
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static bool IsIPSect(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }
        /// <summary>
        /// 验证是否只含有汉字
        /// </summary>
        /// <param name="strln">输入的字符</param>
        /// <returns></returns>
        public static bool IsChinese(string strln)
        {
            if (string.IsNullOrEmpty(strln)) return false;
            return Regex.IsMatch(strln, @"^[\u4e00-\u9fa5]+$");
        }

        /// <summary>
        /// 邮政编码 6个数字
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsPostCode(string source)
        {
            if (string.IsNullOrEmpty(source)) return false;
            return Regex.IsMatch(source, @"^\d{6}$", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证手机号码格式
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile)) return false;
            string pattern = @"^(13|14|15|17|18|19)[0-9]\d{8}$";
            Regex regex = new Regex(pattern, RegexOptions.Compiled);
            return Regex.IsMatch(mobile, pattern);
        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;
            email = email.Trim();
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        }
        /// <summary>
        /// 验证固定电话
        /// </summary>
        /// <param name="phone">固定电话号码</param>
        /// <returns></returns>
        public static bool IsPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            string pattern = @"((\d{11})|(400\d{7})|(400-(\d{4}|\d{3})-(\d{3}|\d{4}))|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";
            Regex regex = new Regex(pattern, RegexOptions.Compiled);
            return Regex.IsMatch(phone, pattern);
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
            {
                return IsNumeric(expression.ToString());
            }
            return false;

        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string str = expression;
                int nLen = Int32.MaxValue.ToString().Length;
                if (str.Length > 0 && str.Length <= nLen && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == nLen && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
            {
                return Regex.IsMatch(expression.ToString(), @"^([-]|[0-9])[0-9]*(\.[0-9]+)?([Ee][\+-][0-9]+)?$");
            }
            return false;
        }
        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            if (string.IsNullOrEmpty(strUrl)) return false;
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }
        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFilename(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return false;
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
                return false;

            string extname = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
            {
                return StrToBool(expression, defValue);
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string strValue, bool defValue)
        {
            if (!string.IsNullOrEmpty(strValue))
            {
                strValue = strValue.ToLower();
                if (string.Compare(strValue, "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(strValue, "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为SByte类型
        /// </summary>
        /// <param name="expression">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的SByte类型结果</returns>
        public static sbyte StrToSByte(object expression, sbyte defValue)
        {
            if (expression != null)
            {
                return StrToSByte(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// 检查是否含有非法字符
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <returns></returns>
        public static bool ChkBadChar(string str)
        {
            bool result = false;
            if (string.IsNullOrEmpty(str))
                return result;
            string strBadChar, tempChar;
            string[] arrBadChar;
            strBadChar = "@@,+,',--,%,^,&,?,(,),<,>,[,],{,},/,\\,;,:,\",\"\"";
            arrBadChar = SplitString(strBadChar, ",");
            tempChar = str;
            for (int i = 0; i < arrBadChar.Length; i++)
            {
                if (tempChar.IndexOf(arrBadChar[i]) >= 0)
                    result = true;
            }
            return result;
        }

        /// <summary>
        /// object型转换为string型
        /// </summary>
        /// <param name="objValue">要转换的对象</param>
        /// <returns>转换后的string类型结果</returns>
        public static string ObjectToString(object objValue)
        {
            if (objValue == null || objValue == DBNull.Value) return string.Empty;
            return objValue.ToString();
        }


        /// <summary>
        /// 将对象转换为SByte类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的SByte类型结果</returns>
        public static sbyte StrToSByte(string str, sbyte defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= sbyte.MaxValue.ToString().Length || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;
            sbyte rv;
            if (sbyte.TryParse(str, out rv))
                return rv;
            return Convert.ToSByte(StrToFloat(str, defValue));
        }
        /// <summary>
        /// 将对象转换为Byte类型
        /// </summary>
        /// <param name="expression">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的Byte类型结果</returns>
        public static byte StrToByte(object expression, byte defValue)
        {
            if (expression != null)
            {
                return StrToByte(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Byte类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的Byte类型结果</returns>
        public static byte StrToByte(string str, byte defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length >= sbyte.MaxValue.ToString().Length || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;
            byte rv;
            if (byte.TryParse(str, out rv))
                return rv;
            return Convert.ToByte(StrToFloat(str, defValue));
        }
        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object expression, int defValue)
        {
            if (expression != null)
            {
                return StrToInt(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str, int defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length > Int32.MaxValue.ToString().Length || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;
            int rv;
            if (Int32.TryParse(str, out rv))
                return rv;
            return Convert.ToInt32(StrToFloat(str, defValue));
        }


        /// <summary>
        /// 将对象转换为Int64类型
        /// </summary>
        /// <param name="expression">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的Int64类型结果</returns>
        public static long StrToLong(object expression, long defValue)
        {
            if (expression != null)
            {
                return StrToLong(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int64类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的Int64类型结果</returns>
        public static long StrToLong(string str, long defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length > Int64.MaxValue.ToString().Length || !Regex.IsMatch(str.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;
            long rv;
            if (Int64.TryParse(str, out rv))
                return rv;
            return Convert.ToInt64(StrToFloat(str, defValue));
        }

        /// <summary>
        /// 将对象转换为UInt32类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的uint类型结果</returns>
        public static uint StrToUInt(object obj, uint defValue)
        {
            if (obj == null) return defValue;
            return StrToUInt(obj.ToString(), defValue);
        }
        /// <summary>
        /// 将对象转换为UInt32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的uint类型结果</returns>
        public static uint StrToUInt(string str, uint defValue)
        {
            if (string.IsNullOrEmpty(str) || str.Trim().Length > UInt32.MaxValue.ToString().Length || !Regex.IsMatch(str.Trim(), @"^([0-9])[0-9]*(\.\w*)?$"))
                return defValue;
            uint rv;
            if (UInt32.TryParse(str, out rv))
                return rv;
            return Convert.ToUInt32(StrToFloat(str, defValue));
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
            {
                return defValue;
            }

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                return defValue;
            }

            //bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
            bool isFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.[0-9]+)?([Ee][\+-][0-9]+)?$");
            if (isFloat)
            {
                float.TryParse(strValue, out defValue);
            }
            return defValue;
        }
        /// <summary>
        /// string型转换为double型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的double类型结果</returns>
        public static double StrToDouble(string strValue, double defValue)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                return defValue;
            }

            //bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
            bool isFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.[0-9]+)?([Ee][\+-][0-9]+)?$");
            if (isFloat)
            {
                double.TryParse(strValue, out defValue);
            }
            return defValue;
        }

        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;
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
        /// <summary>
        /// string型转换为DateTime型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime StrToDateTime(object objValue, DateTime defValue)
        {
            if (defValue == null || defValue == DateTime.MinValue) defValue = DefaultTime;
            if (objValue == null || string.IsNullOrEmpty(objValue.ToString())) return defValue;
            DateTime result = defValue;
            DateTime.TryParse(objValue.ToString(), out result);
            return result;
        }
        /// <summary>
        /// string型转换为DateTime型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime StrToDateTime(string strValue, DateTime defValue)
        {
            if (string.IsNullOrEmpty(strValue)) return defValue;
            DateTime result = defValue;
            DateTime.TryParse(strValue, out result);
            return result;
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="objValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(object objValue, decimal defValue)
        {
            if (objValue == null) return defValue;
            return StrToDecimal(objValue.ToString(),defValue);
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string strValue, decimal defValue)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                return defValue;
            }

            //bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
            bool isFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.[0-9]+)?([Ee][\+-][0-9]+)?$");
            if (isFloat)
            {
                decimal.TryParse(strValue, out defValue);
            }
            return defValue;
        }
        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] ReadFile(string fileName)
        {

            FileStream pFileStream = null;

            byte[] pReadByte = new byte[0];

            try
            {

                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                BinaryReader r = new BinaryReader(pFileStream);

                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开

                pReadByte = r.ReadBytes((int)r.BaseStream.Length);

                return pReadByte;

            }
            catch
            {

                return pReadByte;
            }

            finally
            {

                if (pFileStream != null)

                    pFileStream.Close();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pReadByte"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool WriteFile(byte[] pReadByte, string fileName)
        {

            FileStream pFileStream = null;
            try
            {
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);

                pFileStream.Write(pReadByte, 0, pReadByte.Length);
            }
            catch
            {
                return false;
            }

            finally
            {

                if (pFileStream != null)

                    pFileStream.Close();

            }
            return true;

        }



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
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result); //返回长度为44字节的字符串
        }

        /// <summary>
        /// SHA1加密字符串
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>加密后的字符串</returns> 
        public static string SHA1(string source)
        {
            byte[] value = Encoding.UTF8.GetBytes(source);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] result = sha.ComputeHash(value);
            string delimitedHexHash = BitConverter.ToString(result);
            string hexHash = delimitedHexHash.Replace("-", "");
            return hexHash;
        }


        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>创建是否成功</returns>
        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string content)
        {
            if (string.IsNullOrEmpty(content)) return default(T);
            try
            {
                using (StringReader reader = new StringReader(content))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 建立文件夹
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CreateDir(string name)
        {
            return MakeSureDirectoryPathExists(name);
        }

        /// <summary>
        /// 时间转换为微秒
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToToMillisecond(DateTime dateTime)
        {
            if (dateTime == null) return string.Empty;
            DateTime timeStamp = new DateTime(1970, 1, 1);
            return ((dateTime.AddHours(-8).Ticks - timeStamp.Ticks) / (10000)).ToString();
        }


        /// <summary>
        /// 时间转换为微秒
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ConvertToLongMillisecond(object objDateTime)
        { 
            DateTime dateTime = StrToDateTime(objDateTime);
            if (dateTime == null || dateTime == DateTime.MinValue || dateTime == DefaultTime) return 0;
            DateTime timeStamp = new DateTime(1970, 1, 1);
            return ((dateTime.AddHours(-8).Ticks - timeStamp.Ticks) / (10000));
        }


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
        /// 转换时间为unix时间戳
        /// </summary>
        /// <param name="date">需要传递UTC时间,避免时区误差,例:DataTime.UTCNow</param>
        /// <returns></returns>
        public static long ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Convert.ToInt64( Math.Floor(diff.TotalSeconds));
        }
        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0);
            return start.AddSeconds(timestamp).ToLocalTime();
        }
        ///// <summary>
        ///// unix时间戳转换成日期
        ///// </summary>
        ///// <param name="unixTimeStamp">时间戳（秒）</param>
        ///// <returns></returns>
        //public static DateTime UnixTimestampToDateTime(this DateTime target, long timestamp)
        //{
        //    var start = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
        //    return start.AddSeconds(timestamp);
        //}

        ///<summary>
        /// 身份证验证
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns></returns>
        public bool CheckIDCard(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return false;
            if (Id.Length == 18)
            {
                bool check = CheckIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = CheckIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 18位身份证验证
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns></returns>
        private bool CheckIDCard18(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return false;
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }
        /// <summary>
        /// 15位身份证验证
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns></returns>
        private bool CheckIDCard15(string Id)
        {
            if (string.IsNullOrEmpty(Id)) return false;
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            if (string.IsNullOrEmpty(content)) return string.Empty;
            return Regex.Replace(content, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            if (string.IsNullOrEmpty(content)) return string.Empty;
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }


        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }

            }
            return -1;
        }

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray)
        {
            return InArray(str, SplitString(stringarray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit)
        {
            return InArray(str, SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                {
                    string[] tmp = { strContent };
                    return tmp;
                }
                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
            {
                return new string[0] { };
            }
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }


        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="md5Key">md5Key</param>
        /// <returns>MD5结果</returns>
        public static String MD5ByKey(String text, String md5Key)
        {
            if (string.IsNullOrEmpty(md5Key)) md5Key = string.Empty;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.Default.GetBytes(text + md5Key);//将字符编码为一个字节序列 
            byte[] md5Data = md5.ComputeHash(bytes); //计算data字节数组的哈希值 
            md5.Clear();
            StringBuilder sBuilder = new StringBuilder("");
            for (int i = 0; i < md5Data.Length; i++)
            {
                sBuilder.Append(md5Data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static byte[] SHA256Encode(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            return Sha256.ComputeHash(SHA256Data);
        }

        /// <summary>
        /// to HexString
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String Bytes2Hex(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder("");
            String tmp = null;
            for (int i = 0; i < bytes.Length; i++)
            {
                tmp = (bytes[i] & 0xFF).ToString("x2");
                if (tmp.Length == 1)
                {
                    builder.Append("0");
                }
                builder.Append(tmp);
            }
            return builder.ToString();
        }
        public static byte[] Hex2Byte(byte[] b)
        {
            if ((b.Length % 2) != 0)
            {
                throw new ArgumentException("长度不是偶数");
            }
            byte[] b2 = new byte[b.Length / 2];
            for (int n = 0; n < b.Length; n += 2)
            {
                String item = Encoding.UTF8.GetString(b, n, 2);
                b2[n / 2] = (byte)Convert.ToInt32(item, 16);
            }
            b = null;
            return b2;
        }

        /**
         * 将int 转换为 byte 数组
         *
         * @param i
         * @return
         */
        public static byte[] IntToByteArray(int i)
        {
            byte[] result = new byte[4];
            result[0] = (byte)((i >> 24) & 0xFF);
            result[1] = (byte)((i >> 16) & 0xFF);
            result[2] = (byte)((i >> 8) & 0xFF);
            result[3] = (byte)(i & 0xFF);
            return result;
        }

        /**
         * 将byte数组 转换为int
         *
         * @param b
         * @param offset 位游方式
         * @return
         */
        public static int ByteArrayToInt(byte[] b, int offset)
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                int shift = (4 - 1 - i) * 8;
                value += (b[i + offset] & 0x000000FF) << shift;//往高位游
            }
            return value;
        }
        /// <summary>
        /// json字符串反序列为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T DeserializeDataContract<T>(string strJson)
        {
            if (string.IsNullOrEmpty(strJson)) return default(T);

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        /// <summary>
        /// 对象序列为json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializerDataContract<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                json.WriteObject(memoryStream, obj);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }

        }
        /// <summary>
        /// 获取当前应用网址
        /// </summary>
        /// <returns></returns>
        public static String GetApplicationUrl()
        {
            String url = HttpContext.Current.Request.Url.IsDefaultPort
                ? HttpContext.Current.Request.Url.Host
                : string.Format("{0}:{1}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port.ToString());
            if (HttpContext.Current.Request.ApplicationPath != "/")
                return "http://" + url + HttpContext.Current.Request.ApplicationPath;
            else return "http://" + url;
        }

        /// <summary>
        /// 替换sql语句中的有问题符号
        /// </summary>
        public static string ChkSQL(string str)
        {
            return (str == null) ? "" : str.Replace("'", "''");
        }

        /// <summary>
        /// 描述:生成昵称.刘兰兰.2014-12-16
        /// </summary>
        /// <returns></returns>
        public static string SetNickName()
        {
            int number;
            char code;
            string checkCode = String.Empty;
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            return "PB" + checkCode;
        }

        /// <summary>
        /// 计算文本长度,区分中英文字符,中文算两个长度,英文算一个长度

        /// <seealso cref="Common_Function.Text_Length"/>
        /// </summary>
        /// <param name="Text">需计算长度的字符串</param>
        /// <returns>int</returns>
        public static int TextLength(string Text)
        {
            Encoding encoding = Encoding.GetEncoding("GB2312");
            return encoding.GetBytes(Text).Length;
            
        }

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    strPath = strPath.Replace("\\", "/");
                    return HttpContext.Current.Server.MapPath(strPath);
                }
                else //非web程序引用
                {
                    strPath = strPath.Replace("/", "\\");
                    if (strPath.StartsWith("\\"))
                    {
                        strPath = strPath.TrimStart('\\');
                    }
                    return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }


        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetRealIP(string strIP="")
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(strIP)) strIP = "127.0.0.1";
            if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
            {
                return strIP;
            }
            return result;

        }

        /// <summary>
        /// 支付测试环境
        /// </summary>
        /// <returns></returns>
        public static bool IsPayTest()
        {
            string strPayTest = ConfigurationManager.AppSettings["PayTest"];
            if (string.IsNullOrEmpty(strPayTest)) return false;
            return strPayTest.ToUpper() == Utils.MD5("PayTest").ToUpper();
        }



        /// <summary>
        /// 实现深复制
        /// </summary>
        /// <param name="obj"></param>
        public static T DeepCopy<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        public static string  GetRefundString(string intoString)
        {
            if (intoString.Contains("RefundHKP"))
            {
                intoString = intoString.Replace("Refund", "");
                return intoString;
            }
            return "";
        }


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
        public static string GetIP()
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
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

        #endregion

        /// <summary>
        ///  
        /// </summary>
        /// <param name="dicParam"></param>
        /// <returns></returns>
        public static string JoinRequestString(Dictionary<string, string> dicParam)
        {
            if (dicParam == null || dicParam.Count == 0) return string.Empty;
            StringBuilder paramBuilder = new StringBuilder();
            Encoding encoding = Encoding.UTF8;
            foreach (KeyValuePair<string, string> item in dicParam)//循环数组
            {
                if (!string.IsNullOrEmpty(item.Value))//值不为空的参数进入加签队列
                {
                    paramBuilder.Append(item.Key + "=" + item.Value + "&");
                }
            }
            return paramBuilder.ToString().Trim('&');
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息,并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static Dictionary<string, string> GetRequestParam()
        {
            int i = 0;
            Dictionary<string, string> dicParam = new Dictionary<string, string>();
            NameValueCollection requestParam = null;
            requestParam = System.Web.HttpContext.Current.Request.Form;
            // Get names of all forms into a string array.
            String[] requestItems = requestParam.AllKeys;
            if (requestItems.Length == 0)
            {
                requestParam = System.Web.HttpContext.Current.Request.QueryString;  //Load Form variables into NameValueCollection variable.
                requestItems = requestParam.AllKeys;
                for (i = 0; i < requestItems.Length; i++)
                {
                    dicParam.Add(requestItems[i], System.Web.HttpContext.Current.Request.QueryString[requestItems[i]]);
                }
            }
            else
            {
                for (i = 0; i < requestItems.Length; i++)
                {
                    dicParam.Add(requestItems[i], System.Web.HttpContext.Current.Request.Form[requestItems[i]]);
                }
            }
            return dicParam;
        }

    }
}
