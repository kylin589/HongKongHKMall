using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Common
{

    public static class CodeHelper
    {
        public static byte[] CreateCodeImg(string checkCode, int h = 86, int w = 40)
        {
            if (string.IsNullOrEmpty(checkCode))
            {
                return null;
            }
            Bitmap image = new Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics graphic = Graphics.FromImage(image);

            try
            {
                Random random = new Random();

                graphic.Clear(Color.White);

                int x1 = 0, y1 = 0, x2 = 0, y2 = 0;

                for (int index = 0; index < 25; index++)
                {
                    x1 = random.Next(image.Width);
                    x2 = random.Next(image.Width);
                    y1 = random.Next(image.Height);
                    y2 = random.Next(image.Height);

                    graphic.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Red, Color.DarkRed, 1.2f, true);
                graphic.DrawString(checkCode, font, brush, 2, 2);

                int x = 0;
                int y = 0;

                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    x = random.Next(image.Width);
                    y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                graphic.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //将图片验证码保存为流Stream返回
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
            finally
            {
                graphic.Dispose();
                image.Dispose();
            }
        }
        /// <summary>
        /// 王灿
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        /// <summary>
        /// 描述:获取指定长度的随机数字
        /// 编写者:刘兰兰
        /// 编写日期:2014-11-26
        /// </summary>
        /// <param name="NumberLength">生成随机数字的个数</param>
        /// <returns></returns>
        public static string GetRandomNumber(int NumberLength)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9";

            string[] allCharArray = allChar.Split(',');
            string RandomNumber = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < NumberLength; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomNumber += allCharArray[t];
            }
            return RandomNumber;
        }

        /// <summary>
        /// 根据AreaID查出完整地址信息
        /// </summary>
        /// <param name="AreaID"></param>
        /// <returns></returns>
        //public static string GetAddressByAreaID(int AreaID)
        //{
        //    string str = "";
        //    List<string> strList = new List<string>();
        //    GetAddressByAreaIDAction(AreaID, strList);
        //    for (int i = 0; i < strList.Count; i++)
        //    {
        //        if (!string.IsNullOrEmpty(str))
        //            str += "";
        //        str += strList[strList.Count - 1 - i];
        //    }
        //    return str;
        //}
        /// <summary>
        /// 根据AreaID查出到城市的地址信息
        /// </summary>
        /// <param name="AreaID"></param>
        /// <returns></returns>
        //public static string GetAddressWithCityByAreaID(int AreaID)
        //{
        //    string str = "";
        //    List<string> strList = new List<string>();
        //    GetShortAddressByAreaIDAction(AreaID, strList);
        //    strList.Reverse();
        //    if (strList.Count > 2)
        //        strList.RemoveRange(2, strList.Count - 2);
        //    foreach (var item in strList)
        //    {
        //        str += item;
        //    }

        //    return str;
        //}

        /// <summary>
        /// 截取字符串到多少位,超出部分以"......"替代
        /// </summary>
        /// <param name="str">截取字符串</param>
        /// <param name="num">字符串保留长度</param>
        /// <returns></returns>
        public static string GetLengthNumString(string str, int num)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > num)
                {
                    return str.Substring(0, num) + "......";
                }
                else
                { return str; }
            }
            else
            { return str; }
        }

        /// <summary>
        /// 不知道谁的代码,代码AreaID 为 0 出错,  我需要改 一下才不出错
        /// 修改人张森
        /// 2014/12/18
        /// </summary>
        /// <param name="AreaID"></param>
        /// <param name="strList"></param>
        //public static void GetAddressByAreaIDAction(int AreaID, List<string> strList)
        //{
        //    BD_Dictionary bd_Dictionary = BD_Dictionary.Find(BD_Dictionary._.ID, AreaID);
        //    //修改的代码
        //    if (bd_Dictionary == null)
        //    {
        //        return;
        //    }
        //    //
        //    strList.Add(bd_Dictionary.AreaName);
        //    if (bd_Dictionary.ParentID != 1)
        //    {
        //        GetAddressByAreaIDAction(bd_Dictionary.ParentID, strList);
        //    }
        //}
        //public static void GetShortAddressByAreaIDAction(int AreaID, List<string> strList)
        //{
        //    BD_Dictionary bd_Dictionary = BD_Dictionary.Find(BD_Dictionary._.ID, AreaID);
        //    //修改的代码
        //    if (bd_Dictionary == null || bd_Dictionary.AreaType==5)
        //    {
        //        return;
        //    }

        //    //
        //    strList.Add(bd_Dictionary.ShortName);
        //    if (bd_Dictionary.ParentID != 1)
        //    {
        //        GetShortAddressByAreaIDAction(bd_Dictionary.ParentID, strList);
        //    }
        //}

    }
}