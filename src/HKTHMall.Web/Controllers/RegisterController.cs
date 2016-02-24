using HKSJ.Common;

using HKTHMall.Core;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services;
using HKTHMall.Services.WebLogin;
using HKTHMall.Web.Account;
using HKTHMall.Web.Common;
using HKTHMall.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

using HKTHMall.Services.Sys;
using HKTHMall.Core.Security;
using System.Text;
using HKTHMall.Services.Common.MultiLangKeys;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using HKTHMall.Services.YHUser;
using Newtonsoft.Json;

namespace HKTHMall.Web.Controllers
{
    public class RegisterController : BaseController
    {
        private readonly ILoginService _LoginService;
        private readonly IParameterSetService _ParameterSetService;
        private readonly IEncryptionService _enctyptionService;
        SendPhoneMsg sendMsg = new SendPhoneMsg();
        private readonly IYH_UserService _YH_UserService;
        private const string emailReg = "^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$";
        public RegisterController(ILoginService loginService, IParameterSetService parameterSetService, IEncryptionService enctyptionService, IYH_UserService YH_UserService)
        {
            _LoginService = loginService;
            _ParameterSetService = parameterSetService;
            _enctyptionService = enctyptionService;
            _YH_UserService = YH_UserService;
        }
        // GET: Register
        /// <summary>
        /// 注册页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //_LoginService.LoginThree("523278857828617", "bbbb", 3);


            string Inviter = _ParameterSetService.GetParametePValueById(1856732830).Data;//获取默认推荐人
            ViewBag.Inviter = Inviter;

