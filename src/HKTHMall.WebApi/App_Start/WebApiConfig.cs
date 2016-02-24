using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Http.ExceptionHandling;

namespace HKTHMall.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear(); 

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.UseDataContractJsonSerializer = true;
            json.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;           
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "{controller}/{action}/{id}", new {id = RouteParameter.Optional}
                );

 //           config.Services.Replace(typeof(IExceptionHandler),
 //new HKTHMall.WebApi.GlobalException.GlobalExceptionHandler());

            config.Filters.Add(new HKTHMall.WebApi.GlobalException.ExceptionFilter());
            config.Filters.Add(new HKTHMall.WebApi.GlobalException.ModelValidationFilter());
        }
    }
}