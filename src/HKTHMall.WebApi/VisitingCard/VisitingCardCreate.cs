using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.IO;
using System.Drawing;
using System.Text;
using System.Web.Mvc;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using HKTHMall.Core;
using System.Text.RegularExpressions;
using HKTHMall.WebApi.Models;
using HKTHMall.Core.Config;
using HKTHMall.WebApi.VisitingCard;
using HKTHMall.Core.UploadFile;
using System.Security.Cryptography;

using HKSJ.MidMessage.Protocol;
using HKSJ.MidMessage.Services;
using HKSJ.Common;
using HKSJ.Common.FastDFS;
using HKTHMall.Core.Security;
using HKTHMall.WebApi.Models.Result;
using HKTHMall.Services.WebLogin.Impl;
using HKTHMall.Services.YHUser;
using HKTHMall.Domain.WebModel.Models.YH;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.Common.MultiLangKeys;



namespace HKTHMall.WebApi.VisitingCard
{
    public class VisitingCardCreate
    {
        private static string domain = string.IsNullOrEmpty(ConfigurationManager.AppSettings["domain"]) ? "http://api.0066mall.com" : ConfigurationManager.AppSettings["domain"].ToString();
        private static string defaultImg = domain + "/VisitingCard/default.png";
        private YH_UserVisitingCardService _VisitingCardService = new YH_UserVisitingCardService();
        #region 生成分享二维码
        public ApiResultModel GeneratedVisitingCard(long userId, int lang)
        {
            ApiResultModel result = new ApiResultModel();
            result.flag = 0;
            result.msg = "";
            result.rs = new PicExt();
            string tmp = "";
            bool IsExist = false;//YH_UserVisitingCard是否存在
            bool Recreate = false;//是否需要重新创建
            try
            {
                var model = _VisitingCardService.findByUserid(userId).Data;

                YH_UserModel user = (new LoginService()).GetUserInfoById(userId).Data;
                //检测账号中心数据是否有改变过
                ResponseUserInfoData emData = EmMethodManage.EmFindUpdateInstance.MsgQueryUserInfoReq(1, 1, user.Account);
                IsExist = model != null;
                if (null == user)//用户不存在
                {
                    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_USERNOTEXIST", lang);
                    return result;
                }
                user.Phone = string.IsNullOrEmpty(emData.data[0].phone) ? "" : emData.data[0].phone;
                user.HeadImageUrl = string.IsNullOrEmpty(emData.data[0].img_url) ? "" : emData.data[0].img_url;
                user.NickName = string.IsNullOrEmpty(emData.data[0].nick) ? "" : emData.data[0].nick;
                if (!IsExist || model.IsSuccess == 0 ||
                    !user.HeadImageUrl.Equals(model.HeadImgUrl) || !user.Phone.Equals(model.Phone)
                    || !user.NickName.Equals(model.NickName) || string.IsNullOrEmpty(model.CurrentCardUrl))
                {
                    Recreate = true;
                }
                if (IsExist)
                {
                    tmp = user.OrcodeUrl;
                }
                if (!Recreate)//如果不需要重新创建
                {
                    result.rs.imgUrl = ConfigurationManager.AppSettings["ImagePath"] + model.CurrentCardUrl;
                    result.flag = 1;
                    result.msg = CultureHelper.GetAPPLangSgring("USERINFO_OP_SUCCESS",lang);
                    return result;//刘文宁修改 2015/9/2
                }
                //重新创建
                ParaModel para=new ParaModel();
                para.UserID=userId.ToString();
                para.HeadImageUrl=user.HeadImageUrl;
                para.Phone=user.Phone;
                para.NickName=user.NickName;
                var resp = GeneratedVisitingCard(para, null, lang);
                if (resp != null && resp.flag == 1) //刘文宁修改  2015/9/2
                {
                    result.flag = resp.flag;
                    result.msg = resp.msg;
                    result.rs.imgUrl = resp.rs;
                    if (result.flag == 1)//重新创建成功,写入数据库
                    {
                        if (!IsExist)
                        {
                            model = new YH_UserVisitingCard();
                        }
                        model.CurrentCardUrl = result.rs.imgUrl;
                        model.HeadImgUrl = user.HeadImageUrl;
                        model.IsSuccess = 1;
                        model.NickName = user.NickName;
                        model.Phone = user.Phone;
                        //刘文宁修改  2015/9/2
                        model.UserID = user.UserID;
                        model.CreateDt = DateTime.Now;
                        if (!IsExist)
                        {
                            _VisitingCardService.Add(model);
                        }
                        else
                        {
                            _VisitingCardService.Update(model);
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                FastDFSClient.RemoveFile(FastDFSClient.DefaultGroup.GroupName, tmp);
                            }
                        }
                    }
                    result.rs.imgUrl = (ConfigurationManager.AppSettings["ImagePath"] + result.rs.imgUrl);//刘文宁修改  2015/9/2 
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        public ApiResultModel GeneratedVisitingCard(ParaModel model, IEncryptionService enctyptionService, int lang)
        {
            ParaModel para = new ParaModel();
            para.UserID = Convert.ToBase64String(Encoding.UTF8.GetBytes(model.UserID));
            var apiResult = new ApiResultModel();
            try
            {
                long UserId = 0;
                string ImgUrl = "";
                string Phone = "";
                string NickName = "";
                string VirtualPath = @"VisitingCard\";
                var domain = string.IsNullOrEmpty(ConfigurationManager.AppSettings["domain"]) ? "http://api.0066mall.com" : ConfigurationManager.AppSettings["domain"].ToString();
                string ShareLink = ConfigurationManager.AppSettings["ShareLink"] + para.UserID;//+"?lang=" + lang ;

                if (!long.TryParse(model.UserID.ToString(), out UserId))
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", lang);
                    return apiResult;
                }
                if (UserId <= 0)
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", lang);
                    return apiResult;
                }
                //根据ID获取用户头像URL     
                if (null == model)
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", lang);
                    return apiResult;
                }
                Phone=HidePhone(model.Phone);
                //if (!string.IsNullOrEmpty(model.Phone) && model.Phone.Length > 8)
                //{
                //    Phone = model.Phone.Substring(0, 2) + "******" + model.Phone.Substring(8, model.Phone.Length - 8);
                //}
                //else
                //{
                //    Phone = model.Phone;
                //}
                NickName = model.NickName;
                ImgUrl = model.HeadImageUrl;
                //如果本系统头像为空,从IM获取用户头像
                if (string.IsNullOrEmpty(model.HeadImageUrl))
                {
                    ImgUrl = domain + "/VisitingCard/default.png";
                }
                else
                {
                    //如果头像存在文件服务器上
                    if (!ImgUrl.StartsWith("http://") && !ImgUrl.StartsWith("https://"))
                    {
                        ImgUrl = ConfigurationManager.AppSettings["ImageHeader"] + ImgUrl;
                    }
                }
                if (string.IsNullOrEmpty(ImgUrl) || !IsImgFilename(ImgUrl))
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", lang); //"获取用户头像出错";
                    return apiResult;
                }
                if (!UrlIsExist(ImgUrl))
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", lang);// "图片地址错误";
                    return apiResult;
                }
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(ImgUrl);
                Stream stream = httpWebRequest.GetResponse().GetResponseStream();
                try
                {

                    string folderPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + VirtualPath;
                    //string folderPath = HttpContext.Current.Server.MapPath(VirtualPath);此异步执行有问题
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    Bitmap pic = MergerImgHelper.CreateCard(folderPath + "moban.jpg", stream, ShareLink, Phone, NickName); 

                    MemoryStream ms = new MemoryStream();
                    pic.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    string fileName = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, ms.ToArray(), "jpg");

                    HKTHMall.Services.YHUser.YH_UserService YH_UserService = new HKTHMall.Services.YHUser.YH_UserService(null,null);
                    YH_UserService.UpdateUserOrcodeUrl(Convert.ToInt64(model.UserID), 1, fileName);//地址保存到数据库
                    apiResult.rs = fileName;//ConfigurationManager.AppSettings["ImagePath"]+ 
                    apiResult.flag = 1;
                    apiResult.msg = "个性名片生成成功";
                }
                catch (Exception e)
                {
                    apiResult.flag = 1;
                    apiResult.msg = e.Message;
                }
            }
            catch (Exception e)
            {
                apiResult.flag = 1;
                apiResult.msg = e.Message;
            }
            return apiResult;
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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
        public static bool UrlIsExist(string url)
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

    }
}