using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Threading;

using HKTHMall.Domain.Enum;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.WebApi.GlobalException
{
    public class ModelValidationFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            int lang = (int)LanguageType.defaultLang;
            if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count > 0)
            {
                object argus = actionContext.ActionArguments.First().Value;
                foreach (var pro in argus.GetType().GetProperties())
                {
                    if (pro.Name.ToUpper().Equals("LANG"))
                    {
                        lang = Convert.ToInt32(pro.GetValue(argus, null));
                        break;
                    }
                }
            }
            //语言
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CultureHelper.GetLanguage(lang));
            //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
           if (actionContext.ModelState.IsValid == false)
            {
                // Return the validation errors in the response body.
                // 在响应体中返回验证错误
                //var errors = new Dictionary<string, IEnumerable<string>>();
                //foreach (KeyValuePair<string, ModelState> keyValue in actionContext.ModelState)
                //{
                //    errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
                //}

                //actionContext.Response =
                //    actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);



                //System.Text.StringBuilder sbErrors = new System.Text.StringBuilder();
                //foreach (var item in actionContext.ModelState.Values)
                //{
                //    if (item.Errors.Count > 0)
                //    {
                //        for (int i = item.Errors.Count - 1; i >= 0; i--)
                //        {
                //            sbErrors.Append(item.Errors[i].Exception == null ? item.Errors[i].ErrorMessage : item.Errors[i].Exception.Message);
                //            sbErrors.Append(";");
                //        }
                //    }
                //}
                Models.Result.ApiResultModel result = new Models.Result.ApiResultModel { flag = 0, msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", lang) };//参数错误
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, result, "application/json");
            }
        }

        class LangArgu
        {
            public int lang { get; set; }
        }
    }
}