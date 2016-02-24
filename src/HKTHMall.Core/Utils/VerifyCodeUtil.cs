using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HKTHMall.Core.Utils
{
    /// <summary>
    /// 图片验证码工具类
    /// </summary>
    public static class VerifyCodeUtil
    {

        /// <summary>
        /// 生成验证码（依赖Couchbase 缓存中间键，和ValidCode 配套使用）
        /// </summary>
        /// <param name="cacheKey">缓存依赖键</param>
        /// <param name="length">验证码长度，默认为4</param>
        public static void GenerateVerifyCode(string cacheKey, int length = 4)
        {
            string code = VerifyCodeUtil.CreateValidateCode(length);
            MemCacheFactory.GetCurrentMemCache().AddCache(cacheKey, code);            
            byte[] bytes = CreateValidateGraphic(code);
            System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(bytes));
            image.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        public static string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        private static byte[] CreateValidateGraphic(string verifyCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(verifyCode.Length * 12.0), 24);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(verifyCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 验证 验证码（和GenerateVerifyCode 配套使用）
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <param name="cacheKey">缓存依赖键</param>
        /// <returns>是否通过验证</returns>
        public static bool ValidCode(string verifyCode, string cacheKey)
        {
            string code = MemCacheFactory.GetCurrentMemCache().GetCache<string>(cacheKey);
            return code == verifyCode;
        }
    }
}
