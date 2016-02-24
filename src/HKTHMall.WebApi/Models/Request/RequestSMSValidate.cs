using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Validators;
using HKTHMall.Services.Common.MultiLangKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 3.2.获取验证码（马锋）
    /// </summary>
    [Validator(typeof(SMSValidate))]
    public class RequestSMSValidate
    {
        public int lang { get; set; }
        /// <summary>
        /// 手机号码,(不要验证手机长度,泰国手机和中国不同)
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 1-网站 2-android 3-ios
        /// </summary>
        public int registerSource { get; set; }
        /// <summary>
        /// 1:注册验证码,2:找回密码验证码
        /// </summary>
        public int type { get; set; }
    }

    public class SMSValidate : AbstractValidator<RequestSMSValidate>
    {
        public SMSValidate(int lang)
        {
            var msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_MOBILEISBEEMPTYORCORRECTERROR", lang); //手机号码格式错误,请重新输入
            RuleFor(x => x.phone).NotEmpty().WithMessage(string.IsNullOrEmpty(msg) ? "ERROR" : msg);
            RuleFor(x => x.phone).Length(10, 10).WithMessage(string.IsNullOrEmpty(msg) ? "ERROR" : msg);

        }
    }
    [Validator(typeof(CheckCodeSMSValidate))]
    public class RequestFindPasswordCheckCode
    {
        public int lang { get; set; }
        /// <summary>
        /// 手机号码,(不要验证手机长度,泰国手机和中国不同)
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string validateCode { get; set; }
    }
    public class CheckCodeSMSValidate : AbstractValidator<RequestFindPasswordCheckCode>
    {
        public CheckCodeSMSValidate(int lang)
        {
            var msg = CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_MOBILEISBEEMPTYORCORRECTERROR", lang); //手机号码格式错误,请重新输入
            RuleFor(x => x.phone).NotEmpty().WithMessage(string.IsNullOrEmpty(msg) ? "ERROR" : msg);
            RuleFor(x => x.phone).Length(10, 10).WithMessage(string.IsNullOrEmpty(msg) ? "ERROR" : msg);

        }
    }
}