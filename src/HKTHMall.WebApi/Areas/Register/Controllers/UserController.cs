using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.WebLogin;
using HKTHMall.Services.WebLogin.Impl;

using HKTHMall.Core;
using HKSJ.MidMessage.Protocol;
using HKTHMall.Services;
using HKTHMall.WebApi.Areas.Register.Models;
using HKTHMall.Services.Common;
using HKSJ.Common;
using System.Threading;
using HKTHMall.Services.Sys;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.IO;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using HKTHMall.WebApi.VisitingCard;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.WebApi.Areas.Register.Controllers
{
    public class UserController : Controller
    {
        private LoginService _LoginService = new LoginService();
        // GET: Register/User
        public ActionResult Index(string id, string lang)
        {
            lang = "3";
            int Lang = 3;
            if (!int.TryParse(lang, out Lang))
            {
                Lang = 3;
            }
            try
            {
                ParameterSetService parameterService = new ParameterSetService();

                var defaultPhone = parameterService.GetParametePValueById(1856732830).Data;
                if (string.IsNullOrWhiteSpace(defaultPhone))
                    defaultPhone = "0968893058";
                ViewBag.ID = id;
                ViewBag.Lang = Lang;
                //语言
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CultureHelper.GetLanguage(Lang));
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                try
                {
                    id = Encoding.UTF8.GetString(Convert.FromBase64String(string.IsNullOrEmpty(id) ? "" : id));
                    YH_UserModel user = _LoginService.GetUserInfoById(long.Parse(id)).Data;
                    if (user == null)
                        ViewBag.Refer =  defaultPhone;
                    else
                        ViewBag.Refer = user.Phone;

                    //语言
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CultureHelper.GetLanguage(Lang));
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                }
                catch (Exception e)
                {
                    ViewBag.Refer = defaultPhone;
                }
                ViewBag.RealPhone = ViewBag.Refer;
                ViewBag.Refer = HidePhone(ViewBag.Refer);
            }
            catch(Exception ex)
            {
                NLogHelper.GetCurrentClassLogger().Error(ex);
                return Content(CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", Lang));
            }
            return View();
        }

        public ActionResult Success(string id, int lang=3)
        {
            if (lang < 1 || lang > 3)
            {
                lang = 3;
            }
            try
            {
                id = Encoding.UTF8.GetString(Convert.FromBase64String(id));
                ViewBag.Acc = id;
                ViewBag.Id = "";
                ViewBag.Lang = lang;
                YH_UserModel user = _LoginService.GetUserInfoByPhone(id).Data;
                if (user != null)
                    ViewBag.Id = user.Phone;
                return View();
            }
            catch (Exception e)
            {
                return Content(CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", lang));
            }
        }

        /// <summary>
        /// 注册发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCode(string phone, int lang)
        {
            if (new Regex(@"^[0][6||8-9][0-9]{8}$").IsMatch(phone))
            {
                try
                {
                    ReqBase req = new ReqBase();
                    req.sys_id = Settings.EmSystemId;
                    req.ip = Settings.GetIPAddress();
                    req.mac = Settings.GetMacAddress();
                    req.dev = 1;
                    var resp = HKSJ.MidMessage.Services.EmMethodManage.EmFindUpdateInstance.MsgQueryPhoneVerificationReq(phone, req);
                    if (!resp.isOK || resp.Status != 0)
                    {
                        return Json(new { rs = 0, isAccount = true, Msg = CultureHelper.GetAPPLangSgring("REGISTER_PHONE_REGISTER", lang) });//该手机号码已注册
                    }                      
                    if (Settings.IsEnableEM)   //true账号系统
                    {
                        RequestPhone BindNewPhone = new RequestPhone();
                        BindNewPhone.phone = phone;
                        BindNewPhone.life = 900;
                        BindNewPhone.sys_id = Settings.EmSystemId;
                        BindNewPhone.mac = Settings.GetMacAddress();            //获取本机mac地址
                        BindNewPhone.ip = Settings.GetIPAddress();              //获取ip地址
                        var result = HKSJ.MidMessage.Services.EmMethodManage.EmLoginInstance.MsgRegAccGetCodeReq(BindNewPhone);
                        if (result.isOK)
                        {
                            return Json(new { IsMessage = true, Msg = CultureHelper.GetAPPLangSgring("LOGIN_SEND_SUCCESS", lang) });//发送成功
                        }
                        return Json(new { IsMessage = false, Msg = CultureHelper.GetAPPLangSgring("LOGIN_SEND_FAILURE", lang) });//发送失败
                    }
                    else
                    {
                        PhoneMsg phoneMsg = SendPhoneCode(phone, lang);
                        if (phoneMsg.IsMessage)
                        {
                            HKSJ.Common.DataCache.SetCache(phone, phoneMsg.PhoneCode, DateTime.Now.AddMinutes(15), TimeSpan.Zero);
                        }
                        return Json(phoneMsg.IsMessage);
                    }
                }
                catch (Exception)
                {
                    return Json(new { IsMessage = false, Msg = CultureHelper.GetAPPLangSgring("LOGIN_SEND_FAILURE", lang) });//发送失败
                }
            }
            else
            {
                return Json(new { IsMessage = false, Msg = CultureHelper.GetAPPLangSgring("LOGIN_PHONE_FORMAT_WRONG", lang) });//手机号码格式不正确
            }
        }

        public ActionResult GetImg(string id)
        {
            try
            {
                string url= Request.Url.ToString();
                QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
                qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//设置二维码编码格式 
                qRCodeEncoder.QRCodeScale = 4;//设置编码测量度  
                qRCodeEncoder.QRCodeVersion = 0;//设置编码版本   
                qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//设置错误校验 
                Bitmap image = qRCodeEncoder.Encode(url.Substring(0,url.IndexOf("/Register/User"))+Url.Action("Index","Share", new { id = Convert.ToBase64String(Encoding.UTF8.GetBytes(id)) }));
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
                Response.BinaryWrite(ms.GetBuffer());
                Response.End();
                ms.Dispose();
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
        private string GetRandomNumber(int NumberLength)
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
        private PhoneMsg SendPhoneCode(string PhoneNum, int lang)
        {

            string phoneCode = GetRandomNumber(6);
            PhoneMsg phonMsg = new PhoneMsg();
            if (System.Web.HttpContext.Current.Session["GetPhoneMsgDateTime"] != null)
            {
                DateTime oldTime = Convert.ToDateTime(System.Web.HttpContext.Current.Session["GetPhoneMsgDateTime"]);
                double Seconds = (DateTime.Now - oldTime).TotalSeconds;
                if (Seconds < 120)
                {
                    phonMsg.IsMessage = false;
                    phonMsg.PhoneCode = phoneCode;
                    phonMsg.Msg = CultureHelper.GetAPPLangSgring("LOGIN_SEND_TIMERANGE", lang);//两次发送短信时间间隔不得小于120秒
                    return phonMsg;
                }
            }
            ResultHelper result = new ResultHelper();
            if (Settings.IsMessageEM)
            {
                result.Code = 1;
                phoneCode = "666666";
            }
            else
            {
                result = SMSCommon.QTSubmitSMS(1, PhoneNum, phoneCode, "", 1);
            }

            if (result.Code == 1)
            {
                phonMsg.IsMessage = true;
                phonMsg.Msg = CultureHelper.GetAPPLangSgring("LOGIN_SEND_SUCCESS", lang);//发送成功
            }
            else
            {
                phonMsg.IsMessage = false;
                phonMsg.Msg = CultureHelper.GetAPPLangSgring("LOGIN_SEND_FAILURE", lang);//发送失败              
            }
            phonMsg.PhoneCode = phoneCode;
            System.Web.HttpContext.Current.Session["GetPhoneMsgDateTime"] = DateTime.Now;
            System.Web.HttpContext.Current.Session["phoneNum"] = PhoneNum;
            System.Web.HttpContext.Current.Session["phoneCode"] = phoneCode;
            return phonMsg;
        }

        /// <summary>
        /// 判断手机号有效性（是否已注册,是否锁定,是否删除）
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsAccountOrDelOrLock(string phone, int lang)
        {
            try
            {
                ReqBase req = new ReqBase();
                req.sys_id = Settings.EmSystemId;
                req.ip = Settings.GetIPAddress();
                req.mac = Settings.GetMacAddress();
                req.dev = 1;
                var result = HKSJ.MidMessage.Services.EmMethodManage.EmFindUpdateInstance.MsgQueryPhoneVerificationReq(phone, req);
                if (result.isOK && result.Status == 0)
                {
                    return Json(new { rs = 1, isAccount = false });
                }
                return Json(new { rs = 0, isAccount = true, msg = CultureHelper.GetAPPLangSgring("REGISTER_PHONE_REGISTER", lang) });//该手机号码已注册
            }
            catch (Exception e)
            {
                return Json(new { rs = 1, isAccount = false });
            }
        }
        [HttpPost]
        public JsonResult RegisterUser(string phone, string pwd, long code, string email, string inviatePhone,int lang)
        {
            if (lang < 1 || lang > 3)
            {
                lang = 3;
            }
            //注册用户信息
            YH_UserModel _user = null;
            //接收返回信息
            BackMessage msg=new BackMessage();
            msg.status = 0;             
            if (new Regex(@"^[A-Za-z]+$").IsMatch(pwd) || new Regex(@"^[0-9]*$").IsMatch(pwd))
            {
                msg.message = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORDNUMBERS",lang);
                return Json(msg);
            }
            if (pwd.Length < 8 || pwd.Length > 20 || new Regex(@"[^\x00-\xff]|\s").IsMatch(pwd))
            {
                msg.message = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORD",lang);
                return Json(msg);
            }
            //加密密码
            string psw_md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
            if (Settings.IsEnableEM) //是否启用主账号系统。。。
            {
                msg = _LoginService.RegSynAcc(phone, pwd, psw_md5, code, email, inviatePhone, ref _user);
            }
            else
            {
                msg = _LoginService.Register(phone, pwd, psw_md5, code, email, inviatePhone, out _user);
            }
            if (msg.status == 1)
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    (new VisitingCardCreate()).GeneratedVisitingCard(_user.UserID,CultureHelper.GetLanguageID());
                });

                msg.status = 1;
                msg.message = CultureHelper.GetAPPLangSgring("API_REGISTEREDSUCCESS",lang);
                #region 登录处理
                #endregion
                msg.message = Convert.ToBase64String(Encoding.UTF8.GetBytes(phone));
            }
            return Json(msg);
        }

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
    }
}