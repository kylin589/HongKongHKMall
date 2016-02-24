using FluentValidation;
using FluentValidation.Attributes;
using HKTHMall.Core.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    [Validator(typeof(FeedbackValidator))]
    public class RequestFeedback
    {      
        
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 0:未选择,1:注册登录反馈,2:购物反馈,3惠粉反馈,4:消费收益反馈,5:其它反馈
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 1:中文,2:英文,3:泰文
        /// </summary>
        public int lang { get; set; }
        
    }

    /// <summary>
    /// 输入参数条件判断
    /// </summary>
    public class FeedbackValidator : AbstractValidator<RequestFeedback>
    {
        public FeedbackValidator(int lang)
        {
            //RuleFor(x => x.content).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("CONTENT_NOT_EMPTY", lang));            
        }
    }

}