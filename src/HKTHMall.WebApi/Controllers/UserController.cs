using BrCms.Framework.Infrastructure;
using FluentValidation;
using HKSJ.Common;
using HKSJ.MidMessage.Protocol;
using HKSJ.MidMessage.Services;
using HKTHMall.Core;
using HKTHMall.Core.Security;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Domain.WebModel.Models.Search;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Services;
using HKTHMall.Services.Common;
using HKTHMall.Services.LoginLog;
using HKTHMall.Services.Products;
using HKTHMall.Services.Sys;
using HKTHMall.Services.Users;
using HKTHMall.Services.WebLogin;
using HKTHMall.Services.YHUser;
using HKTHMall.WebApi.Common;
using HKTHMall.WebApi.Models;
using HKTHMall.WebApi.Models.Request;
using HKTHMall.WebApi.Models.Result;
using HKTHMall.WebApi.VisitingCard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Autofac;
using System.Web.Script.Serialization;
using System.Text;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using HKTHMall.Services.WebLogin.Impl;
using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Services.Common.MultiLangKeys;
//using HKTHMall.Domain.Entities;



namespace HKTHMall.WebApi.Controllers
{
    /// <summary>
    /// 3用户相关接口
    /// </summary>
    public class UserController : BaseController
    {
        /// <summary>
        /// 登录服务类
        /// </summary>
        private readonly ILoginService _LoginService;
        private readonly IYH_UserLoginLogService _YH_userLoginLogService;
        private readonly IEncryptionService enctyptionService;
        private readonly IYH_UserService _IYH_UserService;
        private readonly IMyCollectionService myCollectionService;
        private readonly IFeedbackService feedbackService;
        private readonly IUserAddressService userAddressService;
        private readonly IProductSearchListService productSearchListService;
        private readonly IFavoritesService favoritesService;
        private readonly IYh_groupmarkService yh_groupmarkService;
        private readonly IParameterSetService _parameterSetService;
        private readonly IYH_PasswordErrorService _passwordErrorService;

        public static int ForTest = Convert.ToInt32(ConfigurationManager.AppSettings["ForTest"]);


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="acDepartmentService"></param>
        public UserController(IProductSearchListService productSearchListService, IFavoritesService favoritesService, IUserAddressService userAddressService, ILoginService loginService,
            IYH_UserLoginLogService loginlogService, IParameterSetService parameterService, IYH_PasswordErrorService passwordService,
            IEncryptionService enctyptionService, IYH_UserService _IYH_UserService, IMyCollectionService myCollectionService, IFeedbackService feedbackService,
             IYh_groupmarkService yh_groupmarkService)
        {
            this.userAddressService = userAddressService;
            _LoginService = loginService;
            _YH_userLoginLogService = loginlogService;
            this._IYH_UserService = _IYH_UserService;
            this.enctyptionService = enctyptionService;
            this.myCollectionService = myCollectionService;
            this.feedbackService = feedbackService;
            this.productSearchListService = productSearchListService;
            this.favoritesService = favoritesService;
            this.yh_groupmarkService = yh_groupmarkService;
            _parameterSetService = parameterService;
            _passwordErrorService = passwordService;
        }

        #region 3.1.登录验证(马锋)
        /// <summary>
        /// 3.1.登录验证(马锋)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult LoginValidate(RequestLoginValidate request)
        {
            ApiResultModel result = new ApiResultModel();
            result.rs = new { };
            result.flag = 0;
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(result);
            }
            //输入参数验证
            //IValidator validator = new LoginValidator(request.lang);
            //var results = validator.Validate(request);
            //if (!results.IsValid)
            //{
            //    result.msg = results.Errors[0].ErrorMessage;
            //    return Ok(result);
            //}  
            request.account = enctyptionService.RSADecrypt(request.account);
            request.password = enctyptionService.RSADecrypt(request.password);
            if (Settings.IsEnableEM)
            {
                #region 在账号系统登录
                RequestUserLoginOn login = new RequestUserLoginOn();
                login.acc = request.account;
                login.dev = request.registerSource;
                login.pwd = request.password;
                login.mode = 1;
                login.sys_id = Convert.ToInt32(ConfigurationManager.AppSettings["Em_System_Id"]);
                ResponseUserItem userSys = EmMethodManage.EmLoginInstance.MsgLogOnAccReq(login);
                if (userSys.Status == 1048777)
                {
                    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_LOGIN_ACCOUNT_WRONG", request.lang);//登录账号错误！请重新输入!

                }
                else if (userSys.Status == 1048784)
                {
                    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_ACCOUNT_FREEZE", request.lang);//该账号已被冻结！请联系系统管理员解冻

                }
                else if (userSys.Status == 1048778)
                {
                    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_LOGIN_PWD_WRONGTRYAGAIN", request.lang) + userSys.rest_cnt + CultureHelper.GetAPPLangSgring("LOGIN_TIMES", request.lang);//密码错误,还可以再输入  //次!

                }
                else if (userSys.Status == 1048781 || userSys.Status == 1048780)
                {
                    #region 账号锁定

                    if (Convert.ToInt32(userSys.life / 60) <= 0)
                    {
                        result.msg = CultureHelper.GetAPPLangSgring("LOGIN_ACCOUNT_LOCKED_PLS_ASK_UNLOCK", request.lang);//账号已被锁定,请于1分钟解锁后再试!
                    }
                    else
                    {
                        //账号已被锁定,请于     //分钟解锁后再试!
                        result.msg = CultureHelper.GetAPPLangSgring("LOGIN_ACCOUNT_LOCKED_PLS", request.lang) + string.Format(CultureHelper.GetAPPLangSgring("LOGIN_ACCOUNT_LOCKED_PLS_MINUTES_TRY_AGAIN", request.lang), Convert.ToInt32(userSys.life / 60));
                    }

                    #endregion
                }
                else if (userSys.Status == 2 || userSys.Status == 65538)   //2 65538 //查本地
                {
                    // result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_SYSTEM_ERROR", request.lang);//账号系统错误！
                    return Ok(LocationLogin(request.account, request.password, request.lang));
                }
                else if (string.IsNullOrEmpty(userSys.phone))
                {
                    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_PHONE_NOTBIND", request.lang);//手机号未绑定,请先通过其他惠信的子网站进行手机绑定后再登陆！

                }
                else if (userSys.Status == 0)
                {
                    YH_UserModel user = new YH_UserModel();
                    user = _LoginService.GetUserInfoById(userSys.uid).Data;
                    if (user != null)
                    {
                        #region 更新本地库
                        user.Account = userSys.acc;
                        user.Email = userSys.email;
                        user.NickName = userSys.nick;
                        user.HeadImageUrl = userSys.img_url;
                        user.Sex = (byte)userSys.sex;
                        user.Phone = userSys.phone;
                        user.RealName = userSys.rname;
                        user.ActivePhone = (byte)userSys.st_ph;
                        user.ActiveEmail = (byte)userSys.st_mail;
                        var up = _LoginService.Update(user);
                        #endregion
                        #region 成功后返回信息
                        result.flag = 1;
                        result.rs = FormatModel(user);
                        result.msg = CultureHelper.GetAPPLangSgring("LOGIN_LOGIN_SUCCESS", request.lang);//登陆成功
                        #region 创建群
                        Task.Run(() =>
                        {
                            CreateGroupMark(user);
                        });
                        
                        #endregion
                        #endregion
                    }
                    else
                    {
                        var _msg = _LoginService.AddUserInfo(userSys, request.password, out user);
                        if (_msg.status != 1) //添加失败
                        {
                            result.flag = 0;
                            result.msg = _msg.message;
                            return Ok(result);
                        }
                        return Ok(commonLogin(user, request.lang));
                    }
                #endregion
                }
            }
            else
            {//未启动中间件后普通登录
                return Ok(LocationLogin(request.account, request.password, request.lang));
            }
            return Ok(result);
        }
        /// <summary>
        /// 本地登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="acc"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private ApiResultModel LocationLogin(string acc, string pwd, int lang)
        {
            YH_UserModel model = new YH_UserModel();
            ApiResultModel result = new ApiResultModel();
            if (!string.IsNullOrEmpty(acc) && acc.Contains("@"))//邮箱登陆
            {
                model = _LoginService.GetUserInfoByEmail(acc).Data;
            }
            else if (acc[0] >= 'A' && acc[0] <= 'z')  //账号登录
            {
                model = _LoginService.GetUserInfoByAccount(acc).Data;
            }
            else//手机号登陆
            {
                model = _LoginService.GetUserInfoByPhone(acc).Data;
            }

            if (null != model)
            {
                if (model.IsLock == 1)
                {
                    result.flag = 0;
                    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_ACCOUNT_LOCKED", lang);//您输入的账号已经被锁定!
                }
                else if (model.IsDelete == 1)
                {
                    result.flag = 0;
                    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_ACCOUNT_NOT_EXIST", lang);//您输入的账号不存在,请核对后重新输入!
                }
                else
                {
                    return judgePwdError(model, pwd, lang);
                }
            }
            else
            {
                result.flag = 0;
                result.msg = CultureHelper.GetAPPLangSgring("LOGIN_ACCOUNT_NOT_EXIST", lang);//您输入的账号不存在,请核对后重新输入!
            }
            return result;
        }
        /// <summary>
        /// 密码输入错误后的处理方法
        /// </summary>
        /// <param name="model"></param>
        /// <param name="acc"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public ApiResultModel judgePwdError(YH_UserModel model, string passwd, int lang)
        {
            ApiResultModel result = new ApiResultModel();
            int loginNums = int.Parse(_parameterSetService.GetParametePValueById(1215896046).Data);//登陆错误次数
            int loginTimes = int.Parse(_parameterSetService.GetParametePValueById(1215896045).Data);//锁定时间

            List<YH_PasswordErrorModel> pwdlist = _passwordErrorService.GetPasswordErrorInfo(model.UserID, 1).Data;
            YH_PasswordErrorModel pwd = null;
            if (pwdlist.Count > 0)
            {
                pwd = (YH_PasswordErrorModel)pwdlist[0];
            }
            int verNums = pwd == null ? 0 : pwd.FailVerifyTimes;
            if (null != pwd)
            {
                var min = (DateTime.Now - pwd.VerifyTime).TotalMinutes;
                if (min < loginTimes)  //当天两小时内次数大于等于5
                {
                    if (verNums >= loginNums)
                    {
                        result.flag = 0;
                        result.msg = CultureHelper.GetAPPLangSgring("LOGIN_ACCOUNT_LOCKEDAFTERTWO", lang);//账号已被锁定-请在两个小时以后再登录!
                        return result;
                    }
                }
            }
            if (passwd != model.PassWord) //密码输入错误后 判断输入密码错误次数大于等于0并且小于5 就新增或修改一条密码输入错误表数据
            {
                if (pwd == null)
                {
                    pwd = new YH_PasswordErrorModel()
                    {
                        FailVerifyTimes = 1,
                        VerifyTime = DateTime.Now,
                        Account = model.Account,
                        UserID = model.UserID,
                        PassWordType = 1
                    };
                    _passwordErrorService.AddError(pwd);
                }
                else
                {
                    double min = (DateTime.Now - pwd.VerifyTime).TotalMinutes;
                    pwd.Account = model.Account;
                    pwd.FailVerifyTimes = min > loginTimes ? 1 : pwd.FailVerifyTimes + 1;
                    pwd.VerifyTime = DateTime.Now;
                    _passwordErrorService.Update(pwd);
                }

                //"零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" 
                string[] f = { CultureHelper.GetAPPLangSgring("LOGIN_NUM_0", lang), CultureHelper.GetAPPLangSgring("LOGIN_NUM_1", lang), CultureHelper.GetAPPLangSgring("LOGIN_NUM_2", lang), CultureHelper.GetAPPLangSgring("LOGIN_NUM_3", lang), CultureHelper.GetAPPLangSgring("LOGIN_NUM_4", lang), CultureHelper.GetAPPLangSgring("LOGIN_NUM_5", lang), CultureHelper.GetAPPLangSgring("LOGIN_NUM_6", lang), CultureHelper.GetAPPLangSgring("LOGIN_NUM_7", lang), CultureHelper.GetAPPLangSgring("LOGIN_NUM_8", lang), CultureHelper.GetAPPLangSgring("LOGIN_NUM_9", lang), CultureHelper.GetAPPLangSgring("LOGIN_NUM_10", lang) };
                // ModelState.AddModelError("acc", "密码输入错误" + (pwd.FailVerifyTimes < 10 ? f[pwd.FailVerifyTimes] : "多") + "次,请重新输入!");
                result.flag = 0;
                //密码输入错误    //多     //次,请重新输入!
                result.msg = CultureHelper.GetAPPLangSgring("LOGIN_LOGIN_PWD_WRONGTRYAGAIN", lang) + (loginNums - pwd.FailVerifyTimes) + CultureHelper.GetAPPLangSgring("LOGIN_TIMES", lang);//密码错误,还可以再输入  //次!

                //result.msg = CultureHelper.GetAPPLangSgring("LOGIN_LOGIN_PWD_WRONG", lang) + (pwd.FailVerifyTimes < 10 ? f[pwd.FailVerifyTimes] : CultureHelper.GetAPPLangSgring("LOGIN_MUCH", lang)) + CultureHelper.GetAPPLangSgring("LOGIN_TIME_INPUT_AGAIN", lang);
                return result;
                // return View(m);
            }
            else
            {
                if (pwd != null)
                {
                    pwd.VerifyTime = DateTime.Now;
                    pwd.FailVerifyTimes = 0;
                    _passwordErrorService.Update(pwd);
                }
                return commonLogin(model, lang);
            }
        }
        /// <summary>
        /// 本地数据库登录后修改用户登录数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="acc"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public ApiResultModel commonLogin(YH_UserModel model, int lang)
        {
            ApiResultModel result = new ApiResultModel();
            //新增用户登录日志 
            YH_UserLoginLogModel user = new YH_UserLoginLogModel
            {
                UserID = model.UserID,
                LoginSource = 1,
                IP = Util.KeFu_GetIP(),
                LoginTime = DateTime.Now
            };
            int res = _YH_userLoginLogService.AddYH_UserLogin(user).Data.ID;
            if (res == 0)
            {
                result.flag = 0;
                result.msg = CultureHelper.GetAPPLangSgring("LOGIN_ADD_LOGGING_FAILURE", lang);//新增用户登录日志失败!
                return result;
            }

            result.rs = FormatModel(model); ;
            result.msg = CultureHelper.GetAPPLangSgring("LOGIN_LOGIN_SUCCESS", lang);//登录成功
            result.flag = 1;
            return result;
        }

