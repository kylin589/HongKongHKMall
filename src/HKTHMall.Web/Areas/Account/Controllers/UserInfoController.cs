using System.IO;
using HKTHMall.Core;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.AC;
using HKTHMall.Services.Users;
using HKTHMall.Services.WebLogin;
using HKTHMall.Web.Account;
using HKTHMall.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Web.Common;
using HKTHMall.Domain.AdminModel.Models.User;
using System.Text.RegularExpressions;
using HKSJ.Common;
using System.Web.Security;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Core.Security;
using System.Dynamic;
using HKTHMall.Web.Models;
using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Core.Config;

namespace HKTHMall.Web.Areas.Account.Controllers
{

    public class UserInfoController : BaseController
    {
        private readonly IUserAddressService _userAddress;
        private readonly ITHAreaService _thAreaService;
        private readonly ILoginService _LoginService;
        private readonly IBankService _BankService;
        private readonly IEncryptionService _enctyptionService;
        private readonly IYH_UserBankAccountService _UserBankAccountService;
        // public static readonly string ImagePath = HKSJ.Common.ConfigHelper.GetConfigString("ImageHeader");
        SendPhoneMsg phoneMsg = new SendPhoneMsg();
        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        /// <summary>
        /// 构造函数
        /// zhoub 20150716
        /// </summary>
        /// <param name="acDepartmentService"></param>
        public UserInfoController(IUserAddressService userAddress, ITHAreaService thAreaService, ILoginService loginService,
            IBankService bankService, IYH_UserBankAccountService userBankService, IEncryptionService enctyptionService)
        {
            _userAddress = userAddress;
            _thAreaService = thAreaService;
            _LoginService = loginService;
            _BankService = bankService;
            _UserBankAccountService = userBankService;
            _enctyptionService = enctyptionService;
        }

        #region 个人信息操作
        private string areaidlist = "";
        /// <summary>
        /// 递归获取三级城市ID
        /// </summary>
        /// <param name="areaid"></param>
        public void GetAddress(int areaid)
        {
            if (areaid > 0)
            {
                List<THAreaInfo> result = _thAreaService.GetTHAreaByID(CultureHelper.GetLanguageID(), areaid).Data;
                THAreaInfo info = (THAreaInfo)result[0];
                if (info != null)
                {
                    areaidlist += "," + info.THAreaID;
                }
                GetAddress(info.ParentID);
            }
        }

        // GET: Account/UserInfo
        /// <summary>
        /// 个人信息页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            long userId = base.UserID;
            YH_UserModel userInfo = _LoginService.GetUserInfoById(userId).Data;
            //if (Settings.IsEnableEM)
            //{
            //    //查询用户密保激活状态 type 1 邮箱账号  2 手机号 
            //    ReqUserInfoActivateState ActivateState = new ReqUserInfoActivateState();
            //    ActivateState.mac = Core.Settings.GetMacAddress(); //获取本机mac地址
            //    ActivateState.ip = Core.Settings.GetIPAddress();   //获取本机ip地址
            //    ActivateState.sys_id = Core.Settings.EmSystemId;      //获取系统业务编号
            //    ActivateState.dev = Core.Settings.EmDev;//注册来源,1:网站
            //    ActivateState.type = 1;
            //    ActivateState.uid = base.UserID;
            //    var stateresponse = EmMethodManage.EmFindUpdateInstance.MsgQueryInfoActivateStateReq(ActivateState);
            //    userInfo.ActiveEmail = (byte)stateresponse.state;
            //}

            // ViewData["thArea"] = _thAreaService.GetTHAreaByParentID(CultureHelper.GetLanguageID(), 0).Data;

            ViewBag.YEAR = DateTime.Now.Year;
            int year = 0;
            int month = 0;
            int day = 0;
            if (userInfo.Birthday != null && DateTime.Parse(userInfo.Birthday.ToString()).ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            {
                year = Convert.ToInt32(((DateTime)userInfo.Birthday).Year);
                month = Convert.ToInt32(((DateTime)userInfo.Birthday).Month);
                day = Convert.ToInt32(((DateTime)userInfo.Birthday).Day);
            }
            ViewBag.uYear = year;
            ViewBag.uMonth = month;
            ViewBag.uday = day;

            //if (!string.IsNullOrEmpty(userInfo.THAreaID.ToString()))
            //{
            //    int areaid = userInfo.THAreaID;
            //    GetAddress(areaid);
            //    string[] ids = areaidlist.Split(',');
            //    if (ids.Length > 0)
            //    {
            //        ViewBag.area1 = ids[ids.Length - 1];
            //    }
            //    if (ids.Length > 1)
            //    {
            //        ViewBag.area2 = ids[ids.Length - 2];
            //    }
            //    if (ids.Length > 2)
            //    {
            //        ViewBag.area3 = ids[ids.Length - 3];
            //    }
            //}
            return View(userInfo);
        }


