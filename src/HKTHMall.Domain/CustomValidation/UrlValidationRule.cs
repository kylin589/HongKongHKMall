using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace HKTHMall.Domain.CustomValidation
{
    public class UrlValidationRule : RegularExpressionValidator
    {
       /// <summary>
       /// URL正则表达式
       /// </summary>
        public UrlValidationRule()
            : base(@"^(http|https)://([\w-]+\.)+[\w-]+(/[\w-./?%&=#:]*)?$")
            //:base(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$")
            //: base(@"^http\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$")
        {

        }
    }

    /// <summary>
    /// Url验证方法。
    /// </summary>
    public static class UrlValidatorExtensions
    {
        public static IRuleBuilderOptions<T,string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UrlValidationRule());
        }
    }

}
