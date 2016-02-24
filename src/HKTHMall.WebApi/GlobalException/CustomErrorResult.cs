using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HKTHMall.WebApi.GlobalException
{
    public class CustomErrorResult : IHttpActionResult
    {
        private readonly string _errorMessage;
        private readonly HttpRequestMessage _requestMessage;
        private readonly HttpStatusCode _statusCode;

        public CustomErrorResult(HttpRequestMessage requestMessage,
           HttpStatusCode statusCode, string errorMessage)
        {
            _requestMessage = requestMessage;
            _statusCode = statusCode;
            _errorMessage = errorMessage;
        }

        #region Implementation of IHttpActionResult

        /// <summary>
        /// 以异步方式创建 <see cref="T:System.Net.Http.HttpResponseMessage"/>。
        /// </summary>
        /// <returns>
        /// 在完成时包含 <see cref="T:System.Net.Http.HttpResponseMessage"/> 的任务。
        /// </returns>
        /// <param name="cancellationToken">要监视的取消请求标记。</param>
        Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
        {

            return Task.FromResult(_requestMessage.CreateErrorResponse(
               _statusCode, _errorMessage));
        }

        #endregion
    }
}