        private ResponseLoginValidate FormatModel(YH_UserModel user)
        {
            ResponseLoginValidate response = new ResponseLoginValidate();
            response.account = user.Account;
            response.sex = (int)user.Sex;
            response.userId = user.UserID;
            response.nickName = user.NickName;
            response.phone = user.Phone;
            response.imageUrl = string.IsNullOrEmpty(user.HeadImageUrl) ? "" : ConfigurationManager.AppSettings["ImageHeader"].ToString() + user.HeadImageUrl;
            response.realName = user.RealName;
            response.email = user.Email;
            response.address = user.DetailsAddress;
            response.version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (!string.IsNullOrEmpty(user.OrcodeUrl))
            {
                response.orcodeUrl = HKSJ.Common.ConfigHelper.GetConfigString("ImagePath") + user.OrcodeUrl;
            }
            ResultModel userrm = _IYH_UserService.GetUserInfoByUserId(user.ReferrerID.Value);
            if (userrm != null && userrm.Flag == 1)
            {
                List<HKTHMall.Domain.Models.YHUser.YH_UserModel> modeluser = userrm.Data as List<HKTHMall.Domain.Models.YHUser.YH_UserModel>;
                if (modeluser != null && modeluser.Count() > 0)
                {
                    response.referrer = string.IsNullOrEmpty(modeluser.First().RealName) ? (string.IsNullOrEmpty(modeluser.First().NickName) ? modeluser.First().Account : modeluser.First().NickName) : modeluser.First().RealName;
                    response.referrerPhone = modeluser.First().Phone;
                }
            }
            response.isPayPassWord = string.IsNullOrEmpty(user.PayPassWord) ? "0" : "1";
            return response;
        }
        #endregion

