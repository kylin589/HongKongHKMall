using HKSJ.Common;
using HKTHMall.Core;
using HKTHMall.Domain.Enum;
using HKTHMall.Services.Common;
using HKTHMall.Services.Common.MultiLangKeys;
using System;
using System.Web.Mvc;

namespace HKTHMall.Web.Common
{
    public class SendPhoneMsg : Controller
    {
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="PhoneNum">手机号</param>
        /// <returns></returns>
        public PhoneMsg SendPhoneCode(string PhoneNum)
        {            
            string phoneCode =CodeHelper.GetRandomNumber(6);
            PhoneMsg phonMsg = new PhoneMsg();
            if (System.Web.HttpContext.Current.Session["CodeMsg"] != null)
            {
                PhoneCodeMsg codeMsg = (PhoneCodeMsg)System.Web.HttpContext.Current.Session["CodeMsg"];
                DateTime oldTime = Convert.ToDateTime(codeMsg.GetPhoneMsgDateTime);
                double Seconds = (DateTime.Now - oldTime).TotalSeconds;
                if (Seconds < 120)
                {
                    phonMsg.IsMessage = false;
                    phonMsg.PhoneCode = phoneCode;
                    phonMsg.Msg = CultureHelper.GetLangString("LOGIN_SEND_TIMERANGE");//两次发送短信时间间隔不得小于120秒
                    return phonMsg;
                }
            }
            ResultHelper result = new ResultHelper();
            if (Settings.IsMessageEM)
            {
                result.Code = 1;
                phoneCode="666666";
            }
            else 
            { 
                result = SMSCommon.QTSubmitSMS(1, PhoneNum, phoneCode, "", 1);
            }
            if (result.Code == 1)
            {
                phonMsg.IsMessage = true;
                phonMsg.Msg = CultureHelper.GetLangString("LOGIN_SEND_SUCCESS");//发送成功
            }
            else
            {
                phonMsg.IsMessage = false;
                phonMsg.Msg = CultureHelper.GetLangString("LOGIN_SEND_FAILURE");//发送失败              
            }
            phonMsg.PhoneCode = phoneCode;
            PhoneCodeMsg codeSession = new PhoneCodeMsg();
            codeSession.GetPhoneMsgDateTime = DateTime.Now;
            codeSession.PhoneNum = PhoneNum;
            codeSession.PhoneCode = phoneCode;
            System.Web.HttpContext.Current.Session["CodeMsg"] = codeSession;
            return phonMsg;
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="PhoneNum">手机号</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-28</remarks>
        public PhoneMsg SendMerchatPhoneCode(string PhoneNum)
        {
            string phoneCode = CodeHelper.GetRandomNumber(6);
            PhoneMsg phonMsg = new PhoneMsg();
            if (System.Web.HttpContext.Current.Session["SupCodeMsg"] != null)
            {
                PhoneCodeMsg codeMsg = (PhoneCodeMsg)System.Web.HttpContext.Current.Session["SupCodeMsg"];
                DateTime oldTime = Convert.ToDateTime(codeMsg.GetPhoneMsgDateTime);
                double Seconds = (DateTime.Now - oldTime).TotalSeconds;
                if (Seconds < 60)
                {
                    phonMsg.IsMessage = false;
                    phonMsg.PhoneCode = phoneCode;
                   // phonMsg.Msg = CultureHelper.GetLangString("LOGIN_SEND_TIMERANGE");//两次发送短信时间间隔不得小于60秒
                    phonMsg.PhoneCode = "次发送短信时间间隔不得小于60秒";
                    return phonMsg;
                }
            }
            ResultHelper result = new ResultHelper();
            //是否假发送短信，为true值时，表示伪假短信，反之，表示真发送短信
            if (Settings.IsMessageEM)
            {
                result.Code = 1;
                phoneCode = "666666";
            }
            else
            {
                result = SMSCommon.QTSubmitSMS((int)ESendType.AnyContent, PhoneNum, phoneCode, string.Format("尊敬的惠卡用户：您好！您正在进行账号操作，验证码是{0}，如有疑问请致电026355484", phoneCode), (int)EBusinessType.RetrievePwd);
            }
            if (result.Code == 1)
            {
                phonMsg.IsMessage = true;
               // phonMsg.Msg = CultureHelper.GetLangString("LOGIN_SEND_SUCCESS");//发送成功
                phonMsg.Msg = "发送成功";
            }
            else
            {
                phonMsg.IsMessage = false;
               // phonMsg.Msg = CultureHelper.GetLangString("LOGIN_SEND_FAILURE");//发送失败   
                phonMsg.Msg = "发送失败";
            }
            phonMsg.PhoneCode = phoneCode;
            PhoneCodeMsg codeSession = new PhoneCodeMsg();
            codeSession.GetPhoneMsgDateTime = DateTime.Now;
            codeSession.PhoneNum = PhoneNum;
            codeSession.PhoneCode = phoneCode;
            System.Web.HttpContext.Current.Session["SupCodeMsg"] = codeSession;
            return phonMsg;
        }

        /// <summary>
        /// 短信验证码比对
        /// </summary>
        /// <param name="phoneNum">手机号码</param>
        /// <param name="phoneCode">短信验证码</param>
        /// <returns></returns>
        public  bool PhoneVerificationCode(string phoneNum, string phoneCode)
        {
            if (System.Web.HttpContext.Current.Session["CodeMsg"] != null)
            {
                PhoneCodeMsg codeMsg = (PhoneCodeMsg)System.Web.HttpContext.Current.Session["CodeMsg"];
                if ((DateTime.Now - codeMsg.GetPhoneMsgDateTime).TotalSeconds > 900)//超时,验证码失效
                {
                    //清空Session
                    System.Web.HttpContext.Current.Session["CodeMsg"] = null;
                    return false;
                }
                //判断是否同一手机和正确的验证码
                if (codeMsg.PhoneNum == phoneNum && codeMsg.PhoneCode == phoneCode)
                {
                    return true;
                }
                return false;
            }           
            else
            {
                return false;
            }               
        }
    }

    public class PhoneCodeMsg
    {
        public DateTime GetPhoneMsgDateTime { get; set; }
        public string PhoneNum { get; set; }
        public string PhoneCode { get; set; }
    }
}