using FluentValidation;
using FluentValidation.Attributes;
using HKTHMall.Services.Common.MultiLangKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    [Validator(typeof(RegisteredValidator))]
    public class RequestRegistered
    {
     
        /// <summary>
        /// 何总电话
        /// </summary>
        public string Inviting{get;set;}

        private string _phone ;
        /// <summary>
        /// 手机号
        /// </summary>
        public string phone 
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
            }
        }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public int verification { get; set; }
        
        private string _email;
        /// <summary>
        /// 邮箱地址(加密)
        /// </summary>
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }       
    }

    public class RegisteredValidator : AbstractValidator<RequestRegistered>
    {
        public RegisteredValidator(int lang)
        {
            //请输入手机号
            var LOGIN_ENTERNUMBER = CultureHelper.GetAPPLangSgring("LOGIN_ENTERNUMBER", lang);
            //手机号码格式错误,请重新输入
            var msgAPI_FORMATERROR = string.IsNullOrEmpty(CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_MOBILEISBEEMPTYORCORRECTERROR", lang)) ? "ERROR" : CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_MOBILEISBEEMPTYORCORRECTERROR", lang);//
            //输入的验证码错误
            var msgAPI_VERIFICATIONERROR = string.IsNullOrEmpty(CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_CODEERROR", lang)) ? "ERROR" : CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_CODEERROR", lang);//
            //密码由8-20位数字、字母或特殊字符组成,区分大小写
            var msgAPI_PASSWORDFORMAT = string.IsNullOrEmpty(CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORD", lang)) ? "ERROR" : CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_PASSWORD", lang);

            RuleFor(x => x.phone).NotEmpty().WithMessage(LOGIN_ENTERNUMBER);
            RuleFor(x => x.phone).Length(10, 10).WithMessage(msgAPI_FORMATERROR);
            RuleFor(x => x.verification).NotEmpty().WithMessage(msgAPI_VERIFICATIONERROR);
            RuleFor(x => x.password).NotEmpty().WithMessage(msgAPI_PASSWORDFORMAT);
        }
    }

}