            return View();
        }
        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode(bool isRegister=true)
        {
            string code = CreateValidateCode(4);
           // MemCacheFactory.GetCurrentMemCache().AddCache(Const.Cache_ValidateCode, code);
            if (isRegister)
            {
                Session["yzmCode"] = code;
            }
            else
            {
                Session["yzmCode_Update"] = code;
            }
           
            byte[] bytes = CreateValidateGraphic(code);
            System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(bytes));
            image.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            Response.End();
            return View();
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CheckVaildCode(string id, bool isRegister=true)
        {
            //string code = MemCacheFactory.GetCurrentMemCache().GetCache<string>(Const.Cache_ValidateCode);
            string code = "";
            if (isRegister)
            {
               code = Session["yzmCode"] == null ? "" : Session["yzmCode"].ToString();
            }
            else
            {
                code = Session["yzmCode_Update"] == null ? "" : Session["yzmCode_Update"].ToString();
            }
            if (code == id)
            {
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        //// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public string CreateValidateCode(int length)
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
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>
        private byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 24);
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
                g.DrawString(validateCode, font, brush, 3, 2);
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
        /// 验证邮跳转后页面
        /// </summary>
        /// <param name="email"></param>
        /// <param name="Verification"></param>
        /// <returns></returns>
        public ActionResult VerificationEmail(string email, string Verification)
        {
            string code = _enctyptionService.RSADecrypt(Verification);
            string ema = _enctyptionService.RSADecrypt(email);
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache.Get("RegGetCodeTime" + ema) == code)
            {

            }
            return View();
        }
        public ActionResult ValidationEmail(string loginEmail = "494684778@qq.com",bool isLogin=false)
        {
            ViewBag.email = loginEmail;
            int lastlength = loginEmail.IndexOf('@');
            string str = loginEmail.Substring(1, lastlength - 2);
            string fuhao = string.Empty.PadLeft(str.Length, '*');
            string aa = loginEmail.Replace(str, fuhao);
            string emailUrl = "";
            #region 根据邮箱后缀找到其官网
            switch (loginEmail.Substring(lastlength + 1))
            {
                case "qq.com":
                    emailUrl = "http://mail.qq.com";
                    break;
                case "gmail.com":
                    emailUrl = "http://mail.google.com";
                    break;
                case "sina.com":
                    emailUrl = "http://mail.sina.com.cn";
                    break;
                case "163.com":
                    emailUrl = "http://mail.163.com";
                    break;
                case "126.com":
                    emailUrl = "http://mail.126.com";
                    break;
                case "yeah.net":
                    emailUrl = "http://www.yeah.net/";
                    break;
                case "sohu.com":
                    emailUrl = "http://mail.sohu.com/";
                    break;
                case "tom.com":
                    emailUrl = "http://mail.tom.com/";
                    break;
                case "sogou.com":
                    emailUrl = "http://mail.sogou.com/";
                    break;
                case "139.com":
                    emailUrl = "http://mail.10086.cn/";
                    break;
                case "hotmail.com":
                    emailUrl = "http://www.hotmail.com";
                    break;
                case "live.com":
                case "live.cn":
                case "live.com.cn":
                    emailUrl = "http://login.live.com.cn";
                    break;
                case "189.com":
                    emailUrl = "http://webmail16.189.cn/webmail/";
                    break;
                case "yahoo.com.cn":
                case "yahoo.cn":
                    emailUrl = "http://mail.cn.yahoo.com/";
                    break;
                case "eyou.com":
                    emailUrl = "http://www.eyou.com/";
                    break;
                case "21cn.com":
                    emailUrl = "http://mail.21cn.com/";
                    break;
                case "188.com":
                    emailUrl = "http://www.188.com/";
                    break;
                case "foxmail.coom":
                    emailUrl = "http://www.foxmail.com";
                    break;
            }
            #endregion
            ViewBag.loginEmail = aa;
            ViewBag.emailUrl = emailUrl;
            ViewBag.isLogin = isLogin;
            return View();
        }
        /// <summary>
        /// 注册方法
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="pwd">密码</param>
        /// <param name="code">验证码</param>
        /// <param name="email"></param>
        /// <param name="inviatePhone">推荐人手机号</param>
        /// <returns></returns>       
        public JsonResult RegisterUser(string pwd, string email)
        {

            //注册用户信息
            YH_UserModel _user = null;
            //接收返回信息
            BackMessage msg = new BackMessage();
            #region 验证

            DataMSg mailMsg = CheckMail(email);//验证邮箱
            if (!mailMsg.IsTrue)
            {
                msg.status = 0;
                msg.message = mailMsg.Msg;
                return Json(msg);
            }

            if ("" == pwd || new Regex(@"/[^\x00-\xff]|\s/").IsMatch(pwd))
            {
                msg.status = 0;
                msg.message = CultureHelper.GetLangString("LOGIN_PWD_FORMAT");
                return Json(msg);
            }
            #endregion
            #region 发送验证邮箱
            _enctyptionService.RSADecrypt("");
            _enctyptionService.RSAEncrypt("");
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            string cod = CodeHelper.GetRandomNumber(6);
            objCache.Insert("RegGetCodeTime" + email, cod, null, DateTime.Now.AddDays(1), TimeSpan.Zero);
            string key = _enctyptionService.RSAEncrypt(email);
            string link = Request.Url.Authority + "/Register/ValidateEmailResult?rekey=" + key + "&Verification=" + _enctyptionService.RSAEncrypt(cod);
            //string emailContent = CultureHelper.GetLangString("LOGIN_EMAIL_VERIFICATION_CONTENT");
            link = "http://" + link;
            //emailContent = string.Format(emailContent, email, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), email, "<a href='" + link + "'>" + link + "</a>", DateTime.Now.ToString("yyyy-MM-dd"));

            #endregion


            //加密密码
            string psw_md5 = CodeHelper.GetMD5(pwd);

            msg = _LoginService.Register(pwd, psw_md5, email, out _user);
            if (msg.status == 1)
            {
                msg.data = link;
                string emailContent = string.Empty;
                //Mail.sendMail(email, CultureHelper.GetLangString("LOGIN_EMAIL_VERIFICATION"), emailContent);
                string MailContent = MailTempHelper.GetMailTemp(MailTempType.ActiveAccount);
                string style = MailTempHelper.GetMailTemp(MailTempType.MailStyle);
                emailContent = string.Format(MailContent, CultureHelper.GetLangString("HOME_FOOTER_FENXIANGTITLE"), ConfigHelper.GetConfigString("domain"), email, link, style);
                Mail.sendMail(email, CultureHelper.GetLangString("LOGIN_EMAIL_VERIFICATION"), emailContent);
                UserModel cookieModel = new UserModel();
                cookieModel.UserID = _user.UserID;
                cookieModel.Account = _user.Account;
                cookieModel.Phone = _user.Phone;
                cookieModel.NickName = _user.NickName;
                cookieModel.UserType = int.Parse(_user.UserType.ToString());
                cookieModel.Email = _user.Email;
                cookieModel.ValidateEmail = email + "|" + key;


                bool flag = MemCacheFactory.GetCurrentMemCache().AddCache<string>(Const.Cache_ValidateEmail, JsonConvert.SerializeObject(cookieModel), 120);

            }
            return Json(msg);
        }

