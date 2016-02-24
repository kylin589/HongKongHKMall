using HKSJ.Common.FastDFS;
using HKTHMall.Core.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace HKTHMall.Web.Common
{
    public class VisitingCardCreate
    {
        //生成分享二维码
        //public bool GeneratedVisitingCard(ParaModel model, int lang)
        //{
        //    ParaModel para = new ParaModel();
        //    para.UserID = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.UserID));
        //    try
        //    {
        //          string VirtualPath = "~/Content/images/";
        //          var domain = string.IsNullOrEmpty(ConfigurationManager.AppSettings["domain"]) ? "http://api.0066mall.com" : ConfigurationManager.AppSettings["domain"].ToString();
        //          string ShareLink = ConfigurationManager.AppSettings["ShareLink"] + para.UserID;// +"?lang=" + lang;

        //          string ImgUrl = model.HeadImageUrl;
        //          string Phone = "";
        //          //如果本系统头像为空,从IM获取用户头像
        //          if (string.IsNullOrEmpty(model.HeadImageUrl))
        //          {
        //              ImgUrl = domain + "/VisitingCard/default.png";
        //          }
        //          else
        //          {
        //              //如果头像存在文件服务器上
        //              if (!ImgUrl.StartsWith("http://") && !ImgUrl.StartsWith("https://"))
        //              {
        //                  ImgUrl = ConfigurationManager.AppSettings["ImageHeader"] + ImgUrl;
        //              }
        //          }
        //          if (string.IsNullOrEmpty(ImgUrl) || !IsImgFilename(ImgUrl))
        //          {                     
        //              return false;
        //          }
        //          if (!UrlIsExist(ImgUrl))
        //          {
        //              return false;
        //          }
        //          Phone = HidePhone(model.Phone);
        //          //if (!string.IsNullOrEmpty(model.Phone) && model.Phone.Length > 7)
        //          //{
        //          //    Phone = model.Phone.Substring(0, 3) + "****" + model.Phone.Substring(7, model.Phone.Length - 7);
        //          //}
        //          //else
        //          //{
        //          //    Phone = model.Phone;
        //          //}
        //          HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(ImgUrl);
        //          Stream stream = httpWebRequest.GetResponse().GetResponseStream();
        //          try
        //          {
        //              string folderPath = HttpContext.Current.Server.MapPath(VirtualPath);
        //              if (!Directory.Exists(folderPath))
        //              {
        //                  Directory.CreateDirectory(folderPath);
        //              }
        //              Bitmap pic = CreateCard(folderPath + "moban.jpg", stream, ShareLink, Phone, model.NickName);

        //              MemoryStream ms = new MemoryStream();
        //              pic.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //              string fileName = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, ms.ToArray(), "jpg");

        //              HKTHMall.Services.YHUser.YH_UserService YH_UserService = new HKTHMall.Services.YHUser.YH_UserService(null,null);
        //             // YH_UserService.UpdateUserOrcodeUrl(Convert.ToInt64(model.UserID), 1, fileName);//地址保存到数据库
        //          }
        //          catch (Exception ex)
        //          {
        //              Logger.Write(ex.Message);
        //              return false;
        //          }
        //          return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Write(ex.Message);
        //        return false;
        //    }
        //}
        private string HidePhone(string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 4)
            {
                int endIndex = str.Length - 2;
                string hidePart = "";
                for (int i = 0; i < str.Length - 4; i++)
                {
                    hidePart += "*";
                }
                str = str.Substring(0, 2) + hidePart + str.Substring(endIndex, str.Length - endIndex);
            }
            return str;
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
        /// 检测url文件是否存在
        /// </summary>
        /// <param name="url">检测url文件是否存在</param>
        /// <returns></returns>
        private static bool UrlIsExist(string url)
        {
            System.Uri u = null;
            try
            {
                u = new Uri(url);
            }
            catch { return false; }
            bool isExist = false;
            System.Net.HttpWebRequest r = System.Net.HttpWebRequest.Create(u)
                                    as System.Net.HttpWebRequest;
            r.Method = "HEAD";
            try
            {
                System.Net.HttpWebResponse s = r.GetResponse() as System.Net.HttpWebResponse;
                if (s.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    isExist = true;
                }
            }
            catch (System.Net.WebException x)
            {
            }
            return isExist;
        }


        //合并二维码
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



    public class ParaModel
    {
        /// <summary>
        /// 用户ID 
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string HeadImageUrl { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

    }
}