        /// <summary>
        /// 返回一个月的天数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult MonthDayCount(int year = 1900, int month = 1)
        {
            int daycount = DateTime.DaysInMonth(year, month);
            return Json(new { rs = 1, msg = daycount });
        }

        /// <summary>
        /// 个人资料修改
        /// </summary>
        /// <param name="nickName">昵称</param>
        /// <param name="radioName">性别</param>
        /// <param name="email">邮箱</param>
        /// <param name="selectYear">年</param>
        /// <param name="selectMonth">月</param>
        /// <param name="selectDay">日</param>
        /// <param name="areaid">地区ID</param>
        /// <param name="address">详细地址</param>
        /// <returns></returns>
        [Authorize]
        public JsonResult apdateUserinfo(string nickName, string radioName, string selectYear, string selectMonth, string selectDay, string areaid = "", string address = "", string email = "")
        {

            long userId = base.UserID;
            YH_UserModel model = _LoginService.GetUserInfoById(userId).Data;
            if (model == null)
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_USER_NOT_EXIST") });//用户不存在！
            }
            if (model.ThirdType > 0 && !string.IsNullOrEmpty(model.ThirdID) && !string.IsNullOrEmpty(email))
            {
                model.Email = email;
            }
            model.NickName = nickName;
            model.Sex = byte.Parse(radioName);
            if (areaid != "")
            {
                model.THAreaID = int.Parse(areaid);
            }

            model.DetailsAddress = address;
            if (selectYear != "0" && selectMonth != "0" && selectDay != "0")
            {
                selectMonth = selectMonth.Length == 1 ? "0" + selectMonth : selectMonth;
                selectDay = selectDay.Length == 1 ? "0" + selectDay : selectDay;
                string date = selectYear + "-" + selectMonth + "-" + selectDay + " 00:00:00";
                DateTime dt = Convert.ToDateTime(date);
                if (dt > DateTime.Now)
                {
                    return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_BIRTHDAY_NOT_GREATER_THAN_CURRENT_DATETIME") });//生日不能大于当前时期！
                }
                model.Birthday = dt;
            }
            else
            {
                model.Birthday = null;
            }
            model.UpdateDT = DateTime.Now;
            model.UpdateBy = model.Account;
            UserMsg UserMsg = new UserMsg();

            //#region 生成二维码
            //VisitingCardCreate _VisitingCardCreate = new VisitingCardCreate();
            //ParaModel pm = new ParaModel();
            //pm.Account = model.Account;
            //pm.HeadImageUrl = model.HeadImageUrl;
            //pm.Phone = model.Phone;
            //pm.NickName = model.NickName;
            //pm.UserID = model.UserID.ToString();
            //_VisitingCardCreate.GeneratedVisitingCard(pm, CultureHelper.GetLanguageID());
            //#endregion


            if (_LoginService.Update(model).Data == 1)
            {
                UserModel cookieModel = new UserModel();
                cookieModel.UserID = model.UserID;
                cookieModel.Account = model.Account;
                cookieModel.Phone = model.Phone == null ? "" : model.Phone;
                cookieModel.NickName = model.NickName;
                cookieModel.UserType = int.Parse(model.UserType.ToString());
                cookieModel.Email = model.Email;
                SetAuthCookie(cookieModel);
                return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_SAVE_SUCCESS") });//保存成功！
            }
            return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_SAVE_FAILURE") });//保存失败！
        }
        #endregion

