using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
    [Validator(typeof(FeedbackValidator))]
    public class RequestLang
    {
        public int lang { get; set; }
    }

    public class RequestLangValidator : AbstractValidator<RequestLang>
    {
        public RequestLangValidator(int lang)
        {
            //RuleFor(x => x.content).NotEmpty().WithMessage(CultureHelper.GetAPPLangSgring("CONTENT_NOT_EMPTY", lang));            
        }
    }

}