using FluentValidation;
using FluentValidation.Attributes;
using HKTHMall.Services.Common.MultiLangKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    /// 3.4.	找回密码
    /// </summary>
    [Validator(typeof(RetrievePassword))]
    public class RequestRetrievePassword
    {
        private string _phone;
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
        /// 密码(MD5)
        /// </summary>
        public string password { get; set; }
        /// <summary>
        ///短信验证码唯一识别码 从3.2接口而来的数据
        /// </summary>
        public string verification{ get; set; }
        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }

    }

    public class RetrievePassword : AbstractValidator<RequestRetrievePassword>
    {
        public RetrievePassword(int lang)
        {
            When(x => x == null, () => { });
            RuleFor(x => x.phone).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_MOBILEISBEEMPTYORCORRECT", lang));
            RuleFor(x => x.phone).Length(10, 10).WithMessage(CultureHelper.GetAPPLangSgring("ACCOUNT_USERINFO_ADDRESS_MOBILEISBEEMPTYORCORRECT", lang));
            RuleFor(x => x.password).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_ENTERNEWPASSWORDFIRST", lang));
            RuleFor(x => x.verification).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("LOGIN_GETPASSWORD_VERIFICODE", lang));
        }
    }

}