        #region 3.2.获取验证码（马锋）
        /// <summary>
        /// 3.2.获取验证码（马锋）
        /// 注册时:
        /// </summary>
        /// <param name="phone">电话号码（RSA加密）</param>
        /// <param name="registerSource">1-网站 2-android 3-ios</param>
        /// <param name="type">1:注册验证码,2:找回密码验证码, 3:交易密码</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SMSValidate(RequestSMSValidate request)
        {
            ApiResultModel result = new ApiResultModel();
            result.flag = 0;
            result.rs = new ResponseSMSValidate();
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(result);
            }
            request.phone = enctyptionService.RSADecrypt(request.phone);
            IValidator validator = new SMSValidate(request.lang);
            var results = validator.Validate(request);
            if (!results.IsValid)
            {
                result.msg = results.Errors[0].ErrorMessage;
                return Ok(result);
            }
            YH_UserModel user = _LoginService.GetUserInfoByPhone(request.phone).Data;
            #region 注册获取短信
            if (request.type == 1)
            {
                if (user != null)//该手机号码已注册
                {
                    result.flag = 2;
                    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_HASREGISTERED", request.lang) + " " + CultureHelper.GetAPPLangSgring("HOME_INDEX_LOGIN", request.lang); //该手机号码已注册,请登录
                }
                else
                {
                    RequestPhone BindNewPhone = new RequestPhone();
                    BindNewPhone.phone = request.phone;
                    BindNewPhone.life = int.Parse(ConfigurationManager.AppSettings["MessageTimeOut"].ToString());
                    BindNewPhone.sys_id = Settings.EmSystemId;
                    BindNewPhone.mac = Settings.GetMacAddress();            //获取本机mac地址
                    BindNewPhone.ip = Settings.GetIPAddress();              //获取ip地址
                    BindNewPhone.dev = request.registerSource;
                    var result11 = HKSJ.MidMessage.Services.EmMethodManage.EmLoginInstance.MsgRegAccGetCodeReq(BindNewPhone);
                    if (result11.isOK)
                    {
                        //ResponseSMSValidate rs = new ResponseSMSValidate();
                        //rs.verification = 0;
                        result.flag = 1;
                        //rs.verification = 666666;
                        //result.rs = rs;
                        //我们已经给您  USER_PHONE_ALREADY    发送了验证短信 SEND_CODE_NOTE 
                        result.msg = CultureHelper.GetAPPLangSgring("USER_PHONE_ALREADY", request.lang) + request.phone.Substring(0, 2) + "****" + request.phone.Substring(request.phone.Length - 4) + CultureHelper.GetAPPLangSgring("SEND_CODE_NOTE", request.lang);
                    }
                    else
                    {
                        if (result11.Status == 1048820)
                        {
                            result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_MOBILEISBEEMPTYORCORRECTERRO", request.lang);//手机号码格式错误,请重新输入
                        }
                        else
                        {
                            result.msg = CultureHelper.GetIMLangString(result11.Status, request.lang);
                        }
                    }
                }
            }
            #endregion
            #region 找回密码获取短信
            else if (request.type == 2)
            {
                if (user == null)
                {
                    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_USERNOTEXIST", request.lang);//用户不存在
                    return Ok(result);
                }
                else
                {
                    ReqQueryUserInfo userInfo = new ReqQueryUserInfo();
                    userInfo.type = 2;
                    userInfo.whole = 1;
                    userInfo.value = request.phone;
                    userInfo.recnum = 10;
                    userInfo.sys_id = Settings.EmSystemId;
                    userInfo.dev = request.registerSource;
                    var uModel = EmMethodManage.EmFindUpdateInstance.MsgQueryUserInfoReq(userInfo);
                    if (uModel.data.Count == 0)
                    {
                        result.msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_USERNOTEXIST", request.lang);//用户不存在
                        return Ok(result);
                    }
                    else
                    {
                        RequestPhone requestPhone = new RequestPhone();
                        //业务系统编号
                        requestPhone.sys_id = Settings.EmSystemId;
                        //短信失效生命周期
                        requestPhone.life = int.Parse(ConfigurationManager.AppSettings["MessageTimeOut"].ToString());
                        requestPhone.acc = uModel.data[0].acc;
                        requestPhone.dev = request.registerSource;
                        var rsCode = EmMethodManage.EmLoginInstance.MsgResetPwdSendCodeReq(requestPhone);

                        if (rsCode.isOK)
                        {
                            result.flag = 1;
                            string phone = request.phone.Substring(0, 2) + "****" + request.phone.Substring(6, 4);
                            //我们已经给您  USER_PHONE_ALREADY    发送了验证短信 SEND_CODE_NOTE 
                            result.msg = CultureHelper.GetAPPLangSgring("USER_PHONE_ALREADY", request.lang) + phone + CultureHelper.GetAPPLangSgring("SEND_CODE_NOTE", request.lang);

                        }
                        else
                        {
                            result.msg = rsCode.ErrorMsg;
                        }
                    }
                    //int verification = 0;
                    //ToolUtil.GenForInt(100000, 999999, ref verification);
                    //verification = 666666;
                    //string message = CultureHelper.GetAPPLangSgring(string.Format("SMS_CLIENTCONTENT", verification), request.lang);
                    //ResultHelper sms = SMSCommon.QTSubmitSMS(1, request.phone, verification.ToString(), message, request.type);
                    ////ResponseSMSValidate rs = new ResponseSMSValidate();
                    ////rs.verification = 0;
                    //if (sms.Code == 1)
                    //{
                    //    result.flag = 1;
                    //    //rs.verification = 0;
                    //    //result.rs = rs;
                    //    result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_SENDSUCCESS", request.lang);//发送成功  
                    //    //HKSJ.Common.DataCache.SetCache(request.phone, verification, DateTime.Now.AddMinutes(15), TimeSpan.Zero);
                    //}
                    //else
                    //{
                    //    result.flag = 0;
                    //    result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_SENDFAILURE", request.lang);//发送失败                                        
                    //}
                }
            }
            #endregion
            #region 交易密码获取短信
            else if (request.type == 3)
            {
                if (user == null)
                {
                    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_USERNOTEXIST", request.lang);//用户不存在
                    return Ok(result);
                }
                else
                {
                    RequestCommSms reqSMS = new RequestCommSms
                    {
                        phone = request.phone,
                        life = int.Parse(ConfigurationManager.AppSettings["MessageTimeOut"].ToString()),
                        sys_id = Settings.EmSystemId,
                        dev = request.registerSource
                    };
                    var rsCode = EmMethodManage.EmLoginInstance.MsgCommSmsCodeSendReq(reqSMS);
                    if (rsCode.isOK)
                    {
                        result.flag = 1;
                        string phone = request.phone.Substring(0, 2) + "****" + request.phone.Substring(6, 4);
                        //我们已经给您  USER_PHONE_ALREADY    发送了验证短信 SEND_CODE_NOTE 
                        result.msg = CultureHelper.GetAPPLangSgring("USER_PHONE_ALREADY", request.lang) + phone + CultureHelper.GetAPPLangSgring("SEND_CODE_NOTE", request.lang);
                        ResponseSMSValidate rs = new ResponseSMSValidate();
                        rs.key = rsCode.sms_seq;
                        result.rs = rs;
                    }
                    else
                    {
                        result.msg = rsCode.ErrorMsg;
                    }
                }
            }
            #endregion
            return Ok(result);
        }
        #endregion

        #region 3.3.注册(兼容分享注册)（马锋）
        /// <summary>
        /// 3.3.注册(兼容分享注册)（马锋）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Registered(RequestRegistered request)
        {
            //统一返回类型
            ExResult result = new ExResult();
            result.Flag = 0;
            if (request == null)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(result);
            }
            //解密
            request.phone = enctyptionService.RSADecrypt(request.phone);
            request.password = enctyptionService.RSADecrypt(request.password);
            IValidator validator = new RegisteredValidator(request.lang);
            var results = validator.Validate(request);
            if (!results.IsValid)
            {
                result.Msg = results.Errors[0].ErrorMessage;
                return Ok(result);
            }
            if (new Regex(@"^[A-Za-z]+$").IsMatch(request.password) || new Regex(@"^[0-9]*$").IsMatch(request.password))
            {
                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORDNUMBERS", request.lang);
                return Ok(result);
            }
            if (request.password.Length < 8 || request.password.Length > 20 || new Regex(@"[^\x00-\xff]|\s").IsMatch(request.password))
            {
                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORD", request.lang);
                return Ok(result);
            }
            //检测注册验证码
            RequestPhone requestPhone = new RequestPhone();
            requestPhone.phone = request.phone;
            requestPhone.code = request.verification.ToString();
            requestPhone.del = 2;
            requestPhone.sys_id = Settings.EmSystemId;
            var rsCode = EmMethodManage.EmLoginInstance.MsgRegCheckCodeReq(requestPhone);
            if (!rsCode.isOK)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("API_VERIFICATIONERROR", request.lang);
                return Ok(result);
            }

            if (request.Inviting == "HKSJ" || string.IsNullOrEmpty(request.Inviting))
            {
                request.Inviting = "0968893058";
            }
            if (!string.IsNullOrEmpty(request.email))
            {
                request.email = enctyptionService.RSADecrypt(request.email);
            }
            else
            {
                request.email = "";
            }
            //检查邮箱
            if(!string.IsNullOrEmpty(request.email))
            {
                var checkMail = Validator.CheckMail(request.email);
                if (checkMail.status != 1)
                {
                    result.Msg = checkMail.message;
                    return Ok(result);
                }
            }           
            //注册用户信息
            YH_UserModel _user = null;
            //加密密码
            string psw_md5 = CodeHelper.GetMD5(request.password);
            BackMessage msg;
            if (Settings.IsEnableEM)
            {
                msg = _LoginService.RegisterSynAccountSystem(request.phone, request.password, psw_md5, request.verification, request.email, request.Inviting, ref _user);
            }
            else
            {
                msg = _LoginService.Register(request.phone, request.password, psw_md5, request.verification, request.email, request.Inviting, out _user);
            }

            if (msg.status == 1)
            {
                Task task = Task.Factory.StartNew(() =>
               {
                   VisitingCardCreate _VisitingCardCreate = new VisitingCardCreate();
                   ParaModel pm = new ParaModel();
                   pm.Account = _user.Account;
                   pm.HeadImageUrl = _user.HeadImageUrl;
                   pm.Phone = _user.Phone;
                   pm.NickName = _user.NickName;
                   pm.UserID = _user.UserID.ToString();
                   _VisitingCardCreate.GeneratedVisitingCard(pm, enctyptionService, request.lang);
               });

                result.Flag = 1;
                result.Msg = CultureHelper.GetAPPLangSgring("API_REGISTEREDSUCCESS", request.lang);
            }
            else
            {
                result.Flag = 0;
                result.Msg = msg.message;
            }
            //}
            //else
            //{
            //    result.msg = CultureHelper.GetAPPLangSgring("API_VERIFICATIONERROR", request.lang);
            //}
            return Ok(result);
        }
        #endregion

        #region 3.4.找回密码（马锋）
        /// <summary>
        /// 3.4.找回密码（马锋）
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="password">新密码（(MD5+RSA）</param>
        /// <param name="verification">手机验证码</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult RetrievePassword(RequestRetrievePassword request)
        {
            //统一返回类型
            //ApiResultModel result = new ApiResultModel();
            ExResult result = new ExResult();
            result.Flag = 0;
            if (request == null)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(result);
            }
            request.phone = enctyptionService.RSADecrypt(request.phone);
            IValidator validator = new RetrievePassword(request.lang);
            var results = validator.Validate(request);
            if (!results.IsValid)
            {
                result.Msg = results.Errors[0].ErrorMessage;
                return Ok(result);
            }
            request.password = enctyptionService.RSADecrypt(request.password);
            YH_UserModel model = _LoginService.GetUserInfoByPhone(request.phone).Data;
            if (model == null)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_USERNOTEXIST", request.lang);//用户不存在
            }
            if (request.password == null || request.password == "")
            {
                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PWDNOTEMPTY", request.lang);//密码不能为空
            }
            if (request.password.Length < 8 || new Regex(@"^[A-Za-z]+$").IsMatch(request.password) || new Regex(@"^[0-9]*$").IsMatch(request.password))
            {
                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORD", request.lang);//密码由8-20位数字、字母或特殊字符组成,区分大小写
            }
            if (request.password.Length > 20 && new Regex(@"[^\x00-\xff]|\s").IsMatch(request.password))//密码由8-20位数字、字母或特殊字符组成,区分大小写
            {
                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORD", request.lang);//密码由8-20位数字、字母或特殊字符组成,区分大小写
            }
            //object obj = HKSJ.Common.DataCache.GetCache(request.phone);

            //if (string.IsNullOrEmpty(request.verification) || obj == null || obj.ToString() != request.verification)
            //{
            //    result.msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_CORRECTCODE", request.lang);//请输入正确的手机验证码
            //}
            //else
            //{
            var rsCodes = EmMethodManage.EmFindUpdateInstance.MsgResetPwdCodeCheckReq(model.Account, request.verification);
            if (rsCodes.isOK)
            {
                var rsCode = EmMethodManage.EmFindUpdateInstance.MsgResetPwdPostPwdReq(model.UserID, request.password, rsCodes.key);
                if (!rsCode.isOK)
                {
                    result.Msg = rsCode.ErrorMsg;
                    return Ok(result);
                }
                model.PassWord = request.password;
                if (_LoginService.Update(model).Data == 1)
                {
                    result.Flag = 1;
                    result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORDSUCCESS", request.lang);//密码设置成功
                }
                else
                {
                    result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORDFAILURE", request.lang);//密码设置失败
                }
            }
            else
            {
                result.Msg = rsCodes.ErrorMsg;
            }
            //}
            //result.rs = "";
            return Ok(result);
        }
        #endregion

        #region 3.5.获取用户信息（刘文宁）
        /// <summary>
        /// 3.5.获取用户信息（刘文宁）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetUserInfo(RequersUserInfoModel model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.userId))
                {
                    string strUserid = enctyptionService.RSADecrypt(model.userId);
                    long UserId;
                    if (long.TryParse(strUserid, out UserId))
                    {
                        if (UserId > 0)
                        {
                            result = _IYH_UserService.GetUserInfo(UserId, model.lang);
                            if (result.IsValid)
                            {
                                apiResult.flag = 1;
                                apiResult.rs = result.Data;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", model.lang);//查询成功
                                if (result.Messages != null && result.Messages.Count > 0)
                                {
                                    apiResult.msg = result.Messages[0];
                                }
                            }
                            else
                            {
                                apiResult.flag = 0;
                                apiResult.msg = result.Messages[0];
                            }
                        }
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                        return Ok(apiResult);
                    }
                }
                else
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                    return Ok(apiResult);
                }
            }
            else
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang);
            }
            return Ok(apiResult);
        }
        #endregion

        #region 3.6.修改头像（刘文宁）
        /// <summary>
        /// 3.6.修改头像（刘文宁）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateUserHeadImage(RequersUpdateUserHeadImage model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.userId))
                {
                    string strUserid = enctyptionService.RSADecrypt(model.userId);
                    long UserId;
                    if (long.TryParse(strUserid, out UserId))
                    {
                        if (UserId > 0)
                        {
                            result = _IYH_UserService.UpdateUserHeadImage(UserId, model.lang, model.imageUrl);
                            if (result.IsValid)
                            {
                                apiResult.flag = 1;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_SUCCESS", model.lang);
                                Task task = Task.Factory.StartNew(() =>
                                {
                                    (new VisitingCardCreate()).GeneratedVisitingCard(UserId, model.lang);
                                });

                            }
                            else
                            {
                                apiResult.flag = 0;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_Failure", model.lang);
                            }
                        }
                        else
                        {
                            apiResult.flag = 0;
                            apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                            return Ok(apiResult);
                        }
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                        return Ok(apiResult);
                    }
                }
                else
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                    return Ok(apiResult);
                }
            }
            else
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang);
            }
            return Ok(apiResult);
        }
        #endregion

        #region 3.7.修改昵称（刘文宁）
        /// <summary>
        /// 3.7.修改昵称（刘文宁）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateUserNickName(RequersUpdateUserNickNameModel model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.userId))
                {
                    string strUserid = enctyptionService.RSADecrypt(model.userId);
                    long UserId;
                    if (long.TryParse(strUserid, out UserId))
                    {
                        if (UserId > 0)
                        {
                            if (!string.IsNullOrEmpty(model.nickName))
                            {
                                if (Encoding.Default.GetBytes(model.nickName).Length > 32)
                                {
                                    apiResult.flag = 0;
                                    apiResult.msg = CultureHelper.GetAPPLangSgring("BEYONDMAXIMUMNUMBEROFCHARACTERS", model.lang);//超过最大字符数 10
                                    return Ok(apiResult);
                                }
                            }
                            result = _IYH_UserService.UpdateUserNickName(UserId, model.lang, model.nickName);
                            if (result.IsValid)
                            {
                                apiResult.flag = 1;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_SUCCESS", model.lang);//更新成功
                                if (result.Messages.Count > 0)
                                {
                                    apiResult.msg = result.Messages[0];
                                }
                                Task task = Task.Factory.StartNew(() =>
                                {
                                    (new VisitingCardCreate()).GeneratedVisitingCard(UserId, model.lang);
                                });
                            }
                            else
                            {
                                apiResult.flag = 0;
                                apiResult.msg = result.Messages[0];
                            }
                        }
                        else
                        {
                            apiResult.flag = 0;
                            apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                            return Ok(apiResult);
                        }
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                        return Ok(apiResult);
                    }
                }
                else
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang);
                }
            }
            else
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang);
            }
            return Ok(apiResult);
        }
        #endregion

        #region 3.8.修改性别（刘文宁）
        /// <summary>
        /// 3.8.修改性别（刘文宁）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateUserSex(RequersUpdateUserSexModel model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.userId))
                {
                    string strUserid = enctyptionService.RSADecrypt(model.userId);
                    long UserId;
                    if (long.TryParse(strUserid, out UserId))
                    {
                        if (UserId > 0)
                        {
                            result = _IYH_UserService.UpdateUserSex(UserId, model.lang, model.sex);
                            if (result.IsValid)
                            {
                                apiResult.flag = 1;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_SUCCESS", model.lang);//更新成功
                            }
                            else
                            {
                                apiResult.flag = 0;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_Failure", model.lang);//更新失败
                            }
                        }
                        else
                        {
                            apiResult.flag = 0;
                            apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                            return Ok(apiResult);
                        }
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                        return Ok(apiResult);
                    }
                }
                else
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                    return Ok(apiResult);
                }

            }
            else
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang);
            }

            return Ok(apiResult);
        }
        #endregion

        #region 3.9.修改年龄（刘文宁）
        /// <summary>
        /// 3.9.修改年龄（刘文宁）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateUserBirthday(RequersUpdateUserBirthdayModel model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.userId))
                {
                    string strUserid = enctyptionService.RSADecrypt(model.userId);
                    long UserId;
                    if (long.TryParse(strUserid, out UserId))
                    {
                        if (UserId > 0)
                        {
                            long birthday = 0;
                            long.TryParse((model.birthday), out birthday);
                            if (birthday > 0)
                            {
                                DateTime dt = ConvertsTime.TimeStampToDateTime(birthday);
                                if (dt > DateTime.Now)
                                {
                                    apiResult.flag = 0;
                                    apiResult.msg = CultureHelper.GetLangString("USERINFO_BIRTHDAY_NOT_GREATER_THAN_CURRENT_DATETIME");//生日不能大于当前时期！
                                    return Ok(apiResult);
                                }
                            }
                            result = _IYH_UserService.UpdateUserBirthday(UserId, model.lang, model.birthday);
                            if (result.IsValid)
                            {
                                apiResult.flag = 1;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_SUCCESS", model.lang);//更新成功
                            }
                            else
                            {
                                apiResult.flag = 0;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_Failure", model.lang);//更新失败
                            }
                        }
                        else
                        {
                            apiResult.flag = 0;
                            apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                            return Ok(apiResult);
                        }
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                        return Ok(apiResult);
                    }
                }
                else
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                    return Ok(apiResult);
                }
            }
            else
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang);
            }
            return Ok(apiResult);
        }
        #endregion

        #region 3.10.修改签名（刘文宁）
        /// <summary>
        /// 3.10.修改签名（刘文宁）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateUserSignature(RequersUpdateUserSignatureModel model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.userId))
                {
                    string strUserid = enctyptionService.RSADecrypt(model.userId);
                    long UserId;
                    if (long.TryParse(strUserid, out UserId))
                    {
                        if (UserId > 0)
                        {
                            if (!string.IsNullOrEmpty(model.signature))
                            {
                                if (Encoding.Default.GetBytes(model.signature).Length > 100)
                                {
                                    apiResult.flag = 0;
                                    apiResult.msg = CultureHelper.GetAPPLangSgring("BEYONDMAXIMUMNUMBEROFCHARACTERS", model.lang);//超过最大字符数50
                                    return Ok(apiResult);
                                }
                            }
                            result = _IYH_UserService.UpdateUserSignature(UserId, model.lang, model.signature);
                            if (result.IsValid)
                            {
                                apiResult.flag = 1;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_SUCCESS", model.lang);//更新成功
                            }
                            else
                            {
                                apiResult.flag = 0;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_Failure", model.lang);//更新失败
                            }
                        }
                        else
                        {
                            apiResult.flag = 0;
                            apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                            return Ok(apiResult);
                        }
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                        return Ok(apiResult);
                    }
                }
                else
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                    return Ok(apiResult);
                }
            }
            else
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang);
            }
            return Ok(apiResult);
        }
        #endregion

        #region 3.10.修改常驻地址（刘文宁）
        /// <summary>
        /// 3.10.修改签名（刘文宁）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateResidentAddress(RequersUpdateResidentAddress model)
        {
            var result = new ResultModel();
            var apiResult = new ApiResultModel();
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.userId))
                {
                    string strUserid = enctyptionService.RSADecrypt(model.userId);
                    long UserId;
                    if (long.TryParse(strUserid, out UserId))
                    {
                        if (UserId > 0)
                        {
                            result = _IYH_UserService.UpdateResidentAddress(UserId, model.lang, model.tHAreaID);
                            if (result.IsValid)
                            {
                                apiResult.flag = 1;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_SUCCESS", model.lang);
                            }
                            else
                            {
                                apiResult.flag = 0;
                                apiResult.msg = CultureHelper.GetAPPLangSgring("UPDATE_Failure", model.lang);
                            }
                        }
                        else
                        {
                            apiResult.flag = 0;
                            apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                            return Ok(apiResult);
                        }
                    }
                    else
                    {
                        apiResult.flag = 0;
                        apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                        return Ok(apiResult);
                    }
                }
                else
                {
                    apiResult.flag = 0;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", model.lang); //参数错误
                    return Ok(apiResult);
                }
            }
            else
            {
                apiResult.flag = 0;
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", (int)LanguageType.defaultLang);
            }
            return Ok(apiResult);
        }
        #endregion

        #region 3.11.增加收藏商品接口（伍观德）
        /// <summary>
        /// 3.11.增加收藏商品接口（伍观德）
        /// </summary>
        /// <returns></returns>
        /// <remarks>added by 伍观德,2015-8-10</remarks>
        [HttpPost]
        public IHttpActionResult AddFavorites(RequestFavoritesModel model)
        {
            ApiResultModel response = new ApiResultModel();
            IsNotFavorites ft = new IsNotFavorites();
            try
            {
                if (model == null)
                {
                    response.flag = 0;
                    response.msg = CultureHelper.GetAPPLangSgring("USER_ADDFAVORITES_PARAMETERS", model.lang);
                    return Ok(response);
                }

                IValidator validator = new FavoritesModelValidator(model.lang);
                var results = validator.Validate(model);
                response.flag = results.IsValid == true ? 1 : 0;
                if (!results.IsValid)
                {
                    response.msg = results.Errors[0].ErrorMessage;
                    return Ok(response);
                }
                string _userId = enctyptionService.RSADecrypt(model.userId);//解密
                string[] productIds = model.productIds.Split(new char[1] { ',' });
                int resultNum = 0;
                long favoritesID = 0;
                if (productIds != null && productIds.Length > 0)
                {
                    for (int i = 0; i < productIds.Length; i++)
                    {
                        var result = this.myCollectionService.AddFavorites(long.Parse(_userId), long.Parse(productIds[i]), out favoritesID);
                        if (result.IsValid)
                        {
                            resultNum = resultNum + 1;
                        }
                    }
                }

                response.flag = 1;
                response.msg = CultureHelper.GetAPPLangSgring("USER_ADDFAVORITES_COLLSUCCESS", model.lang);
                ft.isFavorites = resultNum > 0 ? 1 : 2;
                ft.favoritesID = favoritesID;
                response.rs = ft;
            }
            catch (Exception ex)
            {
                response.flag = 0;
                response.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", model.lang);
                ft.isFavorites = 0;
                ft.favoritesID = 0;
                response.rs = ft;
            }
            return Ok(response);
        }
        #endregion

        #region 3.12.获取我所有的收藏（黎威）
        /// <summary>
        /// 3.12.获取我所有的收藏（黎威）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>      
        [HttpPost]
        public IHttpActionResult GetFavorites(RequestSearchPagingByUserModel model)
        {
            var apiResult = new ApiPagingResultModel();
            if (model == null)
            {
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(apiResult);
            }
            apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", model.lang);
            var uID = Convert.ToInt64(enctyptionService.RSADecrypt(model.userId));
            int count;
            var result = new ResultModel();
            try
            {
                result = productSearchListService.GetMyCollectionListForApi(uID, new KeyWordsSearch { Page = (model.pageNo), PageSize = model.pageSize, languageId = model.lang }, out count);
            }
            catch (Exception)
            {
                return Ok(apiResult);
            }
            if (result.IsValid)
            {
                apiResult.totalSize = count;
                apiResult.flag = 1;
                apiResult.rs = result.Data;
                apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", model.lang);
            }
            return Ok(apiResult);
        }
        #endregion

        #region 3.13.删除我的收藏（黎威）
        /// <summary>
        /// 3.13.删除我的收藏（黎威）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteFavorites(RequestFavoritesByUserModel model)
        {
            var apiResult = new ApiResultModel();
            if (model == null)
            {
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(apiResult);
            }
            var uID = Convert.ToInt64(enctyptionService.RSADecrypt(model.userId));
            apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_SAVEFAILED", model.lang);
            if (model.favoritesID <= 0 || uID <= 0)
            {
                return Ok(apiResult);
            }
            var result = productSearchListService.DeleteMyCollection(uID, model.favoritesID);
            if (result.IsValid)
            {
                apiResult.flag = 1;
                apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", model.lang);
            }
            return Ok(apiResult);
        }
        #endregion

        #region 3.14.收货地址列表（黎威）
        /// <summary>
        /// 3.14.收货地址列表（黎威）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lang"></param>
        /// <returns></returns>       
        [HttpPost]
        public IHttpActionResult ReceivingAddress(RequestSearchPagingByUserModel model)
        {
            var apiResult = new ApiPagingResultModel();
            if (model == null)
            {
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(apiResult);
            }
            var userId = Convert.ToInt64(enctyptionService.RSADecrypt(model.userId));
            var result = new ResultModel();

            apiResult.flag = 0;
            apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", model.lang);
            if (userId > 0)
            {

                result = userAddressService.GetPagingUserAddress(new SearchUserAddressModel { UserID = userId, PagedIndex = model.pageNo - 1, PagedSize = model.pageSize }, model.lang);
                if (result.IsValid)
                {
                    var userAddress=(result.Data as List<UserAddress>).Select(x=>new {
                        userAddressId=x.UserAddressId,
                        userID = x.UserID,
                       detailsAddress= x.DetailsAddress,
                        receiver=x.Receiver,
                        tHAreaID=x.THAreaID,
                        postalCode=x.PostalCode,
                        mobile=x.Mobile,
                        phone=x.Phone,
                        flag=x.Flag,
                        email=x.Email,
                        shengTHAreaID=x.ShengTHAreaID,
                        shiTHAreaID=x.ShiTHAreaID,
                        shengAreaName=x.ShengAreaName,
                        shiAreaName=x.ShiAreaName,
                        quAreaName=x.QuAreaName,
                        address = FormatAddress(model.lang, x.ShengAreaName, x.ShiAreaName, x.QuAreaName,x.DetailsAddress)                
                     });
                    apiResult.totalSize = result.Data.TotalCount;
                    apiResult.flag = 1;
                    apiResult.rs = userAddress;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_SUCCESS", model.lang);
                }
            }
            //var jsonResult = JsonConvert.SerializeObject(apiResult);
            //return new HttpResponseMessage { Content = new StringContent(jsonResult, System.Text.Encoding.UTF8, "application/json") };
            //return apiResult;
            return Ok(apiResult);
        }
        #endregion

        #region 3.15.新增/修改用户收货地址（黎威）
        /// <summary>
        /// 3.15.新增/修改用户收货地址（黎威）
        /// </summary>
        /// <param name="userAddrModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddOrUpdateReceiverAddress(UserAddressExtension userAddrModel)
        {
            var apiResult = new ApiResultModel();
            if (userAddrModel == null)
            {
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(apiResult);
            }
            userAddrModel.UserID = Convert.ToInt64(enctyptionService.RSADecrypt(userAddrModel.userIdEnc));
            userAddrModel.Phone = Convert.ToString(enctyptionService.RSADecrypt(userAddrModel.phoneEnc));
            userAddrModel.Mobile = Convert.ToString(enctyptionService.RSADecrypt(userAddrModel.mobileEnc));
            return AddOrUpdateUserAddress(userAddrModel, userAddrModel.UserAddressId == 0, userAddrModel.lang);
        }
        #endregion

        #region 3.16.删除用户收货地址（黎威）
        /// <summary>
        /// 3.16.删除用户收货地址（黎威）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteReceiverAddress(RequestSearchByAddressModel model)
        {

            //var userAddressId = Convert.ToInt64(BrEngineContext.Current.Resolve<IEncryptionService>().RSADecrypt(model.userId));
            var apiResult = new ApiResultModel();
            if (model == null)
            {
                apiResult.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(apiResult);
            }
            apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_SAVEFAILED", model.lang);
            if (model.userAddressId <= 0)
            {
                return Ok(apiResult);
            }
            var result = new ResultModel();
            try
            {
                result = userAddressService.DelUserAddress(model.userAddressId);
                if (result.IsValid)
                {
                    apiResult.flag = 1;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", model.lang);
                    return Ok(apiResult);
                }
                return Ok(apiResult);
            }
            catch (Exception)
            {
                return Ok(apiResult);
            }
        }
        #endregion

        #region 3.17.意见反馈(马锋)
        /// <summary>
        /// 3.17.意见反馈(马锋)
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="type"> 0:未选择,1:注册登录反馈,2:购物反馈,3惠粉反馈,4:消费收益反馈,5:其它反馈</param>
        /// <param name="content">内容</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Feedback(RequestFeedback request)
        {
            //统一返回类型
            ExResult result = new ExResult();
            result.Flag = 0;
            if (request == null)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(result);
            }
            IValidator validator = new FeedbackValidator(request.lang);
            var results = validator.Validate(request);
            if (!results.IsValid)
            {
                result.Msg = results.Errors[0].ErrorMessage;
                return Ok(result);
            }
            request.userId = enctyptionService.RSADecrypt(request.userId);
            FeedbackView feed = new FeedbackView();
            feed.FeedbackType = request.type;
            feed.UserID = long.Parse(request.userId);
            feed.MsgContent = request.content;
            feed.FeedbackDate = DateTime.Now;
            ResultModel rm = feedbackService.AddFeedback(feed);
            if (rm.Flag == 1)
            {
                result.Flag = 1;
                result.Msg = CultureHelper.GetAPPLangSgring("APP_FeebackSuccess", request.lang);//您的反馈我们已经记录。感谢您的反馈,祝您购物愉快。3S后将自动返回到惠卡商城首页
            }
            else
            {
                result.Msg = CultureHelper.GetAPPLangSgring("USER_ADDFEEBDACKFAILURE", request.lang);
            }
            return Ok(result);
        }
        #endregion

        #region 3.18.获取意见反馈类型(马锋)
        /// <summary>
        /// 3.18.获取意见反馈类型(马锋)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetFeedbackType(RequestLang request)
        {
            //统一返回类型
            ApiResultModel result = new ApiResultModel();
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(result);
            }
            IValidator validator = new RequestLangValidator(request.lang);
            var results = validator.Validate(request);
            if (!results.IsValid)
            {
                result.msg = results.Errors[0].ErrorMessage;
                return Ok(result);
            }
            ResultModel rm = feedbackService.SelectFeedbackType(request.lang);
            List<FeedbackTypeLangView> ftlv = new List<FeedbackTypeLangView>();
            List<GetFeedbackType> gft = new List<GetFeedbackType>();
            if (rm.Flag == 1 && rm.Data != null)
            {
                ftlv = rm.Data as List<FeedbackTypeLangView>;
                foreach (var item in ftlv)
                {
                    GetFeedbackType it = new GetFeedbackType();
                    it.typeId = item.FeedbackTypeId;
                    it.typeName = item.FeedbackName;
                    gft.Add(it);
                }
                result.flag = rm.Flag;
                result.msg = "";
                result.rs = gft;
                return Ok(result); ;
            }
            result.msg = "";
            return Ok(result); ;
        }
        #endregion

        #region 3.19.惠粉-设置密码（登录密码\交易密码）（李霞）
        /// <summary>
        /// 3.19.惠粉-设置密码（登录密码\交易密码）（李霞）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="password">密码</param>
        /// <param name="type">密码类型 1登陆密码 2交易密码 3找回密码</param>
        /// <param name="phone">输入的手机号码</param>
        /// <param name="validateCode">验证码</param>
        /// <param name="key">验证码唯一识别码</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>        
        [HttpPost]
        public IHttpActionResult SetPassword(RequestSetPassword request)
        {
            //统一返回类型
            ExResult result = new ExResult();
            result.Flag = 0;
            if (request == null)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 2); //参数不能为空
                return Ok(result);
            }
            if (request.type < 1 || string.IsNullOrEmpty(request.userId) || request.lang < 1)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang == 0 ? 1 : request.lang); //参数错误
                return Ok(result);
            }
            request.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(request.userId)) ? "0" : request.userId) : enctyptionService.RSADecrypt(request.userId);
            request.password = (ForTest == 1) ? ((string.IsNullOrEmpty(request.password)) ? "0" : request.password) : enctyptionService.RSADecrypt(request.password);

            if (string.IsNullOrEmpty(request.password))
            {
                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PWDNOTEMPTY", request.lang);//密码不能为空
                return Ok(result);
            }
            if (request.password.Length < 8 || new Regex(@"^[A-Za-z]+$").IsMatch(request.password) || new Regex(@"^[0-9]*$").IsMatch(request.password))
            {
                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORD", request.lang);//密码由8-20位数字、字母或特殊字符组成,区分大小写
                return Ok(result);
            }
            if (request.password.Length > 20 && new Regex(@"[^\x00-\xff]|\s").IsMatch(request.password))//密码由8-20位数字、字母或特殊字符组成,区分大小写
            {
                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORD", request.lang);//密码由8-20位数字、字母或特殊字符组成,区分大小写
                return Ok(result);
            }
            var setUserPwd = new ResultModel();
            long userId;
            if (long.TryParse(request.userId, out userId))
            {
                if (userId > 0)
                {
                    if (request.type == 3)
                    {
                        request.phone = (ForTest == 1) ? ((string.IsNullOrEmpty(request.phone)) ? "0" : request.phone) : enctyptionService.RSADecrypt(request.phone);
                        YH_UserModel user = _LoginService.GetUserInfoById(userId).Data;
                        if (user.Phone != request.phone)
                        {
                            result.Msg = CultureHelper.GetAPPLangSgring("USER_UPDATEPERSON_NOT", request.lang); //手机号与修改人不是同一个
                            return Ok(result);
                        }
                        var reqSms = new RequestCommSmsCheck
                        {
                            sms_seq = request.key,
                            code = request.validateCode,
                            del = 2,
                            sys_id = Settings.EmSystemId,
                            dev = 1
                        };
                        var rsCode = EmMethodManage.EmLoginInstance.MsgCommSmsCodeCheckReq(reqSms);
                        if (!rsCode.isOK)
                        {
                            result.Flag = 0;
                            result.Msg = rsCode.ErrorMsg + ":::" + rsCode.Status;
                            if (rsCode.Status == 1048821)
                            {
                                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_CODEERROR", request.lang);//输入的验证码错误
                            }
                            if (rsCode.Status == 1048787 || rsCode.Status == 1048687)
                            {
                                result.Msg = CultureHelper.GetAPPLangSgring("API_VERIFICATIONINVALID", request.lang); //验证码已失效,请重新获取
                            }
                            return Ok(result);
                        }
                    }
                    setUserPwd = _IYH_UserService.setUserPwd(userId, request.password, request.type, request.lang);
                    if (setUserPwd.IsValid)
                    {
                        result.Flag = 1;
                        result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORDSUCCESS", request.lang); //密码设置成功
                    }
                    else
                    {
                        result.Msg = setUserPwd.Messages[0];
                    }
                }
                else
                {
                    result.Msg = CultureHelper.GetAPPLangSgring("USER_ADDFAVORITES_USERVALID", request.lang); //用户ID不合法
                }
            }
            else
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
            }
            return Ok(result);
        }
        #endregion

        #region 3.20.惠粉-修改密码（登录密码\交易密码）（李霞）
        /// <summary>
        /// 3.20.惠粉-修改密码（登录密码\交易密码）（李霞）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="password">新密码(加密)</param>
        /// <param name="oldPassword">旧密码(加密)</param>
        /// <param name="type">1 修改登陆密码 2 修改交易密码</param>
        /// <param name="registerSource">操作终端的设备类型（1网站 2安卓 3IOS）</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>      
        [HttpPost]
        public IHttpActionResult ChangePassword(RequestChangePassword request)
        {
            //统一返回类型
            ExResult result = new ExResult();
            result.Flag = 0;
            if (request == null)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 2);//参数不能为空
                return Ok(result);
            }
            if (request.type < 1 || string.IsNullOrEmpty(request.userId) || request.lang < 1)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang == 0 ? 1 : request.lang); //参数错误
                return Ok(result);
            }
            request.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(request.userId)) ? "0" : request.userId) : enctyptionService.RSADecrypt(request.userId);
            request.password = (ForTest == 1) ? ((string.IsNullOrEmpty(request.password)) ? "0" : request.password) : enctyptionService.RSADecrypt(request.password);
            request.oldPassword = (ForTest == 1) ? ((string.IsNullOrEmpty(request.oldPassword)) ? "0" : request.oldPassword) : enctyptionService.RSADecrypt(request.oldPassword);

            if (string.IsNullOrEmpty(request.password) || string.IsNullOrEmpty(request.oldPassword))
            {
                result.Msg = CultureHelper.GetAPPLangSgring("LOGIN_ENTERPASSWORD", request.lang); //请输入密码
                return Ok(result);
            }
            var changeUserPwd = new ResultModel();
            long userId;
            if (long.TryParse(request.userId, out userId))
            {
                if (userId > 0)
                {
                    changeUserPwd = _IYH_UserService.changeUserPwd(userId, request.password, request.oldPassword, request.type, request.registerSource, request.lang);
                    if (changeUserPwd.IsValid)
                    {
                        result.Flag = 1;
                        result.Msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_UPDATEPAYPASSWORD_UPDATESUCCESS", request.lang);//恭喜您,修改密码设置成功
                    }
                    else
                    {
                        result.Msg = changeUserPwd.Messages[0];
                    }
                }
                else
                {
                    result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
                }
            }
            else
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
            }
            return Ok(result);
        }
        #endregion

        #region 3.21.惠粉-举报（李霞）
        /// <summary>
        /// 3.21.惠粉-举报（李霞）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="reportId">被举报人ID</param>
        /// <param name="reasontype">举报原因（1.色情,2.骚扰广告,3.侮辱诋毁,4.诈骗钱财,5.其他）</param>
        /// <param name="remark">补充说明（可以为空）</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>	
        /// <returns></returns>        
        [HttpPost]
        public IHttpActionResult Report(RequestReport request)
        {
            //统一返回类型
            ExResult result = new ExResult();
            result.Flag = 0;
            if (request == null)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 2);//参数不能为空
                return Ok(result);
            }
            if (string.IsNullOrEmpty(request.userId) || string.IsNullOrEmpty(request.reportId) || (request.reasonType < 1 || request.reasonType > 5) || request.lang < 1)
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang == 0 ? 1 : request.lang); //参数错误
                return Ok(result);
            }
            request.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(request.userId)) ? "0" : request.userId) : enctyptionService.RSADecrypt(request.userId);
            request.reportId = (ForTest == 1) ? ((string.IsNullOrEmpty(request.reportId)) ? "0" : request.reportId) : enctyptionService.RSADecrypt(request.reportId);

            var reportUser = new ResultModel();
            long userId, reportId;
            if (long.TryParse(request.reportId, out reportId))
            {
                if (reportId < 0)
                {
                    result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
                    return Ok(result);
                }
            }
            else
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
                return Ok(result);
            }
            if (long.TryParse(request.userId, out userId))
            {
                if (userId > 0)
                {
                    reportUser = _IYH_UserService.reportUser(userId, reportId, request.reasonType, request.remark, request.lang);
                    if (reportUser.IsValid)
                    {
                        result.Flag = 1;
                        result.Msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", request.lang); //操作成功
                    }
                    else
                    {
                        result.Msg = reportUser.Messages[0];
                    }
                }
                else
                {
                    result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
                }
            }
            else
            {
                result.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
            }
            return Ok(result);
        }
        #endregion

        #region 3.22.惠粉-手机通讯录（李霞）
        /// <summary>
        /// 3.22.惠粉-手机通讯录（李霞）
        /// </summary>
        /// <param name="type">联系方式类型（1.邮箱账号,2.手机号码）</param>
        /// <param name="key_list">联系方式信息列表</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>	
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetContactsInfo(RequestGetContactsInfo request)
        {
            //统一返回类型
            ApiResultModel result = new ApiResultModel();
            result.flag = 0;
            result.rs = new List<ResponseGetContactsInfo>();
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 2);
                return Ok(result);
            }
            if (request.type != 1 && request.type != 2 || request.lang < 1)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang == 0 ? 1 : request.lang); //参数错误
                return Ok(result);
            }

            long[] userIds = null;//已注册惠粉的用户ID
            var getContactsInfo = new ResultModel();
            if (request.type == 1)//1.邮箱账号
            {
                if (request.key_list.Length > 0)
                {
                    string strWhere = string.Format(" Email in('{0}')", string.Join("','", request.key_list));
                    getContactsInfo = _IYH_UserService.GetList(strWhere);
                    userIds = getContactsInfo.Data;
                }

            }
            else if (request.type == 2)//2.手机号码
            {
                if (request.key_list.Length > 0)
                {
                    string strWhere = string.Format(" Phone in('{0}')", string.Join("','", request.key_list));
                    getContactsInfo = _IYH_UserService.GetList(strWhere);
                    userIds = getContactsInfo.Data;
                }
            }
            if (userIds != null && userIds.Length > 0)
            {
                //查询账户系统数据
                ResponseUserInfoData responseUser = EmMethodManage.EmFindUpdateInstance.MsgBatchGetInfoByIdReq(userIds);
                if (responseUser.isOK && responseUser.data != null)
                {
                    foreach (var item in responseUser.data)
                    {
                        result.rs.Add(new ResponseGetContactsInfo
                        {
                            uid = item.uid.ToString(),
                            acc = item.acc,
                            nick = item.nick,
                            phone = item.phone,
                            img_url = item.img_url,
                            sign = item.sign
                        });
                    }
                }
            }

            result.flag = 1;
            result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", request.lang); //操作成功
            return Ok(result);
        }
        #endregion

        #region 3.23.惠粉-查找群ID（李霞）
        /// <summary>
        /// 3.23.惠粉-查找群ID（李霞）
        /// </summary>
        /// <param name="userId">用户ID（加密）</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetGroupId(RequestBase request)
        {
            //统一返回类型
            ApiResultModel result = new ApiResultModel();
            result.flag = 0;
            result.rs = new List<ResponseGetGroupId>();
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 2);
                return Ok(result);
            }
            if (string.IsNullOrEmpty(request.userId) || request.lang < 1)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang == 0 ? 1 : request.lang); //参数错误
                return Ok(result);
            }
            request.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(request.userId)) ? "0" : request.userId) : enctyptionService.RSADecrypt(request.userId);


            var getGroupId = new ResultModel();
            long userId;
            if (long.TryParse(request.userId, out userId))
            {
                if (userId > 0)
                {
                    YH_UserModel user = _LoginService.GetUserInfoById(userId).Data;
                    if (user == null)
                    {
                        result.msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_USERNOTEXIST", request.lang); //参数错误
                        return Ok(result);
                    }
                    CreateGroupMark(user);
                    getGroupId = _IYH_UserService.GetGroupIdByUserId(userId, request.lang);
                    if (getGroupId.IsValid)
                    {
                        result.flag = 1;
                        result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", request.lang); //操作成功
                        result.rs = getGroupId.Data;
                    }
                    else
                    {
                        result.flag = 1;
                        result.msg = CultureHelper.GetAPPLangSgring("NO_DATA", request.lang); //暂无数据
                    }
                }
                else
                {
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
                }
            }
            else
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
            }
            return Ok(result);
        }
        #endregion

        #region 3.24.惠粉-惠粉人数（李霞）
        /// <summary>
        /// 3.24.惠粉-惠粉人数（李霞）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetGroupNumber(RequestBase request)
        {
            //统一返回类型
            ApiResultModel result = new ApiResultModel();
            result.flag = 0;
            result.rs = new ResponseGetGroupNumber();
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 2);
                return Ok(result);
            }
            if (string.IsNullOrEmpty(request.userId) || request.lang < 1)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang == 0 ? 1 : request.lang); //参数错误
                return Ok(result);
            }
            request.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(request.userId)) ? "0" : request.userId) : enctyptionService.RSADecrypt(request.userId);


            ResponseGetGroupNumber responeGroupN = new ResponseGetGroupNumber();
            long userId;
            if (long.TryParse(request.userId, out userId))
            {
                if (userId > 0)
                {
                    responeGroupN.GnNumber = _IYH_UserService.GetGroupNumber(userId, 1, request.lang).Data.ToString();
                    responeGroupN.GdNumber = _IYH_UserService.GetGroupNumber(userId, 2, request.lang).Data.ToString();
                    responeGroupN.GxNumber = _IYH_UserService.GetGroupNumber(userId, 3, request.lang).Data.ToString();

                    result.flag = 1;
                    result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", request.lang); //操作成功
                    result.rs = responeGroupN;
                }
                else
                {
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
                }
            }
            else
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
            }
            return Ok(result);
        }
        #endregion

        #region 3.25.惠粉-惠粉列表（李霞）
        /// <summary>
        /// 3.25.惠粉-惠粉列表（李霞）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="type">类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉）</param>
        /// <param name="pageNo">分页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetGroupList(RequestGetGroupList request)
        {
            //统一返回类型
            ApiPagingResultModel result = new ApiPagingResultModel();
            result.flag = 0;
            result.rs = new List<ResponseGetGroupList>();
            if (request == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 2);
                return Ok(result);
            }
            if (string.IsNullOrEmpty(request.userId) || (request.type < 1 || request.type > 3) || request.lang < 1)
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang == 0 ? 1 : request.lang); //参数错误
                return Ok(result);
            }
            request.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(request.userId)) ? "0" : request.userId) : enctyptionService.RSADecrypt(request.userId);
            if (request.pageNo == 0)
            {
                request.pageNo = 1;
            }
            if (request.pageSize == 0)
            {
                request.pageSize = 10;
            }

            var getGroupList = new ResultModel();
            long userId;
            if (long.TryParse(request.userId, out userId))
            {
                if (userId > 0)
                {
                    getGroupList = _IYH_UserService.GetGroupList(userId, request.type, request.pageNo, request.pageSize, request.lang);
                    if (getGroupList.IsValid)
                    {
                        List<dynamic> rsList = getGroupList.Data as List<dynamic>;
                        int tc = 0;//总页数
                        int cnt = (getGroupList.Data).Count;
                        if (cnt % request.pageSize == 0)
                        {
                            tc = cnt / request.pageSize;
                        }
                        else
                        {
                            tc = cnt / request.pageSize + 1;
                        }
                        List<ResponseGetGroupList> gft = new List<ResponseGetGroupList>();
                        if (tc >= request.pageNo)
                        {
                            //判断最后一页的个数
                            int pSize = request.pageSize * (request.pageNo - 1) + request.pageSize + 1 > cnt ? cnt - request.pageSize * (request.pageNo - 1) : request.pageSize;
                            rsList = rsList.GetRange(request.pageSize * (request.pageNo - 1), pSize);
                            foreach (var item in rsList)
                            {
                                ResponseGetGroupList groupList = new ResponseGetGroupList();
                                groupList.UserId = item.userId;
                                groupList.AddTime = ConvertsTime.DateTimeToTimeStamp(item.addTime);
                                groupList.ImageUrl = item.imageUrl;
                                groupList.NickName = item.nickName;
                                groupList.Account = item.account;
                                groupList.Phone = item.phone;
                                groupList.RealName = item.realName;
                                groupList.GnNumber = item.gnNumber.ToString();
                                groupList.GdNumber = item.gdNumber.ToString();
                                groupList.GxNumber = item.gxNumber.ToString();
                                gft.Add(groupList);
                            }
                        }
                        result.flag = 1;
                        result.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", request.lang); //操作成功
                        result.totalSize = cnt;
                        result.rs = gft;
                    }
                    else
                    {
                        result.flag = 1;
                        result.msg = CultureHelper.GetAPPLangSgring("NO_DATA", request.lang); //暂无数据
                    }
                }
                else
                {
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
                }
            }
            else
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", request.lang); //参数错误
            }
            return Ok(result);
        }
        #endregion

        #region 3.26.惠粉-我的二维码

        [HttpPost]
        public IHttpActionResult GeneratedVisitingCard(UserInfoBaseModel request)
        {
            ApiResultModel result = new ApiResultModel();
            result.flag = 0;
            result.msg = "";
            result.rs = new PicExt();
            try
            {
                if (request == null)
                {
                    result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 2);
                    return Ok(result);
                }
                request.userId = (ForTest == 1) ? ((string.IsNullOrEmpty(request.userId)) ? "0" : request.userId) : enctyptionService.RSADecrypt(request.userId);
                long userId = 0;
                if (request.lang != 1 && request.lang != 2 && request.lang != 3)
                {
                    request.lang = 2;
                }
                if (!long.TryParse(request.userId, out userId))
                {
                    userId = 0;
                    //刘文宁修改 2015/9/2
                    result.flag = 0;
                    result.rs = null;
                    result.msg = CultureHelper.GetAPPLangSgring("USER_ID_IllEGAL", request.lang);//用户ID不合法 
                    return Ok(result);
                }
                result = (new VisitingCardCreate()).GeneratedVisitingCard(userId, request.lang);
                return Ok(result);
            }
            catch (Exception e)
            {
                result.msg = CultureHelper.GetAPPLangSgring("SYSTEM_INNERERROR", request.lang);
                return Ok(result);
            }
        }

        #endregion

        #region api文档未标记接口

        //[HttpPost]
        //public ApiResultModel ExistMyCollection(long userId, long productId, int lang = 1)
        //{
        //    var apiResult = new ApiResultModel();
        //    apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_SAVEFAILED", lang);
        //    if (productId <= 0 || userId <= 0)
        //    {
        //        return apiResult;
        //    }
        //    try
        //    {
        //        var result = favoritesService.IsExistFavorites(userId, productId);
        //        if (result.IsValid)
        //        {
        //            apiResult.flag = 1;
        //            apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", lang);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return apiResult;
        //}

        #endregion

        #region 创建惠粉群

        [HttpPost]
        public IHttpActionResult CreateGroup([FromBody] RequestCreateGroup para)
        {
            var result = new ApiResultModel();
            if (para == null||string.IsNullOrWhiteSpace(para.userId))
            {
                result.msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", (int)LanguageType.defaultLang); //参数错误
                return Ok(result);
            }
            para.userId = enctyptionService.RSADecrypt(para.userId);
            YH_UserModel user = _LoginService.GetUserInfoById(Convert.ToInt64(para.userId)).Data;
            if (user == null)
            {
                result.msg = CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_USERNOTEXIST", para.lang); //参数错误
                return Ok(result);
            }
            CreateGroupMark(user);
            result.flag = 1;
            result.msg = "";
            return Ok(result); 
        }

        #endregion

        #region 私有方法
        private IHttpActionResult UpdateUserAddressFlag(UserAddress model, int lang = 1)
        {
            //model.UserID = Convert.ToInt64(BrEngineContext.Current.Resolve<IEncryptionService>().RSADecrypt(userId));
            var apiResult = new ApiResultModel();
            if (model == null)
            {
                apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_SAVEFAILED", lang);
                return Ok(apiResult);
            }
            var result = new ResultModel();
            try
            {
                result = userAddressService.UpdateUserAddressFlag(model);
                if (result.IsValid)
                {
                    apiResult.flag = 1;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", lang);
                    return Ok(apiResult);
                }
                apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_SAVEFAILED", lang);
                return Ok(apiResult);
            }
            catch (Exception)
            {
                apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", lang);
                return Ok(apiResult);
            }
        }
        private IHttpActionResult AddOrUpdateUserAddress(UserAddress userAddrModel, bool isAdd, int lang = 1)
        {
            var apiResult = new ApiResultModel();
            if (userAddrModel == null)
            {
                apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_SAVEFAILED", lang);
                return Ok(apiResult);
            }
            var result = new ResultModel();
            try
            {
                if (isAdd)
                {
                    var resultAddress = userAddressService.GetUserAllAddress(new SearchUserAddressModel() { UserID = userAddrModel.UserID }, lang);
                    if ((resultAddress.Data as List<UserAddress>).Count <= 0)
                    {
                        userAddrModel.Flag = 1;
                    }
                    userAddrModel.UserAddressId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    result = userAddressService.AddUserAddress(userAddrModel);
                }
                else
                {
                    result = userAddressService.UpdateUserAddress(userAddrModel);
                }
                if (result.IsValid)
                {
                    if (userAddrModel.Flag == 1)
                    {
                        UpdateUserAddressFlag(userAddrModel, lang);
                    }
                    apiResult.flag = 1;
                    apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_OPERATIONSUCCESSFUL", lang);
                    return Ok(apiResult);
                }
                else
                {
                    if (result.Data == 100)
                    {//已存在
                        apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_ALREADYEXISTS", lang);
                    }
                    else if (result.Data == 101)
                    {//超过20个
                        apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_SavaAddressERROR", lang);
                    }
                    else
                    {
                        if (isAdd)
                        { //新增失败
                            apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", lang);
                        }
                        else
                        { //更新失败
                            apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_UPDATEADDRESSFailure", lang);
                        }

                    }
                }

                return Ok(apiResult);
            }
            catch (Exception)
            {
                apiResult.msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_INDEX_ABNORMSERVER", lang);
                return Ok(apiResult);
            }
        }

        /// <summary>
        /// 创建惠粉群
        /// </summary>
        /// <param name="user"></param>
        private void CreateGroupMark(YH_UserModel user)
        {
            //创建惠粉群代码
            ResultModel rmgroup = yh_groupmarkService.GetGroupMarkByUserId(user.UserID);
            if (rmgroup.Flag == 1)
            {
                List<HKTHMall.Domain.Entities.YH_GroupMark> modelGroupMark = rmgroup.Data as List<HKTHMall.Domain.Entities.YH_GroupMark>;
                URQuerySpecificGroupRsp IMgn = EmMethodManage.EmGroupManage.URQuerySpecificGroup((ulong)user.UserID, 1, 7);
                if (IMgn.exist == 1)
                {
                    var createGroup1 = EmMethodManage.EmGroupManage.SInviteInGroup(Convert.ToUInt64(user.UserID), 7, 3, "感恩惠粉群", 13, null, new long[] { });
                    IMgn.gid = createGroup1.gid;
                }
                if (IMgn.gid != 0)
                {
                    if (modelGroupMark == null || modelGroupMark.Count(P => P.GroupType == 1) == 0)
                    {
                        HKTHMall.Domain.Entities.YH_GroupMark itemGroupMark = new HKTHMall.Domain.Entities.YH_GroupMark();
                        itemGroupMark.UserID = user.UserID;
                        itemGroupMark.GroupID = (int)IMgn.gid;
                        itemGroupMark.GroupType = 1;
                        yh_groupmarkService.AddGroupMark(itemGroupMark);
                    }
                    else
                    {
                        if (modelGroupMark.Count(P => P.GroupType == 1 && P.GroupID == (int)IMgn.gid) == 0)
                        {
                            //删除再添加
                            yh_groupmarkService.DeleteGroupMark(user.UserID, 1);
                            HKTHMall.Domain.Entities.YH_GroupMark itemGroupMark = new HKTHMall.Domain.Entities.YH_GroupMark();
                            itemGroupMark.UserID = user.UserID;
                            itemGroupMark.GroupID = (int)IMgn.gid;
                            itemGroupMark.GroupType = 1;
                            yh_groupmarkService.AddGroupMark(itemGroupMark);
                        }
                    }
                }
                URQuerySpecificGroupRsp IMgd = EmMethodManage.EmGroupManage.URQuerySpecificGroup((ulong)user.UserID, 2, 7);
                if (IMgd.exist == 1 )
                {
                    var createGroup1 = EmMethodManage.EmGroupManage.SInviteInGroup(Convert.ToUInt64(user.UserID), 7, 3, "感动惠粉群", 13, null, new long[] { });
                    IMgd.gid = createGroup1.gid;
                }
                if (IMgd.gid != 0)
                {
                    if (modelGroupMark == null || modelGroupMark.Count(P => P.GroupType == 2) == 0)
                    {
                        HKTHMall.Domain.Entities.YH_GroupMark itemGroupMark = new HKTHMall.Domain.Entities.YH_GroupMark();
                        itemGroupMark.UserID = user.UserID;
                        itemGroupMark.GroupID = (int)IMgd.gid;
                        itemGroupMark.GroupType = 2;
                        yh_groupmarkService.AddGroupMark(itemGroupMark);
                    }
                    else
                    {
                        if (modelGroupMark.Count(P => P.GroupType == 2 && P.GroupID == (int)IMgd.gid) == 0)
                        {
                            //删除再添加
                            yh_groupmarkService.DeleteGroupMark(user.UserID, 2);
                            HKTHMall.Domain.Entities.YH_GroupMark itemGroupMark = new HKTHMall.Domain.Entities.YH_GroupMark();
                            itemGroupMark.UserID = user.UserID;
                            itemGroupMark.GroupID = (int)IMgd.gid;
                            itemGroupMark.GroupType = 2;
                            yh_groupmarkService.AddGroupMark(itemGroupMark);
                        }
                    }
                }
                URQuerySpecificGroupRsp IMgx = EmMethodManage.EmGroupManage.URQuerySpecificGroup((ulong)user.UserID, 3, 7);
                if (IMgx.exist == 1 )
                {
                    var createGroup1 = EmMethodManage.EmGroupManage.SInviteInGroup(Convert.ToUInt64(user.UserID), 7, 3, "感谢惠粉群", 13, null, new long[] { });
                    IMgx.gid = createGroup1.gid;
                }
                if (IMgx.gid != 0)
                {
                    if (modelGroupMark == null || modelGroupMark.Count(P => P.GroupType == 3) == 0)
                    {
                        HKTHMall.Domain.Entities.YH_GroupMark itemGroupMark = new HKTHMall.Domain.Entities.YH_GroupMark();
                        itemGroupMark.UserID = user.UserID;
                        itemGroupMark.GroupID = (int)IMgx.gid;
                        itemGroupMark.GroupType = 3;
                        yh_groupmarkService.AddGroupMark(itemGroupMark);
                    }
                    else
                    {
                        if (modelGroupMark.Count(P => P.GroupType == 3 && P.GroupID == (int)IMgx.gid) == 0)
                        {
                            //删除再添加
                            yh_groupmarkService.DeleteGroupMark(user.UserID, 3);
                            HKTHMall.Domain.Entities.YH_GroupMark itemGroupMark = new HKTHMall.Domain.Entities.YH_GroupMark();
                            itemGroupMark.UserID = user.UserID;
                            itemGroupMark.GroupID = (int)IMgx.gid;
                            itemGroupMark.GroupType = 3;
                            yh_groupmarkService.AddGroupMark(itemGroupMark);
                        }
                    }
                }
            }
            ResultModel rmgroupReferrerID = yh_groupmarkService.GetGroupMarkByUserId(user.ReferrerID.Value);
            if (rmgroupReferrerID.Flag == 1)
            {
                List<HKTHMall.Domain.Entities.YH_GroupMark> modelReferrerGroupMark = rmgroupReferrerID.Data as List<HKTHMall.Domain.Entities.YH_GroupMark>;
                URQuerySpecificGroupRsp IMReferrergn = EmMethodManage.EmGroupManage.URQuerySpecificGroup((ulong)user.ReferrerID.Value, 1, 7);
                if (IMReferrergn.exist == 1)
                {
                    //创建推荐人的感恩惠粉群
                    var createGroup4 = EmMethodManage.EmGroupManage.SInviteInGroup(Convert.ToUInt64(user.ReferrerID.Value), 7, 3, "感恩惠粉群", 13, null, new long[] { });
                    IMReferrergn.gid = createGroup4.gid;
                }
                if (IMReferrergn.gid != 0)
                {
                    if (modelReferrerGroupMark == null || modelReferrerGroupMark.Count(P => P.GroupType == 1) == 0)
                    {
                        HKTHMall.Domain.Entities.YH_GroupMark itemGroupMark = new HKTHMall.Domain.Entities.YH_GroupMark();
                        itemGroupMark.UserID = user.ReferrerID.Value;
                        itemGroupMark.GroupID = (int)IMReferrergn.gid;
                        itemGroupMark.GroupType = 1;
                        yh_groupmarkService.AddGroupMark(itemGroupMark);
                    }
                    else
                    {
                        if (modelReferrerGroupMark.Count(P => P.GroupType == 1 && P.GroupID == (int)IMReferrergn.gid) == 0)
                        {
                            //删除再添加
                            yh_groupmarkService.DeleteGroupMark(user.ReferrerID.Value, 1);
                            HKTHMall.Domain.Entities.YH_GroupMark itemGroupMark = new HKTHMall.Domain.Entities.YH_GroupMark();
                            itemGroupMark.UserID = user.ReferrerID.Value;
                            itemGroupMark.GroupID = (int)IMReferrergn.gid;
                            itemGroupMark.GroupType = 1;
                            yh_groupmarkService.AddGroupMark(itemGroupMark);
                        }
                    }
                    var request444 = EmMethodManage.EmGroupManage.QueryIsExistMember(IMReferrergn.gid, 7, Convert.ToUInt64(user.UserID));
                    if (request444.exist != 2)
                    {
                        var joinGroup = EmMethodManage.EmGroupManage.InviteInGroup(Convert.ToUInt64(user.ReferrerID), 7, 2, null, null, IMReferrergn.gid, new long[] { user.UserID });
                    }
                }

            }
        }
        #endregion
    }
}