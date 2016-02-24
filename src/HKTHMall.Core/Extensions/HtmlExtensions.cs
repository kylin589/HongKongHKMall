using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using HKTHMall.Core.Config;
using HKTHMall.Core.Data;


namespace HKTHMall.Core.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// 获取原始图片 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ImageUrl"></param>
        /// <returns></returns>
        public static string GetImagesUrl(this HtmlHelper helper, string ImageUrl)
        {
            return GetConfig.FullPath() + ImageUrl;
        }

        /// <summary>
        /// 获取原始图片 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ImageUrl"></param>
        /// <returns></returns>
        public static string GetImagesUrl(string ImageUrl)
        {
            return GetConfig.FullPath() + ImageUrl;
        }



        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ImageUrl">地址</param>
        /// <param name="ImageSize">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        public static string GetImagesUrl(this HtmlHelper helper, string ImageUrl, int ImageSize, int height = 0)
        {
            string str = "";
            if (height == 0)
            {
                str = GetThumbsImage(ImageUrl, ImageSize);
            }
            else
            {
                str = GetThumbsImage(ImageUrl, ImageSize, height);
            }
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            return GetConfig.FullPath() + GetThumbsImage(ImageUrl, ImageSize, height);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ImageUrl">地址</param>
        /// <param name="ImageSize">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        public static string GetImagesUrl(string ImageUrl, int ImageSize, int height = 0)
        {
            string str = "";
            if (height == 0)
            {
                str = GetThumbsImage(ImageUrl, ImageSize);
            }
            else
            {
                str = GetThumbsImage(ImageUrl, ImageSize, height);
            }
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            return GetConfig.FullPath() + GetThumbsImage(ImageUrl, ImageSize, height);
        }



        /// <summary>
        /// 生成缩略图地址     
        /// </summary>
        /// <param name="ImageUrl"></param>
        /// <param name="ImageSize"></param>
        /// <returns></returns>
        public static string GetThumbsImage(string ImageUrl, int ImageSize)
        {
            return GetThumbsImage(ImageUrl, ImageSize, ImageSize);
        }

        /// <summary>
        /// 生成缩略图地址
        /// 修改人张森 ,如果传入空字符串报错的修改 2014/12/1
        /// </summary>
        /// <param name="ImageUrl">原图地址</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public static string GetThumbsImage(string ImageUrl, int width, int height)
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


    }
}
