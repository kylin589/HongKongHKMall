using System.Net.Http;
using System.Text;
using HKTHMall.Domain.Models;
using Newtonsoft.Json;

namespace HKTHMall.WebApi
{
    public static class Extensions
    {
        public static HttpResponseMessage ToHttpResponseMessage(this IResult model)
        {
            var jsonStr = JsonConvert.SerializeObject(model);
            return new HttpResponseMessage {Content = new StringContent(jsonStr, Encoding.UTF8, "application/json")};
        }
    }
}