        #region 上传图片
        /// <summary>
        /// 上传图像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult SubmintImg()
        {
            try
            {
                var user = GetUserResultModel();
                if (user == null)
                {
                    return Json(new { flag = false, strMsg = CultureHelper.GetLangString("USERINFO_NOT_LOGGING") });//没有登录
                }
                else
                {
                    HttpFileCollectionBase HttpFileCollectionBase = Request.Files;
                    int count = HttpFileCollectionBase.Count;

                    if (count == 0)
                    {
                        return Json(new { flag = false, strMsg = CultureHelper.GetLangString("USERINFO_NOT_UPLOAD_ANY_FILE") });//没有上传任何文件
                    }
                    else if (count > 2)
                    {
                        return Json(new { flag = false, strMsg = CultureHelper.GetLangString("USERINFO_UPLOAD_PICTUR_TOO_MANY") });//上传图片过多
                    }
                    else
                    {
                        string name = "";
                        string url = "";
                        HKTHMall.Core.UploadFile.FileUploadResult fileUploadResult = new YH_UserInfo().UploadFileCommon(Request.Files[0], user.UserID);

                        name = fileUploadResult.name;
                        url = fileUploadResult.url;

                        //上传成功开始写入数据库
                        if (fileUploadResult.result)
                        {
                            UserMsg UserMsg = new UserMsg();

                            UserMsg = new YH_UserInfo().User_Account_false(user.UserID, url);
                            if (UserMsg.flag)
                            {
                                return Json(new { flag = true, img = GetConfig.FullPath() + url, name = name });
                            }
                            else
                            {
                                return Json(UserMsg, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { flag = false, strMsg = fileUploadResult.ResultExplain });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 邮件发送 验证
        /// <summary>
        /// 发送验证邮件
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult SendEmail(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_E-MAIL_NOT_EMPTY") });//邮箱不能为空
            }
            else
            {
                try
                {
                    string cod = CodeHelper.GetRandomNumber(6);
                    objCache.Insert("UpdatePayPass" + emailAddress, cod, null, DateTime.Now.AddDays(1), TimeSpan.Zero);
                    string link = Request.Url.Authority + "/Account/UserInfo/UpdatePayPasswordTwo?ss_key=" + _enctyptionService.RSAEncrypt(emailAddress) + "&Verification=" + _enctyptionService.RSAEncrypt(cod);
                    //string emailContent = CultureHelper.GetLangString("ACCOUNT_PWD_UPDATETRANSPWDDETAILS");
                    link = "http://" + link;
                    //emailContent = string.Format(emailContent, emailAddress, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), emailAddress, "<a href='" + link + "'>" + link + "</a>", DateTime.Now.ToString("yyyy-MM-dd"));
                    //bool result = Mail.sendMail(emailAddress, CultureHelper.GetLangString("ACCOUNT_PWD_UPDATETRANSPWD"), emailContent);
                    string emailContent = string.Empty;
                   // bool result = Mail.sendMail(email, CultureHelper.GetLangString("LOGIN_EMAIL_VERIFICATION"), emailContent);
                    string MailContent = MailTempHelper.GetMailTemp(MailTempType.SetPaymentPassword);
                    string style = MailTempHelper.GetMailTemp(MailTempType.MailStyle);
                    emailContent = string.Format(MailContent, CultureHelper.GetLangString("HOME_FOOTER_FENXIANGTITLE"), ConfigHelper.GetConfigString("domain"), emailAddress, link, style);
                    bool result = Mail.sendMail(emailAddress, CultureHelper.GetLangString("LOGIN_EMAIL_VERIFICATION"), emailContent);
                    if (result)
                    {
                        return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_SEND_SUCCESS") });//发送成功
                    }
                    else
                    {
                        return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_E-USERINFO_SEND_FAILURE") });//发送失败
                    }

                }
                catch
                {
                    return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_E-USERINFO_SEND_FAILURE") });//发送失败
                }

            }
        }

        /// <summary>
        /// 本地发送邮件方法
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [Authorize]
        public bool sendEmail(string address, string number)
        {
            int languageId = CultureHelper.GetLanguageID();//1:中文 2:英文 3:泰文

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(System.Web.HttpContext.Current.Server.MapPath("~/EmailContent/EmailVerify.xml"));
            string strMailTitle = doc.GetElementsByTagName("TITILE")[0].InnerText;
            string strMailBody = doc.GetElementsByTagName("BODY")[0].InnerText;

            string account = base.Phone;
            strMailBody = strMailBody.Replace("$USERNAME$", account);//用户账号替换

            strMailBody = strMailBody.Replace("$Email$", address);

            string urlstr = HKSJ.Common.DEncrypt.DESEncrypt.Encrypt(base.NickName + "#" + base.UserID + "#" + number + "#" + address, "zhenxindeshiubshi");

            string url = ConfigHelper.GetConfigString("WebSite") + "/account/userinfo/EmailSuccess?url=" + urlstr;

            strMailBody = strMailBody.Replace("$URL$", url);//换地址

            bool result = HKTHMall.Web.Common.Mail.sendMail(address, strMailTitle, strMailBody);
            return result;
        }

        /// <summary>
        /// 链接验证
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public bool LinkValidation(long userid, string email, string Number)
        {

            YH_ValidEmailModel v_model = _LoginService.GetModelByUserID(userid).Data;
            if (v_model == null)
            {
                YH_ValidEmailModel modell = new YH_ValidEmailModel();
                modell.UserID = (int)userid;                //用户id
                modell.Email = email;                  //发送的邮件地址
                modell.PostDT = DateTime.Now;          //发送时间
                modell.SerialNumber = Number;          //邮件的编号
                modell.IsValid = 0;                    //是否验证 0:没有验证   1:验证
                if (_LoginService.AddValidEmail(modell).Data.ID > 0)
                {
                    return true;
                }
                return false;
            }
            else
            {
                v_model.UserID = (int)userid;                //用户id
                v_model.Email = email;                  //发送的邮件地址
                v_model.PostDT = DateTime.Now;          //发送时间
                v_model.SerialNumber = Number;          //邮件的编号
                v_model.IsValid = 0;                    //是否验证 0:没有验证   1:验证

                if (_LoginService.UpdateValidEmail(v_model).Data == 1)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 验证邮箱连接是否有效
        /// </summary>
        /// <param name="userid">用户编号</param>
        /// <param name="neber">连接编号</param>
        /// <returns></returns>       
        public int ValidationURL(int userid, string neber)
        {
            YH_ValidEmailModel model = _LoginService.GetModelByUserID(userid).Data;
            if (model == null) { return 1; }//没有对象,则验证失败
            double Seconds = (DateTime.Now - model.PostDT).TotalHours;
            if (Seconds > 24) { return 1; }//验证的时间超过24个小时,验证失败
            if (!neber.Equals(model.SerialNumber)) { return 1; }//如果连接不是最后一个,验证失败
            if (model.IsValid == 1) { return 2; } //如果被验证了,此链接无效
            model.IsValid = 1;
            if (_LoginService.UpdateValidEmail(model).Data == 1)
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// 邮箱验证失败页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EmailFail()
        {
            ViewBag.Email = base.Email;
            return View();
        }

        /// <summary>
        /// 邮箱验证成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EmailSuccess(string url = "", string sess_key = "")
        {
            string urlstr = HKSJ.Common.DEncrypt.DESEncrypt.Decrypt(url, "zhenxindeshiubshi");
            string[] userInfo = urlstr.Split('#');
            int userid = Convert.ToInt32(userInfo[1]);
            YH_UserModel ymodel = _LoginService.GetUserInfoById(userid).Data;
            if (ymodel == null)
            {
                return View("EmailFail");
            }
            ymodel.ActiveEmail = 1;
            ymodel.UpdateDT = DateTime.Now;
            int runcount = ValidationURL(userid, userInfo[2]);
            switch (runcount)
            {
                case 1:
                    return View("EmailFail");
                case 2:
                    return View("EmailFail");
                default:
                    break;

            }
            bool istrue = _LoginService.Update(ymodel).Data == 1 ? true : false;
            if (istrue)
            {
                return View();
            }
            else
            {
                return View("EmailFail");
            }
        }
        #endregion

        #region 账户安全页面
        /// <summary>
        /// 账户安全页面
        /// </summary>
        /// <returns></returns>        
        [Authorize]
        public ActionResult Safe()
        {
            long userId = base.UserID;
            YH_UserModel userInfo = _LoginService.GetUserInfoById(userId).Data;

            //"初级", "中级", "高级" 
            string[] Rarr = { CultureHelper.GetLangString("USERINFO_PRIMARY"), CultureHelper.GetLangString("USERINFO_INTERMEDIATE"), CultureHelper.GetLangString("USERINFO_ADVANCED") };
            //string[] Rarr = { CultureHelper.GetLangString("USERINFO_INTERMEDIATE"), CultureHelper.GetLangString("USERINFO_ADVANCED") };
            int Email = 0;
            int coun = 0;
            string str = Rarr[coun];
            int percent = 33;
            if (!string.IsNullOrWhiteSpace(userInfo.PayPassWord))
            {
                percent = 33 + percent;
                coun = 1 + coun;
                str = Rarr[coun];
            }
            if (!string.IsNullOrWhiteSpace(userInfo.Email) && userInfo.ActiveEmail == 1)
            {
                percent = 34 + percent;
                coun = 1 + coun;
                str = Rarr[coun];
                Email = 1;
                //int i = userInfo.Email.IndexOf("@");
                //ViewBag.Email = userInfo.Email.Substring(0, 1) + "******" + userInfo.Email.Substring(i - 1);
            }
            ViewBag.Email = userInfo.Email;
            ViewBag.PayShow = string.IsNullOrEmpty(userInfo.PayPassWord) ? 0 : 1;
            ViewBag.Rank = str;
            ViewBag.percent = percent + "%";
            ViewBag.EmailCount = userInfo.ActiveEmail;
            ViewBag.RankCount = coun;
            return View();
        }
        #endregion

        #region 短信操作
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult SendPhoneMsg()
        {
            PhoneMsg PhoneMsg = phoneMsg.SendPhoneCode(base.Phone);
            if (PhoneMsg.IsMessage)
                return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_SEND_SUCCESS") });//发送成功！
            else
                return Json(new { rs = 0, msg = PhoneMsg.Msg });
        }

        /// <summary>
        /// 验证手机短信验证码是否正确
        /// </summary>
        /// <param name="code">短信验证码</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        [Authorize]
        public JsonResult PhoneVerificationCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_SMSCODE_NOT_EMPTY") });//短信校验码不能为空
            }
            if (phoneMsg.PhoneVerificationCode(base.Phone, code))
            {
                return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_VALIDATION_SUCCESS") });//验证成功
            }
            return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_SMSCODE_ERROR") });//短信校验码错误
        }

        #endregion

        #region 交易密码操作
        /// <summary>
        /// 设置交易密码页面
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize]
        public ActionResult SetPayPassword()
        {
            return View();
        }
        /// <summary>
        /// 设置交易密码方法
        /// </summary>
        /// <param name="code">短信验证码</param>
        /// <param name="pwd">新密码</param>
        /// <param name="cpwd">确认密码</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult SetPayPassword(string pwd, string cpwd)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_TRANSACTION_PASSWORD_NOT_EMPTY") });//交易密码不能为空
            }
            if (!pwd.Equals(cpwd))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_TWO_PASSWORD_NOT_SAME") });//新密码与确认密码不一致！
            }
            YH_UserModel model = _LoginService.GetUserInfoById(base.UserID).Data;
            if (model == null)
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_USER_NOT_EXIST") });//用户不存在！
            }
            string payword = CodeHelper.GetMD5(pwd);
            if (payword.Equals(model.PassWord))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_TRANSPASSWORD_NOT_EQUAL_LOGINPWD") });//交易密码不能与登录密码一致
            }
            model.PayPassWord = payword;
            if (_LoginService.Update(model).Data == 1)
            {
                return Json(new { rs = 1, msg = CultureHelper.GetLangString("SETPAYPASSWORD_SUCCESS") });//设置成功
            }
            return Json(new { rs = 0, msg = CultureHelper.GetLangString("SETPAYPASSWORD_FAIL") });//设置失败
        }

