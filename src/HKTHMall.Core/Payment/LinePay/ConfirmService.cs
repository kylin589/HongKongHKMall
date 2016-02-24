using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Core.Utils;

namespace HKTHMall.Core.Payment.LinePay
{
    public class ConfirmService
    {
        /// <summary>
        /// 渠道Id
        /// </summary>
        private static string ChannelId = ConfigurationManager.AppSettings["LinePayChannelId"];

        /// <summary>
        /// 渠道密钥
        /// </summary>
        private static string ChannelSecret = ConfigurationManager.AppSettings["LinePayChannelSecret"];


        /// <summary>
        /// 请求Url
        /// </summary>
        private static string ConfirmUrl = ConfigurationManager.AppSettings["LinePayConfirmUrl"];


        /// <summary>
        /// 请求参数
        /// </summary>
        public ConfirmDetailReq ConfirmDetailReq { get; set; }


        /// <summary>
        /// 确认请求
        /// </summary>
        /// <param name="transactionId">事务Id</param>
        /// <returns></returns>
        public dynamic ConfirmRequest(string transactionId)
        {
            string jsonStr = JsonConverts.ToJson(ConfirmService.GetProperties(this.ConfirmDetailReq));
            return this.ConfirmRequest(transactionId, Encoding.GetEncoding("UTF-8").GetBytes(jsonStr));
        }

        /// <summary>
        /// 确认请求
        /// </summary>
        /// <param name="transactionId">事务Id</param>
        /// <param name="data"></param>
        /// <returns></returns>
        private dynamic ConfirmRequest(string transactionId, byte[] data)
        {

            string url = string.Format(ConfirmUrl, transactionId);

            Stream responseStream;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers.Add("X-LINE-ChannelId:" + ChannelId);
            request.Headers.Add("X-LINE-ChannelSecret:" + ChannelSecret);
            request.Method = "POST";
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            dynamic obj = serializer.Deserialize(str, typeof(object));
            return obj;
        }


        /// <summary>
        /// 获取相关参数属性
        /// </summary>
        /// <param name="confimDetailReq">请求实体</param>
        /// <returns></returns>
        private static Dictionary<string, string> GetProperties(ConfirmDetailReq confimDetailReq)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("amount", confimDetailReq.amount);
            properties.Add("currency", confimDetailReq.currency);

            return properties;
        }

    }
}
