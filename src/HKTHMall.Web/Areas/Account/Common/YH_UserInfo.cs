using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using HKSJ.Common;
using HKTHMall.Core.UploadFile;
using System.Security.Cryptography;
using System.IO;
using HKTHMall.Services.WebLogin;
using HKTHMall.Web.Common;
using HKTHMall.Core;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.WebLogin.Impl;
using HKTHMall.Services.Common.MultiLangKeys;
using Omise;
using HKSJ.Common.FastDFS;


namespace HKTHMall.Web.Account
{
    public class YH_UserInfo
    {

        private readonly ILoginService _loginService = BrEngineContext.Current.Resolve<ILoginService>();

        /// <summary>
        /// 功能:验证账号系统中是否存在此用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>       
        public bool user_isNull(long ID)
        {
            //ResponseUserInfoData results = EmMethodManage.EmFindUpdateInstance.MsgBatchGetInfoByIdReq(new long[] { ID });
            //if (!results.isOK)
            //{
            //    return false;
            //}
            //if (results.data == null)
            //{
            //    return false;
            //}
            //List<ResponseUserInfo> list = new List<ResponseUserInfo>(results.data);
            //ResponseUserInfo model = list[0];
            //if (model.uid != ID)
            //{
            //    return false;
            //}
            if(ID>0)
            {
                return true;
            }
            return false;
        }


        ///// <summary>
        ///// 功能:修改头像时,账号系统打开,启用账号系统
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <param name="img_url_name"></param>
        ///// <returns></returns>    
        //public HKTHMall.Web.Account.UserMsg User_Account(long ID, string img_url_name)
        //{
        //    HKTHMall.Web.Account.UserMsg user_msg = new UserMsg();
        //    //if (!user_isNull(ID))
        //    if (ID>0)
        //    {
        //        user_msg.flag = false;
        //        //user_msg.strMsg = "身份验证失败,请重新登录";
        //        user_msg.strMsg = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_AUTHENFAILED");
        //        return user_msg;
        //    }
        //    var result = EmMethodManage.EmFindUpdateInstance.MsgSetUserInfoReq(new RequestUserInfoData
        //    {
        //        uid = ID,
        //        data = new RequestUserInfo { img_url = img_url_name }
        //    });
        //    if (!result.isOK)
        //    {
        //        user_msg.flag = false;
        //        //user_msg.strMsg = "上传失败";
        //        user_msg.strMsg = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_UPLOADFAILED");
        //        return user_msg;
        //    }
        //    user_msg.flag = true;
        //    //user_msg.strMsg = "上传成功";
        //    user_msg.strMsg = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_SAVESUCCESS");
        //    HKTHMall.Web.Account.UserMsg user_des = User_Account_false(ID, img_url_name);
        //    return user_msg;
        //}


        /// <summary>
        ///  功能:修改头像时,账号系统关闭,操作本地数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="img_url_name"></param>
        /// <returns></returns>
        public HKTHMall.Web.Account.UserMsg User_Account_false(long ID, string img_url_name)
        {
            HKTHMall.Web.Account.UserMsg user_msg = new UserMsg();
            YH_UserModel YHUModel = new LoginService().GetUserInfoById(ID).Data;
            if (YHUModel != null && YHUModel.UserID == ID)
            {
                YHUModel.HeadImageUrl = img_url_name;
                YHUModel.UpdateDT = DateTime.Now;
                if (new LoginService().Update(YHUModel).Data != 1)
                {
                    user_msg.flag = false;
                    //user_msg.strMsg = "上传失败";
                    user_msg.strMsg = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_UPLOADFAILED");
                    return user_msg;
                }
                //#region 生成二维码
                //VisitingCardCreate _VisitingCardCreate = new VisitingCardCreate();
                //ParaModel pm = new ParaModel();
                //pm.Account = YHUModel.Account;
                //pm.HeadImageUrl = YHUModel.HeadImageUrl;
                //pm.Phone = YHUModel.Phone;
                //pm.NickName = YHUModel.NickName;
                //pm.UserID = YHUModel.UserID.ToString();
                //_VisitingCardCreate.GeneratedVisitingCard(pm, CultureHelper.GetLanguageID());
                //#endregion
            }
            else
            {
                user_msg.flag = false;
                //user_msg.strMsg = "上传失败";
                user_msg.strMsg = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_UPLOADFAILED");
                return user_msg;
            }
            user_msg.flag = true;
            //user_msg.strMsg = "上传成功";
            user_msg.strMsg = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_SAVESUCCESS");
            return user_msg;

        }


