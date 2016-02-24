using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HKTHMall.Web.Common
{
    public static class EncryptUtils
    {
        public static string Md5(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            var md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        /// <summary>
        /// 原始字符串生成加密后的密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetUserPassword(string password)
        {
            string temp = password;
            for (int i = 0; i < 3; i++)
            {
                temp = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(temp, "MD5");
            }
            return temp;
        }

        /// <summary> 
        /// DES 加密解密
        /// </summary> 
        public class DES
        {
            //默认密钥向量
            private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            private const string defaultKeys = "0x120x340x560x780x900xAB0xCD0xEF";

            /// <summary>
            /// DES加密字符串
            /// </summary>
            /// <param name="encryptString">待加密的字符串</param>
            /// <returns></returns>
            public static string Encrypt(string encryptString)
            {
                return Encrypt(encryptString, defaultKeys);
            }

            /// <summary>
            /// DES解密字符串
            /// </summary>
            /// <param name="decryptString">待解密的字符串</param>
            /// <returns></returns>
            public static string Decrypt(string decryptString)
            {
                return Decrypt(decryptString, defaultKeys);
            }

            /// <summary>
            /// DES加密字符串
            /// </summary>
            /// <param name="encryptString">待加密的字符串</param>
            /// <param name="encryptKey">加密密钥,要求为8位</param>
            /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
            public static string Encrypt(string encryptString, string encryptKey)
            {
                encryptKey = Util.GetSubString(encryptKey, 8, "");
                encryptKey = encryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());

            }

            /// <summary>
            /// DES解密字符串
            /// </summary>
            /// <param name="decryptString">待解密的字符串</param>
            /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
            /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
            public static string Decrypt(string decryptString, string decryptKey)
            {
                try
                {
                    decryptKey = Util.GetSubString(decryptKey, 8, "");
                    decryptKey = decryptKey.PadRight(8, ' ');
                    byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                    byte[] rgbIV = Keys;
                    byte[] inputByteArray = Convert.FromBase64String(decryptString);
                    DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                    MemoryStream mStream = new MemoryStream();
                    CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                    cStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cStream.FlushFinalBlock();
                    return Encoding.UTF8.GetString(mStream.ToArray());
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}