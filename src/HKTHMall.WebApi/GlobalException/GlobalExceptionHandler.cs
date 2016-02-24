using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using NLog;
using System.Text;
using System.IO;
using HKTHMall.Core;
using HKTHMall.Core.Utils;

namespace HKTHMall.WebApi.GlobalException
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            HttpContext httpContext = HttpContext.Current;
            var exception = context.Exception;
            var httpException = exception as HttpException;
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
                        errorBuilder.Append(",请求内容:" + streamReader.ReadToEnd());
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
            if (!(exception is OperationCanceledException))
            {
                if (httpException != null)
                {
                    int errCode = httpException.GetHttpCode();
                    if (errCode == 500)
                    {
                        NLogHelper.GetCurrentClassLogger().Error(exception,errorBuilder.ToString());
                        context.Result = new CustomErrorResult(context.Request,
                       (HttpStatusCode)httpException.GetHttpCode(),
                        "System Error");
                        return;
                    }
                    if (errCode > 500)
                    {
                        NLogHelper.GetCurrentClassLogger().Error(exception,errorBuilder.ToString());
                    }
                    context.Result = new CustomErrorResult(context.Request,
                     (HttpStatusCode)httpException.GetHttpCode(),
                      httpException.Message);
                    return;
                }

                // Return HttpStatusCode for other types of exception.
                //context.Result = new CustomErrorResult(context.Request,
                //    HttpStatusCode.InternalServerError,
                //    exception.Message);
                NLogHelper.GetCurrentClassLogger().Error(exception,errorBuilder.ToString());
                context.Result = new CustomErrorResult(context.Request,
                  HttpStatusCode.InternalServerError,
                 "System Error");
            }
        }
    }
}