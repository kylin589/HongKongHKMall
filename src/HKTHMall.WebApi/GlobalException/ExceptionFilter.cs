using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Net;
using System.Text;
using HKTHMall.Core.Utils;
using System.IO;
using HKTHMall.Core;

using Newtonsoft.Json.Linq;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.WebApi.GlobalException
{
    /// <summary>
    /// 错误处理类
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            HttpContext httpContext = HttpContext.Current;
            var exception = context.Exception;
            var httpException = exception as HttpException;
            int lang = 2;//默认英文
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            // 获得前一个异常的实例
            StringBuilder errorBuilder = new StringBuilder("请求地址:" + httpContext.Request.Url.AbsoluteUri + ",原始请求地址:" + httpContext.Request.RawUrl);
            try
            {
                string contentType = httpContext.Request.ContentType;
                string[] contentTypeArray = new string[] { "application/json", "text/xml", "application/x-www-form-urlencoded", "application/xml" };
                if (Utils.InArray(contentType, contentTypeArray))
                {
                    var stream = httpContext.Request.InputStream;
                    
                    using (var streamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8")))
                    {
                        var requestStr = streamReader.ReadToEnd();
                        errorBuilder.Append(",请求内容:" + requestStr);
                        if (contentType.Equals("application/json"))
                        {
                            JObject param = JObject.Parse(requestStr);
                            if (param["lang"] != null && !string.IsNullOrWhiteSpace(param["lang"].ToString()))
                                int.TryParse(param["lang"].ToString(), out lang);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

            }

            //创建响应模型

            Models.Result.ApiResultModel response = new Models.Result.ApiResultModel { flag = 0, msg = CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", lang) };//提示系统异常

            NLogHelper.GetCurrentClassLogger().Error(exception, errorBuilder.ToString());
            context.Response = context.Request.CreateResponse(HttpStatusCode.OK, response, "application/json");
        }
    }
}