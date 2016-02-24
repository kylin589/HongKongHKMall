
using HKSJ.Common.FastDFS;
using HKTHMall.Core.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HKTHMall.Core.UploadFile
{
    public class UploadFile
    {
        /// <summary>
        /// 上传文件公用类
        /// </summary>
        /// <param name="_file"></param>
        /// <returns></returns>
        public FileUploadResult UploadFileCommon(HttpPostedFileBase _file)
        {
            try
            {
                string _fileInfotype = _file.ContentType; //Path.GetExtension(file.FileName).ToLower();
                int _fileInfosize = (int)_file.InputStream.Length;
                if (_fileInfosize == 0)
                {
                    return new FileUploadResult { result = false, ResultExplain = "上传空文件" };
                }
                Stream stream = _file.InputStream;
                byte[] buffer = new byte[_fileInfosize];
                stream.Read(buffer, 0, _fileInfosize);
                var t = Path.GetExtension(_file.FileName);
                t = t == null ? "jpg" : t.Replace(".", "");
                //获得原图的路径
                string ImageUrl = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, buffer, t);
                //获得服务器地址 
                string url = GetConfig.FullPath() + GetThumbsImage(ImageUrl, 364, 230);
                return new FileUploadResult { url = url, ResultExplain = "成功", name = ImageUrl, result = true };
            }
            catch (Exception)
            {

                return new FileUploadResult { url = "", ResultExplain = "失败", name = "", result = false };
            }
            


        }

        /// <summary>
        /// 生成缩略图地址
        /// 修改人张森 ,如果传入空字符串报错的修改 2014/12/1
        /// </summary>
        /// <param name="ImageUrl">原图地址</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public string GetThumbsImage(string ImageUrl, int width, int height)
        {
            if (string.IsNullOrEmpty(ImageUrl))
            {
                return string.Empty;
            }
            try
            {
                string ResultString = ImageUrl;

                int last = ImageUrl.LastIndexOf("/") + 1;
                int secondlast = ImageUrl.LastIndexOf(".") - last;

                //得到最后一个“/”和“.”之间的字符串
                string strArray = ImageUrl.Substring(last, secondlast);
                ResultString = ImageUrl.Replace(strArray, strArray + "=" + width.ToString() + "x" + height.ToString());

                return ResultString;
            }
            catch (Exception e)
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// 流的复制更新
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            long tempPos = input.Position;
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    break;
                output.Write(buffer, 0, read);
            }
            input.Position = tempPos;
        }
    }
}