        /// <summary>
        /// 激活邮箱时发送邮件
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult SendValideEmail(string email)
        {
            var result = _LoginService.GetUserInfoByEmail(email);
            if (result.Data != null)
            {

                #region 发送验证邮箱
                try
                {
                    _enctyptionService.RSADecrypt("");
                    _enctyptionService.RSAEncrypt("");
                    System.Web.Caching.Cache objCache = HttpRuntime.Cache;                
                    string cod = CodeHelper.GetRandomNumber(6);
                    objCache.Insert("RegGetCodeTime" + email, cod, null, DateTime.Now.AddDays(1), TimeSpan.Zero);
                    string key = _enctyptionService.RSAEncrypt(email);
                    string link = Request.Url.Authority + "/Register/ValidateEmailResult?rekey=" + key + "&Verification=" + _enctyptionService.RSAEncrypt(cod);
                    string emailContent = CultureHelper.GetLangString("LOGIN_EMAIL_VERIFICATION_CONTENT");
                    link = "http://" + link;
                    emailContent = string.Format(emailContent, email, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), email, "<a href='" + link + "'>" + link + "</a>", DateTime.Now.ToString("yyyy-MM-dd"));
                    bool flag = Mail.sendMail(email, CultureHelper.GetLangString("HOME_LOGIN_APPLYRESETPWDEMAIL"), emailContent);
                    if (flag)
                    {
                        //注册用户信息
                        UserModel cookieModel = new UserModel();
                        cookieModel.UserID = result.Data.UserID;
                        cookieModel.Account = result.Data.Account;
                        cookieModel.Phone = result.Data.Phone;
                        cookieModel.NickName = result.Data.NickName;
                        cookieModel.UserType = result.Data.UserType;
                        cookieModel.Email = result.Data.Email;
                        cookieModel.ValidateEmail = email + "|" + key;


                        MemCacheFactory.GetCurrentMemCache().AddCache<string>(Const.Cache_ValidateEmail, JsonConvert.SerializeObject(cookieModel), 120);
                        return Json(new { status = 1, message = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_EMAILSENDSUCCESS") }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 0, message = CultureHelper.GetLangString("ACCOUNT_USERINFO_INDEX_EMAILSENDFAILURE") }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json("邮件发送异常", JsonRequestBehavior.AllowGet);
                }
                #endregion
            }
            else
            {
                return Json(new { status = 0, message = CultureHelper.GetLangString("ACCOUNT_EMAIL_NOTREGISTER") }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 验证邮箱是否已经绑定过
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public JsonResult CheckMailValid(string email)
        {
            DataMSg Msg = CheckMail(email);
            return Json(Msg);
        }

        /// <summary>
        /// 验证邮件合法性
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public DataMSg CheckMail(string email, bool activate=false)
        {
            DataMSg dataMsg = new DataMSg();
            if (string.IsNullOrEmpty(email))
            {
                dataMsg.IsTrue = false;
                dataMsg.Msg = CultureHelper.GetLangString("REGISTER_EMAIL_MUST");
                return dataMsg;
            }
            if (!new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").IsMatch(email))
            {
                dataMsg.IsTrue = false;
                dataMsg.Msg = CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_EMAILERROR");//邮箱格式错误
                return dataMsg;
            }
            dataMsg.IsTrue = true;


            YH_UserModel model = _LoginService.GetUserInfoByEmail(email).Data;
            if (model != null)
            {
                if (activate)
                {
                    dataMsg.IsTrue = true;
                    dataMsg.Msg = CultureHelper.GetLangString("REGISTER_EMAIL_VALIDATION_SUCCESS");//邮箱通过
                }
                else
                {
                    dataMsg.IsTrue = false;
                    dataMsg.Msg = CultureHelper.GetLangString("REGISTER_EMAIL_HAS_BEEN_BOUND_TO_CHANGE");//邮箱已经被绑定,请更改
                }
            }
            else
            {
                dataMsg.Msg = CultureHelper.GetLangString("REGISTER_EMAIL_VALIDATION_SUCCESS");//邮箱通过
            }
            return dataMsg;
        }

        // GET: Register
        /// <summary>
        /// 注册成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Success()
        {
            return View();
        }
        /// <summary>
        /// 激活邮箱结果
        /// </summary>
        /// <param name="rekey"></param>
        /// <returns></returns>
        public ActionResult ValidateEmailResult(string rekey = "")
        {
            ViewBag.result = "false";
            string cacheValue = MemCacheFactory.GetCurrentMemCache().GetCache<string>(Const.Cache_ValidateEmail);
            if (rekey != "" && cacheValue != null)
            {
                UserModel user = JsonConvert.DeserializeObject<UserModel>(cacheValue);
                string email = user.ValidateEmail.Split('|')[0];
                string cachekey = user.ValidateEmail.Split('|')[1];
                if (rekey == cachekey.Replace("+", " "))
                {
                    var result = _YH_UserService.ActiveEmail(email);
                    if (result.IsValid)
                    {
                        SetAuthCookie(user);
                        MemCacheFactory.GetCurrentMemCache().ClearCache(Const.Cache_ValidateEmail);
                        ViewBag.result = "true";
                    }

                }
            }
            return View();
        }
        public ActionResult Fall()
        {
            return View();
        }
   
    }
}