        /// <summary>
        /// 设置交易密码成功页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SetSuccess()
        {
            return View();
        }


        /// <summary>
        /// 修改交易密码页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UpdatePayPassword()
        {
            ViewBag.Email = base.Email;
            return View();
        }

        /// <summary>
        /// 修改交易密码方法
        /// </summary>
        /// <param name="code">短信验证码</param>
        /// <param name="opwd">原始交易密码</param>
        /// <param name="npwd">新交易密码</param>
        /// <param name="cpwd">确认密码</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult UpdatePayPassword(string code, string npwd, string cpwd)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("CODE_CANNOT_NULL") });//验证码能为空
            }
            bool isValid = VerifyCodeUtil.ValidCode(code, this.GetImageVerifyCodeCacheKey(EImageVerifyCodeType.UpdatePayPwd));
            if (!isValid)
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_CORRECTCODE") });//请输入正确的验证码
            }
            if (string.IsNullOrEmpty(npwd))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_TRANSACTION_PASSWORD_NOT_EMPTY") });//交易密码不能为空
            }
            if (!npwd.Equals(cpwd))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_TWO_PASSWORD_NOT_SAME") });//新密码与确认密码不一致！
            }
            YH_UserModel model = _LoginService.GetUserInfoById(base.UserID).Data;
            if (model == null)
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_USER_NOT_EXIST") });//用户不存在！
            }
            string payword = CodeHelper.GetMD5(npwd);
            if (payword.Equals(model.PassWord))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_TRANSPASSWORD_NOT_EQUAL_LOGINPWD") });//交易密码不能与登录密码一致
            }
            if (payword.Equals(model.PayPassWord))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_NOTCONSISTENTPWD") });//新支付密码不能与旧支付密码一致
            }
            model.PayPassWord = payword;
            if (_LoginService.Update(model).Data == 1)
            {
                return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_UPDATE_SUCCESS") });//修改成功
            }
            return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_UPDATE_FAILURE") });//修改失败
        }

        [Authorize]
        public ActionResult UpdatePayPasswordTwo(string ss_key = "", string Verification = "")
        {
            string code = _enctyptionService.RSADecrypt(Verification);
            string email = _enctyptionService.RSADecrypt(ss_key);
            if (objCache["ClickRecord" + code] == null)
            {
                objCache.Insert("ClickRecord" + code, 1, null, DateTime.Now.AddDays(1), TimeSpan.Zero);
            }
            else
            {
                ViewBag.IsValid = false;
                return View();
            }
            if (objCache["UpdatePayPass" + email] == null || objCache["UpdatePayPass" + email].ToString() != code)
            {
                ViewBag.IsValid = false;
            }
            else
            {
                ViewBag.IsValid = true;
            }
            return View();
        }

        /// <summary>
        /// 修改交易密码成功页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UpdateSuccess()
        {
            return View();
        }

        /// <summary>
        /// 验证新的交易密码
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult PlyPassExist(string type, string StrPlyPass = "")
        {
            if (string.IsNullOrEmpty(StrPlyPass))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_TRANSACTION_PASSWORD_NOT_EMPTY") });//交易密码不能为空
            }
            YH_UserModel model = _LoginService.GetUserInfoById(base.UserID).Data;
            if (model == null)
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_USER_NOT_EXIST") });//用户不存在！
            }
            string payword = CodeHelper.GetMD5(StrPlyPass);
            if (type == "2")
            {
                if (payword.Equals(model.PayPassWord))
                {
                    return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_NOTCONSISTENTPWD") });//新密码不能与旧密码一致
                }
            }
            if (payword.Equals(model.PassWord))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_TRANSPASSWORD_NOT_EQUAL_LOGINPWD") });//交易密码不能与登录密码一致
            }
            return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_VALIDATION_SUCCESS") });//验证通过
        }

        #endregion

        #region 修改登陆密码操作



        /// <summary>
        /// 验证原始登录密码是否正确
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult LoginPassVerify(string pwd)
        {
            if (string.IsNullOrEmpty(pwd))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_PWDNOTEMPTY") });//密码不能为空
            }
            YH_UserModel model = _LoginService.GetUserInfoById(base.UserID).Data;
            string password = CodeHelper.GetMD5(pwd);
            if (!password.Equals(model.PassWord))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_ORIGINAL_PASSWORD_INCORRECT") });//原始密码不正确
            }
            return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_ORIGINAL_PASSWORD_CORRECT") });//原始密码正确
        }


        /// <summary>
        /// 验证新的登录密码是否与之前的相同
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult LoginPassExist(string StrPlyPass)
        {
            if (string.IsNullOrEmpty(StrPlyPass))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_PWDNOTEMPTY") });//密码不能为空
            }
            YH_UserModel model = _LoginService.GetUserInfoById(base.UserID).Data;
            string password = CodeHelper.GetMD5(StrPlyPass);
            if (password.Equals(model.PassWord))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_NOTCONSISTENTPWD") });//新密码不能与旧密码一致
            }
            if (password.Equals(model.PayPassWord))
            {
                return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_LOGINPWD_NOT_EQUAL_TRANSPASSWORD") });//登录密码不能与交易密码一致
            }
            return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_VALIDATION_SUCCESS") });//验证通过
        }

        /// <summary>
        /// 功能:修改登录密码页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UpdatePass()
        {

            return View();
        }



        /// <summary>
        /// 功能:修改登录密码方法
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">密码</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult UpdatePass(string verifyCode, string oldPassword, string newPassword)
        {
            UserMsg updateUserResult = new UserMsg()
            {
                flag = false,
                strMsg = CultureHelper.GetLangString("USERINFO_UPDATE_FAILURE")        //修改失败
            };
            bool isValid = VerifyCodeUtil.ValidCode(verifyCode, this.GetImageVerifyCodeCacheKey(EImageVerifyCodeType.ModifyLoginPwd));
            if (!isValid)
            {
                updateUserResult.strMsg = CultureHelper.GetLangString("LOGIN_GETPASSWORD_CORRECTCODE");//请输入正确的验证码
            }
            else
            {
                oldPassword = CodeHelper.GetMD5(oldPassword);
                newPassword = CodeHelper.GetMD5(newPassword);
                updateUserResult = new YH_UserInfo().UserAccountModifyPassword(this.UserID, oldPassword, newPassword);
                if (updateUserResult.flag)
                {
                    FormsAuthentication.SignOut();
                }
            }

            return Json(new
            {
                rs = updateUserResult.flag ? 1 : 0,
                msg = updateUserResult.strMsg
            });

        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="codeType">图片验证码类型 EImageVerifyCodeType</param>
        public void VerifyCode(EImageVerifyCodeType codeType = EImageVerifyCodeType.ModifyLoginPwd)
        {
            string cacheKey = this.GetImageVerifyCodeCacheKey(codeType);
            if (!string.IsNullOrEmpty(cacheKey))
            {
                VerifyCodeUtil.GenerateVerifyCode(cacheKey);
            }

        }


        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <param name="codeType">图片验证码类型</param>
        /// <returns></returns>
        public ActionResult ValidCode(string verifyCode, EImageVerifyCodeType codeType = EImageVerifyCodeType.ModifyLoginPwd)
        {
            bool isValid = VerifyCodeUtil.ValidCode(verifyCode, this.GetImageVerifyCodeCacheKey(codeType));
            return Json(isValid.ToString().ToLower(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取图片验证码缓存Key
        /// </summary>
        /// <param name="codeType">图片验证码类型</param>
        /// <returns>缓存Key</returns>
        private string GetImageVerifyCodeCacheKey(EImageVerifyCodeType codeType)
        {
            string cacheKey = string.Empty;
            switch (codeType)
            {
                case EImageVerifyCodeType.ModifyLoginPwd:
                    cacheKey = Const.CACHE_VALIDATECODE_FOR_Modify_LOGIN_PWD;
                    break;
                case EImageVerifyCodeType.UpdatePayPwd:
                    cacheKey = Const.CACHE_VALIDATECODE_FOR_Modify_PAY_PWD;
                    break;
            }
            return cacheKey;
        }

        #endregion

        #region 我的银行卡
        /// <summary>
        /// 绑定银行卡
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize]
        public ActionResult MyBankCard(string type = "")
        {
            List<UserBankModel> userModel = _UserBankAccountService.GetUserBank(base.UserID).Data;
            if (userModel.Count > 0 && type == "1")//重新绑定
            {
                ViewBag.Status = 1;
                //ViewData["Bank"] = _BankService.GetBankList().Data;
                ViewBag.ID = userModel[0].ID;
                ViewBag.BankId = userModel[0].BankID;
                return View();
            }
            if (userModel.Count == 0)
            {
                ViewBag.Status = 1;
                //ViewData["Bank"] = _BankService.GetBankList().Data;
                return View();
            }
            if (userModel.Count > 0)
            {
                ViewBag.Status = 0;//绑定过              
            }
            return View(userModel[0]);
        }

        /// <summary>
        /// 绑定银行卡
        /// </summary>
        /// <param name="newBankName">绑定银行卡名称</param>
        /// <param name="account">开户行账户</param>
        /// <param name="address">开户行地址</param>
        /// <param name="userAccount">开户人账号</param>
        /// <param name="id">原始自增ID</param>
        /// <param name="bankId">原始银行卡ID</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public JsonResult AddBankCard(string newBankName, string account, string address, string userAccount, string id = "", string bankId = "")
        {
            //如果用户之前未绑定银行卡,则直接添加
            UserBankModel addModel = new UserBankModel();
            addModel.BankID = 0;
            addModel.UserID = base.UserID;
            addModel.IsDefault = 0;
            addModel.BankAccount = account;
            addModel.BankSubbranch = newBankName;
            addModel.BankAddress = address;
            addModel.BankUserName = userAccount;
            addModel.IsUse = 1;
            addModel.CreateBy = base.Account;
            addModel.CreateDT = DateTime.Now;

            //如果用户之前绑定过银行卡
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(bankId))
            {
                UserBankModel oldmodel = _UserBankAccountService.GetYH_UserBankAccountById(int.Parse(id)).Data;//根据原始自增ID 获取模型
                if (oldmodel.BankSubbranch == newBankName)//用户现在绑定的银行卡和之前绑定的银行卡是同一个银行,则直接修改
                {
                    oldmodel.BankAccount = account;
                    oldmodel.BankAddress = address;
                    oldmodel.BankUserName = userAccount;
                    oldmodel.IsUse = 1;
                    if (_UserBankAccountService.UpdateModel(oldmodel).Data != 1)
                    {
                        return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_BIND_FAILURE") });//绑定失败
                    }
                    return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_BIND_SUCCESS") });//绑定成功
                }
                else//用户现在绑定的银行卡和之前绑定的银行卡不是同一个银行,则直接添加,并将之前的银行卡isuse设置为0
                {
                    oldmodel.IsUse = 0;
                    string status = _UserBankAccountService.Insert(addModel, oldmodel);
                    if (status == "1")
                    {
                        return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_BIND_SUCCESS") });//绑定成功
                    }
                    return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_BIND_FAILURE") });//绑定失败
                }

            }
            int result = _UserBankAccountService.Insert(addModel).Data.ID;
            if (result > 0)
            {
                return Json(new { rs = 1, msg = CultureHelper.GetLangString("USERINFO_BIND_SUCCESS") });//绑定成功
            }

            return Json(new { rs = 0, msg = CultureHelper.GetLangString("USERINFO_BIND_FAILURE") });//绑定失败
        }
        #endregion

        #region 收货地址

        /// <summary>
        /// 收货地址视图
        /// zhoub 20150716
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize]
        public ActionResult Address(int page = 1)
        {
            SearchUserAddressModel model = new SearchUserAddressModel();
            model.UserID = base.UserID;
            model.PagedIndex = page - 1;
            model.PagedSize = 4;
            var result = _userAddress.GetPagingUserAddress(model, CultureHelper.GetLanguageID());
            List<HKTHMall.Domain.Models.THArea_lang> thAreas = _thAreaService.GetTHAreaByParentID(CultureHelper.GetLanguageID(), 0).Data;
            List<UserAddress> userAddress = result.Data;
            dynamic pageView = new ExpandoObject();
            pageView.UserAddresss = userAddress;
            pageView.THAreas = thAreas;
            ViewBag.Page = page;
            ViewBag.Count = result.Data.TotalCount;
            return View(pageView);
        }

        /// <summary>
        /// 省份下拉框数据获取
        /// zhoub 20150717
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetTHAreaSelect(int parentID)
        {
            var result = _thAreaService.GetTHAreaByParentID(CultureHelper.GetLanguageID(), parentID).Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 收货地址保存
        /// zhoub 20150717
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult CreateAddress(UserAddress address)
        {
            ResultModel result = new ResultModel();
            address.UserID = base.UserID;
            if (Request.Params["userAddressId"] == "0")
            {
                address.Flag = 0;
                address.UserAddressId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                result = _userAddress.AddUserAddress(address);
            }
            else
            {
                address.UserAddressId = Convert.ToInt64(Request.Params["userAddressId"]);
                result = _userAddress.UpdateUserAddress(address);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 常用收货地址修改
        /// zhoub 20150720
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult UpdateUserAddressFlag()
        {
            UserAddress address = new UserAddress();
            address.UserAddressId = Convert.ToInt64(Request.Params["userAddressId"]);
            address.UserID = Convert.ToInt64(Request.Params["txtUserId"]);
            address.Flag = 1;
            var result = _userAddress.UpdateUserAddressFlag(address);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 收货地址删除
        /// zhoub 20150717
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult DelUserAddress()
        {
            ResultModel model = new ResultModel();
            if (!string.IsNullOrEmpty(Request.Params["userAddressId"]))
            {
                model = _userAddress.DelUserAddress(Convert.ToInt64(Request.Params["userAddressId"]));
                model.Messages = new List<string> { CultureHelper.GetLangString("USERINFO_OP_SUCCESS") };//操作成功.
            }
            else
            {
                model.IsValid = false;
                model.Messages = new List<string> { CultureHelper.GetLangString("USERINFO_OP_FAILURE") };//操作失败,收货地址ID异常.
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据ID获取收货地址信息
        /// zhoub 20150717
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult GetUserAddressById()
        {
            var result = _userAddress.GetUserAddressById(Convert.ToInt64(Request.Params["userAddressId"])).Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}