using FluentValidation;
using FluentValidation.Attributes;
using HKTHMall.Core.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    //[Validator(typeof(LoginValidator))]
    public class RequestLoginValidate
    {
        private string _account;
        /// <summary>
        /// 手机号/惠信号/邮箱 (加密)
        /// </summary>
        public string account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
            }
        }
        /// <summary>
        /// 密码(MD5)
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 1-网站 2-android 3-ios
        /// </summary>
        public int registerSource { get; set; }
        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string version { get; set; }
    }

    /// <summary>
    /// 输入参数条件判断
    /// </summary>
    //public class LoginValidator : AbstractValidator<RequestLoginValidate>
    //{
    //    public LoginValidator(int lang)
    //    {
    //        RuleFor(x => x.account).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("API_ACCOUNTPASSWORDERROR", lang));
    //        RuleFor(x => x.password).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("API_ACCOUNTPASSWORDERROR", lang));
    //    }
    //}

}