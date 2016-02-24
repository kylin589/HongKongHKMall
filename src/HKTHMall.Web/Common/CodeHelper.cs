using HKSJ.Common;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Xml;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Drawing.Imaging;

namespace HKTHMall.Web.Common
{
    public static class CodeHelper
    {
        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="CodeLength"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="FontSize"></param>
        /// <returns></returns>
        public static byte[] CreateValidateGraphic(out String Code, int CodeLength, int Width, int Height, int FontSize)
        {
            String sCode = String.Empty;
            //颜色列表
            Color[] oColors = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表
            string[] oFontNames = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
            //验证码的字符集，去掉了一些容易混淆的字元
            char[] oCharacter = { '2', '3', '4', '5', '6', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random oRnd = new Random();
            Bitmap oBmp = null;
            Graphics oGraphics = null;
            int N1 = 0;
            System.Drawing.Point oPoint1 = default(System.Drawing.Point);
            System.Drawing.Point oPoint2 = default(System.Drawing.Point);
            string sFontName = null;
            Font oFont = null;
            Color oColor = default(Color);
            //生成验证码字符串
            for (N1 = 0; N1 <= CodeLength - 1; N1++)
            {
                sCode += oCharacter[oRnd.Next(oCharacter.Length)];
            }

            oBmp = new Bitmap(Width, Height);
            oGraphics = Graphics.FromImage(oBmp);
            oGraphics.Clear(System.Drawing.Color.White);
            try
            {
                for (N1 = 0; N1 <= 4; N1++)
                {
                    //画噪线
                    oPoint1.X = oRnd.Next(Width);
                    oPoint1.Y = oRnd.Next(Height);
                    oPoint2.X = oRnd.Next(Width);
                    oPoint2.Y = oRnd.Next(Height);
                    oColor = oColors[oRnd.Next(oColors.Length)];
                    oGraphics.DrawLine(new Pen(oColor), oPoint1, oPoint2);
                }

                float spaceWith = 0, dotX = 0, dotY = 0;
                if (CodeLength != 0)
                {
                    spaceWith = (Width - FontSize * CodeLength - 10) / CodeLength;
                }

                for (N1 = 0; N1 <= sCode.Length - 1; N1++)
                {
                    //画验证码字串
                    sFontName = oFontNames[oRnd.Next(oFontNames.Length)];
                    oFont = new Font(sFontName, FontSize, FontStyle.Italic);
                    oColor = oColors[oRnd.Next(oColors.Length)];

                    dotY = (Height - oFont.Height) / 2 + 2;//中心下移2像素
                    dotX = Convert.ToSingle(N1) * FontSize + (N1 + 1) * spaceWith;

                    oGraphics.DrawString(sCode[N1].ToString(), oFont, new SolidBrush(oColor), dotX, dotY);
                }

                for (int i = 0; i <= 30; i++)
                {
                    //画噪点
                    int x = oRnd.Next(oBmp.Width);
                    int y = oRnd.Next(oBmp.Height);
                    Color clr = oColors[oRnd.Next(oColors.Length)];
                    oBmp.SetPixel(x, y, clr);
                }

                Code = sCode;
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                oBmp.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                oGraphics.Dispose();
            }
        }

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
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        /// <summary>
        /// 生成随机数
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
        /// 生成验证码字符串
        /// </summary>
        /// <param name="codeLen">验证码字符长度</param>
        /// <returns>返回验证码字符串</returns>
        public static string MakeCode(int codeLen)
        {
            if (codeLen < 1)
            {
                return string.Empty;
            }
            int number;
            string checkCode = string.Empty;
            Random random = new Random();
            for (int index = 0; index < codeLen; index++)
            {
                number = random.Next();
                if (number % 2 == 0)
                {
                    checkCode += (char)('0' + (char)(number % 10));     //生成数字
                }
                else
                {
                    checkCode += (char)('A' + (char)(number % 26));     //生成字母
                }
            }
            return checkCode;
        }


        /// <summary>
        /// 截取字符串到多少位,超出部分以"......"替代
        /// </summary>
        /// <param name="str">截取字符串</param>
        /// <param name="num">字符串保留长度</param>
        /// <returns></returns>
        public static string GetLengthNumString(string str,int num)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > num)
                {
                    return str.Substring(0, num) + "......";
                }
                else
                { 
                    return str; 
                }
            }
            else
            {
                return str; 
            }
        }      
      
    }
}