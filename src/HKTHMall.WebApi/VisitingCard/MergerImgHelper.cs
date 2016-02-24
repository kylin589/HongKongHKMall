using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;
using System.IO;

namespace HKTHMall.WebApi.VisitingCard
{
    public class MergerImgHelper
    {
        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="file1">模版图片</param>
        /// <param name="file2"></param>
        /// <param name="file3">生成二维码网址</param>
        /// <returns></returns>
        //public static Bitmap CreateCard(string file1, string file2, string urlStr, int x, int y)
        public static Bitmap CreateCard(string file1, string file2, string urlStr)
        {
            ///模版
            Bitmap maptemplet = (Bitmap)Bitmap.FromFile(file1);
            ///头像
            Bitmap maptitle = (Bitmap)Bitmap.FromFile(file2);
            ///二维码
            Bitmap maperwei = Getcode(urlStr);//(Bitmap)Bitmap.FromFile(file3);
            //求解最大的宽度
            int maxWidth = maptemplet.Width;
            int maxheight = maptemplet.Height;
            //指定要生成的图片的长宽
            Bitmap backgroudImg = new Bitmap(maxWidth, maxheight);
            Graphics g = Graphics.FromImage(backgroudImg);
            //清除画布,背景设置为白色
            g.Clear(System.Drawing.Color.White);
            g.DrawImage(maptemplet, 0, 0, maxWidth, maxheight); 
            g.DrawImage(maptitle, 20, 20, 90, 90); 
            g.DrawImage(maperwei, 82, 174, 125, 125);
            g.Dispose();
            return backgroudImg;
        }

        public static Bitmap CreateCard(string file1, Stream file2, string urlStr, string phone, string realName)
        {
            ///模版
            Bitmap _maptemplet = (Bitmap)Bitmap.FromFile(file1);
            Bitmap maptemplet = new Bitmap(_maptemplet);
            _maptemplet.Dispose();
            ///头像
            Bitmap _maptitle = (Bitmap)Bitmap.FromStream(file2);
            Bitmap maptitle = new Bitmap(_maptitle);
            _maptitle.Dispose();
            ///二维码
            Bitmap maperwei = Getcode(urlStr);//(Bitmap)Bitmap.FromFile(file3);
            phone = null == phone ? "null" : phone;
            realName = null == realName ? "null" : realName;
            //求解最大的宽度
            int maxWidth = maptemplet.Width;
            int maxheight = maptemplet.Height;
            //指定要生成的图片的长宽
            Bitmap backgroudImg = new Bitmap(maxWidth, maxheight);
            Font font = new Font("Arial", 20);
            SolidBrush brush = new SolidBrush(Color.Black);
            Graphics g = Graphics.FromImage(backgroudImg);
            //清除画布,背景设置为白色
            g.Clear(System.Drawing.Color.White);
            g.DrawImage(maptemplet, 0, 0, maxWidth, maxheight);
            g.DrawImage(maptitle, 43, 46, 150, 150);
            g.DrawImage(maperwei, 155, 372, 417, 418);
            g.DrawString(realName, font, brush, 267, 74);
            g.DrawString(phone, font, brush, 293, 135);
            g.Dispose();
            return backgroudImg;
        }

        /// <summary> 
        /// 获取二维码生成图片         
        /// </summary> 
        /// <returns></returns> 
        public static Bitmap Getcode(string writeStr)
        {
            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//设置二维码编码格式 
            qRCodeEncoder.QRCodeScale = 4;//设置编码测量度  
            qRCodeEncoder.QRCodeVersion = 0;//设置编码版本   
            qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//设置错误校验 
            Bitmap image = qRCodeEncoder.Encode(writeStr);
            return image;
        }




    }
}