        /// <summary>
        /// 功能:修改性别,昵称,邮箱账号系统开启
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public HKTHMall.Web.Account.UserMsg User_Account_Nickname(long ID, YH_UserModel model)
        {
            HKTHMall.Web.Account.UserMsg user_msg = new UserMsg();
            if (!user_isNull(ID))
            {
                user_msg.flag = false;
                //user_msg.strMsg = "身份验证失败,请重新登录";
                user_msg.strMsg = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_AUTHENFAILED");
                return user_msg;
            }
            //var result = EmMethodManage.EmFindUpdateInstance.MsgSetUserInfoReq(new RequestUserInfoData
            //{
            //    uid = ID,
            //    data = new RequestUserInfo { nick = model.NickName, sex = model.Sex }
            //});
            try
            {
                LoginService service = new LoginService();
                if (!(service.Update(model).Data == 1))
                {
                    user_msg.flag = false;
                    user_msg.strMsg = CultureHelper.GetLangString("USERINFO_UPDATE_FAILURE");
                    //user_msg.strMsg = result.Status != 0 ? ErrMsg.GetErrorMsg(result.Status) : CultureHelper.GetLangString("USERINFO_UPDATE_FAILURE");
                    return user_msg;
                }

                user_msg.flag = true;
                //user_msg.strMsg = "修改成功";
                user_msg.strMsg = CultureHelper.GetLangString("USERINFO_UPDATE_SUCCESS");
                return user_msg;
            }
            catch (Exception ex)
            {

                user_msg.flag = false;
                //user_msg.strMsg = "修改失败";
                user_msg.strMsg = CultureHelper.GetLangString("USERINFO_UPDATE_FAILURE");
                return user_msg;
            }
        }



        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <author>樊利民</author>
        /// <param name="userId">用户Id</param>
        /// <param name="oldPassword">旧密码(MD5加密)</param>
        /// <param name="newPassword">新密码(MD5加密)</param>
        /// <returns></returns>
        public HKTHMall.Web.Account.UserMsg UserAccountModifyPassword(long userId, string oldPassword, string newPassword)
        {
            HKTHMall.Web.Account.UserMsg userMsg = new UserMsg()
            {
                flag = false,
                strMsg = CultureHelper.GetLangString("USERINFO_UPDATE_FAILURE")        //修改失败
            };


            //密码不能为空
            if (string.IsNullOrEmpty(newPassword))
            {
                userMsg.strMsg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_PWDNOTEMPTY");//密码不能为空
                return userMsg;
            }

            YH_UserModel userModel = _loginService.GetUserInfoById(userId).Data;

            if (userModel.PassWord != oldPassword)
            {
                userMsg.strMsg = CultureHelper.GetLangString("USERINFO_ORIGINAL_PASSWORD_INCORRECT"); //原始密码不正确
                return userMsg;
            }

            //新密码不能与旧密码一致！
            if (userModel.PassWord == newPassword)
            {
                userMsg.strMsg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_NOTCONSISTENTPWD");//新密码不能与旧密码一致！
                return userMsg;
            }

            //登录密码不能与交易密码一致
            if (userModel.PayPassWord == newPassword)
            {
                userMsg.strMsg = CultureHelper.GetLangString("USERINFO_LOGINPWD_NOT_EQUAL_TRANSPASSWORD ");//登录密码不能与交易密码一致
                return userMsg;
            }
                userModel.PassWord = newPassword;
                userMsg.flag = _loginService.Update(userModel).Data == 1;
            if (userMsg.flag)
            {
                userMsg.strMsg = CultureHelper.GetLangString("USERINFO_UPDATE_SUCCESS");    //修改成功
            }
            return userMsg;
        }


        /// <summary>
        /// 账号系统上传图片
        /// </summary>
        /// <param name="_file">图片</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public FileUploadResult UploadFileCommon(HttpPostedFileBase _file, long userId)
        {
            string imageUrl = string.Empty;
            try
            {
                Stream stream = _file.InputStream;
                Byte[] buffer = new Byte[stream.Length];
                //从流中读取字节块并将该数据写入给定缓冲区buffer中
                stream.Read(buffer, 0, Convert.ToInt32(stream.Length));

                string localFileName = System.IO.Path.GetFileName(_file.FileName);
                string myFileExtension = System.IO.Path.GetExtension(localFileName);
                string prefix_name =System.IO.Path.GetFileNameWithoutExtension(localFileName);
                if (!getFileExtension(myFileExtension))
                {
                    return new FileUploadResult { result = false, ResultExplain = CultureHelper.GetLangString("USERINFO_UPLOAD_FILE_TYPE_ERROR") };//文件类型异常
                }
                myFileExtension = System.IO.Path.GetExtension(localFileName).Substring(1);
                var fileName = FastDFSClient.UploadFile(FastDFSClient.DefaultGroup, buffer, myFileExtension);


                //fileName = ConfigHelper.GetConfigString("ImagePath") + fileName;// +new UploadFile().GetThumbsImage(fileName, 364, 230);
               
                return new FileUploadResult { url = fileName, ResultExplain = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_SAVESUCCESS"), name = imageUrl, result = true };

                //StorageNode sn = new StorageNode();
                //sn.GroupName = "acc";
                ////sn.StorePathIndex=
                //string aaa = FastDFSClient.UploadFile(sn, buffer, myFileExtension);

                //var bde = FastDFSClient.UploadSlaveFile("acc", buffer, localFileName, prefix_name, myFileExtension);

                //ConfigHelper.GetConfigString("ImageHeader") + new UploadFile().GetThumbsImage(imageUrl, 364, 230);


                //----------------账号系统-------------------
                //RequestASFileUpload reqASFileUpInfo = new RequestASFileUpload();
                //reqASFileUpInfo.uid = userId;
                //RequestASFileData mfile = new RequestASFileData();
                //mfile.ext = Path.GetExtension(_file.FileName).Replace(".", "");
                //mfile.type = 1;   //1:图片,2:语音,3:视频,4:文件  
                //mfile.tag = 1;//文件数据tlv对应tag值(写死)

                //string digresule = string.Empty;

                //MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                //byte[] md5byte = md5.ComputeHash(buffer);
                //digresule = System.BitConverter.ToString(md5byte);
                //digresule = digresule.Replace("-", "");

                //mfile.digest = digresule; //文件摘要
                //mfile.size = _file.ContentLength; //文件

                //stream.Close();
                //stream.Dispose();

                //reqASFileUpInfo.file = new RequestASFileData[] { mfile };

                //reqASFileUpInfo.type = 1; //1:MD5(标记加密方式)
                //reqASFileUpInfo.modle = 3; //1和2:IM模块,3:帐号模块

               // ResponseASFileUpload code = EmMethodManage.EmFASTDFSManageInstance.ASFileUpload(reqASFileUpInfo, buffer);

                //if (code == null)
                //{
                //    return new FileUploadResult { result = false, ResultExplain = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_ABNORMSERVER") };//服务器异常
                //}

                //if (code.rt == null)
                //{
                //    return new FileUploadResult { result = false, ResultExplain = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_ABNORMSERVER") };
                //}

                //foreach (var ImgUrldata in code.rt)
                //{
                //    if (ImgUrldata.code == 0) //0:成功
                //    {
                //        imageUrl = ImgUrldata.url;
                //        //获得服务器地址 
                //        string url = ConfigHelper.GetConfigString("ImageHeader") + new UploadFile().GetThumbsImage(imageUrl, 364, 230);
                //        return new FileUploadResult { url = url, ResultExplain = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_SAVESUCCESS"), name = imageUrl, result = true };
                //    }
                //    else
                //    {
                //        return new FileUploadResult { result = false, ResultExplain = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_FAILUREUPLOAD") };//上传图片失败
                //    }
                //}




                return new FileUploadResult { result = false, ResultExplain = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_FAILUREUPLOAD") };//上传图片失败
                //----------------账号系统-------------------
            }
            catch (Exception ex)
            {
                //Logger.Error("UploadUserImg", ex);
                return new FileUploadResult { result = false, ResultExplain = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_UPLOADFAILED") };//上传图片失败

            }
        }

        /// <summary>
        ///  功能:验证文件格式
        /// </summary>
        /// <param name="myFileName">文件</param>
        /// <returns></returns>
        private bool getFileExtension(string myFileName)
        {
            //是否在允许的格式内
            bool flag = false;

            //文件格式
            string flagtype = "jpg|jpeg|png|gif";

            //文件扩展名
            string myFileExtension = null;

            //获得文件扩展名
            myFileExtension = System.IO.Path.GetExtension(myFileName);//若为null,表明文件无后缀名;

            if (string.IsNullOrWhiteSpace(myFileExtension))
            {
                return flag;
            }
            //分解允许上传文件的格式
            myFileExtension = myFileExtension.ToLower();//转化为小写
            string[] temp = flagtype.Split('|');


            //判断上传的文件是否是允许的格式
            foreach (string data in temp)
            {
                if (("." + data) == myFileExtension)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }


        ///// <summary>
        ///// 功能:邮箱是否存在
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <param name="old_pwd"></param>
        ///// <param name="new_pwd"></param>
        ///// <returns></returns>
        //public HKTHMall.Web.Account.UserMsg User_Account_EmailExist(string Email)
        //{
        //    HKTHMall.Web.Account.UserMsg user_msg = new UserMsg();
        //    //var result = EmMethodManage.EmEmailInstance.MsgQueryEmailIsBindReq(Email);
        //    if (result.Status != 0)
        //    {
        //        user_msg.flag = false;
        //        if (result.Status == 1048830)
        //        {
        //            user_msg.strMsg = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_EMAILUSE");//邮箱已经被使用,请更改
        //        }
        //        else if (result.Status == 1048828)
        //        {
        //            user_msg.strMsg = CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_EMAILERROR");//邮箱格式错误
        //        }
        //        else
        //        {
        //            user_msg.strMsg = CultureHelper.GetLangString("SYSTEM_ERROR");//系统异常,请稍后再试
        //        }
        //        return user_msg;
        //    }
        //    user_msg.flag = true;
        //    user_msg.strMsg = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_EMAILCANUSED");
        //    return user_msg;

        //}
    }
}